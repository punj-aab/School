using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.Repository
{
    public class ClassRoomRepository : StudentTracker.Core.Repository.CommonRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        //public List<ClassRoom> GetClassRooms()
        //{
        //    string sql = "select ClassRoomId,Name,Description,Location,ClassRoom.InsertedOn,ClassRoom.ModifiedOn,ClassRoom.DepartmentId,Departments.DepartmentName, ClassRoom.InsertedBy,ClassRoom.ModifiedBy ,Users.Username as InsertedByName,Users_1.Username as ModifiedByName ";
        //    sql += "from ClassRoom join Departments on ClassRoom.ClassRoomId=Departments.DepartmentId join Users on ClassRoom.InsertedBy=Users.UserId left join Users as Users_1 on  ClassRoom.ModifiedBy=Users_1.UserId";
        //    List<ClassRoom> classList = db.Query<ClassRoom>(sql).ToList();
        //    return classList;
        //}
        //public ClassRoom GetClassRooms(long id)
        //{
        //    string sql = "select ClassRoomId,Name,Description,Location,ClassRoom.InsertedOn,ClassRoom.ModifiedOn,ClassRoom.DepartmentId,Departments.DepartmentName, ClassRoom.InsertedBy,ClassRoom.ModifiedBy ,Users.Username as InsertedByName,Users_1.Username as ModifiedByName ";
        //    sql += "from ClassRoom join Departments on ClassRoom.ClassRoomId=Departments.DepartmentId join Users on ClassRoom.InsertedBy=Users.UserId left join Users as Users_1 on  ClassRoom.ModifiedBy=Users_1.UserId ";
        //    sql += "where ClassRoomId=@0";
        //    ClassRoom objClass = db.Query<ClassRoom>(sql, id).FirstOrDefault();
        //    return objClass;
        //}
        public bool CreateClassRoom(ClassRoom objClass)
        {
            int recAffected = 0;
            DBConnectionString.ClassRoom Class = new DBConnectionString.ClassRoom();
            Class.Name = objClass.Name;
            Class.Description = objClass.Description;
            Class.Location = objClass.Location;
            Class.InsertedOn = DateTime.Now;
            Class.InsertedBy = objClass.InsertedBy;
            Class.DepartmentId = objClass.DepartmentId;
            recAffected = Convert.ToInt32(Class.Insert());
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool UpdateClassRoom(ClassRoom objClass)
        {
            int recAffected = 0;
            DBConnectionString.ClassRoom Class = new DBConnectionString.ClassRoom();
            Class.ClassRoomId = objClass.ClassRoomId;
            Class.Name = objClass.Name;
            Class.Description = objClass.Description;
            Class.Location = objClass.Location;
            Class.InsertedOn = objClass.InsertedOn;
            Class.InsertedBy = objClass.InsertedBy;
            Class.ModifiedBy = objClass.ModifiedBy;
            Class.ModifiedOn = DateTime.Now;
            Class.DepartmentId = objClass.DepartmentId;
            recAffected = Class.Update();
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool DeleteClassRoom(long id)
        {
            int recAffectd = 0;
            recAffectd = DBConnectionString.ClassRoom.Delete(id);
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