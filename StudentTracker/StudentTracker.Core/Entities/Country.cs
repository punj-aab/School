using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTracker.Core.Entities
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public Country()
        {
        }

        public Country(string countryCode, string countryName)
        {
            this.CountryCode = countryCode;
            this.CountryName = countryName;
        }
    }
}