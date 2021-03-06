﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace StudentTracker.Core.Entities
{
    public class Schedule
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ScheduleId { get; set; }

        [Required]
        public string ScheduleName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public long OrganizationId { get; set; }

        [Required]
        public long CourseId { get; set; }

        [Required]
        public long ClassId { get; set; }

        [Required]
        public long SubjectId { get; set; }

        [Required]
        public long DepartmentId { get; set; }

        [Required]
        public long ClassRoomId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime InsertedOn { get; set; }

        [ScaffoldColumn(false)]
        public long InsertedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public long? ModifiedBy { get; set; }

        public virtual Subject Subjects { get; set; }
        public virtual ClassRoom ClassRooms { get; set; }

        [NotMapped]
        public SelectList OrganizationList { get; set; }

        [NotMapped]
        public SelectList CourseList { get; set; }

        [NotMapped]
        public SelectList ClassList { get; set; }

        [NotMapped]
        public SelectList SubjectList { get; set; }

        [NotMapped]
        public SelectList DepartmentList { get; set; }

        [NotMapped]
        public SelectList ClassRoomList { get; set; }

        [NotMapped]
        public SelectList DayList { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        [Required]
        public int DayId { get; set; }

        [Required]
        public string DayIds { get; set; }

        [NotMapped]
        public string OrganizationName { get; set; }
        [NotMapped]
        public string CourseName { get; set; }
        [NotMapped]
        public string ClassName { get; set; }
        [NotMapped]
        public string SubjectName { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
        [NotMapped]
        public string ClassRoomName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }
        [NotMapped]
        public string InsertedByName { get; set; }
    }
}
