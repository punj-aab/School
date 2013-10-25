namespace StudentTracker.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    [Table("Users")]
    public class User
    {
        [Key]
        public long UserId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public int StatusId { get; set; }

        [ScaffoldColumn(false)]
        public long MasterId { get; set; }

        [Required(ErrorMessage = "*")]
        [Remote("CheckUser", "Organization")]
        public string Username { get; set; }

        [Editable(false)]
        [Required(ErrorMessage = "*")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "*"), DataType(DataType.Password)]
        public virtual string Password { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "*"), DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "<span title='The password and confirmation password do not match.'>*</span>")]
        public virtual string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string LastName { get; set; }

        [ScaffoldColumn(false)]
        [Required]
        public virtual string RegistrationToken { get; set; }

        [ScaffoldColumn(false)]
        public long OrgainzationId { get; set; }

        [ScaffoldColumn(false)]
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime? LastPasswordFailureDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime? LastActivityDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime? LastLockoutDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime? LastLoginDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual string ConfirmationToken { get; set; }

        [ScaffoldColumn(false)]
        public virtual Boolean IsLockedOut { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime? LastPasswordChangedDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual string PasswordVerificationToken { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Role> Roles { get; set; }

        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "*")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "*")]
        public string HomeTelephoneNumber { get; set; }
        [Required(ErrorMessage = "*")]
        public long? SecurityQuestionId { get; set; }
        [Required(ErrorMessage = "*")]
        public string SecurityAnswer { get; set; }

        [NotMapped]
        public long RoleId { get; set; }

        [NotMapped]
        public SelectList SecurityQuestionList { get; set; }
    }
}