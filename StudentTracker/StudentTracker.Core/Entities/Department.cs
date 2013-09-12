﻿using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Summary description for Department
/// </summary>
[Table("Departments")]
public class Department
{
    [Key]
    public long DepartmentId { get; set; }

    [Required]
    [Display(Name = "Name")]
    public string DepartmentName { get; set; }

    [Required]
    [Display(Name = "Description")]
    public string DepartmentDesc { get; set; }

    [Required]
    [ScaffoldColumn(false)]
    [Display(Name = "Organization")]
    public long OrganizationId { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? CreatedDate { get; set; }

    [ScaffoldColumn(false)]
    public long? CreatedBy { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? UpdatedDate { get; set; }

    [ScaffoldColumn(false)]
    public long? UpdatedBy { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? DeletedDate { get; set; }

    [ScaffoldColumn(false)]
    public long? DeletedBy { get; set; }

    public virtual ICollection<Organization> Roles { get; set; }

    [NotMapped]
    public SelectList OrganizationList { get; set; }
}