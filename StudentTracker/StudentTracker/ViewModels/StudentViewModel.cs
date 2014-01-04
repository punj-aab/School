using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;

namespace StudentTracker.ViewModels
{
    public class StudentViewModel : Student
    {
        public Profile Profile { get; set; }
        public Student Student { get; set; }
        public string GroupMembers { get; set; }
        public string StudentSubjects { get; set; }
        public string GroupIds { get; set; }
        public string SubjectIds { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string HomeTelephoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string InsertedByName { get; set; }
        public string ModifiedByName { get; set; }
        public string SubjectNames { get; set; }
    }
}