using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
namespace StudentTracker.Models
{
    public class ScheduleRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        public List<Schedule> GetSchdeule()
        {
            string sql = "select Schedule.ScheduleId, Schedule.ScheduleName, Schedule.Description, Schedule.InsertedOn, Schedule.StartTime, Schedule.EndTime, Schedule.DayIds, Organizations.OrganizationName,Courses.CourseName,Classes.ClassName, Subjects.SubjectName, Departments.DepartmentName, ClassRoom.Name as ClassRoomName  from Schedule ";
            sql += "join Organizations on Schedule.OrganizationId=Organizations.OrganizationId join Courses on Schedule.CourseId = Courses.CourseId join Classes on Schedule.ClassId=Classes.ClassId join Subjects on Schedule.SubjectId=Subjects.SubjectId join Departments on Schedule.DepartmentId=Departments.DepartmentId join ClassRoom on Schedule.ClassRoomId=ClassRoom.ClassRoomId";
            List<Schedule> objSchedule = db.Query<Schedule>(sql).ToList();
            return objSchedule;
        }
        public Schedule GetSchedule(long id)
        {
            string sql = "select Schedule.ScheduleId, Schedule.ScheduleName, Schedule.Description, Schedule.InsertedOn, Schedule.StartTime, Schedule.EndTime, Schedule.DayIds, Organizations.OrganizationName,Courses.CourseName,Classes.ClassName, Subjects.SubjectName, Departments.DepartmentName, ClassRoom.Name as ClassRoomName  from Schedule ";
            sql += "join Organizations on Schedule.OrganizationId=Organizations.OrganizationId join Courses on Schedule.CourseId = Courses.CourseId join Classes on Schedule.ClassId=Classes.ClassId join Subjects on Schedule.SubjectId=Subjects.SubjectId join Departments on Schedule.DepartmentId=Departments.DepartmentId join ClassRoom on Schedule.ClassRoomId=ClassRoom.ClassRoomId where ScheduleId=@0";
            Schedule objSchedule = db.Query<Schedule>(sql, id).SingleOrDefault();
            return objSchedule;
        }
        public bool CreateSchedule(Schedule objSchedule)
        {
            int recAffected = 0;
            DBConnectionString.Schedule schedule = new DBConnectionString.Schedule();
            schedule.ScheduleName = objSchedule.ScheduleName;
            schedule.Description = objSchedule.Description;
            schedule.OrganizationId = objSchedule.OrganizationId;
            schedule.CourseId = objSchedule.CourseId;
            schedule.ClassId = objSchedule.ClassId;
            schedule.SubjectId = objSchedule.SubjectId;
            schedule.DepartmentId = objSchedule.DepartmentId;
            schedule.ClassRoomId = objSchedule.ClassRoomId;
            schedule.InsertedOn = DateTime.Now;
            schedule.InsertedBy = objSchedule.InsertedBy;
            schedule.StartTime = objSchedule.StartTime;
            schedule.EndTime = objSchedule.EndTime;
            schedule.DayIds = objSchedule.DayIds;
            recAffected = Convert.ToInt32(schedule.Insert());
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool UpdateSchedule(Schedule objSchedule)
        {
            int recAffected = 0;
            DBConnectionString.Schedule schedule = new DBConnectionString.Schedule();
            schedule.ScheduleId = objSchedule.ScheduleId;
            schedule.ScheduleName = objSchedule.ScheduleName;
            schedule.Description = objSchedule.Description;
            schedule.OrganizationId = objSchedule.OrganizationId;
            schedule.CourseId = objSchedule.CourseId;
            schedule.ClassId = objSchedule.ClassId;
            schedule.SubjectId = objSchedule.SubjectId;
            schedule.DepartmentId = objSchedule.DepartmentId;
            schedule.ClassRoomId = objSchedule.ClassRoomId;
            schedule.InsertedOn = objSchedule.InsertedOn;
            schedule.InsertedBy = objSchedule.InsertedBy;
            schedule.StartTime = objSchedule.StartTime;
            schedule.EndTime = objSchedule.EndTime;
            schedule.DayIds = objSchedule.DayIds;
            recAffected = schedule.Update();
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool DeleteSchedule(long id)
        {
            int recAffectd = 0;
            DBConnectionString.Schedule schedule = db.Query<DBConnectionString.Schedule>("select * from schedule where ScheduleId=@0", id).SingleOrDefault();
            recAffectd = schedule.Delete();
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