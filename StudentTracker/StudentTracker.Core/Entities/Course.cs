using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string CourseName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string CourseDescription { get; set; }

        [ScaffoldColumn(false)]
        public int OrganisationId { get; set; }
        [ScaffoldColumn(false)]
        public long CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }
        [ScaffoldColumn(false)]
        public long ModifiedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

       
    }
}
