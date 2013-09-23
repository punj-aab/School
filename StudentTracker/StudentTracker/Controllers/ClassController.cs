using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
using StudentTracker.Core.DAL;

namespace StudentTracker.Controllers
{
    public class ClassController : BaseController
    {
        private StudentContext db = new StudentContext();

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
            Class objClass = db.Classes.Find(id);
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
                    db.Classes.Add(objClass);
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
                    objClass.ModifiedOn = DateTime.Now;
                    objClass.ModifiedBy = _userStatistics.UserId;
                    db.Entry(objClass).State = EntityState.Modified;
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
                Class objClass = db.Classes.Find(id);
                db.Classes.Remove(objClass);
                db.SaveChanges();
                return Convert.ToString(true);
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
            List<Class> classList = db.Classes.ToList();
            return PartialView(classList);
        }

        private void LoadSelectLists(out SelectList organizationList, out SelectList courseList, long organizationId = -1, long courseId = -1)
        {
            organizationList = null;
            courseList = null;
            List<Organization> objOrganizationList = null;
            List<Course> objCourseList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                objOrganizationList = db.Organizations.ToList();
                if (organizationId != -1)
                {
                    objCourseList = db.Courses.Where(x => x.OrganisationId == organizationId).ToList();
                }
                else
                {
                    objCourseList = new List<Course>();
                }
            }
            else
            {
                var organization = db.Organizations.Single(x => x.CreatedBy == _userStatistics.UserId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;
                objCourseList = db.Courses.Where(x => x.OrganisationId == organization.OrganizationId).ToList();
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