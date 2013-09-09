using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class Profile
    {
        [Key]
        public long ProfileId { get; set; }

        [ForeignKey("UserId")]
        public long UserId { get; set; }

        [Required]
        [Display(Name = "Address #1")]
        public string Address1 { get; set; }

        [Required]
        [Display(Name = "Address #2")]
        public string Address2 { get; set; }

        public string City { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailAddress1 { get; set; }
        public string EmailAddress2 { get; set; }
        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }
        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }
    }
}
