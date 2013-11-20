using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    class Parents
    {
        [Key]
        public long ParentId { get; set; }

        public long UserId { get; set; }
       
        [Required]
        public long ChildId { get; set; }
        
        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }
        
        [ScaffoldColumn(false)]
        public long InsertedBy { get; set; }
        
        [ScaffoldColumn(false)]
        public DateTime? UpdatedOn  { get; set; }

        [ScaffoldColumn(false)]
        public long? UpdatedBy { get; set; }
        
        [NotMapped]
        public string ChildName { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public string InsertedByName { get; set; }

        [NotMapped]
        public string UpdatedByName { get; set; }



    }
}


