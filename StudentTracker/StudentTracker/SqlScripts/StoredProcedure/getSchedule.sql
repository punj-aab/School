create procedure getSchedules
(
@userRole as varchar(50)=null,
@organizationId as bigint=-1,
@courseId as bigint=-1,
@departmentId as bigint=-1,
@classId as bigint=-1,
@createdBy as bigint=-1
)
as
if(@userRole='SiteAdmin')
BEGIN
select * from Organizations
END
else
BEGIN
select * from Organizations where CreatedBy = @createdBy
END
select * from Courses where OrganisationId = @organizationId
select * from Classes where CourseId = @courseId
select * from Subjects where ClassId = @classId
select * from Departments where OrganizationId = @organizationId
select * from ClassRoom where DepartmentId = @departmentId
  