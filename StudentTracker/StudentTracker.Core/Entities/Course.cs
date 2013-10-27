using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace StudentTracker.Core.Entities
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CourseId { get; set; }
        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string CourseDescription { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Organization")]
        public int OrganisationId { get; set; }
        [ScaffoldColumn(false)]
        public long? CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? InsertedOn { get; set; }
        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }


        [ScaffoldColumn(false)]
        public virtual Organization Organization { get; set; }

        [NotMapped]
        public SelectList OrganizationList { get; set; }

        [NotMapped]
        public string OrganizationName { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }
    }
}
