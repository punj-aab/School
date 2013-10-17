using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;
namespace StudentTracker.Repository
{
    public class StudentRepository : StudentTracker.Core.Repository.CommonRepository
    {
        public List<Organization> Organizations()
        {
            return this.Get<Organization>("Select OrganizationId, OrganizationName from organizations");
        }
        public Organization Organizations(long id)
        {
            return this.SingleOrDefault<Organization>("Select OrganizationId, OrganizationName from organizations where OrganizationId = @id", id);
        }
        public List<Class> ClassByCourse(long courseId)
        {
            return this.Find<Class>("select * from Classes where CourseId = @id", courseId);
        }
        public List<Section> SectionByClass(long classId)
        {
            return this.Find<Section>("select * from sections where ClassId = @id", classId);
        }
        public List<Course> CourseByOrganization(long id)
        {
            return this.Find<Course>("select * from Courses where OrganisationId = @id", id);
        }
        public List<Department> DepartmenstByOrganization(long id)
        {
            return this.Find<Department>("select * from Departments where OrganizationId = @id", id);
        }
    }
}