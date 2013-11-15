using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Models;
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
        ScheduleRepository repository = new ScheduleRepository();
        public ScheduleController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }
        //Index for schedule
        public ActionResult Index()
        {
            return View();
        }

        //details
        public ActionResult Details(long id = 0)
        {
            ScheduleRepository objRep = new ScheduleRepository();
            Schedule objSchedule = objRep.GetSchedule(id);
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
                    objSchedule.InsertedBy = _userStatistics.UserId;
                    ScheduleRepository objRep = new ScheduleRepository();
                    if (objRep.CreateSchedule(objSchedule))
                    {
                        return Convert.ToString(true);
                    }
                    return Convert.ToString(false);
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
                    ScheduleRepository objRep = new ScheduleRepository();
                    objSchedule.ModifiedBy = _userStatistics.UserId;
                    if (objRep.UpdateSchedule(objSchedule))
                    {
                        return Convert.ToString(true);
                    }
                    return Convert.ToString(false);
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
                //Schedule objSchedule = db.Schedules.Find(id);
                //db.Schedules.Remove(objSchedule);
                //db.SaveChanges();
                ScheduleRepository objRep = new ScheduleRepository();
                if (objRep.DeleteSchedule(id))
                {
                    return Convert.ToString(true);
                }
                return Convert.ToString(true);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public ActionResult ViewSchedules()
        {

            return PartialView(repository.GetSchdeule());
        }

        //public Schedule LoadSelectLists(long scheduleId = -1)
        //{
        //    PetaPoco.Database dbPeta = new PetaPoco.Database("DBConnectionString");
        //    Schedule objSchedule = new Schedule();
        //    List<Organization> orgList = null;
        //    List<Course> courseList = null;
        //    List<Class> classList = null;
        //    List<Subject> subjectList = null;
        //    List<Department> departmentList = null;
        //    List<ClassRoom> classRoomList = null;
        //    if (scheduleId != -1)
        //    {
        //        objSchedule = dbPeta.Query<Schedule>("select * from Schedule where ScheduleId=@0", scheduleId).SingleOrDefault();
        //        classList = dbPeta.Query<Class>("select * from Classes where CourseId=@0", objSchedule.CourseId).ToList();
        //        subjectList = dbPeta.Query<Subject>("select * from Subjects where ClassId=@0", objSchedule.ClassId).ToList();
        //        classRoomList = dbPeta.Query<ClassRoom>("select * from ClassRoom where DepartmentId=@0", objSchedule.DepartmentId).ToList();
        //    }
        //    else
        //    {
        //        classList = new List<Class>();
        //        subjectList = new List<Subject>();
        //        classRoomList = new List<ClassRoom>();
        //    }
        //    if (User.IsInRole("SiteAdmin"))
        //    {
        //        orgList = dbPeta.Query<Organization>("select * from Organizations").ToList();
        //        if (scheduleId != -1)
        //        {
        //            courseList = dbPeta.Query<Course>("select * from Courses where OrganisationId=@0", objSchedule.OrganizationId).ToList(); //dbPeta.Courses.Where(x => x.OrganisationId == objSchedule.OrganizationId).ToList();
        //            departmentList = dbPeta.Query<Department>("select * from Departments where OrganizationId=@0", objSchedule.OrganizationId).ToList(); //dbPeta.Departments.Where(x => x.OrganizationId == objSchedule.OrganizationId).ToList();
        //        }
        //        else
        //        {
        //            courseList = new List<Course>();
        //            departmentList = new List<Department>();
        //        }
        //    }
        //    else
        //    {
        //        var organization = dbPeta.SingleOrDefault<Organization>("select * from Organizations where CreatedBy=@0", _userStatistics.UserId);
        //        objSchedule.OrganizationId = organization.OrganizationId;
        //        ViewBag.Organization = organization.OrganizationName;
        //        courseList = dbPeta.Query<Course>("select * from Courses where OrganisationId=@0", organization.OrganizationId).ToList(); //dbPeta.Courses.Where(x => x.OrganisationId == objSchedule.OrganizationId).ToList();
        //        departmentList = dbPeta.Query<Department>("select * from Departments where OrganizationId=@0", organization.OrganizationId).ToList(); //dbPeta.Departments.Where(x => x.OrganizationId == objSchedule.OrganizationId).ToList();

        //    }
        //    objSchedule.OrganizationList = new SelectList(orgList, "OrganizationId", "OrganizationName", objSchedule.OrganizationId);
        //    objSchedule.CourseList = new SelectList(courseList, "CourseId", "CourseName", objSchedule.CourseId);
        //    objSchedule.ClassList = new SelectList(classList, "ClassId", "ClassName", objSchedule.ClassId);
        //    objSchedule.SubjectList = new SelectList(subjectList, "SubjectId", "SubjectName", objSchedule.SubjectId);
        //    objSchedule.DepartmentList = new SelectList(departmentList, "DepartmentId", "DepartmentName", objSchedule.DepartmentId);
        //    objSchedule.ClassRoomList = new SelectList(classRoomList, "ClassRoomId", "Name", objSchedule.ClassRoomId);
        //    List<SelectListItem> daysList = Enum.GetValues(typeof(StudentTracker.Core.Utilities.Days)).Cast<StudentTracker.Core.Utilities.Days>().Select(v => new SelectListItem
        //    {
        //        Text = v.ToString(),
        //        Value = ((int)v).ToString()
        //    }).ToList();
        //    objSchedule.DayList = new SelectList(daysList, "Value", "Text");
        //    return objSchedule;
        //}
        public Schedule LoadSelectLists(long scheduleId = -1)
        {
            Schedule objSchedule = null;
            if (!User.IsInRole("SiteAdmin"))
            {
                objSchedule = repository.LoadScheduleLists(null, _userStatistics.OrganizationId, scheduleId);
                ViewBag.Organization = objSchedule.OrganizationName;
            }
            else
            {
                objSchedule = repository.LoadScheduleLists("SiteAdmin", _userStatistics.OrganizationId, scheduleId);
            }
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
