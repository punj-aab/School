using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StudentTracker.ViewModels
{
    public class OrganizationServicesViewModel
    {
        public IEnumerable<ServiceViewModel> Servcies { get; set; }

        [Display(Name = "Organization")]
        public long OrganizationId { get; set; }

    }

    public class ServiceViewModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string ServiceDescription { get; set; }

        public bool IsAdded { get; set; }

        public bool Modified { get; set; }
    }
}