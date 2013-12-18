using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.ViewModels
{
    public class OrganizationServicesViewModel
    {
        public List<ServiceViewModel> Servcies { get; set; }

        public long OrganizationId { get; set; }

    }

    public class ServiceViewModel
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string ServiceDescription { get; set; }

        public bool IsAdded { get; set; }

        public bool Modified { get; set; }
    }
}