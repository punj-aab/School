using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace StudentTracker.Core.Entities
{
    public class Staff
    {
        public long StaffId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeNumber { get; set; }
        [Required]
        public string Email { get; set; }

        public DateTime InsertedOn { get; set; }
        public long InsertedBy { get; set; }
        public long? ModifieBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [NotMapped]
        public string InsertedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }

        [NotMapped]
        public long OrganizationId { get; set; }
        [NotMapped]
        public long CourseId { get; set; }
        [NotMapped]
        public long DepartmentId { get; set; }
        [NotMapped]
        public long ClassId { get; set; }
        [NotMapped]
        public long SectionId { get; set; }
        [NotMapped]
        public long GroupId { get; set; }
        [NotMapped]
        public long SubjectId { get; set; }

        [NotMapped]
        public SelectList OrganizationList { get; set; }
        [NotMapped]
        public SelectList CourseList { get; set; }
        [NotMapped]
        public SelectList DepartmentList { get; set; }
        [NotMapped]
        public SelectList ClassList { get; set; }
        [NotMapped]
        public SelectList SectionList { get; set; }
        [NotMapped]
        public SelectList GroupList { get; set; }
        [NotMapped]
        public SelectList SubjectList { get; set; }

        [NotMapped]
        public string OrganizationName { get; set; }
    }
}
