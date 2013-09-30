using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Repository;
namespace StudentTracker.Controllers
{
    public class DepartmentController : BaseController
    {
        private StudentContext db = new StudentContext();
        private DepartmentRepository objRep = new DepartmentRepository();
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
            Department department = objRep.GetDepartments(id);
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
                    department.CreatedBy = _userStatistics.UserId;
                    department.CreatedDate = DateTime.Now;
                    if (objRep.CreateDepartment(department))
                    {
                        return true;
                    }
                    return false;
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
            Department department = objRep.GetDepartments(id);
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
                    department.UpdatedBy = _userStatistics.UserId;
                    if (objRep.UpdateDepartment(department))
                    {
                        return Convert.ToString(true);
                    }
                    return Convert.ToString(false);
                }
                return Convert.ToString(false);
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

       //delete
        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                if (objRep.DeleteDepartment(id))
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult ViewDepartments()
        {
            List<Department> objDepartmentsList = objRep.GetDepartments();
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
                var organization = db.Organizations.Single(x => x.OrganizationName == User.Identity.Name);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;

                //OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", organization.OrganizationId);
            }

            return OrganizationList;
        }
    }
}