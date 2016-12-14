using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventSpot.Startup))]
namespace EventSpot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
