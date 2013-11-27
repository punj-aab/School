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
    }
}