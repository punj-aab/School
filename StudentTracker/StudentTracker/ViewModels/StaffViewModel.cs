using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.ViewModels
{
    public class StaffViewModel : Staff
    {
        public List<ListFields> ListFields { get; set; }
        public List<TeacherSubjects> TeacherSubjectsList { get; set; }
        public Profile Profile { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string HomeTelephoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public long UserId { get; set; }
        public SelectList StaffTypeList { get; set; }
        public long ProfileId { get; set; }
        public string MobileNumber { get; set; }
        public int Count { get; set; }
    }

    public class ListFields
    {
        public long Id { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public long DepartmentId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public int SectionId { get; set; }
        [Required]
        public long SubjectId { get; set; }
        public SelectList CourseList { get; set; }
        public SelectList DepartmentList { get; set; }
        public SelectList ClassList { get; set; }
        public SelectList SectionList { get; set; }
        public SelectList GroupList { get; set; }
        public SelectList SubjectList { get; set; }
    }
}