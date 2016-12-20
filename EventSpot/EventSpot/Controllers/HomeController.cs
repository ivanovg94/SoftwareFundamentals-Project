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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            ViewBag.Message = "Main page";

            return RedirectToAction("List", "Event");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult ListCategories()
        {
            using (var database = new EventSpotDbContext())
            {
                var categories = database.Categories
                    .Include(c => c.Events)
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(categories);

            }
        }

        public ActionResult ListEvents(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new EventSpotDbContext())
            {
                var events = database.Events
                    .Where(a => a.CategoryId == id)
                    .Include(a => a.Organizer)
                    .Include(a => a.Tags)
                    .ToList();

                return View(events);

            }
           
        }

        public ActionResult ListCities()
        {
            using (var database = new EventSpotDbContext())
            {
                var cities = database.Cities
                    .Include(c => c.Events)
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(cities);

            }
        }

        public ActionResult ListEventsByCity(int? cityId)
        {
            if (cityId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new EventSpotDbContext())
            {
                var events = database.Events
                    .Where(a => a.CityId == cityId)
                    .Include(a => a.Organizer)
                    .ToList();

                return View(events);

            }

        }

    }
}