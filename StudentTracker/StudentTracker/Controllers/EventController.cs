using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
using StudentTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class EventController : BaseController
    {
        //
        // GET: /Event/
        StudentRepository repository = new StudentRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(long id = 0)
        {
            Event objEvent = repository.GetEvents(id);
            objEvent.NotificationTypeName = ((NotificationTypes)Convert.ToInt16(objEvent.NotificationTypeId)).ToString();
            if (objEvent == null)
            {
                return HttpNotFound();
            }
            return PartialView(objEvent);
        }

        public ActionResult Create()
        {
            Event objEvent = new Event();

            SelectList courseList = null;
            SelectList classList = null;
            SelectList organizationList = null;
            SelectList sectionList = null;
            SelectList eventTypeList = null;
            SelectList notificationTypeList = null;

            LoadSelectLists(out classList, out courseList, out organizationList, out sectionList, out eventTypeList, out notificationTypeList, false);


            objEvent.OrganizationList = organizationList;
            objEvent.CourseList = courseList;
            objEvent.ClassList = classList;
            objEvent.SectionList = sectionList;
            objEvent.EventTypeList = eventTypeList;
            objEvent.NotificationTypeList = notificationTypeList;

            objEvent.OrganizationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);

            return PartialView(objEvent);
        }
        [HttpPost]
        public string Create(Event objEvent, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objEvent.InsertedBy = _userStatistics.UserId;
                    objEvent.InsertedOn = DateTime.Now;
                    if (this.repository.CreateEvent(objEvent))
                    {
                        List<ELetter> objEletterList = this.repository.GetUsersForEvent(objEvent.EventId);
                        ELetter objEletter = null;
                        foreach (var eLetter in objEletterList)
                        {
                            objEletter = new ELetter();
                            objEletter.UserId = eLetter.UserId;
                            objEletter.EventId = eLetter.EventId;
                            objEletter.TemplateId = eLetter.TemplateId;
                            objEletter.OrganizationId = eLetter.OrganizationId;
                            objEletter.InsertedBy = _userStatistics.UserId;
                            this.repository.CreateEletter(objEletter);
                        }
                        SaveFiles(token, this.GetType().Name, objEvent.EventId);
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

        public ActionResult Edit(long id)
        {
            Event objModel = repository.GetEvents(eventId: id);
            if (objModel == null)
            {
                return HttpNotFound();
            }
            SelectList courseList = null;
            SelectList classList = null;
            SelectList organizationList = null;
            SelectList sectionList = null;
            SelectList eventTypeList = null;
            SelectList notificationTypeList = null;
            LoadSelectLists(out classList, out courseList, out organizationList, out sectionList, out eventTypeList, out notificationTypeList, true, objModel.ClassId, objModel.CourseId, objModel.OrganizationId, objModel.SectionId, objModel.EventTypeId, objModel.NotificationTypeId);

            objModel.OrganizationList = organizationList;
            objModel.CourseList = courseList;
            objModel.ClassList = classList;
            objModel.SectionList = sectionList;

            objModel.EventTypeList = eventTypeList;
            objModel.NotificationTypeList = notificationTypeList;

            objModel.OrganizationId = ViewBag.OrganizationId == null ? objModel.OrganizationId : Convert.ToInt32(ViewBag.OrganizationId);

            return PartialView(objModel);

        }

        [HttpPost]
        public string Edit(DBConnectionString.Event objEvent, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objEvent.ModifiedBy = _userStatistics.UserId;
                    objEvent.ModifiedOn = DateTime.Now;
                    if (objEvent.Update() > 0)
                    {
                        SaveFiles(token, this.GetType().Name, objEvent.EventId);
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

        public ActionResult ViewEvents()
        {
            List<Event> eventList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                eventList = repository.GetEvents();
            }
            else
            {
                eventList = repository.GetEvents(organizationId: _userStatistics.OrganizationId);
            }
            for (int i = 0; i < eventList.Count; i++)
            {
                eventList[i].NotificationTypeName = ((NotificationTypes)Convert.ToInt16(eventList[i].NotificationTypeId)).ToString();
            }

            return PartialView(eventList);
        }

        private void LoadSelectLists(out SelectList classList, out SelectList courseList, out SelectList organizationList, out SelectList sectionList, out SelectList eventTypeList, out SelectList notificationTypeList, bool isEdit, long classId = -1, long courseId = -1, long organizationId = -1, long sectionId = -1, long evenTypeId = -1, long notificationTypeId = -1)
        {
            classList = null;
            courseList = null;
            organizationList = null;
            sectionList = null;
            eventTypeList = null;
            notificationTypeList = null;

            List<Course> objCourseList = null;
            List<Class> objClassList = null;
            List<Section> objSectionList = null;
            List<Organization> objorganizationList = new List<Organization>();
            List<TemplateType> objEventTypeList = null;

            if (User.IsInRole("SiteAdmin"))
            {
                objorganizationList = repository.SelectOrganizations();

            }
            else
            {
                var organization = repository.SelectOrganizations(_userStatistics.OrganizationId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;
            }

            if (isEdit)
            {
                objCourseList = repository.GetCourses(organizationId: organizationId);
                objClassList = repository.ClassByCourse(courseId);// db.Classes.Where(x => x.CourseId == courseId).ToList();
                objSectionList = repository.SectionByClass(classId);
            }
            else
            {
                objCourseList = new List<Course>();
                objClassList = new List<Class>();
                objSectionList = new List<Section>();
            }
            List<SelectListItem> notificationTypes = Enum.GetValues(typeof(StudentTracker.Core.Utilities.NotificationTypes)).Cast<StudentTracker.Core.Utilities.NotificationTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            objEventTypeList = repository.TemplateTypes();

            organizationList = new SelectList(objorganizationList, "OrganizationId", "OrganizationName", organizationId);
            courseList = new SelectList(objCourseList, "CourseId", "CourseName", courseId);
            classList = new SelectList(objClassList, "ClassId", "ClassName", classId);
            sectionList = new SelectList(objSectionList, "SectionId", "SectionName", sectionId);
            eventTypeList = new SelectList(objEventTypeList, "TemplateTypeId", "Name", evenTypeId);
            notificationTypeList = new SelectList(notificationTypes, "Value", "Text", notificationTypeId);
        }

        public JsonResult GetClasses(long id)
        {
            List<Class> classList = repository.ClassByCourse(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCourses(long id)
        {
            List<Course> classList = repository.CourseByOrganization(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSections(long id)
        {
            List<Section> classList = repository.SectionByClass(id);
            return Json(classList, JsonRequestBehavior.AllowGet);
        }
    }
}
