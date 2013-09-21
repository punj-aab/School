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
    public class SubjectController : BaseController
    {
        private StudentContext db = new StudentContext();

        //
        // GET: /Subject/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Subject/Details/5

        public ActionResult Details(long id = 0)
        {
            Subject subject = db.Subjects.Find(id);
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
                    subject.InsertedOn = DateTime.Now;
                    subject.CreatedBy = _userStatistics.UserId;
                    db.Subjects.Add(subject);
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
        // GET: /Subject/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Subject subject = db.Subjects.Find(id);
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
                    subject.ModifiedOn = DateTime.Now;
                    subject.ModifiedBy = _userStatistics.UserId;
                    db.Entry(subject).State = EntityState.Modified;
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
        // POST: /Subject/Delete/5
        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                Subject subject = db.Subjects.Find(id);
                db.Subjects.Remove(subject);
                db.SaveChanges();
                return Convert.ToString(true);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public ActionResult ViewSubjects()
        {
            List<Subject> subjectList = db.Subjects.ToList();
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
                objCourseList = db.Courses.ToList();
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
                objCourseList = db.Courses.Where(x => x.OrganisationId == _userStatistics.UserId).ToList();
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