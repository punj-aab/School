using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class UserSubjects
    {
        [Key]
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? StudentId { get; set; }
        public long SubjectId { get; set; }
        public DateTime InsertedOn { get; set; }
        public long InsertedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
