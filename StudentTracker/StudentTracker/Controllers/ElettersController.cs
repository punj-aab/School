using StudentTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Core.Entities;
namespace StudentTracker.Controllers
{
    public class ElettersController : BaseController
    {
        //
        // GET: /Eletters/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AddRecipients()
        {
            List<User> objUserList = new List<Core.Entities.User>();
            objUserList = this.repository.Users(_userStatistics.OrganizationId);
            return PartialView(objUserList);
        }
    }
}
