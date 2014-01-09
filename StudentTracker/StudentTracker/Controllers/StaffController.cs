using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
using StudentTracker.Repository;
using StudentTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentTracker.Controllers
{
    public class StaffController : BaseController
    {
        //
        // GET: /Staff/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index()
        {
            List<TeacherSubjects> objTeacherSubjectsList = null;

            List<string> CourseList = null;
            List<string> DepartmentList = null;
            List<string> ClassList = null;
            List<string> SectionList = null;
            List<string> SubjectList = null;

            List<Staff> objStaffList = this.repository.GetStaff(organizationId: _userStatistics.OrganizationId);
            for (int i = 0; i < objStaffList.Count; i++)
            {
                if (objStaffList[i].UserId != null)
                {
                    objTeacherSubjectsList = this.repository.GetTeacherSubjects(objStaffList[i].UserId.Value);
                }
                CourseList = new List<string>();
                DepartmentList = new List<string>();
                ClassList = new List<string>();
                SectionList = new List<string>();
                SubjectList = new List<string>();
                if (objTeacherSubjectsList != null)
                {
                    foreach (var subject in objTeacherSubjectsList)
                    {
                        CourseList.Add(subject.CourseName);
                        DepartmentList.Add(subject.DepartmentName);
                        ClassList.Add(subject.CourseName);
                        SectionList.Add(subject.SectionName);
                        SubjectList.Add(subject.SubjectName);
                    }
                }
                objStaffList[i].CourseName = string.Join(",", CourseList);
                objStaffList[i].DepartmentName = string.Join(",", DepartmentList);
                objStaffList[i].ClassName = string.Join(",", ClassList);
                objStaffList[i].SectionName = string.Join(",", SectionList);
                objStaffList[i].SubjectName = string.Join(",", SubjectList);
            }
            return View(objStaffList);
        }

        public ActionResult Create()
        {
            StaffViewModel objVM = this.LoadViewModel();
            return View(objVM);
        }

        [HttpPost]
        public ActionResult Create(StaffViewModel objViewModel)
        {
            string token = this.CreateToken(objViewModel, _userStatistics.UserId, _userStatistics.OrganizationId);
            if (!(string.IsNullOrEmpty(token)))
            {
                long userId = WebSecurity.RegisterNewUser(objViewModel.Email, "none", objViewModel.Email, false, objViewModel.Profile.FirstName, objViewModel.Profile.LastName, objViewModel.OrganizationId, token);
                DBConnectionString.Profile Profile = new DBConnectionString.Profile();
                if (userId != -1)
                {
                    Profile.UserId = userId;
                    Profile.Title = objViewModel.Profile.Title;
                    Profile.Address1 = "none";
                    Profile.Address2 = "none";
                    Profile.InsertedOn = DateTime.Now;
                    Profile.EmailAddress1 = objViewModel.Email;
                    Profile.HomeTelephoneNumber = objViewModel.Profile.HomeTelephoneNumber;
                    Profile.SecurityQuestionId = 1;
                    Profile.SecurityAnswer = "none";
                    Profile.DateOfBirth = objViewModel.DateOfBirth.Value;
                    Profile.ModifiedOn = null;
                    Profile.MobileNumber = objViewModel.Profile.MobileNumber;

                    int recAffected = Convert.ToInt32(Profile.Insert());
                    long id = Profile.ProfileId;
                    string roleName = UserRoles.Student.ToString(); //((UserRoles)Convert.ToInt16(Token.RoleId)).ToString();
                    Roles.AddUserToRole(objViewModel.Email, roleName);
                    //objViewModel.UserId = userId;

                    objViewModel.InsertedBy = _userStatistics.UserId;
                    objViewModel.OrganizationId = _userStatistics.OrganizationId;
                    objViewModel.UserId = userId;
                    objViewModel.StaffId = this.repository.CreateNewStaff(objViewModel);

                    this.repository.AssignSubjectToTeacher(objViewModel);

                    SaveFiles(token, this.GetType().Name, objViewModel.StaffId);
                    EmailHandler.Utilities.SendRegistationEmail(token, objViewModel.Email);
                    return RedirectToAction("Index");
                }
            }
            StaffViewModel objVM = this.LoadViewModel();
            return View(objVM);
        }

        private void LoadSelectLists(out SelectList classList, out SelectList courseList, out SelectList sectionList, out SelectList departmentList, out SelectList subjectList, out SelectList staffTypeList, bool isEdit, long organizationId = -1, long courseId = -1, long? departmentId = -1, long classId = -1, int sectionId = -1, long subjectId = -1, int staffTypeId = -1)
        {
            classList = null;
            courseList = null;
            sectionList = null;
            departmentList = null;
            subjectList = null;
            staffTypeList = null;

            List<Course> objCourseList = null;
            List<Class> objClassList = null;
            List<Section> objSectionList = null;
            List<Department> objDepartmentList = null;
            List<Subject> objSubjectList = null;
            List<StaffTypes> objStaffTypes = null;

            objCourseList = repository.GetCourses(organizationId: _userStatistics.OrganizationId);
            objDepartmentList = repository.DepartmenstByOrganization(_userStatistics.OrganizationId);
            objStaffTypes = repository.GetStaffTypes();
            if (isEdit)
            {
                objClassList = repository.ClassByCourse(courseId);
                objSectionList = repository.SectionByClass(classId);
                objSubjectList = repository.SubjectByClass(classId);
            }
            else
            {
                objClassList = new List<Class>();
                objSectionList = new List<Section>();
                objSubjectList = new List<Subject>();
            }

            courseList = new SelectList(objCourseList, "CourseId", "CourseName", courseId);
            classList = new SelectList(objClassList, "ClassId", "ClassName", classId);
            sectionList = new SelectList(objSectionList, "SectionId", "SectionName", sectionId);
            departmentList = new SelectList(objDepartmentList, "DepartmentId", "DepartmentName", departmentId);
            subjectList = new SelectList(objSubjectList, "SubjectId", "SubjectName", subjectId);
            staffTypeList = new SelectList(objStaffTypes, "StaffTypeId", "StaffTypeName", staffTypeId);
        }

        public string CreateToken(StaffViewModel objViewModel, long userId, long organizationId)
        {
            int recordAffected = 0;
            DBConnectionString.RegistrationToken registrationToken = new DBConnectionString.RegistrationToken();
            registrationToken.OrganizationId = organizationId;
            // registrationToken.CourseId = (int)objViewModel.CourseId;
            // registrationToken.ClassId = objViewModel.ClassId;
            // registrationToken.SectionId = objViewModel.SectionId;
            //if (objViewModel.DepartmentId != null)
            //{
            //    registrationToken.DepartmentId = (int)objViewModel.DepartmentId;
            //}
            registrationToken.Token = UserStatistics.GenerateToken();
            registrationToken.CreatedBy = userId;
            registrationToken.RoleId = (int)UserRoles.Teacher;
            recordAffected = Convert.ToInt32(registrationToken.Insert());
            if (recordAffected > 0)
            {
                return registrationToken.Token;
            }
            return string.Empty;
        }

        public StaffViewModel LoadViewModel()
        {
            StaffViewModel objVM = new StaffViewModel();
            ListFields objFields = null;
            objVM.ListFields = new List<ListFields>();

            SelectList classList = null;
            SelectList courseList = null;
            SelectList sectionList = null;
            SelectList departmentList = null;
            SelectList subjectList = null;
            SelectList staffTypeList = null;
            this.LoadSelectLists(out  classList, out  courseList, out  sectionList, out  departmentList, out subjectList, out staffTypeList, false);

            for (int i = 0; i < 10; i++)
            {
                objFields = new ListFields();
                objFields.CourseList = courseList;
                objFields.DepartmentList = departmentList;
                objFields.ClassList = classList;
                objFields.SectionList = sectionList;
                objFields.SubjectList = subjectList;
                objVM.ListFields.Add(objFields);
            }
            objVM.StaffTypeList = staffTypeList;
            return objVM;
        }

        public ActionResult Edit(long id)
        {

            StaffViewModel objStaff = this.repository.GetStaff(id);
            List<TeacherSubjects> objTeacherSubjectsList = this.repository.GetTeacherSubjects(objStaff.UserId);
            ListFields objFields = null;
            objStaff.Count = objTeacherSubjectsList.Count;
            objStaff.ListFields = new List<ListFields>();

            SelectList classList = null;
            SelectList courseList = null;
            SelectList sectionList = null;
            SelectList departmentList = null;
            SelectList subjectList = null;
            SelectList staffTypeList = null;


            for (int i = 0; i < objTeacherSubjectsList.Count; i++)
            {
                this.LoadSelectLists(out  classList, out  courseList, out  sectionList, out  departmentList, out subjectList, out staffTypeList, true, objStaff.OrganizationId, objTeacherSubjectsList[i].CourseId, objTeacherSubjectsList[i].DepartmentId, objTeacherSubjectsList[i].ClassId, (int)objTeacherSubjectsList[i].SectionId, objTeacherSubjectsList[i].SubjectId, objStaff.StaffTypeId);
                objFields = new ListFields();
                objFields.CourseList = courseList;
                objFields.DepartmentList = departmentList;
                objFields.ClassList = classList;
                objFields.SectionList = sectionList;
                objFields.SubjectList = subjectList;
                objFields.Id = objTeacherSubjectsList[i].Id;
                objStaff.ListFields.Add(objFields);
                if (i + 1 == objTeacherSubjectsList.Count)
                {
                    if (objTeacherSubjectsList.Count < 10)
                    {
                        for (int j = objTeacherSubjectsList.Count; j < 10; j++)
                        {
                            this.LoadSelectLists(out  classList, out  courseList, out  sectionList, out  departmentList, out subjectList, out staffTypeList, false);
                            objFields = new ListFields();
                            objFields.CourseList = courseList;
                            objFields.DepartmentList = departmentList;
                            objFields.ClassList = classList;
                            objFields.SectionList = sectionList;
                            objFields.SubjectList = subjectList;
                            objStaff.ListFields.Add(objFields);
                        }
                    }
                }
            }
            objStaff.StaffTypeList = staffTypeList;
            return View(objStaff);
        }

        [HttpPost]
        public ActionResult Edit(StaffViewModel objVM)
        {
            DBConnectionString.Profile profile = DBConnectionString.Profile.Fetch("select * from profile where userId=@0", objVM.UserId).SingleOrDefault();
            DBConnectionString.Staff staff = DBConnectionString.Staff.Fetch("select * from staff where userId=@0", objVM.UserId).SingleOrDefault();
            DBConnectionString.User user = DBConnectionString.User.Fetch("select * from Users where userId=@0", objVM.UserId).SingleOrDefault();
            profile.Title = objVM.Title;
            profile.DateOfBirth = objVM.DateOfBirth.Value;
            profile.MobileNumber = objVM.MobileNumber;
            profile.HomeTelephoneNumber = objVM.HomeTelephoneNumber;
            profile.EmailAddress1 = objVM.Email;
            staff.StaffTypeId = objVM.StaffTypeId;
            staff.Email = objVM.Email;
            user.FirstName = objVM.FirstName;
            user.LastName = objVM.LastName;
            user.Email = objVM.Email;
            profile.Update();
            staff.Update();
            user.Update();


            return RedirectToAction("Index");
        }

        public long AddMoreDepartmentClasses(DBConnectionString.TeacherSubject teacher)
        {
            int recAffected = Convert.ToInt32(teacher.Insert());
            if (recAffected > 0)
            {
                return teacher.Id;
            }
            return 0;
        }

        public bool UpdateTeacherSubjects(DBConnectionString.TeacherSubject teacher)
        {

            int recAffected = teacher.Update();
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }

        public ActionResult Details(long id)
        {
            StaffViewModel objVM = this.repository.GetStaff(staffId: id);

            if (objVM != null)
            {
                if (objVM.UserId != null)
                {
                    objVM.TeacherSubjectsList = this.repository.GetTeacherSubjects(objVM.UserId);
                }
            }
            return View(objVM);
        }

        public ActionResult Delete(long id)
        {
            this.repository.DeleteStaff(id);
            return RedirectToAction("Index");
        }
    }
}
