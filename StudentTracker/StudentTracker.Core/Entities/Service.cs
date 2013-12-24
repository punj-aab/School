using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    [Table("Services")]
    public class Service
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }

        public static int Emails = 1;
        public static int SMS = 2;
        public static int Reports = 3;
        public static int Payments = 4;
        public static int Calendar = 5;
        public static int ELetters = 6;
    }
}
