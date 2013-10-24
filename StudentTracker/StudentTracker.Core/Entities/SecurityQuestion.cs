using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class SecurityQuestion
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public DateTime InsertedOn { get; set; }
        public string InsertedBy { get; set; }
    }
}
