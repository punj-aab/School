using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class ServiceCategory
    {
        [Key]
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public static int Communication = 1;
        public static int Payments = 2;
        public static int Academics = 3;
    }
}
