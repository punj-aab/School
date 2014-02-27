using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTracker.Core.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Post
    {
        [Required(ErrorMessage = "Id: Field is required")]
        public virtual int Id
        { get; set; }

        [Required(ErrorMessage = "Title: Field is required")]
        [StringLength(500, ErrorMessage = "Title: Length should not exceed 500 characters")]
        public virtual string Title
        { get; set; }

        [Required(ErrorMessage = "ShortDescription: Field is required")]
      
        [AllowHtml()]
        public virtual string ShortDescription
        { get; set; }

        [Required(ErrorMessage = "Description: Field is required")]
      
        [AllowHtml()]
        public virtual string Description
        { get; set; }

        [Required(ErrorMessage = "Meta: Field is required")]
        [StringLength(1000, ErrorMessage = "Meta: Length should not exceed 1000 characters")]
        public virtual string Meta
        { get; set; }

        [Required(ErrorMessage = "Meta: Field is required")]
        [StringLength(1000, ErrorMessage = "Meta: UrlSlug should not exceed 50 characters")]
        public virtual string UrlSlug
        { get; set; }

        public virtual bool Published
        { get; set; }

        [Required(ErrorMessage = "PostedOn: Field is required")]
        public virtual DateTime PostedOn
        { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public virtual Int64? PostedBy { get; set; }

        public virtual DateTime? Modified
        { get; set; }

        public virtual Category Category
        { get; set; }

        public virtual IList<Tag> Tags
        { get; set; }

        [NotMapped]
        public List<int> TagList { get; set; }
        [NotMapped]
        public int CategoryId { get; set; }

    }
}
