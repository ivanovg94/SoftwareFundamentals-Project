using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventSpot.Models;

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
        public ActionResult Create()
        {

            return View();
        }

        //
        //POST: Event/Create
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
    }
}