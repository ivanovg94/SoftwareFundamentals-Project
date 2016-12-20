﻿using EventSpot.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventSpot.Controllers.Admin
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var database = new EventSpotDbContext())
            {
                var categories = database.Categories
                    .ToList();

                return View(categories);
            }
        }
        //
        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        //POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var database = new EventSpotDbContext())
                {
                    database.Categories.Add(category);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }

            }
            return View(category);
        }

        //
        // GET: Category/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new EventSpotDbContext())
            {
                var category = database.Categories
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
        }
        //
        //Post: Cateogry/Edit
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var database = new EventSpotDbContext())
                {
                    database.Entry(category).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }

            }
            return View(category);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new EventSpotDbContext())
            {
                var category = database.Categories
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var database = new EventSpotDbContext())
            {
                var category = database.Categories
                    .FirstOrDefault(c => c.Id == id);

                var categoryEvents = category.Events
                    .ToList();

                foreach (var events in categoryEvents)
                {
                    database.Events.Remove(events);
                }

                database.Categories.Remove(category);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [ChildActionOnly]
        public ActionResult GetNavbarCategories()
        {
            using (var db = new EventSpotDbContext())
            {
                var navbarCategories = db.Categories.ToList();
                return PartialView("_NavbarCategoriesDropdown", navbarCategories);
            }
        }
    }
}