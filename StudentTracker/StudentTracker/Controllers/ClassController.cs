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
    public class ClassController : Controller
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
            objClass.OrganizationList = LoadSelectList();
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
            objClass.OrganizationList = LoadSelectList(objClass.OrganizationId);
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

        public SelectList LoadSelectList(long organizationId = -1)
        {
            SelectList list = new SelectList(db.Organizations.ToList(), "OrganizationId", "OrganizationName", organizationId);
            return list;
        }
    }
}