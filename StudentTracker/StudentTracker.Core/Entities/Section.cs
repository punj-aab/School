using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

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
        public string SectionDescription { get; set; }

        [Required]
        [ForeignKey("ClassId")]
        public string ClassId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }
        [ScaffoldColumn(false)]
        public long CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public long ModifiedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }
    }
}
