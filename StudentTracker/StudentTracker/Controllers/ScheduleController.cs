using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class ScheduleController : BaseController
    {
        StudentContext db = new StudentContext();

        //Index for schedule
        public ActionResult Index()
        {
            return View();
        }

        //details
        public ActionResult Details(long id = 0)
        {
            Schedule objSchedule = db.Schedules.Find(id);
            if (objSchedule == null)
            {
                return HttpNotFound();
            }
            return PartialView(objSchedule);
        }

        //create
        public ActionResult Create()
        {
            Schedule objSchedule = LoadSelectLists();
            return PartialView(objSchedule);
        }

        //create post
        [HttpPost]
        public string Create(Schedule objSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objSchedule.InsertedOn = DateTime.Now;
                    objSchedule.InsertedBy = _userStatistics.UserId;
                    db.Schedules.Add(objSchedule);
                    db.SaveChanges();
                    return Convert.ToString(true);
                }

                return Convert.ToString(false);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public ActionResult Edit(long id = 0)
        {
            Schedule objSchedule = LoadSelectLists(id);
            if (objSchedule == null)
            {
                return HttpNotFound();
            }
            return PartialView(objSchedule);
        }

        [HttpPost]
        public string Edit(Schedule objSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objSchedule.ModifiedOn = DateTime.Now;
                    objSchedule.ModifiedBy = _userStatistics.UserId;
                    db.Entry(objSchedule).State = EntityState.Modified;
                    db.SaveChanges();
                    return Convert.ToString(true);
                }
                return Convert.ToString(false);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                Schedule objSchedule = db.Schedules.Find(id);
                db.Schedules.Remove(objSchedule);
                db.SaveChanges();
                return Convert.ToString(true);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public ActionResult ViewSchedules()
        {
            return View(db.Schedules.ToList());
        }

        public Schedule LoadSelectLists(long scheduleId = -1)
        {
            Schedule objSchedule = new Schedule();
            List<Organization> orgList = db.Organizations.ToList();
            List<Course> courseList = null;
            List<Class> classList = null;
            List<Subject> subjectList = null;
            List<Department> departmentList = null;
            List<ClassRoom> classRoomList = null;
            if (scheduleId != -1)
            {
                objSchedule = db.Schedules.Find(scheduleId);
                classList = db.Classes.Where(x => x.CourseId == objSchedule.CourseId).ToList();
                subjectList = db.Subjects.Where(x => x.ClassId == objSchedule.ClassId).ToList();
                classRoomList = db.ClassRooms.Where(x => x.DepartmentId == objSchedule.DepartmentId).ToList();
            }
            else
            {
                classList = new List<Class>();
                subjectList = new List<Subject>();
                classRoomList = new List<ClassRoom>();
            }
            if (User.IsInRole("SiteAdmin"))
            {
                orgList = db.Organizations.ToList();
                if (scheduleId != -1)
                {
                    courseList = db.Courses.Where(x => x.OrganisationId == objSchedule.OrganizationId).ToList();
                    departmentList = db.Departments.Where(x => x.OrganizationId == objSchedule.OrganizationId).ToList();
                }
                else
                {
                    courseList = new List<Course>();
                    departmentList = new List<Department>();
                }
            }
            else
            {
                var organization = db.Organizations.Single(x => x.CreatedBy == _userStatistics.UserId);
                objSchedule.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;
                courseList = db.Courses.Where(x => x.OrganisationId == organization.OrganizationId).ToList();
                departmentList = db.Departments.Where(x => x.OrganizationId == organization.OrganizationId).ToList();
            }
            objSchedule.OrganizationList = new SelectList(orgList, "OrganizationId", "OrganizationName", objSchedule.OrganizationId);
            objSchedule.CourseList = new SelectList(courseList, "CourseId", "CourseName", objSchedule.CourseId);
            objSchedule.ClassList = new SelectList(classList, "ClassId", "ClassName", objSchedule.ClassId);
            objSchedule.SubjectList = new SelectList(subjectList, "SubjectId", "SubjectName", objSchedule.SubjectId);
            objSchedule.DepartmentList = new SelectList(departmentList, "DepartmentId", "DepartmentName", objSchedule.DepartmentId);
            objSchedule.ClassRoomList = new SelectList(classRoomList, "ClassRoomId", "Name", objSchedule.ClassRoomId);
            return objSchedule;
        }

        public JsonResult GetCourse(long id)
        {
            return Json(db.Courses.Where(x => x.OrganisationId == id).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClass(long id)
        {
            return Json(db.Classes.Where(x => x.CourseId == id).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSubject(long id)
        {
            return Json(db.Subjects.Where(x => x.ClassId == id).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClassRoom(long id)
        {
            return Json(db.ClassRooms.Where(x => x.DepartmentId == id).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepartmentandCourse(long id)
        {
            ScheduleViewModel objVM = new ScheduleViewModel();
            objVM.CourseList = db.Courses.Where(x => x.OrganisationId == id).ToList();
            objVM.DepartmentList = db.Departments.Where(x => x.OrganizationId == id).ToList();
            return Json(objVM, "application/json;", JsonRequestBehavior.AllowGet);
        }
    }
}
