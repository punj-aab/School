using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentTracker.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace StudentTracker.Core.Entities
{
    [Table("ClassRoom")]
    public class ClassRoom
    {
        [Key]
        public long ClassRoomId { get; set; }

        [Required]
        //[Display(Name = "Organization Name")]
        public long OrganizationId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public long DepartmentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public long InsertedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Departments { get; set; }

        [NotMapped]
        public SelectList DepartmentList { get; set; }

        [NotMapped]
        public string DepartmentName { get; set; }

        [NotMapped]
        public string InsertedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }

        
        [NotMapped]
        public SelectList OrganizationList { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organizations { get; set; }
        [NotMapped]
        public string OrganizationName { get; set; }
    }
}
