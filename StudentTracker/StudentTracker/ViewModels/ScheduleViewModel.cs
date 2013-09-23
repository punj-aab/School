using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;
namespace StudentTracker.ViewModels
{
    public class ScheduleViewModel
    {
        public List<Course> CourseList { get; set; }
        public List<Department> DepartmentList { get; set; }
    }
}