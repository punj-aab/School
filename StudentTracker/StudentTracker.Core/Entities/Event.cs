using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace StudentTracker.Core.Entities
{
    public class Event
    {
        [Key]
        public long EventId { get; set; }

        [Required]
        [Display(Name = "Event Type")]
        public long EventTypeId { get; set; }

        [Required]
        public string EventName { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Notification Type")]
        public int NotificationTypeId { get; set; }//enums

        [Required]
        [Display(Name = "Organization")]
        public long OrganizationId { get; set; }

        [Required]
        [Display(Name = "Course")]
        public long CourseId { get; set; }

        [Required]
        [Display(Name = "Class")]
        public long ClassId { get; set; }

        [Required]
        [Display(Name = "Section")]
        public int SectionId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public long InsertedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }

        [NotMapped]
        public string InsertedByName { get; set; }

        [NotMapped]
        public string UpdatedByName { get; set; }

        [NotMapped]
        public string EventTypeName { get; set; }

        [NotMapped]
        public string OrganizationName { get; set; }

        [NotMapped]
        public string CourseName { get; set; }

        [NotMapped]
        public string ClassName { get; set; }

        [NotMapped]
        public string SectionName { get; set; }

        [NotMapped]
        public string NotificationTypeName { get; set; }

        [NotMapped]
        public string TemplateName { get; set; }

        [ForeignKey("EventTypeId")]
        public virtual TemplateType EventTypes { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organizations { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Courses { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Classes { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section Sections { get; set; }

        [NotMapped]
        public SelectList OrganizationList { get; set; }
        [NotMapped]
        public SelectList CourseList { get; set; }
        [NotMapped]
        public SelectList ClassList { get; set; }
        [NotMapped]
        public SelectList SectionList { get; set; }

        [NotMapped]
        public SelectList NotificationTypeList { get; set; }
        [NotMapped]
        public SelectList EventTypeList { get; set; }

    }
}
