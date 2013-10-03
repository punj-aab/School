using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Repository;
namespace StudentTracker.Repository
{
    public class CourseRepository : StudentTracker.Core.Repository.CommonRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        //public List<Course> GetCourses()
        //{
        //    string sql = "select CourseId,CourseName,CourseDescription,Courses.CreatedBy,Courses.InsertedOn,Courses.ModifiedBy,ModifiedOn,Courses.OrganisationId,Organizations.OrganizationName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName ";
        //    sql += "from Courses join Organizations on Courses.OrganisationId=Organizations.OrganizationId join Users on Courses.CreatedBy=Users.UserId left join Users as Users_1 on  Courses.ModifiedBy=Users_1.UserId";
        //    List<Course> modelList = db.Query<Course>(sql).ToList();
        //    return modelList;
        //}
        //public Course GetCourses(long id)
        //{
        //    string sql = "select CourseId,CourseName,CourseDescription,Courses.CreatedBy,Courses.InsertedOn,Courses.ModifiedBy,ModifiedOn,Courses.OrganisationId,Organizations.OrganizationName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName ";
        //    sql += "from Courses join Organizations on Courses.OrganisationId=Organizations.OrganizationId join Users on Courses.CreatedBy=Users.UserId left join Users as Users_1 on  Courses.ModifiedBy=Users_1.UserId ";
        //    sql += "where CourseId=@0";
        //    Course objModel = db.Query<Course>(sql, id).FirstOrDefault();
        //    return objModel;
        //}
        public bool Create(Course objCourse)
        {
            int recAffected = 0;
            DBConnectionString.Course Course = new DBConnectionString.Course();
            Course.CourseName = objCourse.CourseName;
            Course.CourseDescription = objCourse.CourseDescription;
            Course.OrganisationId = objCourse.OrganisationId;
            Course.CreatedBy = objCourse.CreatedBy;
            Course.InsertedOn = DateTime.Now;
            recAffected = Convert.ToInt32(Course.Insert());
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool Update(Course objCourse)
        {
            int recAffected = 0;
            DBConnectionString.Course Course = new DBConnectionString.Course();
            Course.CourseId = objCourse.CourseId;
            Course.CourseName = objCourse.CourseName;
            Course.CourseDescription = objCourse.CourseDescription;
            Course.OrganisationId = objCourse.OrganisationId;
            Course.CreatedBy = objCourse.CreatedBy;
            Course.InsertedOn = objCourse.InsertedOn;
            Course.ModifiedOn = DateTime.Now;
            Course.ModifiedBy = objCourse.ModifiedBy;

            recAffected = Course.Update();
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool Delete(long id)
        {
            int recAffected = 0;
            DBConnectionString.Course objCourse = db.SingleOrDefault<DBConnectionString.Course>("select * from Courses where CourseId=@0", id);
            recAffected = objCourse.Delete();
            if (recAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}