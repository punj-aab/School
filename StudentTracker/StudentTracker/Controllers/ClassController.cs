using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
using StudentTracker.Core.DAL;
using StudentTracker.Repository;
namespace StudentTracker.Controllers
{
    public class ClassController : BaseController
    {
        private StudentContext db = new StudentContext();
        ClassRepository objRep = new ClassRepository();
        //
        // GET: /Class/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Class/Details/5

        public ActionResult Details(long id = 0)
        {
            Class objClass = objRep.GetClasses(id);
            if (objClass == null)
            {
                return HttpNotFound();
            }
            return PartialView(objClass);
        }

        //
        // GET: /Class/Create

        public ActionResult Create()
        {
            Class objClass = new Class();
            SelectList organizationList;
            SelectList courseList;
            LoadSelectLists(out organizationList, out courseList);
            objClass.OrganizationList = organizationList;
            objClass.CourseList = courseList;
            objClass.OrganizationId = ViewBag.OrganizationId == null ? 0 : ViewBag.OrganizationId;
            return PartialView(objClass);
        }

        //
        // POST: /Class/Create

        [HttpPost]
        public string Create(Class objClass)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objClass.InsertedOn = DateTime.Now;
                    objClass.InsertedBy = _userStatistics.UserId;
                    if (objRep.CreateClass(objClass))
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

        //
        // GET: /Class/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Class objClass = db.Classes.Find(id);
            if (objClass == null)
            {
                return HttpNotFound();
            }
            SelectList organizationList;
            SelectList courseList;
            LoadSelectLists(out organizationList, out courseList, objClass.OrganizationId, objClass.CourseId);
            objClass.OrganizationList = organizationList;
            objClass.CourseList = courseList;
            objClass.OrganizationId = ViewBag.OrganizationId == null ? objClass.OrganizationId : ViewBag.OrganizationId;
            return PartialView(objClass);
        }

        //
        // POST: /Class/Edit/5

        [HttpPost]
        public string Edit(Class objClass)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objClass.ModifiedBy = _userStatistics.UserId;
                    if (objRep.UpdateClass(objClass))
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
                if (objRep.DeleteClass(id))
                {
                    return Convert.ToString(true);
                }
                return Convert.ToString(false);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult ViewClasses()
        {
            List<Class> classList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                classList = objRep.GetClasses();
            }
            else
            {
                classList = objRep.GetClasses(organizationId: _userStatistics.OrganizationId);
            }

            return PartialView(classList);
        }

        private void LoadSelectLists(out SelectList organizationList, out SelectList courseList, long organizationId = -1, long courseId = -1)
        {
            organizationList = null;
            courseList = null;
            List<Organization> objOrganizationList = new List<Organization>();
            List<Course> objCourseList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                objOrganizationList = objRep.SelectOrganizations();
                if (organizationId != -1)
                {
                    objCourseList = objRep.GetCourses(organizationId: organizationId);
                }
                else
                {
                    objCourseList = new List<Course>();
                }
            }
            else
            {
                var organization = objRep.SelectOrganizations(_userStatistics.OrganizationId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;
                objCourseList = objRep.GetCourses(organizationId: organization.OrganizationId);
            }
            organizationList = new SelectList(objOrganizationList, "OrganizationId", "OrganizationName", organizationId);
            courseList = new SelectList(objCourseList, "CourseId", "CourseName", courseId);
        }

        public JsonResult GetCourse(long id)
        {
            List<Course> courseList = db.Courses.Include("Organization").Where(x => x.OrganisationId == id).ToList();
            return Json(courseList, JsonRequestBehavior.AllowGet);
        }
    }
}