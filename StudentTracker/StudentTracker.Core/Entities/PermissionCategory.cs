using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class PermissionCategory
    {
        [Key]
        public long PermissionCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public const int BasicPermissions = 1;
        public const int Communication = 2;
        public const int Payments = 3;
        public const int GroupAndIndividuals = 4;
        public const int Academic = 5;
        public const int Reports = 6;
    }
}
