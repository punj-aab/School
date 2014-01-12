using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class UserGroup
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public DateTime InsertedOn { get; set; }
        public long InsertedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long? StudentId { get; set; }
        public long? StaffId { get; set; }

        [NotMapped]
        public string Username { get; set; }
        [NotMapped]
        public string GroupName { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string LastName { get; set; }

        [NotMapped]
        public long? Default { get; set; }
    }
}
