using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Mvc;
using StudentTracker.ViewModels;
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
        public List<SecurityQuestion> SecurityQuestions()
        {
            return this.Get<SecurityQuestion>("select * from SecurityQuestion");
        }

        public List<TemplateType> TemplateTypes()
        {
            return this.Get<TemplateType>("select * from TemplateType");
        }
        public List<Template> Templates()
        {
            return this.Get<Template>("select * from Template");
        }

        public List<FormattingField> FormattingFields(long templateTypeId)
        {
            return this.Find<FormattingField>("select * from FormattingField where TemplateTypeId = @id", templateTypeId);
        }

        public List<User> Users(long organizationId)
        {
            return this.Find<User>("select * from Users where OrgainzationId = @id", organizationId);
        }
        public List<UserGroup> UserGroupsByGroup(long groupId)
        {
            return this.Find<UserGroup>("select * from UserGroup where GroupId = @id", groupId);
        }

        public bool CreateNewStudent(StudentViewModel objViewModel)
        {
            Student objStudent = new Student();
            objStudent.UserId = objViewModel.UserId;
            objStudent.CourseId = objViewModel.CourseId;
            objStudent.DepartmentId = objViewModel.DepartmentId;
            objStudent.ClassId = objViewModel.ClassId;
            objStudent.SectionId = objViewModel.SectionId;
            objStudent.RollNo = objViewModel.RollNo;
            objStudent.InsertedOn = DateTime.Now;
            objStudent.InsertedBy = objViewModel.InsertedBy;
            objStudent.Email = objViewModel.Email;
            objStudent.OrganizationId = objViewModel.OrganizationId;
            objStudent.FullName = objViewModel.Profile.FirstName + " " + objViewModel.Profile.LastName;
            if (this.CreateStudent(objStudent))
            {
                return true;
            }
            return false;
        }

        public Staff LoadStaffLists(string userRole, long organizationId = -1, long scheduleId = -1)
        {
            Staff objStaff = null;
            if (scheduleId != -1)
            {
                //objStaff = this.GetSchedule(scheduleId);
            }
            if (objStaff == null)
            {
                objStaff = new Staff();
            }
            //list class objects
            List<Organization> listOrganizations = new List<Organization>();
            List<Course> courseList = new List<Course>();
            List<Class> classList = new List<Class>();
            List<Subject> subjectList = new List<Subject>();
            List<Department> departmentList = new List<Department>();
            List<Section> sectionList = new List<Section>();

            //class objects
            Organization objOrganization = null;
            Course objCourse = null;
            Class objClass = null;
            Subject objSubject = null;
            Department objDep = null;
            Section objSection = null;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["StudentContext"].ConnectionString);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            using (SqlCommand cmd = new SqlCommand("usp_getSchedules", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userRole", SqlDbType.VarChar, 50).Value = userRole;
                cmd.Parameters.Add("@organizationId", SqlDbType.BigInt).Value = objStaff.OrganizationId;
                cmd.Parameters.Add("@courseId", SqlDbType.BigInt).Value = objStaff.CourseId;
                cmd.Parameters.Add("@departmentId", SqlDbType.BigInt).Value = objStaff.DepartmentId;
                cmd.Parameters.Add("@classId", SqlDbType.BigInt).Value = objStaff.ClassId;
                cmd.Parameters.Add("@createdByOrganization", SqlDbType.BigInt, 50).Value = organizationId;

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
                        objStaff.OrganizationName = objOrganization.OrganizationName;
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

                foreach (DataRow row in dataSet.Tables[6].Rows)
                {
                    objSection = new Section();
                    objSection.SectionId = Convert.ToInt32(row["SectionId"]);
                    objSection.SectionName = Convert.ToString(row["SectionName"]);
                    sectionList.Add(objSection);
                }


            }
            objStaff.OrganizationList = new SelectList(listOrganizations, "OrganizationId", "OrganizationName", objStaff.OrganizationId);
            objStaff.CourseList = new SelectList(courseList, "CourseId", "CourseName", objStaff.CourseId);
            objStaff.ClassList = new SelectList(classList, "ClassId", "ClassName", objStaff.ClassId);
            objStaff.SubjectList = new SelectList(subjectList, "SubjectId", "SubjectName", objStaff.SubjectId);
            objStaff.DepartmentList = new SelectList(departmentList, "DepartmentId", "DepartmentName", objStaff.DepartmentId);
            objStaff.SectionList = new SelectList(sectionList, "SectionId", "SectionName", objStaff.SectionId);
            return objStaff;
        }

        public List<Group> GroupsByOrganization(long organizationId)
        {
            return this.Find<Group>("select * from Group where OrganizationId= = @id", organizationId);
        }
        public List<Subject> SubjectByClass(long classId)
        {
            return this.Find<Subject>("select * from Subjects where classId = @id", classId);
        }

        public StudentViewModel GetStudents(long organizationId, long studentId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                Dictionary<string, long> parameters = new Dictionary<string, long>();
                parameters["OrganizationId"] = organizationId;
                parameters["StudentId"] = studentId;
                const string storedProcedure = "usp_GetStudents";
                return this.GetStudents<StudentViewModel>(storedProcedure, organizationId, studentId);
            }
        }

        public bool UpdateStudent(StudentViewModel objViewModel)
        {
            PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");

            DBConnectionString.User user = DBConnectionString.User.SingleOrDefault(objViewModel.UserId);
            DBConnectionString.Student student = DBConnectionString.Student.SingleOrDefault(objViewModel.StudentId);
            DBConnectionString.Profile profile = db.Query<DBConnectionString.Profile>("Select * from  profile where UserId = @0", student.UserId).SingleOrDefault();
            db.BeginTransaction();
            try
            {
                user.FirstName = objViewModel.FirstName;
                user.LastName = objViewModel.LastName;
                user.Update();

                student.ClassId = objViewModel.ClassId;
                student.CourseId = objViewModel.CourseId;
                student.DepartmentId = objViewModel.DepartmentId;
                student.Email = objViewModel.Email;
                student.FullName = objViewModel.FirstName + " " + objViewModel.LastName;
                student.ModifiedBy = objViewModel.ModifiedBy;
                student.ModifiedOn = objViewModel.ModifiedOn;
                student.SectionId = objViewModel.SectionId;
                student.Update();

                profile.Title = objViewModel.Title;
                profile.DateOfBirth = objViewModel.DateOfBirth;
                profile.Phone1 = objViewModel.Phone1;
                profile.HomeTelephoneNumber = objViewModel.HomeTelephoneNumber;
                profile.EmailAddress1 = objViewModel.Email;
                profile.Update();
                db.CompleteTransaction();
                return true;
            }
            catch
            {
                db.AbortTransaction();
                return false;
            }
        }

        public List<string> GetGroupOfUser(long userId)
        {
            string query = "select G.GroupName from UserGroup as UG " +
                            "inner join [Group] as G on UG.GroupId = G.GroupId " +
                            "inner join Users as U on UG.UserId = U.UserId " +
                            "where UG.UserId = @id";
            return this.Find<string>(query, userId);
        }
    }
}