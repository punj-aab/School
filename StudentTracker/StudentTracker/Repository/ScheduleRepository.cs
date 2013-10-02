using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
namespace StudentTracker.Models
{
    public class ScheduleRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        public List<Schedule> GetSchdeule()
        {
            string sql = "select Schedule.ScheduleId, Schedule.ScheduleName, Schedule.Description, Schedule.InsertedOn, Schedule.StartTime, Schedule.EndTime, Schedule.DayIds,Organizations.OrganizationId, Organizations.OrganizationName,Courses.CourseId, Courses.CourseName,Classes.ClassId, Classes.ClassName,Subjects.SubjectId, Subjects.SubjectName,Departments.DepartmentId, Departments.DepartmentName,ClassRoom.ClassRoomId, ClassRoom.Name as ClassRoomName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName, Schedule.ModifiedOn, Schedule.InsertedBy, Schedule.ModifiedBy ";
            sql += "from Schedule join Organizations on Schedule.OrganizationId=Organizations.OrganizationId join Courses on Schedule.CourseId = Courses.CourseId join Classes on Schedule.ClassId=Classes.ClassId join Subjects on Schedule.SubjectId=Subjects.SubjectId join Departments on Schedule.DepartmentId=Departments.DepartmentId join ClassRoom on Schedule.ClassRoomId=ClassRoom.ClassRoomId join Users on Schedule.InsertedBy=Users.UserId left join Users as Users_1 on  Schedule.ModifiedBy=Users_1.UserId";
            List<Schedule> objSchedule = db.Query<Schedule>(sql).ToList();
            return objSchedule;
        }
        public Schedule GetSchedule(long id)
        {
            string sql = "select Schedule.ScheduleId, Schedule.ScheduleName, Schedule.Description, Schedule.InsertedOn, Schedule.StartTime, Schedule.EndTime, Schedule.DayIds,Organizations.OrganizationId, Organizations.OrganizationName,Courses.CourseId, Courses.CourseName,Classes.ClassId, Classes.ClassName,Subjects.SubjectId, Subjects.SubjectName,Departments.DepartmentId, Departments.DepartmentName,ClassRoom.ClassRoomId, ClassRoom.Name as ClassRoomName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName, Schedule.ModifiedOn, Schedule.InsertedBy, Schedule.ModifiedBy ";
            sql += "from Schedule join Organizations on Schedule.OrganizationId=Organizations.OrganizationId join Courses on Schedule.CourseId = Courses.CourseId join Classes on Schedule.ClassId=Classes.ClassId join Subjects on Schedule.SubjectId=Subjects.SubjectId join Departments on Schedule.DepartmentId=Departments.DepartmentId join ClassRoom on Schedule.ClassRoomId=ClassRoom.ClassRoomId join Users on Schedule.InsertedBy=Users.UserId left join Users as Users_1 on  Schedule.ModifiedBy=Users_1.UserId where ScheduleId=@0";
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
            schedule.ModifiedOn = DateTime.Now;
            schedule.ModifiedBy = objSchedule.ModifiedBy;
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

        public Schedule LoadScheduleLists(string userRole, long createdBy = -1, long scheduleId = -1)
        {
            Schedule schedule = null;
            if (scheduleId != -1)
            {
                schedule = this.GetSchedule(scheduleId);
            }
            if (schedule == null)
            {
                schedule = new Schedule();
            }
            //list class objects
            List<Organization> listOrganizations = new List<Organization>();
            List<Course> courseList = new List<Course>();
            List<Class> classList = new List<Class>();
            List<Subject> subjectList = new List<Subject>();
            List<Department> departmentList = new List<Department>();
            List<ClassRoom> classRoomList = new List<ClassRoom>();

            //class objects
            Organization objOrganization = null;
            Course objCourse = null;
            Class objClass = null;
            Subject objSubject = null;
            Department objDep = null;
            ClassRoom objRoom = null;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["StudentContext"].ConnectionString);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            using (SqlCommand cmd = new SqlCommand("getSchedules", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userRole", SqlDbType.VarChar, 50).Value = userRole;
                cmd.Parameters.Add("@organizationId", SqlDbType.BigInt).Value = schedule.OrganizationId;
                cmd.Parameters.Add("@courseId", SqlDbType.BigInt).Value = schedule.CourseId;
                cmd.Parameters.Add("@departmentId", SqlDbType.BigInt).Value = schedule.DepartmentId;
                cmd.Parameters.Add("@classId", SqlDbType.BigInt).Value = schedule.ClassId;
                cmd.Parameters.Add("@createdBy", SqlDbType.BigInt, 50).Value = userName;

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adp.Fill(dataSet);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    objOrganization = new Organization();
                    objOrganization.OrganizationId = Convert.ToInt64(row["OrganizationId"]);
                    objOrganization.OrganizationName = Convert.ToString(row["OrganizationName"]);
                    listOrganizations.Add(objOrganization);
                    if (userRole != "SiteAdmin")
                    {
                        schedule.OrganizationName = objOrganization.OrganizationName;
                    }
                }

                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    objCourse = new Course();
                    objCourse.CourseId = Convert.ToInt64(row["CourseId"]);
                    objCourse.CourseName = Convert.ToString(row["CourseName"]);
                    courseList.Add(objCourse);
                }

                foreach (DataRow row in dataSet.Tables[2].Rows)
                {
                    objClass = new Class();
                    objClass.ClassId = Convert.ToInt64(row["ClassId"]);
                    objClass.ClassName = Convert.ToString(row["ClassName"]);
                    classList.Add(objClass);
                }

                foreach (DataRow row in dataSet.Tables[3].Rows)
                {
                    objSubject = new Subject();
                    objSubject.SubjectId = Convert.ToInt64(row["SubjectId"]);
                    objSubject.SubjectName = Convert.ToString(row["SubjectName"]);
                    subjectList.Add(objSubject);
                }

                foreach (DataRow row in dataSet.Tables[4].Rows)
                {
                    objDep = new Department();
                    objDep.DepartmentId = Convert.ToInt64(row["DepartmentId"]);
                    objDep.DepartmentName = Convert.ToString(row["DepartmentName"]);
                    departmentList.Add(objDep);
                }

                foreach (DataRow row in dataSet.Tables[5].Rows)
                {
                    objRoom = new ClassRoom();
                    objRoom.ClassRoomId = Convert.ToInt64(row["ClassRoomId"]);
                    objRoom.Name = Convert.ToString(row["Name"]);
                    classRoomList.Add(objRoom);
                }


            }
            schedule.OrganizationList = new SelectList(listOrganizations, "OrganizationId", "OrganizationName", schedule.OrganizationId);
            schedule.CourseList = new SelectList(courseList, "CourseId", "CourseName", schedule.CourseId);
            schedule.ClassList = new SelectList(classList, "ClassId", "ClassName", schedule.ClassId);
            schedule.SubjectList = new SelectList(subjectList, "SubjectId", "SubjectName", schedule.SubjectId);
            schedule.DepartmentList = new SelectList(departmentList, "DepartmentId", "DepartmentName", schedule.DepartmentId);
            schedule.ClassRoomList = new SelectList(classRoomList, "ClassRoomId", "Name", schedule.ClassRoomId);
            List<SelectListItem> daysList = Enum.GetValues(typeof(StudentTracker.Core.Utilities.Days)).Cast<StudentTracker.Core.Utilities.Days>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            schedule.DayList = new SelectList(daysList, "Value", "Text");

            return schedule;
        }
    }
}