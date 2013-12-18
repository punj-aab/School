using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Repository;
using StudentTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    [Authorize(Roles = "OrganizationAdmin,SiteAdmin")]
    public class ServicesController : BaseController
    {
        //
        // GET: /Services/
        private StudentContext db = new StudentContext();
        OrganizationRepository objRep = new OrganizationRepository();
        public ActionResult Index()
        {
            OrganizationServicesViewModel obj = new OrganizationServicesViewModel();
            ViewBag.OrganizationList = LoadSelectLists();
            obj.OrganizationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);
            if (_userStatistics.OrganizationId != 0)
            {
                obj.Servcies = objRep.GetOrganizationServices(_userStatistics.OrganizationId);
            }

            return View(obj);
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
