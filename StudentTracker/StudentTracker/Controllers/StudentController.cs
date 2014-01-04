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
            List<Student> objModel = this.repository.GetStudents(_userStatistics.OrganizationId);
            List<string> listGroups = new List<string>();
            for (int i = 0; i < objModel.Count; i++)
            {
                if (objModel[i].UserId != null)
                {
                    listGroups = this.repository.GetGroupOfUser(objModel[i].UserId.Value);
                    objModel[i].GroupNames = string.Join(", ", listGroups);
                }
            }

            return View(objModel);
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
            string token = repository.CreateToken(objViewModel, _userStatistics.UserId, _userStatistics.OrganizationId);
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

                    string roleName = UserRoles.Student.ToString(); //((UserRoles)Convert.ToInt16(Token.RoleId)).ToString();
                    Roles.AddUserToRole(objViewModel.Email, roleName);
                    objViewModel.UserId = userId;

                    objViewModel.InsertedBy = _userStatistics.UserId;
                    objViewModel.OrganizationId = _userStatistics.OrganizationId;
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

        public ActionResult Edit(long id)
        {
            SelectList classList = null;
            SelectList courseList = null;
            SelectList sectionList = null;
            SelectList departmentList = null;
            StudentViewModel objViewModel = new StudentViewModel();
            objViewModel = this.repository.GetStudents(_userStatistics.OrganizationId, id);
            this.LoadSelectLists(out  classList, out  courseList, out  sectionList, out  departmentList, true, objViewModel.OrganizationId, objViewModel.CourseId, objViewModel.DepartmentId, objViewModel.ClassId, objViewModel.SectionId);
            objViewModel.CourseList = courseList;
            objViewModel.DepartmentList = departmentList;
            objViewModel.ClassList = classList;
            objViewModel.SectionList = sectionList;

            return View(objViewModel);
        }

        [HttpPost]
        public ActionResult Edit(StudentViewModel objViewModel)
        {
            if (this.repository.UpdateStudent(objViewModel))
            {
                return RedirectToAction("Index");
            }
            SelectList classList = null;
            SelectList courseList = null;
            SelectList sectionList = null;
            SelectList departmentList = null;
            this.LoadSelectLists(out  classList, out  courseList, out  sectionList, out  departmentList, true, objViewModel.OrganizationId, objViewModel.CourseId, objViewModel.DepartmentId, objViewModel.ClassId, objViewModel.SectionId);
            objViewModel.CourseList = courseList;
            objViewModel.DepartmentList = departmentList;
            objViewModel.ClassList = classList;
            objViewModel.SectionList = sectionList;

            return View(objViewModel);
        }



        private void LoadSelectLists(out SelectList classList, out SelectList courseList, out SelectList sectionList, out SelectList departmentList, bool isEdit, long organizationId = -1, long courseId = -1, long? departmentId = -1, long classId = -1, int sectionId = -1)
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

        public ActionResult DeleteConfirm(long id)
        {
            DBConnectionString.Student student = DBConnectionString.Student.SingleOrDefault(id);

            PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
            db.Execute("Delete from UserGroup where UserId=@0", student.UserId);
            db.Execute("Delete from UserSubject where UserId=@0", student.UserId);
            db.Execute("Delete from ELetter where UserId=@0", student.UserId);
            db.Execute("Delete from Profile where UserId=@0", student.UserId);
            db.Execute("Delete from Users where UserId=@0", student.UserId);

            if (student != null)
            {
                student.Delete();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(long id)
        {
            StudentViewModel objViewModel = new StudentViewModel();
            objViewModel = this.repository.GetStudents(_userStatistics.OrganizationId, id);
            PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
            objViewModel.GroupNames = string.Join(",", db.Query<string>("select GroupName from [Group] as G join UserGroup As UG on G.GroupId = UG.GroupId where UG.UserId = @0", objViewModel.UserId));
            objViewModel.SubjectNames = string.Join(",", db.Query<string>("select SubjectName from Subjects as S join UserSubjects As US on S.SubjectId = US.SubjectId where US.UserId = @0", objViewModel.UserId));
            return View(objViewModel);
        }

    }
}
