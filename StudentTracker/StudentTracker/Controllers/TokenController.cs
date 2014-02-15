using EmailHandler;
using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Repository;
using StudentTracker.Core.Utilities;
using StudentTracker.Repository;
using StudentTracker.ViewModels;
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
        private StudentRepository repository = new StudentRepository();
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
        public ActionResult Create(RegistrationToken token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    token.Token = UserStatistics.GenerateToken();
                    token.CreatedBy = _userStatistics.UserId;
                    if (repository.CreateToken(token) > 0)
                    {
                        ViewBag.Token = token.Token;
                        return View("ViewToken", token);
                    }
                    return View(token);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost]
        public bool EmailToken(string email, string token)
        {
            StudentContext context = new StudentContext();
            var tokenObj = context.RegistrationTokens.Where(t => t.Token == token).FirstOrDefault();
            tokenObj.Email = email;
            context.SaveChanges();
            Utilities.SendRegistationEmail(token, email);
            return true;
        }

        [Authorize]
        public ActionResult ViewToken(RegistrationToken token)
        {
            ViewBag.Token = token.Token;
            return View();
        }

        [Authorize(Roles="OrganizationAdmin")]
        public ActionResult ViewAllTokensForOrg()
        {
            StudentContext context = new StudentContext();
            var tokens = context.RegistrationTokens.Where(t => t.OrganizationId == _userStatistics.OrganizationId).ToList();

            return View(tokens);
        }

        public void LoadSelectLists(ref RegistrationToken objToken)
        {
            SelectList OrganizationList = null;
            List<Organization> organizationList = new List<Organization>();
            List<Department> depList = new List<Department>();
            List<Course> crsList = new List<Course>();
            List<Class> clsList = new List<Class>();
            List<Section> secList = new List<Section>();
            List<SelectListItem> roleTypes = Enum.GetValues(typeof(StudentTracker.Core.Utilities.UserRoles)).Cast<StudentTracker.Core.Utilities.UserRoles>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            if (User.IsInRole("SiteAdmin"))
            {
                OrganizationList = new SelectList(repository.Organizations(), "OrganizationId", "OrganizationName", "");
                objToken.RoleList = new SelectList(roleTypes, "Value", "Text");
            }
            else
            {
                var organization = repository.Organizations(_userStatistics.OrganizationId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;
                objToken.RoleList = new SelectList(roleTypes.Skip(2), "Value", "Text");
                crsList = repository.CourseByOrganization(_userStatistics.OrganizationId);
            }

            objToken.OrganizationList = OrganizationList;
            objToken.CourseList = new SelectList(crsList, "CourseId", "CourseName");
            objToken.ClassList = new SelectList(clsList, "ClassId", "ClassName");
            objToken.SectionList = new SelectList(secList, "SectionId", "SectionName");
            objToken.DepartmentList = new SelectList(depList, "DepartmentId", "DepartmentName");
        }

        public JsonResult GetCourse(long id)
        {
            return Json(repository.CourseByOrganization(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClass(long id)
        {
            return Json(repository.ClassByCourse(id), JsonRequestBehavior.AllowGet); //Json(repository.Find<Class>("select * from Classes where CourseId = @id", id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSection(long id)
        {
            return Json(repository.SectionByClass(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepartmentandCourse(long id)
        {
            ScheduleViewModel objVM = new ScheduleViewModel();
            objVM.CourseList = repository.CourseByOrganization(id); //repository.Find<Course>("select * from Courses where OrganisationId = @id", id);
            objVM.DepartmentList = repository.DepartmenstByOrganization(id); //repository.Find<Department>("select * from Departments where OrganizationId = @id", id);
            return Json(objVM, "application/json;", JsonRequestBehavior.AllowGet);
        }
    }
}
