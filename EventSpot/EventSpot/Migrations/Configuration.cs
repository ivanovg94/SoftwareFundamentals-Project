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


        //    createRolesandUsers();
            //if (!context.Roles.Any())
            //{
            //    this.CreateRole(context, "Admin");
            //    this.CreateRole(context, "Attendant");
            //    this.CreateRole(context, "Organizer");
            //}
            //if (!context.Users.Any())
            //{
            //    this.CreateUser(context, "admin@admin.com", "Admin", "123456");
            //    this.SetRoleToUser(context, "admin@admin.com", "Admin");
            //}
      
        }


        private void createRolesandUsers()
        {
            EventSpotDbContext context = new EventSpotDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "shanu";
                user.Email = "syedshanumcain@gmail.com";

                string userPWD = "A@Z200711";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }
        }






        //private void SetRoleToUser(EventSpotDbContext context, string email, string role)
        //{
        //    var userManager = new UserManager<ApplicationUser>(
        //        new UserStore<ApplicationUser>(context));

        //    var user = context.Users.Where(u => u.Email == email).First();

        //    var result = userManager.AddToRole(user.Id, role);

        //    if (!result.Succeeded)
        //    {
        //        throw new Exception(string.Join(";", result.Errors));
        //    }
        //}

        //private void CreateRole(EventSpotDbContext context, string roleName)
        //{
        //    var roleManager = new RoleManager<IdentityRole>(
        //        new RoleStore<IdentityRole>(context));


        //    //// creating Creating Manager role    
        //    //if (!roleManager.RoleExists("Organizer"))
        //    //{
        //    //    var role = new IdentityRole();
        //    //    role.Name = "Organizer";
        //    //    roleManager.Create(role);

        //    //}

        //    //// creating Creating Employee role    
        //    //if (!roleManager.RoleExists("Attendant"))
        //    //{
        //    //    var role = new IdentityRole();
        //    //    role.Name = "Attendant";
        //    //    roleManager.Create(role);

        //    //}

        //    var result = roleManager.Create(new IdentityRole(roleName));

        //    if (!result.Succeeded)
        //    {
        //        throw new Exception(string.Join(";", result.Errors));
        //    }
        //}
        //private void CreateUser(EventSpotDbContext context, string email, string fullName, string password)
        //{
        //    // Create user manager
        //    var userManager = new UserManager<ApplicationUser>(
        //        new UserStore<ApplicationUser>(context));

        //    // Set user manager password validator
        //    userManager.PasswordValidator = new PasswordValidator
        //    {
        //        RequiredLength = 6,
        //        RequireDigit = true,
        //        RequireLowercase = false,
        //        RequireNonLetterOrDigit = false,
        //        RequireUppercase = false,
        //    };

        //    // Create user object
        //    var admin = new ApplicationUser
        //    {
        //        UserName = email,
        //        FullName = fullName,
        //        Email = email,
        //    };

        //    // Create user
        //    var result = userManager.Create(admin, password);

        //    // Validate result
        //    if (!result.Succeeded)
        //    {
        //        throw new Exception(string.Join(";", result.Errors));
        //    }

        //}
    }
}