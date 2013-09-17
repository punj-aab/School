using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
using StudentTracker.Core.DAL;
using StudentTracker.Core;
using StudentTracker.ViewModels;
using System.Web.Security;
using System.Data;
namespace StudentTracker.Controllers
{
    public class OrganizationController : BaseController
    {
        //
        // GET: /Organization/
        StudentContext db = new StudentContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrganizations()
        {
            SelectList ContList = null;
            SelectList stateList = null;
            SelectList organizationTypeList = null;
            LoadSelectLists(out ContList, out stateList, out organizationTypeList);
            Organization objModel = new Organization();
            objModel.CountryList = ContList;
            objModel.StateList = stateList;
            objModel.OrganizationTypeList = organizationTypeList;
            return PartialView(objModel);
        }

        [HttpPost]
        public bool AddOrganizations(Organization objOrganization)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (StudentContext db = new StudentContext())
                    {
                        objOrganization.CreatedDate = DateTime.Now;
                        objOrganization.CreatedBy = _userStatistics.UserId;
                        db.Organizations.Add(objOrganization);
                        WebSecurity.Register(objOrganization.UserName, objOrganization.Password, objOrganization.Email, false, "", "");
                        Roles.AddUserToRole(objOrganization.UserName, "OrganizationAdmin");
                        db.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void LoadSelectLists(out SelectList ContList, out SelectList stateList, out SelectList organizationTypeList, int countryId = -1, int organizationTypeId = -1, long RegionId = -1)
        {
            List<Country> countryList = new List<Country>();
            List<Region> regionList = new List<Region>();
            using (StudentContext db = new StudentContext())
            {
                countryList = db.Countries.ToList();
                regionList = db.Regions.ToList();
            }
            ContList = new SelectList(countryList, "CountryId", "name", countryId);
            stateList = new SelectList(regionList, "RegionId", "name", RegionId);
            List<SelectListItem> organizationTypes = Enum.GetValues(typeof(StudentTracker.Core.Utilities.OrganizationTypes)).Cast<StudentTracker.Core.Utilities.OrganizationTypes>().Select(v => new SelectListItem
{
    Text = v.ToString(),//.Replace("_", " "),
    Value = ((int)v).ToString()
}).ToList();
            //            ViewBag.OrganizationTypeId = organizationTypes;
            organizationTypeList = new SelectList(organizationTypes, "Value", "Text", organizationTypeId);
        }

        public ActionResult ViewOrganizations()
        {
            OrganizationsViewModel objViewModel = new OrganizationsViewModel();
            objViewModel.OrganizationList = OrganizationList();
            return PartialView(objViewModel);
        }
        public ActionResult Details(int id)
        {
            Organization organization = db.Organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return PartialView(organization);

        }

        public ActionResult Edit(long id)
        {
            Organization objModel = db.Organizations.Find(id);
            if (objModel == null)
            {
                return HttpNotFound();
            }
            SelectList ContList = null;
            SelectList stateList = null;
            SelectList organizationTypeList = null;
            LoadSelectLists(out ContList, out stateList, out organizationTypeList, objModel.CountryId, objModel.OrganizationTypeId, objModel.StateId);
            objModel.CountryList = ContList;
            objModel.StateList = stateList;
            objModel.OrganizationTypeList = organizationTypeList;
            objModel.Password = "test@1234";
            objModel.ConfirmPassword = "test@1234";
            objModel.UserName = "TEST";
            return PartialView(objModel);

        }

        [HttpPost]
        public string Edit(Organization organization)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    organization.ModifiedDate = DateTime.Now;
                    organization.ModifiedBy = _userStatistics.UserId;
                    db.Entry(organization).State = EntityState.Modified;
                    db.SaveChanges();
                    return Convert.ToString(true);
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
                Organization organization = db.Organizations.Find(id);
                db.Organizations.Remove(organization);
                db.SaveChanges();
                return Convert.ToString(true);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public List<Organization> OrganizationList()
        {
            List<Organization> orgList = null;
            using (StudentContext db = new StudentContext())
            {
                orgList = new List<Organization>();
                return orgList = db.Organizations.ToList();
            }
        }

        public JsonResult GetRegions(int id)
        {
            List<Region> regionList = db.Regions.Where(x => x.country_id == id).ToList();
            return Json(regionList, JsonRequestBehavior.AllowGet);
        }

    }
}
