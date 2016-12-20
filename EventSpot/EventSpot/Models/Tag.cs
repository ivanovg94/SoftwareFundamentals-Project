using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EventSpot.Models
{
    public class Tag
    {
        private ICollection<Event> events;

        public Tag()
        {
            this.events = new HashSet<Event>();
        }

        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(20)]
         public string Name { get; set; }

        public virtual ICollection<Event> Events
        {
            get { return this.events; }
            set { this.events = value; }
        }
    }
}