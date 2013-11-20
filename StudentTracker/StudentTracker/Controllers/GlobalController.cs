using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
using StudentTracker.Repository;
namespace StudentTracker.Controllers
{
    public class GlobalController : BaseController
    {
        //
        // GET: /Global/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddRecipients()
        {
            List<User> objUserList = new List<Core.Entities.User>();
            objUserList = this.repository.Users(_userStatistics.OrganizationId);
            return PartialView(objUserList);
        }

        public JsonResult GetAllUsers()
        {
            List<User> userList = this.repository.Users(_userStatistics.OrganizationId);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroups()
        {
            List<Group> modelList = this.repository.GetGroups(organizationId: _userStatistics.OrganizationId);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClasses()
        {
            List<Class> modelList = this.repository.GetClasses(organizationId: _userStatistics.OrganizationId);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

    }
}
