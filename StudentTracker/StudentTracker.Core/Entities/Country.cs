namespace StudentTracker.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    [Table("Country")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public int Id { get; set; }
        public int idSpecified { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public long continent_id { get; set; }
        public int continent_idSpecified { get; set; }
        public long country_group_id { get; set; }
        public int country_group_idSpecified { get; set; }
    }
}
