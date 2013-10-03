using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.Repository
{
    public class SubjectRepository : StudentTracker.Core.Repository.CommonRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        //public List<Subject> GetSubjects()
        //{
        //    string sql = "select SubjectId,SubjectName,SubjectDescription,Subjects.CourseId,Courses.CourseName, Subjects.InsertedOn,Subjects.CreatedBy,Subjects.ModifiedBy,Subjects.ModifiedOn,Subjects.ModifiedBy,Subjects.ClassId,Classes.ClassName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName ";
        //    sql += "from Subjects join Courses on Subjects.CourseId=Courses.CourseId join Classes on Subjects.ClassId = Classes.ClassId join Users on Subjects.CreatedBy=Users.UserId left join Users as Users_1 on  Subjects.ModifiedBy=Users_1.UserId";
        //    List<Subject> modelList = db.Query<Subject>(sql).ToList();
        //    return modelList;
        //}
        //public Subject GetSubjects(long id)
        //{
        //    string sql = "select SubjectId,SubjectName,SubjectDescription,Subjects.CourseId,Courses.CourseName, Subjects.InsertedOn,Subjects.CreatedBy,Subjects.ModifiedBy,Subjects.ModifiedOn,Subjects.ModifiedBy,Subjects.ClassId,Classes.ClassName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName ";
        //    sql += "from Subjects join Courses on Subjects.CourseId=Courses.CourseId join Classes on Subjects.ClassId = Classes.ClassId join Users on Subjects.CreatedBy=Users.UserId left join Users as Users_1 on  Subjects.ModifiedBy=Users_1.UserId ";
        //    sql += "where SubjectId=@0";
        //    Subject objModel = db.Query<Subject>(sql, id).FirstOrDefault();
        //    return objModel;
        //}
        public bool Create(Subject objSubject)
        {
            int recAffected = 0;
            DBConnectionString.Subject Subject = new DBConnectionString.Subject();
            Subject.SubjectName = objSubject.SubjectName;
            Subject.SubjectDescription = objSubject.SubjectDescription;
            Subject.CourseId = objSubject.CourseId;
            Subject.ClassId = objSubject.ClassId;
            Subject.CreatedBy = objSubject.CreatedBy;
            Subject.InsertedOn = DateTime.Now;
            recAffected = Convert.ToInt32(Subject.Insert());
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool Update(Subject objSubject)
        {
            int recAffected = 0;
            DBConnectionString.Subject Subject = new DBConnectionString.Subject();
            Subject.SubjectId = objSubject.SubjectId;
            Subject.SubjectName = objSubject.SubjectName;
            Subject.SubjectDescription = objSubject.SubjectDescription;
            Subject.CourseId = objSubject.CourseId;
            Subject.ClassId = objSubject.ClassId;
            Subject.CreatedBy = objSubject.CreatedBy;
            Subject.InsertedOn = objSubject.InsertedOn;
            Subject.ModifiedBy = objSubject.ModifiedBy;
            Subject.ModifiedOn = DateTime.Now;
            recAffected = Subject.Update();
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool Delete(long id)
        {
            int recAffectd = 0;
            recAffectd = DBConnectionString.Subject.Delete(id);
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