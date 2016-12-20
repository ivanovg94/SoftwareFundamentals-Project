using EventSpot.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventSpot.Controllers.Admin
{
    public class CityController : Controller
    {
        // GET: City
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var database = new EventSpotDbContext())
            {
                var cities = database.Cities
                    .ToList();

                return View(cities);
            }
        }

        //
        // GET: City/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        //POST: City/Create
        [HttpPost]
        public ActionResult Create(City city)
        {
            if (ModelState.IsValid)
            {
                using (var database = new EventSpotDbContext())
                {
                    database.Cities.Add(city);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }

            }
            return View(city);
        }

        //
        // GET: Cityy/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new EventSpotDbContext())
            {
                var city = database.Cities
                    .FirstOrDefault(c => c.Id == id);

                if (city == null)
                {
                    return HttpNotFound();
                }
                return View(city);
            }
        }
        //
        //Post: City/Edit
        [HttpPost]
        public ActionResult Edit(City city)
        {
            if (ModelState.IsValid)
            {
                using (var database = new EventSpotDbContext())
                {
                    database.Entry(city).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }

            }
            return View(city);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new EventSpotDbContext())
            {
                var city = database.Cities
                    .FirstOrDefault(c => c.Id == id);

                if (city == null)
                {
                    return HttpNotFound();
                }
                return View(city);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var database = new EventSpotDbContext())
            {
                var city = database.Cities
                    .FirstOrDefault(c => c.Id == id);

                var cityEvents = city.Events
                    .ToList();

                foreach (var events in cityEvents)
                {
                    database.Events.Remove(events);
                }

                database.Cities.Remove(city);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [ChildActionOnly]
        public ActionResult GetNavbarCities()
        {
            using (var db = new EventSpotDbContext())
            {
                var navbarCities = db.Cities.ToList();
                return PartialView("_NavbarCitiesDropdown", navbarCities);
            }
        }
    }
}