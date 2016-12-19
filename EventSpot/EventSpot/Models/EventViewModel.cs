using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventSpot.Models
{
    public class EventViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string StartTime { get; set; }

        public string EventDescription { get; set; }

        public string OrganizerId { get; set; }

        public byte EventImage { get; set; }

        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }

        public ICollection<Event> Events { get; set; }

    }
}