using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class Region
    {
        public long RegionId { get; set; }
        public int idSpecified { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int country_id { get; set; }
        public int country_idSpecified { get; set; }
        public long has_tz { get; set; }
        public int has_tzSpecified { get; set; }
        public long std_offset { get; set; }
        public int std_offsetSpecified { get; set; }
        public string timezone { get; set; }
    }
}
