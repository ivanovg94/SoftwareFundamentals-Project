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
        private ICollection<Tag> tags;

      
        public Event()
        {
            this.tags = new HashSet<Tag>();
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
            this.tags = new HashSet<Tag>();
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
        [RegularExpression(@"([01]?[0-9]|2[0-3]):[0-5][0-9]", ErrorMessage = "Invalid Time.")]
        public string StartTime { get; set; }

        public string EventDescription { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }



        public virtual Category Category { get; set; }   
        

        [ForeignKey("City")]
        public int CityId { get; set; }


       public virtual City City { get; set; }

        public virtual ICollection<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        [ForeignKey("Organizer")]
        public string OrganizerId { get; set; }

        public ApplicationUser Organizer { get; set; }

        public byte[] EventPhoto { get; set; }

        
        public int Attends { get; set; }

        public bool IsOrganizer(string name)
        {
            return this.Organizer.UserName.Equals(name);
        }



    }
}