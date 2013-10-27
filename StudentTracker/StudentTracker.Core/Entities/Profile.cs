using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace StudentTracker.Core.Entities
{
    public class Profile
    {
        [Key]
        public long ProfileId { get; set; }

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

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string EmailAddress1 { get; set; }
        public string EmailAddress2 { get; set; }
        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "*")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "*")]
        public string HomeTelephoneNumber { get; set; }

        [Required(ErrorMessage = "*")]
        public long SecurityQuestionId { get; set; }

        [Required(ErrorMessage = "*")]
        public string SecurityAnswer { get; set; }

        [NotMapped]
        public SelectList SecurityQuestionList { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "*")]
        public string RegistrationToken { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "*"), DataType(DataType.Password)]
        public virtual string Password { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "*"), DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "<span title='Password and confirm password must match.'>*</span>")]
        public virtual string ConfirmPassword { get; set; }

    }
}
