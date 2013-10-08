using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Repository;
namespace StudentTracker.Controllers
{
    public class SectionController : BaseController
    {
        private StudentContext db = new StudentContext();
        SectionRepository objRep = new SectionRepository();
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
            Section objSection = objRep.GetSections(id);
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
                    section.CreatedBy = _userStatistics.UserId;
                    if (objRep.CreateSection(section))
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

        //GET EDIT
        public ActionResult Edit(long id = 0)
        {
            Section objSection = objRep.GetSections(id);
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
                    objSection.ModifiedBy = _userStatistics.UserId;
                    if (objRep.Update(objSection))
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

        //DELETE SECTION
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

        //VIEW ALL SECTIONS
        public ActionResult ViewSections()
        {
            List<Section> sectionList = objRep.GetSections();
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
