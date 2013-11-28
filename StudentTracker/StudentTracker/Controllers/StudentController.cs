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
    public class StudentController : BaseController
    {
        //
        // GET: /Student/
        StudentRepository repository = new StudentRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateStudent()
        {
            SelectList classList = null;
            SelectList courseList = null;
            SelectList sectionList = null;
            SelectList departmentList = null;
            this.LoadSelectLists(out  classList, out  courseList, out  sectionList, out  departmentList, false);
            StudentViewModel objViewModel = new StudentViewModel();
            objViewModel.CourseList = courseList;
            objViewModel.DepartmentList = departmentList;
            objViewModel.ClassList = classList;
            objViewModel.SectionList = sectionList;
            return View(objViewModel);
        }

        [HttpPost]
        public ActionResult CreateStudent(StudentViewModel objViewModel)
        {
            string token = CreateToken(objViewModel);
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
                    Profile.MobileNumber = "none";

                    int recAffected = Convert.ToInt32(Profile.Insert());

                    string roleName = UserRoles.Student.ToString(); //((UserRoles)Convert.ToInt16(Token.RoleId)).ToString();
                    Roles.AddUserToRole(objViewModel.Email, roleName);
                    objViewModel.UserId = userId;
                    if (repository.CreateNewStudent(objViewModel))
                    {
                        this.AssignGroup(objViewModel.GroupIds, userId);
                        this.AssignSubject(objViewModel.SubjectIds, userId);
                        SaveFiles(token, this.GetType().Name, objViewModel.StudentId);
                        EmailHandler.Utilities.SendRegistationEmail(token, objViewModel.Email);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public string CreateToken(StudentViewModel objViewModel)
        {
            int recordAffected = 0;
            RegistrationToken registrationToken = new RegistrationToken();
            registrationToken.OrganizationId = _userStatistics.OrganizationId;
            registrationToken.CourseId = (int)objViewModel.CourseId;
            registrationToken.ClassId = objViewModel.ClassId;
            registrationToken.SectionId = objViewModel.SectionId;
            if (objViewModel.DepartmentId != null)
            {
                registrationToken.DepartmentId = (int)objViewModel.DepartmentId;
            }
            registrationToken.Token = UserStatistics.GenerateToken();
            registrationToken.CreatedBy = _userStatistics.UserId;
            registrationToken.RoleId = (int)UserRoles.Student;
            recordAffected = repository.CreateToken(registrationToken);
            if (recordAffected > 0)
            {
                return registrationToken.Token;
            }
            return string.Empty;
        }

        private void LoadSelectLists(out SelectList classList, out SelectList courseList, out SelectList sectionList, out SelectList departmentList, bool isEdit, long organizationId = -1, long courseId = -1, long departmentId = -1, long classId = -1, long sectionId = -1)
        {
            classList = null;
            courseList = null;
            sectionList = null;
            departmentList = null;

            List<Course> objCourseList = null;
            List<Class> objClassList = null;
            List<Section> objSectionList = null;
            List<Department> objDepartmentList = null;

            objCourseList = repository.GetCourses(organizationId: _userStatistics.OrganizationId);
            objDepartmentList = repository.DepartmenstByOrganization(_userStatistics.OrganizationId);
            if (isEdit)
            {
                objClassList = repository.ClassByCourse(courseId);
                objSectionList = repository.SectionByClass(classId);
            }
            else
            {
                objClassList = new List<Class>();
                objSectionList = new List<Section>();
            }

            courseList = new SelectList(objCourseList, "CourseId", "CourseName", courseId);
            classList = new SelectList(objClassList, "ClassId", "ClassName", classId);

            sectionList = new SelectList(objSectionList, "SectionId", "SectionName", sectionId);
            departmentList = new SelectList(objDepartmentList, "DepartmentId", "DepartmentName", departmentId);

        }

        private void AssignSubject(string subjectIds, long userId)
        {
            if (subjectIds != null)
            {
                var idList = subjectIds.Split(',');
                UserSubjects objUserSubject = null;
                foreach (var subjectId in idList)
                {
                    objUserSubject = new UserSubjects();
                    objUserSubject.UserId = userId;
                    objUserSubject.SubjectId = Convert.ToInt32(subjectId);
                    objUserSubject.InsertedBy = _userStatistics.UserId;
                    this.repository.AssignSubjectToUser(objUserSubject);
                }
            }
        }

        private void AssignGroup(string groupIds, long userId)
        {
            if (groupIds != null)
            {
                var idList = groupIds.Split(',');
                UserGroup objUserGroup = null;
                foreach (var groupId in idList)
                {
                    objUserGroup = new UserGroup();
                    objUserGroup.UserId = userId;
                    objUserGroup.GroupId = Convert.ToInt32(groupId);
                    objUserGroup.InsertedBy = _userStatistics.UserId;
                    this.repository.AssignGroupToUser(objUserGroup);
                }
            }
        }
    }
}
