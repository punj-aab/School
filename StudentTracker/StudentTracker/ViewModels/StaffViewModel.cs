using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.ViewModels
{
    public class StaffViewModel : Staff
    {
        public List<ListFields> ListFields { get; set; }
        public List<TeacherSubjects> TeacherSubjectsList { get; set; }
        public Profile Profile { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string HomeTelephoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public long UserId { get; set; }
        public SelectList StaffTypeList { get; set; }
        public long ProfileId { get; set; }
        public string MobileNumber { get; set; }
        public int Count { get; set; }

        //For Permission Management
        public Communication Communication { get; set; }
        public Payment Payment { get; set; }
        public GroupAndIndividuals GroupAndIndividuals { get; set; }
        public Academic Academic { get; set; }
        public Reports Reports { get; set; }
        public StaffPermission StaffPermission { get; set; }

    }

    public class ListFields
    {
        public long Id { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public long DepartmentId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public int SectionId { get; set; }
        [Required]
        public long SubjectId { get; set; }
        public SelectList CourseList { get; set; }
        public SelectList DepartmentList { get; set; }
        public SelectList ClassList { get; set; }
        public SelectList SectionList { get; set; }
        public SelectList GroupList { get; set; }
        public SelectList SubjectList { get; set; }
    }

    public class Communication
    {
        public bool SendEmail { get; set; }
        public bool ViewEmail { get; set; }
        public bool SendEletter { get; set; }
        public bool ViewEletter { get; set; }
        public bool SendSmsMessage { get; set; }
        public bool ViewSmsMessage { get; set; }
        public bool AbsentReporting { get; set; }
        public bool PrintLetters { get; set; }
        public bool TopUpSMSBalance { get; set; }
    }

    public class Payment
    {
        public bool CreateAndManageFee { get; set; }
        public bool CreateAndManageTrips { get; set; }
        public bool CreateAndManageTickets { get; set; }
        public bool CreateAndManageShop { get; set; }
        public bool ManageRefunds { get; set; }
        public bool ManageCashPayments { get; set; }
        public bool ManageOrders { get; set; }
    }

    public class GroupAndIndividuals
    {
        public bool CreateAndManageStaff { get; set; }
        public bool AssignTeachers { get; set; }
        public bool AssignDepartments { get; set; }
        public bool CreateAndManageStudents { get; set; }
        public bool ManageParents { get; set; }
        public bool CreateAndManageGroups { get; set; }
        public bool ViewStudents { get; set; }
        public bool ViewStaff { get; set; }
        public bool ViewParents { get; set; }
    }

    public class Academic
    {
        public bool CreateAndManageCalendarEvents { get; set; }
        public bool ViewCalendarEvents { get; set; }
        public bool CreateAndManageCoursework { get; set; }
        public bool CreateAndManageAttendance { get; set; }
        public bool CreateAndManageTimetable { get; set; }
        public bool ViewTimeTable { get; set; }
    }

    public class Reports
    {
        public bool ImportData { get; set; }
        public bool ExportData { get; set; }
        public bool ManagePaymentReports { get; set; }
        public bool ManageAcademicReports { get; set; }
    }
}