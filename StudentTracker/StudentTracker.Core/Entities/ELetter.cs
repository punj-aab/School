using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using StudentTracker.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudentTracker.Core.Entities
{
    public class ELetter
    {
        [Key]
        public long EletterId { get; set; }

        [Required]
        [Display(Name = "User")]
        public long UserId { get; set; }

        [Display(Name = "Event")]
        [Required]
        public long EventId { get; set; }

        [Display(Name = "Template")]
        [Required]
        public long TemplateId { get; set; }

        [Display(Name = "Organizaion")]
        [Required]
        public long OrganizationId { get; set; }

        public bool IsRead { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public long InsertedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }

        [NotMapped]
        public string InsertedByName { get; set; }

        [NotMapped]
        public string UpdatedByName { get; set; }

        [NotMapped]
        public string EventName { get; set; }

        [NotMapped]
        public string TemplateName { get; set; }

        [NotMapped]
        public string TemplateText { get; set; }

        [NotMapped]
        public string OrganizationName { get; set; }

        [ForeignKey("UserId")]
        public virtual User Users { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Events { get; set; }

        [ForeignKey("TemplateId")]
        public virtual Template Templates { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organizations { get; set; }
    }
}
