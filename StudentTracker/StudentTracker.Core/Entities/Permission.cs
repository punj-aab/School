using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Entities
{
    public class Permission
    {
        [Key]
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
        public long PermissionCategoryId { get; set; }
        public string Description { get; set; }

        //Basic Permissions
        public const int SendEmails = 1;
        public const int ViewEmails = 2;
        public const int SendEletters = 3;
        public const int ViewEletters = 4;
        public const int SendSms = 5;
        public const int ViewSms = 6;
        public const int CreateManageCalendarEvents = 7;
        public const int CreateManageCourseWork = 8;
        public const int ViewTimeTable = 9;
        public const int CreateManageAttendance = 10;
        public const int SelfPayments = 11;
        public const int CreateManageGroups = 12;

        //Communication
        public const int SendEmail = 13;
        public const int ViewEmail = 14;
        public const int SendEletter = 15;
        public const int ViewEletter = 16;
        public const int SendSmsMessage = 17;
        public const int ViewSmsMessage = 18;
        public const int AbsentReporting = 19;
        public const int PrintLetters = 20;
        public const int TopUpSMSBalance = 21;

        ///Payments
        public const int CreateAndManageFee = 22;
        public const int CreateAndManageTrips = 23;
        public const int CreateAndManageTickets = 24;
        public const int CreateAndManageShop = 25;
        public const int ManageRefunds = 26;
        public const int ManageCashPayments = 27;
        public const int ManageOrders = 28;

        //GroupAndIndividuals
        public const int CreateAndManageStaff = 29;
        public const int AssignTeachers = 30;
        public const int AssignDepartments = 31;
        public const int CreateAndManageStudents = 32;
        public const int ManageParents = 33;
        public const int CreateAndManageGroups = 34;
        public const int ViewStudents = 35;
        public const int ViewStaff = 36;
        public const int ViewParents = 37;

        //Academic
        public const int CreateAndManageCalendarEvents = 38;
        public const int ViewCalendarEvents = 39;
        public const int CreateAndManageCoursework = 40;
        public const int CreateAndManageAttendance = 41;
        public const int CreateAndManageTimetable = 42;
        public const int ViewTimeTables = 43;

        //Reports:
        public const int ImportData = 44;
        public const int ExportData = 45;
        public const int ManagePaymentReports = 46;
        public const int ManageAcademicReports = 47;
    }
}
