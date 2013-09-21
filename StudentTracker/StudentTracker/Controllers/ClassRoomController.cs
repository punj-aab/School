using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class ClassRoomController : BaseController
    {
        StudentContext db = new StudentContext();
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Class/Details/5

        public ActionResult Details(long id = 0)
        {
            ClassRoom objClass = db.ClassRooms.Find(id);
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
            ClassRoom objClass = new ClassRoom();
            objClass.DepartmentList = LoadSelectList();
            return PartialView(objClass);
        }

        //create post
        [HttpPost]
        public string Create(ClassRoom objClass)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objClass.InsertedOn = DateTime.Now;
                    objClass.InsertedBy = _userStatistics.UserId;
                    db.ClassRooms.Add(objClass);
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
            ClassRoom objClass = db.ClassRooms.Find(id);
            if (objClass == null)
            {
                return HttpNotFound();
            }
            objClass.DepartmentList = LoadSelectList(objClass.DepartmentId);
            return PartialView(objClass);
        }

        //
        // POST: /Class/Edit/5

        [HttpPost]
        public string Edit(ClassRoom objClass)
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
                ClassRoom objClass = db.ClassRooms.Find(id);
                db.ClassRooms.Remove(objClass);
                db.SaveChanges();
                return Convert.ToString(true);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public ActionResult ViewClassRooms()
        {
            List<ClassRoom> classList = db.ClassRooms.ToList();
            return PartialView(classList);
        }
        public SelectList LoadSelectList(long id = -1)
        {
            SelectList list = new SelectList(db.Departments.ToList(), "DepartmentId", "DepartmentName", id);
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
