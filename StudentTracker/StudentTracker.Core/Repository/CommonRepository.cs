using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentTracker.Core.Entities;
using System.Data;
using Dapper;
using StudentTracker.Core.Repositories;
namespace StudentTracker.Core.Repository
{
    public class CommonRepository : BaseRepository
    {
        public List<Organization> SelectOrganizations()
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "dbo.getOrganizations";
                return connection.Query<Organization>(storedProcedure, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Organization SelectOrganizations(long organizationId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "dbo.getOrganizations";
                return connection.Query<Organization>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public Course GetCourses(long courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getCourses";
                return connection.Query<Course>(storedProcedure, new { courseId = courseId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public List<Course> GetCourses(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getCourses";
                return connection.Query<Course>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<Class> GetClasses(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getClasses";
                return connection.Query<Class>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Class GetClasses(long classId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getClasses";
                return connection.Query<Class>(storedProcedure, new { classId = classId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<Subject> GetSubjects(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getSubjects";
                return connection.Query<Subject>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Subject GetSubjects(long subjectId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getSubjects";
                return connection.Query<Subject>(storedProcedure, new { subjectId = subjectId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<ClassRoom> GetClassRooms(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getClassRooms";
                return connection.Query<ClassRoom>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public ClassRoom GetClassRooms(long classRoomId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getClassRooms";
                return connection.Query<ClassRoom>(storedProcedure, new { classRoomId = classRoomId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<Department> GetDepartments(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getDepartments";
                return connection.Query<Department>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Department GetDepartments(long departmentId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "getDepartments";
                return connection.Query<Department>(storedProcedure, new { departmentId = departmentId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public bool CreateOrganization(Organization objOrganization)
        {
            var parameters = new
            {
                Address1 = objOrganization.Address1,
                Address2 = objOrganization.Address2,
                City = objOrganization.City,
                CountryId = objOrganization.CountryId,
                CreatedBy = objOrganization.CreatedBy,
                CreatedDate = DateTime.Now,
                Email = objOrganization.Email,
                OrganizationDesc = objOrganization.OrganizationDesc,
                OrganizationName = objOrganization.OrganizationName,
                OrganizationTypeId = objOrganization.OrganizationTypeId,
                Phone1 = objOrganization.Phone1,
                Phone2 = objOrganization.Phone2,
                RegisterationNumber = objOrganization.RegisterationNumber,
                StateId = objOrganization.StateId
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "addOrganization";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objOrganization.OrganizationId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //ADD NEW COURSE
        public bool CreateCourse(Course objCourse)
        {
            var parameters = new
            {
                CourseName          = objCourse.CourseName,
                CourseDescription = objCourse.CourseDescription,
                OrganisationId      = objCourse.OrganisationId,
                CreatedBy           = objCourse.CreatedBy,
                InsertedOn          = DateTime.Now,
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "addCourse";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objCourse.CourseId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //ADD NEW CLASS
        public bool CreateClass(Class objClass)
        {
            var parameters = new
            {
                ClassName           = objClass.ClassName,
                Description         = objClass.Description,
                OrganizationId      = objClass.OrganizationId,
                CourseId            = objClass.    CourseId,
                InsertedOn      = DateTime.Now,
                InsertedBy = objClass.InsertedBy
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "addClass";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objClass.ClassId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //ADD NEW DEPARTMENT
        public bool CreateDepartment(Department objDep)
        {
            var parameters = new
            {
                DepartmentName = objDep.DepartmentName,
                DepartmentDesc = objDep.DepartmentDesc,
                OrganizationId = objDep.OrganizationId,
                CreatedDate     = objDep.CreatedDate,
                CreatedBy       = objDep.CreatedBy
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "sp_AddDepartment";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objDep.DepartmentId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //ADD NEW SUBJECT
        public bool CreateSubject(Subject objSubject)
        {
            var parameters = new
            {
                SubjectName          = objSubject.SubjectName,
                SubjectDescription = objSubject.SubjectDescription,
                CourseId            = objSubject.CourseId,
                ClassId             = objSubject.ClassId,
                CreatedBy           = objSubject.CreatedBy,
                InsertedOn              = DateTime.Now
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "addSubject";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objSubject.SubjectId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //ADD NEW CLASSROOM
        public bool CreateClassRoom(ClassRoom objClass)
        {
            var parameters = new
            {
                Name        = objClass.Name,
                Description = objClass.Description,
                Location    = objClass.Location,
                InsertedOn  = DateTime.Now,
                InsertedBy  = objClass.InsertedBy,
                DepartmentId = objClass.DepartmentId
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "addClassRoom";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objClass.ClassRoomId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }

        //ADD NEW SECTION
        public bool CreateSection(Section objSection)
        {
            var parameters = new
            {
            SectionName         = objSection.SectionName,
            SectionDescription = objSection.SectionDescription,
            ClassId             = objSection.ClassId,
            CreatedBy           = objSection.CreatedBy,
            InsertedOn          = DateTime.Now
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "addSection";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objSection.SectionId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}



