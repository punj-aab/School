using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class OrganizationCoursesRoot
    {
        public long OrganizationId { get; set; }
        public long? CourseId { get; set; }
        public long? ClassId { get; set; }
        public long? SubjectId { get; set; }
        public int? SectionId { get; set; }
        public long? DepartmentId { get; set; }
        public long? ClassRoomId { get; set; }
    }
}
