using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string ClassName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public long InsertedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public long ModifiedBy { get; set; }
    }
}
