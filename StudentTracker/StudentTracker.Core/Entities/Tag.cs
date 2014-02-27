﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Tag
    {
        public virtual int Id
        { get; set; }

        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 characters")]
        public virtual string Name
        { get; set; }

        [Required(ErrorMessage = "UrlSlug: Field is required")]
        [StringLength(500, ErrorMessage = "UrlSlug: Length should not exceed 500 characters")]
        public virtual string UrlSlug
        { get; set; }

        public virtual string Description
        { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public virtual IList<Post> Posts
        { get; set; }
    }
}
