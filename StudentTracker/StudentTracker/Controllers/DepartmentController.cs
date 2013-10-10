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
        private DepartmentRepository objRep = new DepartmentRepository();

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
        public bool Create(Department department, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    department.CreatedBy = _userStatistics.UserId;
                    department.CreatedDate = DateTime.Now;
                    if (objRep.CreateDepartment(department))
                    {
                        SaveFiles(token, this.GetType().Name, department.DepartmentId);
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
        public string Edit(Department department, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    department.UpdatedBy = _userStatistics.UserId;
                    if (objRep.UpdateDepartment(department))
                    {
                        SaveFiles(token, this.GetType().Name, department.DepartmentId);
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
                    DeleteFiles(this.GetType().Name, id);
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
            objRep = null;
            base.Dispose(disposing);
        }

        public ActionResult ViewDepartments()
        {
            List<Department> objDepartmentsList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                objDepartmentsList = objRep.GetDepartments();
            }
            else
            {
                objDepartmentsList = objRep.GetDepartments(organizationId: _userStatistics.OrganizationId);
            }
            return PartialView(objDepartmentsList);
        }

        public SelectList LoadSelectLists(long id = -1)
        {
            SelectList OrganizationList = null;
            List<Organization> organizationList = new List<Organization>();

            if (User.IsInRole("SiteAdmin"))
            {
                organizationList = objRep.SelectOrganizations();
            }
            else
            {
                var organization = objRep.SelectOrganizations(_userStatistics.OrganizationId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;

                //OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", organization.OrganizationId);
            }
            OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", id);
            return OrganizationList;
        }
    }
}