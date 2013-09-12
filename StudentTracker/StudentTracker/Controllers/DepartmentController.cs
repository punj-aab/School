using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;

namespace StudentTracker.Controllers
{
    public class DepartmentController : Controller
    {
        private StudentContext db = new StudentContext();

        //
        // GET: /Department/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Department/Details/5

        public ActionResult Details(long id = 0)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            department.OrganizationList = LoadSelectLists();
            return PartialView(department);
        }

        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            Department objDepartment = new Department();
            objDepartment.OrganizationList = LoadSelectLists();
            objDepartment.OrganizationId = ViewBag.OrganizationId == null ? 0 : ViewBag.OrganizationId;
            return PartialView(objDepartment);
        }

        //
        // POST: /Department/Create

        [HttpPost]
        public bool Create(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //department.CreatedBy = User.Identity.Name;
                    department.CreatedDate = DateTime.Now;
                    db.Departments.Add(department);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //
        // GET: /Department/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            department.OrganizationList = LoadSelectLists(department.OrganizationId);
            department.OrganizationId = ViewBag.OrganizationId == null ? department.OrganizationId : ViewBag.OrganizationId;
            return PartialView(department);
        }

        //
        // POST: /Department/Edit/5

        [HttpPost]
        public string Edit(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(department).State = EntityState.Modified;
                    db.SaveChanges();
                    return Convert.ToString(true);
                }
                return Convert.ToString(false);
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

        //
        // GET: /Department/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // POST: /Department/Delete/5

        [HttpPost]
        public bool DeleteConfirmed(long id)
        {
            try
            {
                Department department = db.Departments.Find(id);
                db.Departments.Remove(department);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult ViewDepartments()
        {
            List<Department> objDepartmentsList = new List<Department>();
            if (User.IsInRole("SiteAdmin"))
            {
                objDepartmentsList = db.Departments.ToList();
            }
            else
            {
                var organization = db.Organizations.Single(x => x.UserName == User.Identity.Name);
                objDepartmentsList = db.Departments.Where(x => x.OrganizationId == organization.OrganizationId).ToList();
            }

            return PartialView(objDepartmentsList);
        }

        public SelectList LoadSelectLists(long id = -1)
        {
            SelectList OrganizationList = null;
            List<Organization> organizationList = new List<Organization>();
            organizationList = db.Organizations.ToList();
            if (User.IsInRole("SiteAdmin"))
            {
                OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", "");
            }
            else
            {
                OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", id);
                var organization = db.Organizations.Single(x => x.UserName == User.Identity.Name);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;

                //OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", organization.OrganizationId);
            }
           
            return OrganizationList;
        }
    }
}