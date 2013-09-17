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
    public class SectionController : Controller
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
            Section objSection = db.Sections.Find(id);
            if (objSection == null)
            {
                return HttpNotFound();
            }
            return PartialView(objSection);
        }

       //GET CREATE
        public ActionResult Create()
        {
            Section objSection = new Section();
            objSection.ClassList = LoadSelectLists();
            return PartialView(objSection);
        }

        //POST CREATE
        [HttpPost]
        public string Create(Section section)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    section.InsertedOn = DateTime.Now;
                    db.Sections.Add(section);
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

       //GET EDIT
        public ActionResult Edit(long id = 0)
        {
            Section objSection = db.Sections.Find(id);
            if (objSection == null)
            {
                return HttpNotFound();
            }
            objSection.ClassList = LoadSelectLists(objSection.ClassId);
            return PartialView(objSection);
        }

        //POST EDIT
        [HttpPost]
        public string Edit(Section objSection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objSection.ModifiedOn = DateTime.Now;
                    db.Entry(objSection).State = EntityState.Modified;
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

        //DELETE SECTION
        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                Section section = db.Sections.Find(id);
                db.Sections.Remove(section);
                db.SaveChanges();
                return Convert.ToString(true);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //VIEW ALL SECTIONS
        public ActionResult ViewSections()
        {
            List<Section> sectionList = db.Sections.ToList();
            return PartialView(sectionList);
        }

        //LOAD SELECT LIST
        public SelectList LoadSelectLists(long id = -1)
        {
            SelectList list = new SelectList(db.Classes.ToList(), "ClassId", "ClassName", id);
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
