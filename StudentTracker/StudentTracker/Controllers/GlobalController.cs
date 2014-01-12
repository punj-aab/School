using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
using StudentTracker.Repository;
using StudentTracker.ViewModels;
using StudentTracker.Core.DAL;
namespace StudentTracker.Controllers
{
    public class GlobalController : BaseController
    {
        //
        // GET: /Global/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddRecipients()
        {
            List<User> objUserList = new List<Core.Entities.User>();
            objUserList = this.repository.Users(_userStatistics.OrganizationId);
            return PartialView(objUserList);
        }

        public JsonResult GetAllUsers()
        {
            List<User> userList = this.repository.Users(_userStatistics.OrganizationId);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroups()
        {
            List<Group> modelList = this.repository.GetGroups(organizationId: _userStatistics.OrganizationId);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetClasses()
        //{
        //    List<Class> modelList = this.repository.GetClasses(organizationId: _userStatistics.OrganizationId);
        //    return Json(modelList, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetGroupUsers(long id)
        {
            List<UserGroup> objModelList = repository.GetGroupUsers(id);
            return Json(objModelList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClassUsers(long id)
        {
            List<UserGroup> objModelList = new List<UserGroup>(); //repository.GetGroupUsers(id);
            return Json(objModelList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourses(long id)
        {
            List<Course> classList = repository.CourseByOrganization(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClasses(long id)
        {
            List<Class> classList = repository.ClassByCourse(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSection(long id)
        {
            List<Section> sectionList = repository.SectionByClass(id);
            return Json(sectionList, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetSubject(long id)
        //{
        //    //List<Subject>objModelList=repository.su
        //   // return Json(db.Subjects.Where(x => x.ClassId == id).ToList(), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetClassRoom(long id)
        //{
        //    //List<ClassRoom>ObjModelList=repository
        //    //return Json(db.ClassRooms.Where(x => x.DepartmentId == id).ToList(), JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetDepartmentandCourse(long id)
        {
            ScheduleViewModel objVM = new ScheduleViewModel();
            objVM.CourseList = repository.CourseByOrganization(id); //db.Courses.Where(x => x.OrganisationId == id).ToList();
            objVM.DepartmentList = repository.DepartmenstByOrganization(id); //db.Departments.Where(x => x.OrganizationId == id).ToList();
            return Json(objVM, "application/json;", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddGroups()
        {
            GroupViewModel objVM = new GroupViewModel();
            objVM.OrganizationGroupList = GroupList(_userStatistics.OrganizationId);
            for (int i = 0; i < objVM.OrganizationGroupList.Count; i++)
            {
                objVM.OrganizationGroupList[i].UserCount = this.repository.GroupUserCount(objVM.OrganizationGroupList[i].GroupId);
            }
            return PartialView(objVM);
        }
        public ActionResult AddSubjects()
        {
            SubjectViewModel objSubject = new SubjectViewModel();
            objSubject.CourseList = new SelectList(repository.CourseByOrganization(_userStatistics.OrganizationId), "CourseId", "CourseName");
            objSubject.ClassList = new SelectList(new List<Class>());
            objSubject.SectionList = new SelectList(new List<Section>());
            return PartialView(objSubject);
        }

        public ActionResult EditGroupsPopUp(long organizationId, long? userId, long? studentId)
        {
            GroupViewModel objVM = new GroupViewModel();
            objVM.OrganizationGroupList = GroupList(organizationId);
            if (userId != null)
            {
                objVM.AssignedGroupList = repository.GetUserGroups(userId.Value);

                objVM.UserGroupList = new List<Group>();
                for (int i = 0; i < objVM.AssignedGroupList.Count; i++)
                {
                    var groupList = objVM.OrganizationGroupList.Where(x => x.GroupId == objVM.AssignedGroupList[i].GroupId).ToList();
                    foreach (var item in groupList)
                    {
                        objVM.UserGroupList.Add(item);
                    }
                }
                objVM.UserId = userId.Value;

            }
            if (studentId != null)
            {
                objVM.StudentId = studentId.Value;
            }
            return PartialView(objVM);
        }

        public ActionResult EditUserSubjectsPopUp(long organizationId, long userId)
        {
            SubjectViewModel objVM = this.repository.GetCourseClassIds(userId);
            objVM.CourseList = new SelectList(this.repository.CourseByOrganization(_userStatistics.OrganizationId), "CourseId", "CourseName", objVM.CourseId);
            objVM.ClassList = new SelectList(this.repository.ClassByCourse(objVM.CourseId), "ClassId", "ClassName", objVM.ClassId);
            objVM.SectionList = new SelectList(this.repository.SectionByClass(objVM.ClassId), "SectionId", "SectionName");

            objVM.UserSubjectList = this.repository.GetUserSubjects(userId);
            objVM.ClassSubjects = this.repository.SubjectByClass(objVM.ClassId);

            return PartialView(objVM);
        }

        public JsonResult GetSubjectByClass(long classId)
        {
            List<Subject> objSubject = repository.SubjectByClass(classId);
            return Json(objSubject, JsonRequestBehavior.AllowGet);
        }

        public List<Group> GroupList(long organizationId)
        {
            StudentContext db = new StudentContext();
            return db.Groups.Where(x => x.OrganizationId == _userStatistics.OrganizationId).ToList();
        }

        public bool AddNewUserGroup(long userId, long? groupId, long? studentId)
        {
            UserGroup objUserGroup = new UserGroup();
            objUserGroup.UserId = userId;
            objUserGroup.GroupId = Convert.ToInt32(groupId);
            objUserGroup.InsertedBy = _userStatistics.UserId;
            objUserGroup.StudentId = studentId;
            if (this.repository.AssignGroupToUser(objUserGroup))
            {
                return true;
            }
            return false;
        }

        public bool DeleteUserGroup(long userId, long groupId)
        {
            int recAffected = 0;
            PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
            recAffected = db.Execute("delete from UserGroup where UserId = @0 and GroupId = @1", userId, groupId);
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }

        public bool AddNewSubject(long userId, long subjectId)
        {
            UserSubjects objUserSubject = null;
            objUserSubject = new UserSubjects();
            objUserSubject.UserId = userId;
            objUserSubject.SubjectId = Convert.ToInt32(subjectId);
            objUserSubject.InsertedBy = _userStatistics.UserId;
            if (this.repository.AssignSubjectToUser(objUserSubject))
            {
                return true;
            }
            return false;
        }

        public bool DeleteUserSubject(long userId, long subjectId)
        {
            int recAffected = 0;
            PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
            recAffected = db.Execute("delete from UserSubjects where UserId = @0 and SubjectId = @1", userId, subjectId);
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
    }
}
