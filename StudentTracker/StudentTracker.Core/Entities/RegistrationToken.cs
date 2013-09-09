using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class RegistrationToken
    {
        [Key]
        public long TokenId { get; set; }

        [ScaffoldColumn(false)]
        public Guid Token { get; set; }

        [Required]
        [Display(Name = "Organization")]
        public int OrganizationId { get; set; }


        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }


        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Display(Name = "Section")]
        public int SectionId { get; set; }
    }
}
