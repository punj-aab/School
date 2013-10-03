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

        
    }
}
