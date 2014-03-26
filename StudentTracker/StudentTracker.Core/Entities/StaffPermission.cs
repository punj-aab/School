using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class StaffPermission
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }

        //Basic
        public bool SendClassEmails { get; set; }
        public bool ViewClassEmails { get; set; }
        public bool SendClassEletters { get; set; }
        public bool ViewClassEletters { get; set; }
        public bool SendClassSms { get; set; }
        public bool ViewClassSms { get; set; }
        public bool CreateManageCalendarEvents { get; set; }
        public bool CreateManageCourseWork { get; set; }
        public bool ViewTimeTable { get; set; }
        public bool CreateManageAttendance { get; set; }
        public bool SelfPayments { get; set; }
        public bool CreateManageGroups { get; set; }

        //Communication
        public bool SendEmail { get; set; }
        public bool ViewEmail { get; set; }
        public bool SendEletter { get; set; }
        public bool ViewEletter { get; set; }
        public bool SendSms { get; set; }
        public bool ViewSms { get; set; }
        public bool AbsentReporting { get; set; }
        public bool PrintLetters { get; set; }
        public bool TopUpSMSBalance { get; set; }

        ///Payments
        public bool CreateAndManageFee { get; set; }
        public bool CreateAndManageTrips { get; set; }
        public bool CreateAndManageTickets { get; set; }
        public bool CreateAndManageShop { get; set; }
        public bool ManageRefunds { get; set; }
        public bool ManageCashPayments { get; set; }
        public bool ManageOrders { get; set; }

        //GroupAndIndividuals
        public bool CreateAndManageStaff { get; set; }
        public bool AssignTeachers { get; set; }
        public bool AssignDepartments { get; set; }
        public bool CreateAndManageStudents { get; set; }
        public bool ManageParents { get; set; }
        public bool CreateAndManageGroups { get; set; }
        public bool ViewStudents { get; set; }
        public bool ViewStaff { get; set; }
        public bool ViewParents { get; set; }

        //Academic
        public bool CreateAndManageCalendarEvents { get; set; }
        public bool ViewCalendarEvents { get; set; }
        public bool CreateAndManageCoursework { get; set; }
        public bool CreateAndManageAttendance { get; set; }
        public bool CreateAndManageTimetable { get; set; }
        public bool ViewTimeTables { get; set; }

        //Reports:
        public bool ImportData { get; set; }
        public bool ExportData { get; set; }
        public bool ManagePaymentReports { get; set; }
        public bool ManageAcademicReports { get; set; }

        public long CreatedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("UserId")]
        public virtual User Users { get; set; }
    }
}
