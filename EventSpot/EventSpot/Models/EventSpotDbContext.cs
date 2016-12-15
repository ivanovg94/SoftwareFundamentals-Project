using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventSpot.Models
{
   

    public class EventSpotDbContext : IdentityDbContext<ApplicationUser>
    {
        public EventSpotDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        public virtual IDbSet<Event> Events { get; set; }

        public static EventSpotDbContext Create()
        {
            return new EventSpotDbContext();
        }
    }
}