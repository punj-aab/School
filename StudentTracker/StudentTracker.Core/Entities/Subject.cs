﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace StudentTracker.Core.Entities
{
    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public long SubjectId { get; set; }

        [Required]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Required]
        [Display(Name = "Description")]

        [DataType(DataType.MultilineText)]
        public string SubjectDescription { get; set; }

        [Required]
        public long CourseId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public long CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        public long ClassId { get; set; }

        [ScaffoldColumn(false)]
        [ForeignKey("ClassId")]
        public virtual Class Classes { get; set; }

        [NotMapped]
        public SelectList ClassList { get; set; }

        [NotMapped]
        public SelectList CourseList { get; set; }

        [NotMapped]
        public string CourseName { get; set; }

        [NotMapped]
        public string ClassName { get; set; }
        [NotMapped]
        public string InsertedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }
    }
}
