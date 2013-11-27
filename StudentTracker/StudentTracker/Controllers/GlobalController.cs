using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
using StudentTracker.Repository;
using StudentTracker.ViewModels;
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
        public JsonResult GetClasses()
        {
            List<Class> modelList = this.repository.GetClasses(organizationId: _userStatistics.OrganizationId);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

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

        public JsonResult GetCourse(long id)
        {
            List<Course> classList = repository.CourseByOrganization(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClass(long id)
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
    }
}
