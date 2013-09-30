using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.Repository
{
    public class ClassRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        public List<Class> GetClasses()
        {
            string sql = "select Classes.ClassId,Classes.ClassName,Classes.Description,Classes.InsertedOn,Classes.ModifiedOn,Courses.CourseName,Organizations.OrganizationName,Classes.InsertedBy,Classes.ModifiedBy,Users.Username as InsertedByName,Users_1.Username as ModifiedByName ";
            sql += "from Classes join Organizations on Classes.OrganizationId=Organizations.OrganizationId join Courses on Classes.CourseId = Courses.CourseId join Users on Classes.InsertedBy=Users.UserId left join Users as Users_1 on  Classes.ModifiedBy=Users_1.UserId";
            List<Class> classList = db.Query<Class>(sql).ToList();
            return classList;
        }
        public Class GetClasses(long id)
        {
            string sql = "select Classes.ClassId,Classes.ClassName,Classes.Description,Classes.InsertedOn,Classes.ModifiedOn,Courses.CourseName,Organizations.OrganizationName,Classes.InsertedBy,Classes.ModifiedBy,Users.Username as InsertedByName,Users_1.Username as ModifiedByName ";
            sql += "from Classes join Organizations on Classes.OrganizationId=Organizations.OrganizationId join Courses on Classes.CourseId = Courses.CourseId join Users on Classes.InsertedBy=Users.UserId left join Users as Users_1 on  Classes.ModifiedBy=Users_1.UserId ";
            sql += "where ClassId=@0";
            Class objClass = db.Query<Class>(sql, id).SingleOrDefault();
            return objClass;
        }
        public bool CreateClass(Class objClass)
        {
            int recAffected = 0;
            DBConnectionString.Class Class = new DBConnectionString.Class();
            Class.ClassName = objClass.ClassName;
            Class.Description = objClass.Description;
            Class.OrganizationId = objClass.OrganizationId;
            Class.CourseId = objClass.CourseId;
            Class.ClassId = objClass.ClassId;
            Class.InsertedOn = DateTime.Now;
            Class.InsertedBy = objClass.InsertedBy;
            recAffected = Convert.ToInt32(Class.Insert());
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool UpdateClass(Class objClass)
        {
            int recAffected = 0;
            DBConnectionString.Class Class = new DBConnectionString.Class();
            Class.ClassId = objClass.ClassId;
            Class.ClassName = objClass.ClassName;
            Class.Description = objClass.Description;
            Class.OrganizationId = objClass.OrganizationId;
            Class.CourseId = objClass.CourseId;
            Class.ClassId = objClass.ClassId;
            Class.InsertedOn = objClass.InsertedOn;
            Class.InsertedBy = objClass.InsertedBy;
            Class.ModifiedBy = objClass.ModifiedBy;
            Class.ModifiedOn = DateTime.Now;
            recAffected = Class.Update();
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool DeleteClass(long id)
        {
            int recAffectd = 0;
            recAffectd = DBConnectionString.Class.Delete(id);
            if (recAffectd > 0)
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