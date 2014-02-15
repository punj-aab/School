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
        StudentRepository repository = new StudentRepository();
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
            Section objSection = repository.GetSections(id);
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
            SelectList courseList = null;
            SelectList classList = null;
            SelectList organizationList = null;
            LoadSelectLists(out classList, out courseList, out organizationList, false);
            objSection.CourseList = courseList;
            objSection.ClassList = classList;
            objSection.OrganizationList = organizationList;
            objSection.OrganizationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(objSection);
        }

        //POST CREATE
        [HttpPost]
        public string Create(DBConnectionString.Section section)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    section.CreatedBy = _userStatistics.UserId;
                    section.InsertedOn = DateTime.Now;
                    if (Convert.ToInt32(section.Insert()) > 0)
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
            Section objSection = repository.GetSections(id);
            if (objSection == null)
            {
                return HttpNotFound();
            }
            SelectList courseList = null;
            SelectList classList = null;
            SelectList organizationList = null;
            LoadSelectLists(out classList, out courseList, out organizationList, true, objSection.ClassId, objSection.CourseId, objSection.OrganizationId.Value);
            objSection.CourseList = courseList;
            objSection.ClassList = classList;
            objSection.OrganizationList = organizationList;
            objSection.OrganizationId = ViewBag.OrganizationId == null ? objSection.OrganizationId : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(objSection);
        }

        //POST EDIT
        [HttpPost]
        public string Edit(DBConnectionString.Section objSection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objSection.ModifiedBy = _userStatistics.UserId;
                    objSection.ModifiedOn = DateTime.Now;
                    if (objSection.Update() > 0)
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
                if (DBConnectionString.Section.Delete(id) > 0)
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
            List<Section> sectionList = repository.GetSections();
            return PartialView(sectionList);
        }

        //LOAD SELECT LIST
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
                objorganizationList = repository.SelectOrganizations();

            }
            else
            {
                var organization = repository.SelectOrganizations(_userStatistics.OrganizationId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;
            }

            if (isEdit)
            {
                objCourseList = repository.GetCourses(organizationId: _userStatistics.OrganizationId);
                objClassList = repository.ClassByCourse(courseId);// db.Classes.Where(x => x.CourseId == courseId).ToList();
            }
            else
            {
                objCourseList = repository.GetCourses(organizationId: _userStatistics.OrganizationId);
                objClassList = new List<Class>();
            }

            organizationList = new SelectList(objorganizationList, "OrganizationId", "OrganizationName", organizationId);
            courseList = new SelectList(objCourseList, "CourseId", "CourseName", courseId);
            classList = new SelectList(objClassList, "ClassId", "ClassName", classId);

        }

        public JsonResult GetClasses(long id)
        {
            List<Class> classList = repository.ClassByCourse(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourses(long id)
        {
            List<Course> classList = repository.CourseByOrganization(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
