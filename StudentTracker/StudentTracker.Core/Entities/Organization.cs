namespace StudentTracker.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    using StudentTracker.Core.Entities;
    using System.Web.Mvc;
    /// <summary>
    /// Summary description for Organization
    /// </summary>
    [Table("Organizations")]
    public class Organization
    {
        [Key]
        public long OrganizationId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string OrganizationName { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string OrganizationDesc { get; set; }

        [Required]
        [Display(Name = "Organization Type")]
        public int OrganizationTypeId { get; set; }

        [Required]
        [Display(Name = "Registeration Number")]
        public string RegisterationNumber { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required]
        [Display(Name = "State")]
        public long StateId { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        [ScaffoldColumn(false)]
        public Int64? CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public Int64? ModifiedBY { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedDate { get; set; }

        [ScaffoldColumn(false)]
        public Int64? Deletedby { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DeletedDate { get; set; }

        [ScaffoldColumn(false)]
        public long RegionId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string StateName { get; set; }

        //[ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        [ForeignKey("RegionId")]
        public virtual Region States { get; set; }

        [NotMapped]
        public SelectList CountryList { get; set; }

        [NotMapped]
        public SelectList StateList { get; set; }

        [NotMapped]
        public SelectList OrganizationTypeList { get; set; }


    }
}