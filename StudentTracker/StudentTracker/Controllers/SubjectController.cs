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
    public class SubjectController : BaseController
    {
        private StudentContext db = new StudentContext();
        SubjectRepository objRep = new SubjectRepository();

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
            LoadSelectLists(out classList, out courseList);
            objSubject.CourseList = courseList;
            objSubject.ClassList = classList;
            return PartialView(objSubject);
        }

        //
        // POST: /Subject/Create

        [HttpPost]
        public string Create(Subject subject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    subject.CreatedBy = _userStatistics.UserId;
                    if (objRep.CreateSubject(subject))
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
            LoadSelectLists(out classList, out courseList, subject.ClassId, subject.CourseId);
            subject.CourseList = courseList;
            subject.ClassList = classList;
            return PartialView(subject);
        }

        //
        // POST: /Subject/Edit/5

        [HttpPost]
        public string Edit(Subject subject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    subject.ModifiedBy = _userStatistics.UserId;
                    if (objRep.Update(subject))
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
        // POST: /Subject/Delete/5
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

        private void LoadSelectLists(out SelectList classList, out SelectList courseList, long classId = -1, long courseId = -1)
        {
            classList = null;
            courseList = null;
            List<Course> objCourseList = null;
            List<Class> objClassList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                objCourseList = objRep.GetCourses();
                if (courseId != -1)
                {
                    objClassList = db.Classes.Where(x => x.CourseId == courseId).ToList();
                }
                else
                {
                    objClassList = new List<Class>();
                }
            }
            else
            {
                objCourseList = objRep.GetCourses(organizationId: _userStatistics.OrganizationId);
                if (courseId != -1)
                {
                    objClassList = db.Classes.Where(x => x.CourseId == courseId).ToList();
                }
                else
                {
                    objClassList = new List<Class>();
                }
            }
            courseList = new SelectList(objCourseList, "CourseId", "CourseName", courseId);
            classList = new SelectList(objClassList, "ClassId", "ClassName", classId);
        }

        public JsonResult GetClasses(long id)
        {
            List<Class> classList = db.Classes.Where(x => x.CourseId == id).ToList();
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}