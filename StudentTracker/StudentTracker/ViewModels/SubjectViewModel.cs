using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;
using System.Web.Mvc;
namespace StudentTracker.ViewModels
{
    public class SubjectViewModel : Subject
    {
        public long SectionId { get; set; }
        public SelectList SectionList { get; set; }
        public List<Subject> UserSubjectList { get; set; }
        public List<Subject> ClassSubjects { get; set; }
    }
}