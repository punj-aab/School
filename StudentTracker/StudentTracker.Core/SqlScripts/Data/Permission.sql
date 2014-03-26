------------------  Permission Categories  ------------------
insert into PermissionCategory values('BasicPermissions','Basic permissions')
insert into PermissionCategory values('Communication','')
insert into PermissionCategory values('Payments','')
insert into PermissionCategory values('GroupAndIndividuals','')
insert into PermissionCategory values('Academic','')
insert into PermissionCategory values('Reports','')

------------------  Permissions  ------------------
--  Basic  --
 insert into  Permission values('SendEmails',1,'Allows the selected users to create and send emails to assigned classes only.')
 insert into  Permission values('ViewEmails',1,'')
 insert into  Permission values('SendEletters',1,'')
 insert into  Permission values('ViewEletters',1,'')
 insert into  Permission values('SendSms',1,'')
 insert into  Permission values('ViewSms',1,'')
 insert into  Permission values('CreateManageCalendarEvents',1,'')
 insert into  Permission values('CreateManageCourseWork',1,'')
 insert into  Permission values('ViewTimeTable',1,'')
 insert into  Permission values('CreateManageAttendance',1,'')
 insert into  Permission values('SelfPayments',1,'')
 insert into  Permission values('CreateManageGroups',1,'')
 
--  communication  --
insert into  Permission values('SendEmail',2,'')
insert into  Permission values('ViewEmail',2,'')
insert into  Permission values('SendEletter',2,'')
insert into  Permission values('ViewEletter',2,'')
insert into  Permission values('SendSmsMessage',2,'')
insert into  Permission values('ViewSmsMessage',2,'')
insert into  Permission values('AbsentReporting',2,'')
insert into  Permission values('PrintLetters',2,'')
insert into  Permission values('TopUpSMSBalance',2,'')

--  Payments  --
insert into  Permission values('CreateAndManageFee',3,'')
insert into  Permission values('CreateAndManageTrips',3,'')
insert into  Permission values('CreateAndManageTickets',3,'')
insert into  Permission values('CreateAndManageShop',3,'')
insert into  Permission values('ManageRefunds',3,'')
insert into  Permission values('ManageCashPayments',3,'')
insert into  Permission values('ManageOrders',3,'')

--  GroupAndIndividuals  --
insert into  Permission values('CreateAndManageStaff',4,'')
insert into  Permission values('AssignTeachers',4,'')
insert into  Permission values('AssignDepartments',4,'')
insert into  Permission values('CreateAndManageStudents',4,'')
insert into  Permission values('ManageParents',4,'')
insert into  Permission values('CreateAndManageGroups',4,'')
insert into  Permission values('ViewStudents',4,'')
insert into  Permission values('ViewStaff',4,'')
insert into  Permission values('ViewParents',4,'')

--  Academic  --
insert into  Permission values('CreateAndManageCalendarEvents',5,'')
insert into  Permission values('ViewCalendarEvents',5,'')
insert into  Permission values('CreateAndManageCoursework',5,'')
insert into  Permission values('CreateAndManageAttendance',5,'')
insert into  Permission values('CreateAndManageTimetable',5,'')
insert into  Permission values('ViewTimeTables',5,'')

--  Reports  --
insert into  Permission values('ImportData',6,'')
insert into  Permission values('ExportData',6,'')
insert into  Permission values('ManagePaymentReports',6,'')
insert into  Permission values('ManageAcademicReports',6,'')

