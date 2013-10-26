﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace StudentTracker.Core.Entities
{
    public class RegistrationToken
    {
        [Key]
        public long TokenId { get; set; }

        [ScaffoldColumn(false)]
        //[Required(ErrorMessage = "*")]
        public string Token { get; set; }

        [Required]
        [Display(Name = "Organization")]
        public long OrganizationId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Display(Name = "Class")]
        public long ClassId { get; set; }

        [Display(Name = "Section")]
        public int SectionId { get; set; }

        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [ScaffoldColumn(false)]
        public long CreatedBy { get; set; }

        [NotMapped]
        public SelectList OrganizationList { get; set; }

        [NotMapped]
        public SelectList DepartmentList { get; set; }

        [NotMapped]
        public SelectList CourseList { get; set; }

        [NotMapped]
        public SelectList SectionList { get; set; }

        [NotMapped]
        public SelectList RoleList { get; set; }

        [NotMapped]
        public SelectList ClassList { get; set; }
    }
}
