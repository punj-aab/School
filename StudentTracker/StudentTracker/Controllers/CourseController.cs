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
    public class CourseController : Controller
    {
        private StudentContext db = new StudentContext();

        //
        // GET: /Cource/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Cource/Details/5

        public ActionResult Details(long id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return PartialView(course);
        }

        //
        // GET: /Cource/Create

        public ActionResult Create()
        {
            Course objCourse = new Course();
            objCourse.OrganizationList = LoadSelectLists();
            objCourse.OrganisationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(objCourse);
        }

        //
        // POST: /Cource/Create

        [HttpPost]
        public string Create(Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
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
        // GET: /Cource/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Course objCourse = db.Courses.Find(id);
            objCourse.OrganizationList = LoadSelectLists();
            objCourse.OrganisationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);
            if (objCourse == null)
            {
                return HttpNotFound();
            }
            return PartialView(objCourse);
        }

        //
        // POST: /Cource/Edit/5

        [HttpPost]
        public string Edit(Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(course).State = EntityState.Modified;
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
        // GET: /Cource/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // POST: /Cource/Delete/5

        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                Course course = db.Courses.Find(id);
                db.Courses.Remove(course);
                db.SaveChanges();
                return Convert.ToString(true);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public ActionResult ViewCourses()
        {
            List<Course> objCourceList = db.Courses.ToList();
            return PartialView(objCourceList);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public SelectList LoadSelectLists()
        {
            SelectList OrganizationList = null;
            List<Organization> organizationList = new List<Organization>();
            organizationList = db.Organizations.ToList();
            if (User.IsInRole("SiteAdmin"))
            {
                OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", "");
            }
            else
            {
                var organization = db.Organizations.Single(x => x.UserName == User.Identity.Name);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;

                //OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", organization.OrganizationId);
            }
            return OrganizationList;
        }
    }
}