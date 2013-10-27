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
using StudentTracker.Core.Utilities;
using StudentTracker.Repository;
namespace StudentTracker.Controllers
{
    [Authorize]
    public class OrganizationController : BaseController
    {

        // GET: /Organization/
        StudentContext db = new StudentContext();
        StudentRepository objRep = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SiteAdmin")]
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
        public string AddOrganizations(string token, Organization objOrganization)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (StudentContext db = new StudentContext())
                    {
                        objOrganization.CreatedDate = DateTime.Now;
                        objOrganization.CreatedBy = _userStatistics.UserId;
                        RegistrationToken objToken = new RegistrationToken();
                        objToken.Token = UserStatistics.GenerateToken();
                        objToken.RoleId = (int)UserRoles.OrganizationAdmin;
                        objToken.CreatedBy = _userStatistics.UserId;
                        if (objRep.CreateOrganization(objOrganization, objToken))
                        {
                            //objRep.CreateRegistrationToken(objToken);
                            SaveFiles(token, this.GetType().Name, objOrganization.OrganizationId);
                            EmailHandler.Utilities.SendRegistationEmail(objToken.Token, objOrganization.Email);
                            return Convert.ToString(true);
                        }
                        return Convert.ToString(false);
                    }
                }
                return Convert.ToString(false);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        public void LoadSelectLists(out SelectList ContList, out SelectList stateList, out SelectList organizationTypeList, int countryId = -1, int organizationTypeId = -1, long RegionId = -1)
        {
            List<Country> countryList = null;
            List<State> regionList = null;
            using (StudentContext db = new StudentContext())
            {
                countryList = db.Countries.ToList();
                if (countryId != -1)
                {
                    var country = db.Countries.Where(c => c.Id == countryId).FirstOrDefault();
                    regionList = db.States.Where(x => x.CountryCode == country.CountryCode).ToList();
                }
                else
                {
                    regionList = new List<State>();
                }
            }
            List<SelectListItem> organizationTypes = Enum.GetValues(typeof(StudentTracker.Core.Utilities.OrganizationTypes)).Cast<StudentTracker.Core.Utilities.OrganizationTypes>().Select(v => new SelectListItem
{
    Text = v.ToString(),
    Value = ((int)v).ToString()
}).ToList();
            organizationTypeList = new SelectList(organizationTypes, "Value", "Text", organizationTypeId);
            ContList = new SelectList(countryList, "Id", "CountryName", countryId);
            stateList = new SelectList(regionList, "id", "StateName", RegionId);
        }

        public ActionResult ViewOrganizations()
        {
            OrganizationsViewModel objViewModel = new OrganizationsViewModel();
            if (User.IsInRole("SiteAdmin"))
                objViewModel.OrganizationList = objRep.SelectOrganizations();
            else
            {
                objViewModel.OrganizationList = new List<Organization>();
                objViewModel.OrganizationList.Add(objRep.SelectOrganizations(_userStatistics.OrganizationId));
            }
            return PartialView(objViewModel);
        }
        public ActionResult Details(long id)
        {
            Organization organization = objRep.SelectOrganizations(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return PartialView(organization);
        }

        
        public ActionResult Edit(long id)
        {
            Organization objModel = objRep.SelectOrganizations(id);
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

            return PartialView(objModel);

        }

        [HttpPost]
        public string Edit(DBConnectionString.Organization organization, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    organization.ModifiedBy = _userStatistics.UserId;
                    if (organization.Update() > 0)
                    {
                        SaveFiles(token, this.GetType().Name, organization.OrganizationId);
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

        [Authorize(Roles = "SiteAdmin")]
        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                if (objRep.DeleteOrganization(id))
                {
                    //DeleteFiles(this.GetType().Name, id);
                    return Convert.ToString(true);
                }
                return Convert.ToString(false);
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
            Country country = db.Countries.Where(s => s.Id == id).FirstOrDefault();
            List<State> regionList = db.States.Where(x => x.CountryCode == country.CountryCode).ToList();
            return Json(regionList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUser(string Username)
        {
            var user = db.Users.Where(x => x.Username == Username).SingleOrDefault();
            if (user != null)
            {
                return Json("User with this name already exist", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
