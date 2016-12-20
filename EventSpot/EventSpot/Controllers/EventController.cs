using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EventSpot.Models;
using System.Numerics;
using static EventSpot.Models.Event;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System;

namespace EventSpot.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: Event
        public ActionResult Main()
        {
            return RedirectToAction("List");
        }

        //
        //GET: Event/List
        public ActionResult List()
        {
            using (var database = new EventSpotDbContext())
            {
                //Get Events from DB
                var events = database.Events
                    .Include(o => o.Organizer)
                    .Include(o => o.Tags)
                   .ToList();
                return View(events);
            }
        }

        //
        //GET: Event/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new EventSpotDbContext())
            {
                //Get Events from DB
                var events = database.Events
                    .Where(e => e.Id == id)
                    .Include(o => o.Organizer)
                    .Include(o => o.Tags)
                    .First();

                if (events == null)
                {
                    return HttpNotFound();
                }
                return View(events);
            }
        }

        //
        //GET: Event/Create
        [Authorize]
        public ActionResult Create()
        {
            
            using (var database = new EventSpotDbContext())
            {

                if (User.IsInRole("Attendant"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                var model = new EventViewModel();
                model.Categories = database.Categories
                    .OrderBy(c => c.Name)
                    .ToList();

                model.Cities = database.Cities
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(model);
            }

        }

        //
        //POST: Event/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(EventViewModel model)
        {


            if (ModelState.IsValid)
            {

                // To convert the user uploaded Photo as Byte Array before save to DB 
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["Event"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }



                //insert event in DB 
                using (var database = new EventSpotDbContext())
                {
                    //Get OrganizerID
                    var organizerId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

              
 

                    var events = new Event(organizerId, model.EventName,
                        model.EventDescription, model.EventDate,
                        model.StartTime, model.CategoryId, model.CityId);

                    //
                   //var cityName = database.Cities.Where(c => c.Id == events.CityId).
                   //     First().
                   //     Name;

                    this.SetEventTags(events, model, database);


                    //Set Event Organizer
                    events.OrganizerId = organizerId;

                    //
                   // events.City.Name = cityName;

                    events.EventPhoto = imageData;

                    //Save event in DB

                    database.Events.Add(events);

                    database.SaveChanges();

                    return RedirectToAction("Main");
                }
            }

            return View(model);
        }


        public ActionResult DisplayImg(int Id)
        {
            var bdEvents = HttpContext.GetOwinContext().Get<EventSpotDbContext>();
            var eventImage = bdEvents.Events.Where(x => x.Id == Id).FirstOrDefault();
            return new FileContentResult(eventImage.EventPhoto, "image/jpg");
        }


        //
        //GET: Event/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new EventSpotDbContext())
            {
                var events = database.Events
                    .Where(a => a.Id == id)
                    .Include(a => a.Organizer)
                    .Include(a => a.Category)
                    .Include(a => a.City)
                    .First();

                if (!IsOrganizerAuthorizedToEdit(events))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (User.IsInRole("Attendant"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ViewBag.TagsString = string.Join(", ", events.Tags.Select(t => t.Name));

                if (events == null)
                {
                    return HttpNotFound();
                }
                return View(events);
            }
        }

        //
        //POST: Event/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var database = new EventSpotDbContext())
            {
                var events = database.Events
                    .Where(a => a.Id == id)
                    .Include(a => a.Organizer)
                    .First();


                if (events == null)
                {
                    return HttpNotFound();
                }

                database.Events.Remove(events);
                database.SaveChanges();

                return RedirectToAction("Main");
            }

        }

        //
        //Get: Event/Edit

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new EventSpotDbContext())
            {
                //Get event from database
                var events = database.Events
                    .Where(a => a.Id == id)
                    .Include(a => a.Organizer)
                    .First();

                if (!IsOrganizerAuthorizedToEdit(events))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (User.IsInRole("Attendant"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                //Check if event exists
                if (events == null)
                {
                    return HttpNotFound();
                }

                //Create the view model
                var model = new EventViewModel();
                model.Id = events.Id;
                model.EventName = events.EventName;
                model.EventDate = events.EventDate;
                model.StartTime = events.StartTime;
                model.EventDescription = events.EventDescription;
                model.CategoryId = events.CategoryId;
                model.Categories = database.Categories
                    .OrderBy(c => c.Name)
                    .ToList();
                model.CityId = events.CityId;
                model.Cities = database.Cities
                    .OrderBy(c => c.Name)
                    .ToList();
                model.Tags = string.Join(", ", events.Tags.Select(t => t.Name));

                //Pass the view model to view
                return View(model);
            }


        }
        //
        //POST: Event/Edit
        [HttpPost]
        public ActionResult Edit(EventViewModel model)
        {
            if (ModelState.IsValid)
            {

                using (var database = new EventSpotDbContext())
                {
                    //Get article from database
                    var events = database.Events
                        .FirstOrDefault(a => a.Id == model.Id);

                    events.EventName = model.EventName;
                    events.EventDate = model.EventDate;
                    events.StartTime = model.StartTime;
                    events.EventDescription = model.EventDescription;
                    events.CategoryId = model.CategoryId;
                    events.CityId = model.CityId;
                    this.SetEventTags(events, model, database);
                    database.Entry(events).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Main");
                }

            }
            return View(model);
        }

        private void SetEventTags(Event events, EventViewModel model, EventSpotDbContext database)
        {
            //Split tags
            var tagsSplitter = model.Tags
                .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //Clear current article tags
            events.Tags.Clear();
            //Set new article tags
            foreach (var tagString in tagsSplitter)
            {
                //Get tag from db by its name
                Tag tag = database.Tags.FirstOrDefault(t => t.Name.Equals(tagString));
                //if the tag is null,create new tag
                if (tag == null)
                {
                    tag = new Tag() { Name = tagString };
                    database.Tags.Add(tag);
                }
                //Add tag to article tags
                events.Tags.Add(tag);
            }
        }

        private bool IsOrganizerAuthorizedToEdit(Event events)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isOrganizer = events.IsOrganizer(this.User.Identity.Name);

            return isAdmin || isOrganizer;
        }



    }
}