using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.ViewModels
{
    public class OrganizationsViewModel
    {
        public int Id { get; set; }
        public List<Organization> OrganizationList { get; set; }
    }
}