﻿using EventSpot.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventSpot.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View("List");
        }
        // GET: User/List
        public ActionResult List()
        {
            using (var database = new EventSpotDbContext())
            {
                var users = database.Users
                    .ToList();
                var admins = GetAdminUserNames(users, database);
                ViewBag.Admins = admins;

                return View(users);
            }
        }

        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, EventSpotDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var admins = new HashSet<string>();

            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id, "Admin"))
                {
                    admins.Add(user.UserName);
                }
            }

            return admins;
        }
        //GET: User/Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var database = new EventSpotDbContext())
            {
                var user = database.Users
                    .Where(u => u.Id == id)
                    .First();

                if (user == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new EditUserViewModel();
                viewModel.User = user;
                viewModel.Roles = GetUserRoles(user, database);

                return View(viewModel);
            }
        }

        private List<Role> GetUserRoles(ApplicationUser user, EventSpotDbContext db)
        {
            //Create user manager
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            //Get all application roles
            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();

            //For each application role, check if the user has it
            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if (userManager.IsInRole(user.Id, roleName))
                {
                    role.IsSelected = true;
                }

                userRoles.Add(role);
            }

            //Return a list with all roles
            return userRoles;
        }
        //POST: User/Edit
        [HttpPost]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {
            //Check if model is valid
            if (ModelState.IsValid)
            {
                using (var database = new EventSpotDbContext())
                {
                    //Get user from databese
                    var user = database.Users.FirstOrDefault(u => u.Id == id);

                    //Check if user exists
                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    //If password field is not empty, change password
                    if (!string.IsNullOrEmpty(viewModel.Password))
                    {
                        var hasher = new PasswordHasher();
                        var passwordHash = hasher.HashPassword(viewModel.Password);
                        user.PasswordHash = passwordHash;
                    }

                    //Set user properties
                    user.Email = viewModel.User.Email;
                    user.FullName = viewModel.User.FullName;
                    user.UserName = viewModel.User.Email;
                    this.SetUserRoles( user, database, viewModel);

                    //Save changes
                    database.Entry(user).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("List");
                }
            }
            return View(viewModel);
        }

        private void SetUserRoles(ApplicationUser user, EventSpotDbContext context, EditUserViewModel model)
        {
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            foreach (var role in model.Roles)
            {
                if (role.IsSelected)
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if (!role.IsSelected)
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            using (var database = new EventSpotDbContext())
            {
                //Get user from database
                var user = database.Users
                    .Where(u => u.Id.Equals(id))
                    .First();

                //Check if user exists
                if (user == null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            using (var database = new EventSpotDbContext())
            {

                //Get user from database
                var user = database.Users
                    .Where(u => u.Id.Equals(id))
                    .First();

                //Get user articles from database
                var userEvents = database.Events
                    .Where(a => a.Organizer.Id == user.Id);

                foreach (var events in userEvents)
                {
                    database.Events.Remove(events);
                }

                //Delete user and save changes
                database.Users.Remove(user);
                database.SaveChanges();

                return RedirectToAction("List");
            }

        }

    }

}