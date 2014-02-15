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
            obj.OrganizationId = _userStatistics.OrganizationId;
           
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

            }
            OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", id);
            return OrganizationList;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoadServices(long organizationId)
        {
            try
            {
                OrganizationServicesViewModel obj = new OrganizationServicesViewModel();
                if (organizationId != 0)
                {
                    obj.OrganizationId = organizationId;
                    obj.Servcies = objRep.GetOrganizationServices(organizationId);
                }
                else
                {
                    return HttpNotFound();
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddRemoveService(int id, long organizationId, int serviceId, bool value)
        {
            StudentContext db = new StudentContext();

            OrganizationServices orgService = db.OrganizationServices.SingleOrDefault(s => s.Id == id && s.OrganizationId == organizationId && s.ServiceId == serviceId);
            if (orgService != null)
            {
                orgService.StatusId = value ? 1 : 2;
                orgService.UpdatedBy = _userStatistics.UserId;
                orgService.UpdatedOn = DateTime.Now;

            }
            else
            {
                orgService = new OrganizationServices
                {
                    StatusId = value ? 1 : 2,
                    ServiceId = serviceId,
                    OrganizationId = organizationId,
                    InsertedBy = _userStatistics.UserId,
                    InsertedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };
                db.OrganizationServices.Add(orgService);
            }
            db.SaveChanges();
            return true;
        }
    }
}
