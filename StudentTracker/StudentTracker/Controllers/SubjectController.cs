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
    public class SubjectController : Controller
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
            objSubject.CourseList = LoadSelectLists();
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
                    db.Subjects.Add(subject);
                    db.SaveChanges();
                    return Convert.ToString(true);
                }

                ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subject.CourseId);
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
            subject.CourseList = LoadSelectLists(subject.CourseId);
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

        public SelectList LoadSelectLists(long id = -1)
        {
            SelectList list = null;
            if (id == -1)
            {
                list = new SelectList(db.Courses.ToList(), "CourseId", "CourseName", "");
            }
            else
            {
                list = new SelectList(db.Courses.ToList(), "CourseId", "CourseName", id);
            }
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}