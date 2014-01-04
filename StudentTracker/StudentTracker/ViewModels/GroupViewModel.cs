using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;
namespace StudentTracker.ViewModels
{
    public class GroupViewModel
    {
        public List<Group> OrganizationGroupList { get; set; }
        public List<Group> UserGroupList { get; set; }
        public List<UserGroup> AssignedGroupList { get; set; }
        public long UserId { get; set; }
        
    }
}