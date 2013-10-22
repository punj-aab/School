using StudentTracker.Core.Entities;
using StudentTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class TemplateController : BaseController
    {
        //
        // GET: /Template/
        StudentRepository repository = new StudentRepository();
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public string Create(Template objTemplate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objTemplate.InsertedOn = DateTime.Now;
                    objTemplate.InsertedBy = _userStatistics.UserId;
                    if (repository.CreateTemplate(objTemplate))
                    {
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewTemplates()
        {
            List<Template> templateList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                templateList = repository.GetTemplates();
            }
            else
            {
                templateList = repository.GetTemplates(organizationId: _userStatistics.OrganizationId);
            }
            return PartialView(templateList);
        }
        public ActionResult Details(long id)
        {
            Template objTemplate = repository.GetTemplates(templateId: id);
            return PartialView(objTemplate);
        }
        public ActionResult Edit(long id)
        {
            Template objTemplate = repository.GetTemplates(templateId: id);
            return PartialView(objTemplate);
        }
        [HttpPost]
        public string Edit(DBConnectionString.Template objTemplate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objTemplate.UpdatedOn = DateTime.Now;
                    objTemplate.UpdatedBy = _userStatistics.UserId;
                    if (objTemplate.Update() > 0)
                    {
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
        [HttpPost]
        public string DeleteConfirmed(long id)
        {
            try
            {
                if (DBConnectionString.Template.Delete(id) > 0)
                {
                    return Convert.ToString(true);
                }
                return Convert.ToString(false);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
