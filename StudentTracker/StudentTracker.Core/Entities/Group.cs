using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class Group
    {
        [Key]
        public long GroupId { get; set; }
        [Required]
        public string GroupName { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime InsertedOn { get; set; }
        public long InsertedBy { get; set; }
        public long? ModifieBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [NotMapped]
        public string InsertedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }
    }
}
