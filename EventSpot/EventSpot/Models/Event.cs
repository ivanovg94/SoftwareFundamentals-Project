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
        public Event()
        {

        }

        public Event(string organizerId, string eventName, string discription, DateTime date, string time, int categoryId, int cityId)
        {
            this.OrganizerId = organizerId;
            this.EventName = eventName;
            this.EventDescription = discription;
            this.EventDate = date;
            this.StartTime = time;
            this.CategoryId = categoryId;
            this.CityId = cityId;
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string EventName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }


        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string StartTime { get; set; }

        public string EventDescription { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }

        public virtual City City { get; set; }


        [ForeignKey("Organizer")]
        public string OrganizerId { get; set; }

        public ApplicationUser Organizer { get; set; }

        public byte[] EventPhoto { get; set; }

        public bool IsOrganizer(string name)
        {
            return this.Organizer.UserName.Equals(name);
        }



    }
}