using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    [Authorize]
    public class TokenController : BaseController
    {
        //
        // GET: /Token/
        private StudentContext db = new StudentContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateToken()
        {
            RegistrationToken objToken = new RegistrationToken();
            LoadSelectLists(ref objToken);
            objToken.OrganizationId = ViewBag.OrganizationId == null ? 0 : (int)ViewBag.OrganizationId;
            return View(objToken);
        }

        [HttpPost]
        public string Create(RegistrationToken token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    token.Token = UserStatistics.GetToken();
                    token.CreatedBy = _userStatistics.UserId;
                    db.RegistrationTokens.Add(token);
                    db.SaveChanges();
                    return token.Token;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [Authorize]
        public ActionResult ViewToken(RegistrationToken token)
        {
            ViewBag.Token = token.Token;
            return View();
        }

        public void LoadSelectLists(ref RegistrationToken objToken)
        {
            SelectList OrganizationList = null;
            List<Organization> organizationList = new List<Organization>();
            organizationList = db.Organizations.ToList();

            objToken.DepartmentList = new SelectList(db.Departments.ToList(), "DepartmentId", "DepartmentName", "");

            objToken.CourseList = new SelectList(db.Courses.ToList(), "CourseId", "CourseName", "");

            objToken.SectionList = new SelectList(db.Sections.ToList(), "SectionId", "SectionName", "");

            if (User.IsInRole("SiteAdmin"))
            {
                OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", "");
            }
            else
            {
                var organization = db.Organizations.Single(x => x.OrganizationName == User.Identity.Name);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;

                //OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", organization.OrganizationId);
            }
            objToken.OrganizationList = OrganizationList;


        }

    }
}
