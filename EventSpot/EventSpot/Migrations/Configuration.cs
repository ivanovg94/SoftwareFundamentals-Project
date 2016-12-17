namespace EventSpot.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public sealed class Configuration : DbMigrationsConfiguration<EventSpot.Models.EventSpotDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "EventSpot.Models.EventSpotDbContext";
        }

        protected override void Seed(EventSpot.Models.EventSpotDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.CreateRole(context, "Admin");
                this.CreateRole(context, "Organizer");
                this.CreateRole(context, "User");

            }

            if (!context.Users.Any())
            {
                this.CreateUser(context, "admin@abv.bg", "Admin", "123");
                this.SetRoleToUser(context, "admin@abv.bg", "Admin");
            }
        }

        private void SetRoleToUser(EventSpotDbContext context, string email, string role)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var user = context.Users.Where(u => u.Email == email).First();

            var result = userManager.AddToRole(user.Id, role);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateRole(EventSpotDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            var result = roleManager.Create(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }

        }

        private void CreateUser(EventSpotDbContext context, string email, string fullName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(
                 new UserStore<ApplicationUser>(context));

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };

            var admin = new ApplicationUser
            {
                UserName = email,
                FullName = fullName,
                Email = email,
            };

            var result = userManager.Create(admin, password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

    }
}
