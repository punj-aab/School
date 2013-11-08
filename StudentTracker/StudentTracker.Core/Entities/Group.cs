using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace StudentTracker.Core.Entities
{
    [Table("Group")]
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

        [Required]
        public long OrganizationId { get; set; }

        [NotMapped]
        public SelectList OrganizationList { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organizations { get; set; }
        [NotMapped]
        public string OrganizationName { get; set; }
        [NotMapped]
        [Display(Name = "Group Members")]
        public string GroupMembers { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
