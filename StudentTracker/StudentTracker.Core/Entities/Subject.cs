using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public string SubjectId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string SubjectName { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string SubjectDescription { get; set; }

        [Required]
        [ForeignKey("CourseId")]
        public string CourseId { get; set; }
        [ScaffoldColumn(false)]
        public string InsertedOn { get; set; }
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public string ModifiedOn { get; set; }
        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
    }
}
