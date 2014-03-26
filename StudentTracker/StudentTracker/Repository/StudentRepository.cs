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
using StudentTracker.Core.Utilities;
using System.Text;

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
            return this.Find<User>("select * from Users where OrganizationId = @id", organizationId);
        }
        public List<UserGroup> UserGroupsByGroup(long groupId)
        {
            return this.Find<UserGroup>("select * from UserGroup where GroupId = @id", groupId);
        }

        public long CreateNewStudent(StudentViewModel objViewModel)
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
            return objStudent.StudentId = this.CreateStudent(objStudent);
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

        //public StudentViewModel GetStudents(long organizationId, long studentId)
        //{
        //    using (IDbConnection connection = OpenConnection())
        //    {
        //        Dictionary<string, long> parameters = new Dictionary<string, long>();
        //        parameters["OrganizationId"] = organizationId;
        //        parameters["StudentId"] = studentId;
        //        const string storedProcedure = "usp_GetStudents";
        //        return this.GetStudents<StudentViewModel>(storedProcedure, organizationId, studentId);
        //    }
        //}

        public StudentViewModel GetStudents(long organizationId, long studentId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var parameters = new
                    {
                        OrganizationId = organizationId,
                        StudentId = studentId
                    };
                const string storedProcedure = "usp_GetStudents";
                return this.ExecuteSP<StudentViewModel>(storedProcedure, parameters);
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
                if (user != null)
                {
                    user.FirstName = objViewModel.FirstName;
                    user.LastName = objViewModel.LastName;
                    user.Update();
                }

                if (student != null)
                {
                    student.ClassId = objViewModel.ClassId;
                    student.CourseId = objViewModel.CourseId;
                    student.DepartmentId = objViewModel.DepartmentId;
                    student.Email = objViewModel.Email;
                    student.FullName = objViewModel.FirstName + " " + objViewModel.LastName;
                    student.ModifiedBy = objViewModel.ModifiedBy;
                    student.ModifiedOn = objViewModel.ModifiedOn;
                    student.SectionId = objViewModel.SectionId;
                    student.Update();
                }

                if (profile != null)
                {
                    profile.Title = objViewModel.Title;
                    profile.DateOfBirth = objViewModel.DateOfBirth;
                    profile.MobileNumber = objViewModel.MobileNumber;
                    profile.HomeTelephoneNumber = objViewModel.HomeTelephoneNumber;
                    profile.EmailAddress1 = objViewModel.Email;
                    profile.Update();
                }

                //var subjectIdArray = objViewModel.SubjectIds.Split(',');
                //foreach (var subjectId in subjectIdArray)
                //{
                //    DBConnectionString.UserSubject userSubject = db.Query<DBConnectionString.UserSubject>("select * from UserSubjects where UserId = @0 and SubjectId = @1", objViewModel.UserId, subjectId).SingleOrDefault();
                //    if (userSubject == null)
                //    {
                //        userSubject.UserId = objViewModel.UserId.Value;
                //        userSubject.SubjectId = Convert.ToInt64(subjectId);
                //        userSubject.InsertedOn = DateTime.Now;
                //        userSubject.InsertedBy = objViewModel.InsertedBy;
                //        userSubject.Update();
                //    }
                //}

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

        public string CreateToken(StudentViewModel objViewModel, long userId, long organizationId)
        {
            int recordAffected = 0;
            DBConnectionString.RegistrationToken registrationToken = new DBConnectionString.RegistrationToken();
            registrationToken.OrganizationId = organizationId;
            registrationToken.CourseId = (int)objViewModel.CourseId;
            registrationToken.ClassId = objViewModel.ClassId;
            registrationToken.SectionId = objViewModel.SectionId;
            if (objViewModel.DepartmentId != null)
            {
                registrationToken.DepartmentId = (int)objViewModel.DepartmentId;
            }
            registrationToken.Token = UserStatistics.GenerateToken();
            registrationToken.CreatedBy = userId;
            registrationToken.InsertedOn = DateTime.Now;
            registrationToken.RoleId = (int)UserRoles.Student;
            recordAffected = Convert.ToInt32(registrationToken.Insert());
            if (recordAffected > 0)
            {
                return registrationToken.Token;
            }
            return string.Empty;
        }

        public long CreateNewStaff(StaffViewModel objViewModel)
        {
            DBConnectionString.Staff objStaff = new DBConnectionString.Staff();
            objStaff.UserId = objViewModel.UserId;
            objStaff.Title = objViewModel.Title;
            objStaff.InsertedOn = DateTime.Now;
            objStaff.InsertedBy = objViewModel.InsertedBy;
            objStaff.Title = objViewModel.Title;
            objStaff.Email = objViewModel.Email;
            objStaff.StaffTypeId = objViewModel.StaffTypeId;
            objStaff.OrganizationId = objViewModel.OrganizationId;
            objStaff.FullName = objViewModel.Profile.FirstName + " " + objViewModel.Profile.LastName;
            objStaff.Insert();
            return objStaff.StaffId;
        }

        public void AssignSubjectToTeacher(StaffViewModel objVM)
        {
            foreach (var data in objVM.ListFields)
            {
                DBConnectionString.TeacherSubject teacherSubject = new DBConnectionString.TeacherSubject();
                teacherSubject.CourseId = data.CourseId;
                teacherSubject.ClassId = data.ClassId;
                teacherSubject.SectionId = data.SectionId;
                teacherSubject.SubjectId = data.SubjectId;
                teacherSubject.DepartmentId = data.DepartmentId;
                teacherSubject.UserId = objVM.UserId;
                teacherSubject.StaffId = objVM.StaffId;
                teacherSubject.Insert();
            }
        }

        public StaffViewModel GetStaff(long staffId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetStaff";
                var parameters = new
                {
                    StaffId = staffId
                };
                return this.ExecuteSP<StaffViewModel>(storedProcedure, parameters);
            }
        }

        public bool DeleteStaff(long staffId)
        {
            try
            {
                PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
                var staff = DBConnectionString.Staff.SingleOrDefault(staffId);
                db.Execute("delete from RoleUser where User_UserId = @0", staff.UserId);
                db.Execute("delete from [Profile] where UserId = @0", staff.UserId);
                db.Execute("delete from TeacherSubjects where UserId = @0", staff.UserId);
                db.Execute("delete from Staff where StaffId = @0", staffId);
                db.Execute("delete from Users where UserId = @0", staff.UserId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public SubjectViewModel GetCourseClassIds(long userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "select top(1) CourseId, ClassId from Subjects as S " +
                                     "join UserSubjects as US on S.SubjectId = US.SubjectId " +
                                     "where US.UserId = @id";
                return this.SingleOrDefault<SubjectViewModel>(query, userId);
            }
        }

        public bool UpdateUserProfile(Profile objModel)
        {
            PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
            DBConnectionString.User user = DBConnectionString.User.SingleOrDefault(objModel.UserId);
            DBConnectionString.Profile profile = db.Query<DBConnectionString.Profile>("Select * from  profile where UserId = @0", objModel.UserId).SingleOrDefault();
            try
            {
                if (user != null)
                {
                    user.FirstName = objModel.FirstName;
                    user.LastName = objModel.LastName;
                    user.Update();
                }

                if (profile != null)
                {
                    profile.Title = objModel.Title;
                    profile.DateOfBirth = objModel.DateOfBirth;
                    profile.MobileNumber = objModel.MobileNumber;
                    profile.HomeTelephoneNumber = objModel.HomeTelephoneNumber;
                    profile.EmailAddress1 = objModel.EmailAddress1;
                    profile.EmailAddress2 = objModel.EmailAddress2;
                    profile.Phone1 = objModel.Phone1;
                    profile.Phone2 = objModel.Phone2;
                    profile.Address1 = objModel.Address1;
                    profile.Address2 = objModel.Address2;
                    profile.ModifiedOn = objModel.ModifiedOn;
                    profile.Update();
                }
                else
                {
                    profile = new DBConnectionString.Profile();
                    profile.UserId = objModel.UserId;
                    profile.Title = objModel.Title;
                    profile.DateOfBirth = objModel.DateOfBirth;
                    profile.MobileNumber = objModel.MobileNumber;
                    profile.HomeTelephoneNumber = objModel.HomeTelephoneNumber;
                    profile.EmailAddress1 = objModel.EmailAddress1;
                    profile.EmailAddress2 = objModel.EmailAddress2;
                    profile.Phone1 = objModel.Phone1;
                    profile.Phone2 = objModel.Phone2;
                    profile.Address1 = objModel.Address1;
                    profile.Address2 = objModel.Address2;
                    profile.InsertedOn = DateTime.Now;
                    profile.SecurityAnswer = "None";
                    profile.Insert();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateProfileImage(Profile objModel)
        {
            PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
            DBConnectionString.User user = DBConnectionString.User.SingleOrDefault(objModel.UserId);
            DBConnectionString.Profile profile = db.Query<DBConnectionString.Profile>("Select * from  profile where UserId = @0", objModel.UserId).SingleOrDefault();
            try
            {
                if (profile != null)
                {
                    profile.ProfileImageUrl = objModel.ProfileImageUrl;
                    profile.Update();
                }
                else
                {
                    profile.UserId = objModel.UserId;
                    profile.ProfileImageUrl = objModel.ProfileImageUrl;
                    profile.Insert();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ClassRoom> ClassRoomByDepartment(long userId)
        {
            string query = "select * from ClassRoom where DepartmentId = @id";
            return this.Find<ClassRoom>(query, userId);
        }

        public string GetUserEmail(long userId)
        {
            string query = "select Email from Users where UserId=@id";
            return this.SingleOrDefault<string>(query, userId);
        }

        public bool SetStaffPermissions(StaffViewModel objVM)
        {
            DBConnectionString.StaffPermission staffPermission = new DBConnectionString.StaffPermission();
            //Basic
            staffPermission.SendClassEmails = true;
            staffPermission.ViewClassEmails = true;
            staffPermission.SendClassEletters = true;
            staffPermission.ViewClassEletters = true;
            staffPermission.SendClassSms = true;
            staffPermission.ViewClassSms = true;
            staffPermission.CreateManageCalendarEvents = true;
            staffPermission.CreateManageCourseWork = true;
            staffPermission.ViewTimeTable = true;
            staffPermission.CreateManageAttendance = true;
            staffPermission.SelfPayments = true;
            staffPermission.CreateManageGroups = true;

            //Communication
            staffPermission.SendEmail = objVM.Communication.SendEmail;
            staffPermission.ViewEmail = objVM.Communication.ViewEmail;
            staffPermission.SendEletter = objVM.Communication.SendEletter;
            staffPermission.ViewEletter = objVM.Communication.ViewEletter;
            staffPermission.SendSms = objVM.Communication.SendSmsMessage;
            staffPermission.ViewSms = objVM.Communication.ViewSmsMessage;
            staffPermission.AbsentReporting = objVM.Communication.AbsentReporting;
            staffPermission.PrintLetters = objVM.Communication.PrintLetters;
            staffPermission.TopUpSMSBalance = objVM.Communication.TopUpSMSBalance;
            ///Payments
            staffPermission.CreateAndManageFee = objVM.Payment.CreateAndManageFee;
            staffPermission.CreateAndManageTrips = objVM.Payment.CreateAndManageTrips;
            staffPermission.CreateAndManageTickets = objVM.Payment.CreateAndManageTickets;
            staffPermission.CreateAndManageShop = objVM.Payment.CreateAndManageShop;
            staffPermission.ManageRefunds = objVM.Payment.ManageRefunds;
            staffPermission.ManageCashPayments = objVM.Payment.ManageCashPayments;
            staffPermission.ManageOrders = objVM.Payment.ManageOrders;
            //GroupAndIndividuals                           
            staffPermission.CreateAndManageStaff = objVM.GroupAndIndividuals.CreateAndManageStaff;
            staffPermission.AssignTeachers = objVM.GroupAndIndividuals.AssignTeachers;
            staffPermission.AssignDepartments = objVM.GroupAndIndividuals.AssignDepartments;
            staffPermission.CreateAndManageStudents = objVM.GroupAndIndividuals.CreateAndManageStudents;
            staffPermission.ManageParents = objVM.GroupAndIndividuals.ManageParents;
            staffPermission.CreateAndManageGroups = objVM.GroupAndIndividuals.CreateAndManageGroups;
            staffPermission.ViewStudents = objVM.GroupAndIndividuals.ViewStudents;
            staffPermission.ViewStaff = objVM.GroupAndIndividuals.ViewStaff;
            staffPermission.ViewParents = objVM.GroupAndIndividuals.ViewParents;
            //Academic                                       
            staffPermission.CreateAndManageCalendarEvents = objVM.Academic.CreateAndManageCalendarEvents;
            staffPermission.ViewCalendarEvents = objVM.Academic.ViewCalendarEvents;
            staffPermission.CreateAndManageCoursework = objVM.Academic.CreateAndManageCoursework;
            staffPermission.CreateAndManageAttendance = objVM.Academic.CreateAndManageAttendance;
            staffPermission.CreateAndManageTimetable = objVM.Academic.CreateAndManageTimetable;
            staffPermission.ViewTimeTables = objVM.Academic.ViewTimeTable;
            //Reports:                                       
            staffPermission.ImportData = objVM.Reports.ImportData;
            staffPermission.ExportData = objVM.Reports.ExportData;
            staffPermission.ManagePaymentReports = objVM.Reports.ManagePaymentReports;
            staffPermission.ManageAcademicReports = objVM.Reports.ManageAcademicReports;

            staffPermission.CreatedBy = objVM.InsertedBy;
            staffPermission.InsertedOn = DateTime.Now;
            staffPermission.UserId = objVM.UserId;
            if (Convert.ToInt32(staffPermission.Insert()) > 0)
            {
                return true;
            }
            return false;
        }

        public bool UpdateStaffPermissions(StaffPermission objVM)
        {
            DBConnectionString.StaffPermission staffPermission = DBConnectionString.StaffPermission.SingleOrDefault("select * from StaffPermission where UserId=@0", objVM.UserId);
            if (staffPermission != null)
            {
                //Communication
                staffPermission.SendEmail = objVM.SendEmail;
                staffPermission.ViewEmail = objVM.ViewEmail;
                staffPermission.SendEletter = objVM.SendEletter;
                staffPermission.ViewEletter = objVM.ViewEletter;
                staffPermission.SendSms = objVM.SendSms;
                staffPermission.ViewSms = objVM.ViewSms;
                staffPermission.AbsentReporting = objVM.AbsentReporting;
                staffPermission.PrintLetters = objVM.PrintLetters;
                staffPermission.TopUpSMSBalance = objVM.TopUpSMSBalance;
                ///Payments
                staffPermission.CreateAndManageFee = objVM.CreateAndManageFee;
                staffPermission.CreateAndManageTrips = objVM.CreateAndManageTrips;
                staffPermission.CreateAndManageTickets = objVM.CreateAndManageTickets;
                staffPermission.CreateAndManageShop = objVM.CreateAndManageShop;
                staffPermission.ManageRefunds = objVM.ManageRefunds;
                staffPermission.ManageCashPayments = objVM.ManageCashPayments;
                staffPermission.ManageOrders = objVM.ManageOrders;
                //GroupAndIndividuals                           
                staffPermission.CreateAndManageStaff = objVM.CreateAndManageStaff;
                staffPermission.AssignTeachers = objVM.AssignTeachers;
                staffPermission.AssignDepartments = objVM.AssignDepartments;
                staffPermission.CreateAndManageStudents = objVM.CreateAndManageStudents;
                staffPermission.ManageParents = objVM.ManageParents;
                staffPermission.CreateAndManageGroups = objVM.CreateAndManageGroups;
                staffPermission.ViewStudents = objVM.ViewStudents;
                staffPermission.ViewStaff = objVM.ViewStaff;
                staffPermission.ViewParents = objVM.ViewParents;
                //Academic                                       
                staffPermission.CreateAndManageCalendarEvents = objVM.CreateAndManageCalendarEvents;
                staffPermission.ViewCalendarEvents = objVM.ViewCalendarEvents;
                staffPermission.CreateAndManageCoursework = objVM.CreateAndManageCoursework;
                staffPermission.CreateAndManageAttendance = objVM.CreateAndManageAttendance;
                staffPermission.CreateAndManageTimetable = objVM.CreateAndManageTimetable;
                staffPermission.ViewTimeTables = objVM.ViewTimeTables;
                //Reports:                                       
                staffPermission.ImportData = objVM.ImportData;
                staffPermission.ExportData = objVM.ExportData;
                staffPermission.ManagePaymentReports = objVM.ManagePaymentReports;
                staffPermission.ManageAcademicReports = objVM.ManageAcademicReports;

                staffPermission.UpdatedOn = DateTime.Now;
                staffPermission.UpdatedBy = objVM.UpdatedBy;
                staffPermission.UserId = objVM.UserId;
                if (staffPermission.Update() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }


        public void UpdateStaff(StaffViewModel objVM)
        {
            DBConnectionString.Profile profile = DBConnectionString.Profile.Fetch("select * from profile where userId=@0", objVM.UserId).SingleOrDefault();
            DBConnectionString.Staff staff = DBConnectionString.Staff.Fetch("select * from staff where userId=@0", objVM.UserId).SingleOrDefault();
            DBConnectionString.User user = DBConnectionString.User.Fetch("select * from Users where userId=@0", objVM.UserId).SingleOrDefault();

            if (profile != null)
            {
                profile.Title = objVM.Title;
                profile.DateOfBirth = objVM.DateOfBirth;
                profile.MobileNumber = objVM.MobileNumber;
                profile.HomeTelephoneNumber = objVM.HomeTelephoneNumber;
                profile.EmailAddress1 = objVM.Email;
                profile.Update();
            }

            if (staff != null)
            {
                staff.StaffTypeId = objVM.StaffTypeId;
                staff.Email = objVM.Email;
                staff.Update();
            }

            if (user != null)
            {
                user.FirstName = objVM.FirstName;
                user.LastName = objVM.LastName;
                user.Email = objVM.Email;
                user.Update();
            }
            objVM.StaffPermission.UserId = objVM.UserId;
            this.UpdateStaffPermissions(objVM.StaffPermission);
        }
    }
}









