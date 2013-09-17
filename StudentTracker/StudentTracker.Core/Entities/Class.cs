using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace StudentTracker.Core.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public long ClassId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string ClassName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public long InsertedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }
        
        [Required]
        public long OrganizationId { get; set; }

        [NotMapped]
        public SelectList OrganizationList { get; set; }

        [ForeignKey("ClassId")]
        public virtual Organization Organizations { get; set; }
    }
}
