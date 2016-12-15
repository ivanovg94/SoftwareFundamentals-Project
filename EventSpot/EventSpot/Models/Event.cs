using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EventSpot.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string EventName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        public string EventDescription { get; set; }

        [ForeignKey("Organizer")]
        public string OrganizerId { get; set; }

        public ApplicationUser Organizer { get; set; }
    }
}