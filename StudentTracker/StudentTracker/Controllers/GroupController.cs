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
            Group objGroup = new Group();
            objGroup.OrganizationList = LoadSelectLists();
            objGroup.OrganizationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(objGroup);
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
            objGroup.OrganizationList = LoadSelectLists(objGroup.OrganizationId);
            objGroup.OrganizationId = ViewBag.OrganizationId == null ? objGroup.OrganizationId : Convert.ToInt32(ViewBag.OrganizationId);
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
            if (User.IsInRole("SiteAdmin"))
            {
                GroupList = objRep.GetGroups();
            }
            else
            {
                GroupList = objRep.GetGroups(organizationId: _userStatistics.OrganizationId);
            }
            return PartialView(GroupList);
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
