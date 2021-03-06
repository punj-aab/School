﻿using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Repository;
using StudentTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace StudentTracker.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        //
        // GET: /Group/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Group/Details/5

        public ActionResult Groups()
        {
            return View();
        }

        public ActionResult Details(long id = 0)
        {
            Group objGroup = repository.GetGroups(id);
            if (objGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(objGroup);
        }

        //
        // GET: /Group/Create

        public ActionResult Create()
        {
            Group objGroup = new Group();

            objGroup.Users = this.repository.Users(_userStatistics.OrganizationId);

            objGroup.OrganizationList = LoadSelectLists();
            objGroup.OrganizationId = ViewBag.OrganizationId == null ? 0 : Convert.ToInt32(ViewBag.OrganizationId);
            return View(objGroup);
        }

        //
        // POST: /Group/Create

        [HttpPost]
        public string Create(Group objGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objGroup.InsertedOn = DateTime.Now;
                    objGroup.InsertedBy = _userStatistics.UserId;
                    //objGroup.UserCount = objGroup.Members.Split(',').Length;
                    UserGroup objUserGroup = null;
                    if (repository.CreateGroup(objGroup))
                    {

                        if (objGroup.Members != null)
                        {
                            var userIds = objGroup.Members.Split(',');

                            foreach (var id in userIds)
                            {
                                objUserGroup = new UserGroup();
                                objUserGroup.UserId = Convert.ToInt32(id);
                                objUserGroup.GroupId = objGroup.GroupId;
                                objUserGroup.InsertedBy = _userStatistics.UserId;
                                this.repository.AssignGroupToUser(objUserGroup);
                            }

                            return Convert.ToString(true);
                        }
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

        public JsonResult LoadMembersOfGroup(int groupId)
        {
            StudentContext context = new StudentContext();
            var users = (from g in context.Groups
                         join ug in context.UserGroups on g.GroupId equals ug.GroupId
                         join us in context.Users on ug.UserId equals us.UserId
                         where g.GroupId == groupId
                         select us).ToList();

            var userList = users.Select(us => new User
                        {
                            UserId = us.UserId,
                            Username = us.Username,
                            Email = us.Email,
                            FirstName = us.FirstName,
                            LastName = us.LastName
                        }).ToList();

            //context.Groups.Include("Users").Where(g => g.GroupId == groupId).ToList();
            return Json(new { GroupMembers = userList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadMembersOfClass(int classId)
        {
            StudentContext context = new StudentContext();
            var users = (from c in context.Classes
                         join s in context.Students on c.ClassId equals s.ClassId
                         join us in context.Users on s.UserId equals us.UserId
                         where c.ClassId == classId
                         select us).ToList();

            var userList = users.Select(us => new User
            {
                UserId = us.UserId,
                Username = us.Username,
                Email = us.Email,
                FirstName = us.FirstName,
                LastName = us.LastName
            }).ToList();

            //context.Groups.Include("Users").Where(g => g.GroupId == groupId).ToList();
            return Json(new { ClassMembers = userList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadGroups()
        {
            List<Group> GroupList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                GroupList = repository.GetGroups();
            }
            else
            {
                GroupList = repository.GetGroups(organizationId: _userStatistics.OrganizationId);
            }

            var classList = this.repository.GetClasses(organizationId: _userStatistics.OrganizationId);

            List<User> userList = this.repository.Users(_userStatistics.OrganizationId);

            return Json(new { Groups = GroupList, Individuals = userList, Classes = classList }, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Group/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Group objGroup = repository.GetGroups(id);

            StudentContext context = new StudentContext();
            //string userIds = string.Join(",", context.UserGroups.Where(ug => ug.GroupId == id).Select(s => s.UserId).ToArray());
            //objGroup.Users = this.repository.Users(_userStatistics.OrganizationId);
            objGroup.UserGroups = this.repository.UserGroupsByGroup(objGroup.GroupId);
            objGroup.Members = string.Join(",", objGroup.UserGroups.Select(s => s.UserId).ToArray());

            objGroup.OrganizationList = LoadSelectLists(objGroup.OrganizationId);
            objGroup.OrganizationId = ViewBag.OrganizationId == null ? objGroup.OrganizationId : Convert.ToInt32(ViewBag.OrganizationId);
            return PartialView(objGroup);
        }

        //
        // POST: /Group/Edit/5

        [HttpPost]
        public string Edit(Group objGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objGroup.ModifieBy = _userStatistics.UserId;
                    objGroup.ModifiedOn = DateTime.Now;
                    DBConnectionString.Group group = new DBConnectionString.Group
                    {
                        Description = objGroup.Description,
                        GroupId = objGroup.GroupId,
                        GroupName = objGroup.GroupName,
                        ModifieBy = _userStatistics.UserId,
                        ModifiedOn = DateTime.Now,
                        OrganizationId = objGroup.OrganizationId,
                        InsertedBy=objGroup.InsertedBy,
                        InsertedOn=objGroup.InsertedOn
                    };
                    UserGroup objUserGroup = null;
                    int recAffected = group.Update();
                    if (recAffected > 0)
                    {
                        objGroup.UserGroups = this.repository.UserGroupsByGroup(objGroup.GroupId);
                        if (objGroup.Members != null)
                        {
                            var userIds = objGroup.Members.Split(',');
                            foreach (var id in userIds)
                            {
                                if (!objGroup.UserGroups.Select(s => s.UserId).Contains(Convert.ToInt64(id)))
                                {
                                    objUserGroup = new UserGroup();
                                    objUserGroup.UserId = Convert.ToInt32(id);
                                    objUserGroup.GroupId = objGroup.GroupId;
                                    objUserGroup.InsertedBy = _userStatistics.UserId;
                                    this.repository.AssignGroupToUser(objUserGroup);
                                }
                            }

                            foreach (var ug in objGroup.UserGroups)
                            {
                                if (!userIds.Contains(ug.UserId.ToString()))
                                {
                                    repository.DeleteUserGroup(ug.UserId, ug.GroupId);
                                }
                            }
                            return Convert.ToString(true);
                        }
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
                if (repository.DeleteGroup(id))
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

        protected override void Dispose(bool disposing)
        {
            repository = null;
            base.Dispose(disposing);
        }

        public ActionResult ViewGroups()
        {
            List<Group> GroupList = null;
            if (User.IsInRole("SiteAdmin"))
            {
                GroupList = repository.GetGroups();
            }
            else
            {
                GroupList = repository.GetGroups(organizationId: _userStatistics.OrganizationId);
            }
            return PartialView(GroupList);
        }



        public SelectList LoadSelectLists(long id = -1)
        {
            SelectList OrganizationList = null;
            List<Organization> organizationList = new List<Organization>();

            if (User.IsInRole("SiteAdmin"))
            {
                organizationList = repository.SelectOrganizations();
            }
            else
            {
                var organization = repository.SelectOrganizations(_userStatistics.OrganizationId);
                ViewBag.OrganizationId = organization.OrganizationId;
                ViewBag.Organization = organization.OrganizationName;

                //OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", organization.OrganizationId);
            }
            OrganizationList = new SelectList(organizationList, "OrganizationId", "OrganizationName", id);
            return OrganizationList;
        }

        public void AddNewUserGroup(long userId, long groupId)
        {
            UserGroup objGroup = new UserGroup();
            objGroup.UserId = userId;
            objGroup.GroupId = groupId;
            objGroup.InsertedBy = _userStatistics.UserId;
            this.repository.AssignGroupToUser(objGroup);
        }

        public void DeleteUserGroup(long userId, long groupId)
        {
            this.repository.DeleteUserGroup(userId, groupId);
        }
    }
}
