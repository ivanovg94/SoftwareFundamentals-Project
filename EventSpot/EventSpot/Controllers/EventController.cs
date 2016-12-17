using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventSpot.Models;
<<<<<<< HEAD
using System.IO;
=======
using System.Numerics;
using static EventSpot.Models.Event;
>>>>>>> 569c9b30eafd52d35221c6f6141de72e7aef1fdf

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

            return View();
        }

        //
        //POST: Event/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Event events)
        {
        

            if (ModelState.IsValid)
            {
                //insert event in DB 
                using (var database = new EventSpotDbContext())
                {
                    //Get OrganizerID
                    var organizerId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    
                    //Set Event Organizer
                    events.OrganizerId = organizerId;


                    //Save event in DB
                    database.Events.Add(events);
                    database.SaveChanges();

                    return RedirectToAction("Main");
                }
            }

            return View(events);
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
                    .First();

                if (!IsOrganizerAuthorizedToEdit(events))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

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
                model.EventDescription = events.EventDescription;

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
                    events.EventDescription = model.EventDescription;


                    database.Entry(events).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Main");
                }

            }
            return View(model);
        }
        private bool IsOrganizerAuthorizedToEdit(Event events)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isOrganizer = events.IsOrganizer(this.User.Identity.Name);

            return isAdmin || isOrganizer;
        }
    }
}