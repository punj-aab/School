using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
using StudentTracker.Core.DAL;
using StudentTracker.Core.Repository;
using StudentTracker.Repository;
namespace StudentTracker.Controllers
{
    public class SubjectController : BaseController
    {
        private StudentContext db = new StudentContext();
        StudentRepository objRep = new StudentRepository();

        //
        // GET: /Subject/
        public SubjectController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Subject/Details/5

        public ActionResult Details(long id = 0)
        {
            Subject subject = objRep.GetSubjects(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return PartialView(subject);
        }

        //
        // GET: /Subject/Create

        public ActionResult Create()
        {
            Subject objSubject = new Subject();
            SelectList courseList = null;
            SelectList classList = null;
            SelectList organizationList = null;
            LoadSelectLists(out classList, out courseList, out organizationList, false);
            objSubject.CourseList = courseList;
            objSubject.ClassList = classList;
            objSubject.OrganizationList = organizationList;
            objSubject.OrganizationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(objSubject);
        }

        //
        // POST: /Subject/Create

        [HttpPost]
        public string Create(Subject subject, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    subject.CreatedBy = _userStatistics.UserId;
                    if (objRep.CreateSubject(subject))
                    {
                        SaveFiles(token, this.GetType().Name, subject.SubjectId);
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
        // GET: /Subject/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Subject subject = objRep.GetSubjects(subjectId: id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            SelectList courseList = null;
            SelectList classList = null;
            SelectList organizationList = null;
            LoadSelectLists(out classList, out courseList, out organizationList, true, subject.ClassId, subject.CourseId, subject.OrganizationId);
            subject.CourseList = courseList;
            subject.ClassList = classList;
            subject.OrganizationList = organizationList;
            subject.OrganizationId = ViewBag.OrganizationId == null ? subject.OrganizationId : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(subject);
        }

        //
        // POST: /Subject/Edit/5

        [HttpPost]
        public string Edit(DBConnectionString.Subject subject, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    subject.ModifiedBy = _userStatistics.UserId;
                    subject.ModifiedOn = DateTime.Now;
                    if (subject.Update() > 0)
                    {
                        SaveFiles(token, this.GetType().Name, subject.SubjectId);
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
        // POST: /Subject/Delete/5
        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                if (objRep.DeleteSubject(id))
                {
                    DeleteFiles(this.GetType().Name, id);
                    return Convert.ToString(true);
                }
                return Convert.ToString(false);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public ActionResult ViewSubjects()
        {
            List<Subject> subjectList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                subjectList = objRep.GetSubjects();
            }
            else
            {
                subjectList = objRep.GetSubjects(organizationId: _userStatistics.OrganizationId);
            }
            return PartialView(subjectList);
        }

        private void LoadSelectLists(out SelectList classList, out SelectList courseList, out SelectList organizationList, bool isEdit, long classId = -1, long courseId = -1, long organizationId = -1)
        {
            classList = null;
            courseList = null;
            organizationList = null;

            List<Course> objCourseList = null;
            List<Class> objClassList = null;
            List<Organization> objorganizationList = new List<Organization>();

            if (User.IsInRole("SiteAdmin"))
            {
                objorganizationList = objRep.SelectOrganizations();

            }
            else
            {
                var organization = objRep.SelectOrganizations(_userStatistics.OrganizationId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;
            }

            if (isEdit)
            {
                objCourseList = objRep.GetCourses(organizationId: organizationId);
                objClassList = objRep.ClassByCourse(courseId);// db.Classes.Where(x => x.CourseId == courseId).ToList();
            }
            else
            {
                objCourseList = objRep.GetCourses(organizationId: _userStatistics.OrganizationId);
                //objCourseList = new List<Course>();
                objClassList = new List<Class>();
            }

            organizationList = new SelectList(objorganizationList, "OrganizationId", "OrganizationName", organizationId);
            courseList = new SelectList(objCourseList, "CourseId", "CourseName", courseId);
            classList = new SelectList(objClassList, "ClassId", "ClassName", classId);

        }

        public JsonResult GetClasses(long id)
        {
            List<Class> classList = objRep.ClassByCourse(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCourses(long id)
        {
            List<Course> classList = objRep.CourseByOrganization(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}