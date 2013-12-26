using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class TeacherSubjects
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public long DepartmentId { get; set; }
        public long ClassId { get; set; }
        public long SectionId { get; set; }
        public long SubjectId { get; set; }
        [NotMapped]
        public string Username { get; set; }
        [NotMapped]
        public string CourseName { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
        [NotMapped]
        public string ClassName { get; set; }
        [NotMapped]
        public string SectionName { get; set; }
        [NotMapped]
        public string SubjectName { get; set; }

    }
}
