using StudentTracker.Core.Entities;
using StudentTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class IndividualController : BaseController
    {
        //
        // GET: /Individual/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index()
        {
            List<Student> objModel = this.repository.GetStudents(_userStatistics.OrganizationId);
            List<string> listGroups = new List<string>();
            for (int i = 0; i < objModel.Count; i++)
            {
                listGroups = this.repository.GetGroupOfUser(objModel[i].UserId.Value);
                objModel[i].GroupNames = string.Join(", ", listGroups);
            }
            return View(objModel);
        }

        public ActionResult Parents()
        {
            return View();
        }
    }
}
