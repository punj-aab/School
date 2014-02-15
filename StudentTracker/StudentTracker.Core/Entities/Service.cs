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
        public int ServiceCategoryId { get; set; }
        public string ServiceDescription { get; set; }


        public static int Email = 1;
        public static int SMS = 2;
        public static int Reports = 3;

        public static int Payments = 4;
        public static int Calendar = 5;
        public static int ELetter = 6;

        public static int LunchDinnerMoney = 7;
        public static int Trips = 8;
        public static int Tickets = 9;

        public static int Shop = 10;
        public static int Attendence = 11;
        public static int TimeTable = 12;
        public static int CourseWork = 13;
    }
}
