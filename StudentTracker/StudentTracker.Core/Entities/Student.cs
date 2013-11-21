using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace StudentTracker.Core.Entities
{
    public class Student
    {
        [Key]
        public long StudentId { get; set; }

        [ScaffoldColumn(false)]
        public long? UserId { get; set; }

        [Required]
        public long CourseId { get; set; }

        public long? DepartmentId { get; set; }

        [Required]
        public long ClassId { get; set; }

        [Required]
        public int SectionId { get; set; }

        [Required]
        public string RollNo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public long InsertedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }

        [NotMapped]
        public SelectList CourseList { get; set; }

        [NotMapped]
        public SelectList ClassList { get; set; }

        [NotMapped]
        public SelectList DepartmentList { get; set; }

        [NotMapped]
        public SelectList SectionList { get; set; }

        [NotMapped]
        public string StudentName { get; set; }

        [NotMapped]
        public string CourseName { get; set; }

        [NotMapped]
        public string DepartmentName { get; set; }

        [NotMapped]
        public string SectionName { get; set; }

        [NotMapped]
        public string ClassName { get; set; }

        public string ImportId { get; set; }

        public string Remarks { get; set; }

        public string Email { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }

        [ForeignKey("ClassId")]
        public Class Class { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string ImportDateString { get { return InsertedOn.ToString("MM/dd/yyyy"); } }

    }
}
