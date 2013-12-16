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
            return View();
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
                    Profile.DateOfBirth = objViewModel.Profile.DateOfBirth;
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

        private void LoadSelectLists(out SelectList classList, out SelectList courseList, out SelectList sectionList, out SelectList departmentList, out SelectList subjectList, bool isEdit, long organizationId = -1, long courseId = -1, long? departmentId = -1, long classId = -1, int sectionId = -1, long subjectId = -1)
        {
            classList = null;
            courseList = null;
            sectionList = null;
            departmentList = null;
            subjectList = null;

            List<Course> objCourseList = null;
            List<Class> objClassList = null;
            List<Section> objSectionList = null;
            List<Department> objDepartmentList = null;
            List<Subject> objSubjectList = null;

            objCourseList = repository.GetCourses(organizationId: _userStatistics.OrganizationId);
            objDepartmentList = repository.DepartmenstByOrganization(_userStatistics.OrganizationId);
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
            this.LoadSelectLists(out  classList, out  courseList, out  sectionList, out  departmentList, out subjectList, false);

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
            return objVM;
        }
    }
}
