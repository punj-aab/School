using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace StudentTracker.Core.Entities
{
    [Table("Sections")]
    public class Section
    {
        [Key]
        public int SectionId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string SectionName { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string SectionDescription { get; set; }

        [Required]
        public long ClassId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }
        [ScaffoldColumn(false)]
        public long CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Classes { get; set; }

        [NotMapped]
        public SelectList ClassList { get; set; }

        [NotMapped]
        public string InsertedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }

        [NotMapped]
        public string ClassName { get; set; }
    }
}
