namespace EventSpot.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<EventSpotDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "EventSpot.Models.EventSpotDbContext";
        }

        protected override void Seed(EventSpotDbContext context)
        {
            //Uncoment this to create roles and for the first time

            //createRolesandUsers();
        }


        private void createRolesandUsers()
        {
            EventSpotDbContext context = new EventSpotDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //create a Admin super user             

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@admin.com";

                string userPWD = "123456";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }

            }
            // creating Creating Manager role    
            if (!roleManager.RoleExists("Organizer"))
            {
                var role = new IdentityRole();
                role.Name = "Organizer";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Attendant"))
            {
                var role = new IdentityRole();
                role.Name = "Attendant";
                roleManager.Create(role);

            }
        }
    }
}