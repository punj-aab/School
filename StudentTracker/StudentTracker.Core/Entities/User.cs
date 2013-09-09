namespace StudentTracker.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Users")]
    public class User
    {
        [Key]
        public long UserId { get; set; }

        public DateTime InsertedOn { get; set; }

        public int StatusId { get; set; }

        public long MasterId { get; set; }

        [Required]
        public virtual string Username { get; set; }

        public virtual string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public virtual string Password { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }


        public virtual int PasswordFailuresSinceLastSuccess { get; set; }

        public virtual DateTime? LastPasswordFailureDate { get; set; }

        public virtual DateTime? LastActivityDate { get; set; }

        public virtual DateTime? LastLockoutDate { get; set; }

        public virtual DateTime? LastLoginDate { get; set; }

        public virtual string ConfirmationToken { get; set; }

        public virtual Boolean IsLockedOut { get; set; }

        public virtual DateTime? LastPasswordChangedDate { get; set; }

        public virtual string PasswordVerificationToken { get; set; }

        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}