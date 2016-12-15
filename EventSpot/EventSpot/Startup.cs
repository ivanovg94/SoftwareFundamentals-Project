using System.Data.Entity;
using EventSpot.Migrations;
using EventSpot.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventSpot.Startup))]
namespace EventSpot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<EventSpotDbContext, Configuration>());

            ConfigureAuth(app);
        }
    }
}
