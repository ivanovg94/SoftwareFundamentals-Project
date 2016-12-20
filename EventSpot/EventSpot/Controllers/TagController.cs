using EventSpot.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventSpot.Controllers
{
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult List(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new EventSpotDbContext())
            {
                //Get articles from db
                var events = database.Tags
                    .Include(t => t.Events.Select(a => a.Tags))
                    .Include(t => t.Events.Select(a => a.Organizer))
                    .FirstOrDefault(t => t.Id == id)
                    .Events
                    .ToList();
                return View(events);
            }

        }
    }
}