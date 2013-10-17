using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.ViewModels
{
    public class OrganizationCursesRootViewModel
    {
        public long OrganizationId { get; set; }
        public long CourseId { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
        public long SectionId { get; set; }
        public long DepartmentId { get; set; }
        public long ClassRoomId { get; set; }
    }
}