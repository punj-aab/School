using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    [Table("Attachments")]
    public class Attachment
    {
        [Key]
        public long Id { get; set; }
        public string Filename { get; set; }
        public string ParentType { get; set; }
        public long ItemId { get; set; }
        public string FilePath { get; set; }
        public bool IsDeleted { get; set; }
    }
}
