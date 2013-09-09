using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
using StudentTracker.Core.DAL;
using StudentTracker.Core;
namespace StudentTracker.Controllers
{
    public class OrganizationController : Controller
    {
        //
        // GET: /Organization/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrganizations()
        {
            List<Country> countryList = new List<Country>();
            LoadSelectLists();
            return PartialView();
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
                        db.Organizations.Add(objOrganization);
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


        public void LoadSelectLists()
        {
            List<Country> countryList = new List<Country>();
            List<Region> regionList = new List<Region>();
            ViewBag.CountryId = new SelectList(countryList, "id", "name", "");
            ViewBag.StateId = new SelectList(regionList, "id", "name", "");
            List<SelectListItem> organizationTypes = Enum.GetValues(typeof(StudentTracker.Core.Utilities.OrganizationTypes)).Cast<StudentTracker.Core.Utilities.OrganizationTypes>().Select(v => new SelectListItem
{
    Text = v.ToString(),//.Replace("_", " "),
    Value = ((int)v).ToString()
}).ToList();
            ViewBag.OrganizationTypeId = organizationTypes;

        }

    }
}
