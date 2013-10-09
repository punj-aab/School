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
        StudentContext db = new StudentContext();
        ClassRoomRepository objRep = new ClassRoomRepository();
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
            objClass.DepartmentList = LoadSelectList();
            return PartialView(objClass);
        }

        //create post
        [HttpPost]
        public string Create(ClassRoom objClass,string token)
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
                    objClass.ModifiedBy = _userStatistics.UserId;
                    if (objRep.UpdateClassRoom(objClass))
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

        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                if (objRep.DeleteClassRoom(id))
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
        public SelectList LoadSelectList(long id = -1)
        {
            List<Department> deparmentList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                deparmentList = objRep.GetDepartments();
            }
            else
            {
                deparmentList = objRep.GetDepartments(organizationId: _userStatistics.OrganizationId);
            }
            SelectList list = new SelectList(deparmentList, "DepartmentId", "DepartmentName", id);
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
