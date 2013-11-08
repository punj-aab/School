using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace StudentTracker.Core.Entities
{
    public class Template
    {
        public long TemplateId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string TemplateText { get; set; }
        public bool IsActive { get; set; }
        public DateTime InsertedOn { get; set; }
        public long InsertedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }

        [NotMapped]
        public string InsertedByName { get; set; }
        [NotMapped]
        public string UpdatedByName { get; set; }

        [Required]
        [Display(Name = "Organization")]
        public long OrganizationId { get; set; }

        [Required]
        [Display(Name = "Template Type")]
        public long TemplateTypeId { get; set; }

        [ForeignKey("TemplateTypeId")]
        public TemplateType TemplateTypes { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }


        [NotMapped]
        public string OrganizationName { get; set; }

        [NotMapped]
        public string TemplateTypeName { get; set; }

        [NotMapped]
        public SelectList OrganizationList { get; set; }
        [NotMapped]
        public SelectList TemplateTypeList { get; set; }
    }
}
