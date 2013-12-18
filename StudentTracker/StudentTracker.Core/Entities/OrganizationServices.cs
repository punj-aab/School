using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    [Table("OrganizationServices")]
    public class OrganizationServices
    {
        [Key]
        public int Id { get; set; }

        public long OrganizationId { get; set; }
        public int ServiceId { get; set; }
        public int StatusId { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long InsertedBy { get; set; }
        public long UpdatedBy { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }
    }
}
