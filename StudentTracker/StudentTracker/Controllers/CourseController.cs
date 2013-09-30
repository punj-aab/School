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
    public class CourseController : BaseController
    {
        private StudentContext db = new StudentContext();
        CourseRepository objRep = new CourseRepository();
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
            Course course = objRep.GetCourses(id);
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
                    course.CreatedBy = _userStatistics.UserId;
                    if (objRep.Create(course))
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
        // GET: /Cource/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Course objCourse = objRep.GetCourses(id);
            objCourse.OrganizationList = LoadSelectLists(objCourse.OrganisationId);
            objCourse.OrganisationId = ViewBag.OrganizationId == null ? objCourse.OrganisationId : Convert.ToInt32(ViewBag.OrganizationId);
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
                    course.ModifiedBy = _userStatistics.UserId;
                    course.ModifiedOn = DateTime.UtcNow;
                    if (objRep.Update(course))
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
        // POST: /Cource/Delete/5
        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                if (objRep.Delete(id))
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

        public ActionResult ViewCourses()
        {
            List<Course> objCourceList = objRep.GetCourses();
            return PartialView(objCourceList);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public SelectList LoadSelectLists(int id = -1)
        {
            SelectList OrganizationList = null;
            List<Organization> organizationList = new List<Organization>();
            organizationList = db.Organizations.ToList();
            if (User.IsInRole("SiteAdmin"))
            {
                OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", id);
            }
            else
            {
                var organization = db.Organizations.SingleOrDefault(x => x.CreatedBy == _userStatistics.UserId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;

                //OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", organization.OrganizationId);
            }
            return OrganizationList;
        }
    }
}