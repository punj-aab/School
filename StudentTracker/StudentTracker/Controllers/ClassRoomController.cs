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
    public class ClassRoomController : BaseController
    {
        StudentRepository objRep = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Class/Details/5

        public ActionResult Details(long id = 0)
        {
            ClassRoom objClass = objRep.GetClassRooms(id);
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
            SelectList departmentList = null;
            SelectList organizationList = null;
            LoadSelectList(out organizationList, out departmentList);
            objClass.DepartmentList = departmentList;
            objClass.OrganizationList = organizationList;
            objClass.OrganizationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(objClass);
        }

        //create post
        [HttpPost]
        public string Create(ClassRoom objClass, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objClass.InsertedBy = _userStatistics.UserId;
                    if (objRep.CreateClassRoom(objClass))
                    {
                        SaveFiles(token, this.GetType().Name, objClass.ClassRoomId);
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

        //
        // GET: /Class/Edit/5

        public ActionResult Edit(long id = 0)
        {
            ClassRoom objClass = objRep.GetClassRooms(id);
            if (objClass == null)
            {
                return HttpNotFound();
            }
            SelectList departmentList = null;
            SelectList organizationList = null;
            LoadSelectList(out organizationList, out departmentList, objClass.OrganizationId, objClass.DepartmentId);
            objClass.DepartmentList = departmentList;
            objClass.OrganizationList = organizationList;
            objClass.OrganizationId = ViewBag.OrganizationId == null ? objClass.OrganizationId : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(objClass);
        }

        //
        // POST: /Class/Edit/5

        [HttpPost]
        public string Edit(DBConnectionString.ClassRoom objClass, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objClass.ModifiedBy = _userStatistics.UserId;
                    objClass.ModifiedOn = DateTime.Now;
                    if (objClass.Update() > 0)
                    {
                        SaveFiles(token, this.GetType().Name, objClass.ClassRoomId);
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

        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                if (objRep.DeleteClassRoom(id))
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

        public ActionResult ViewClassRooms()
        {
            List<ClassRoom> classList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                classList = objRep.GetClassRooms();
            }
            else
            {
                classList = objRep.GetClassRooms(organizationId: _userStatistics.OrganizationId);
            }
            return PartialView(classList);
        }
        public void LoadSelectList(out SelectList organizationList, out SelectList deparmentList, long organizationId = -1, long departmentId = -1)
        {
            organizationList = null;
            deparmentList = null;
            List<Department> objDeparmentList = null;
            List<Organization> objOrganizationList = null;
            objOrganizationList = new List<Organization>();
            if (User.IsInRole("SiteAdmin"))
            {
                objOrganizationList = objRep.SelectOrganizations();
            }
            else
            {
                var organization = objRep.SelectOrganizations(_userStatistics.OrganizationId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;
            }

            if (departmentId != -1)
            {
                objDeparmentList = objRep.GetDepartments(organizationId: organizationId);
            }
            else
            {
                objDeparmentList = new List<Department>();
            }
            organizationList = new SelectList(objOrganizationList, "OrganizationId", "OrganizationName", organizationId);
            deparmentList = new SelectList(objDeparmentList, "DepartmentId", "DepartmentName", departmentId);
        }

        public JsonResult GetDepartments(long id)
        {
            List<Department> classList = objRep.DepartmenstByOrganization(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }
    }
}
