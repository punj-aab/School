using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentTracker.Core.Entities
{
    [Table("States")]
    public class State
    {
        [Key]
        public int Id { get; set; }
        public string StateId { get; set; }
        public string CountryCode { get; set; }
        public string StateName { get; set; }
        public string Description { get; set; }
        public State()
        {
        }
        public State(string countryCode, string stateId, string name, string desc)
        {
            this.CountryCode = countryCode.Trim();
            this.StateId = stateId.Trim();
            this.StateName = name.Trim();
            this.Description = desc.Trim();
        }
    }
}