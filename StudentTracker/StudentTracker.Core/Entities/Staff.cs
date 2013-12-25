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

        public long? UserId { get; set; }

        public string Title { get; set; }

        public string FullName { get; set; }

        public int StaffTypeId { get; set; }

        public string ImportId { get; set; }

        public string Remarks { get; set; }

        [NotMapped]
        public string StaffTypeName { get; set; }

        [ForeignKey("StaffTypeId")]
        public StaffTypes StaffType { get; set; }

        public string Email { get; set; }

        public DateTime InsertedOn { get; set; }
        public long InsertedBy { get; set; }
        public long? ModifieBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string ImportDateString { get { return InsertedOn.ToString("MM/dd/yyyy"); } }


        [NotMapped]
        public string InsertedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }

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
