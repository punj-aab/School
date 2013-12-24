using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    [Table("StaffTypes")]
    public class StaffTypes
    {
        [Key]
        public int StaffTypeId { get; set; }
        public string  StaffTypeName { get; set; }
        public string  Description { get; set; }

        public static int Teacher = 1;
        public static int Clerk = 2;
        public static int Coach = 3;
    }
}
