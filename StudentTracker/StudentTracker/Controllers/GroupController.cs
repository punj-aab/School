using StudentTracker.Core.Entities;
using StudentTracker.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace StudentTracker.Controllers
{
    public class GroupController : BaseController
    {
        //
        // GET: /Group/
        CommonRepository objRep = new CommonRepository();
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Group/Details/5

        public ActionResult Details(long id = 0)
        {
            Group objGroup = objRep.GetGroups(id);
            if (objGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(objGroup);
        }

        //
        // GET: /Group/Create

        public ActionResult Create()
        {
            return PartialView();
        }

        //
        // POST: /Group/Create

        [HttpPost]
        public string Create(Group objGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objGroup.InsertedOn = DateTime.Now;
                    objGroup.InsertedBy = _userStatistics.UserId;
                    if (objRep.CreateGroup(objGroup))
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

        //
        // GET: /Group/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Group objGroup = objRep.GetGroups(id);
            return PartialView(objGroup);
        }

        //
        // POST: /Group/Edit/5

        [HttpPost]
        public string Edit(DBConnectionString.Group objGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objGroup.ModifieBy = _userStatistics.UserId;
                    objGroup.ModifiedOn = DateTime.Now;
                    int recAffected = objGroup.Update();
                    if (recAffected > 0)
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
                if (objRep.DeleteGroup(id))
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
            objRep = null;
            base.Dispose(disposing);
        }

        public ActionResult ViewGroups()
        {
            List<Group> GroupList = null;
            GroupList = objRep.GetGroups();
            return PartialView(GroupList);
        }

    }
}
