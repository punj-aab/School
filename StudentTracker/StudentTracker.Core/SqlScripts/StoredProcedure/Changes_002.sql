ALTER procedure [dbo].[usp_addSubject]
(
@SubjectName as varchar(100),       
@SubjectDescription as varchar(100),
@OrganizationId as bigint,
@CourseId as bigint,          
@ClassId  as bigint,          
@CreatedBy as bigint,         
@InsertedOn as datetime
)
as
insert into Subjects values(
@SubjectName,
@SubjectDescription,
@CourseId ,
@InsertedOn,
@CreatedBy ,
null,
null,
@ClassId,
@OrganizationId
)

Go
ALTER procedure [dbo].[usp_getSubjects] --@organizationId=17
(
@organizationId bigint=null,
--@courseId bigint=null,
@subjectId bigint=null
)
as
select SubjectId,
       SubjectName,
       SubjectDescription,
       Subjects.CourseId,
       Courses.CourseName, 
       Subjects.InsertedOn,
       Subjects.CreatedBy,
       Subjects.ModifiedBy,
       Subjects.ModifiedOn,
       Subjects.ModifiedBy,
       Subjects.ClassId,
       Classes.ClassName, 
       Users.Username as InsertedByName,
       Users_1.Username as ModifiedByName,
       Organizations.OrganizationId,
       OrganizationName
from Subjects 
join Organizations on Subjects.OrganizationId = Organizations.OrganizationId
join Courses on Subjects.CourseId = Courses.CourseId 
join Classes on Subjects.ClassId = Classes.ClassId join Users on Subjects.CreatedBy=Users.UserId left join Users as Users_1 on  Subjects.ModifiedBy=Users_1.UserId
where (@organizationId is null or Courses.OrganisationId =@organizationId)
and (@subjectId is null or SubjectId=@subjectId)

go  
alter procedure [dbo].[usp_addClassRoom]
(
@Name as varchar(100),              
@Description as varchar(100),
@Location as varchar(100),   
@InsertedOn as datetime, 
@InsertedBy as bigint, 
@DepartmentId as bigint,
@OrganizationId as bigint
)
as 
BEGIN
     insert into ClassRoom values
     (
		@OrganizationId,
		@DepartmentId,
		@Name,        
		@Description, 
		@Location,    
		@InsertedOn,  
		@InsertedBy,  
		null,
		null
)
END

go

ALTER procedure [dbo].[usp_getClassRooms] --@organizationId=18
(
@organizationId bigint=null,
--@courseId bigint=null,
@classRoomId bigint=null
)
as
select ClassRoomId,
       Name,
       Description,
       Location,
       ClassRoom.InsertedOn,
       ClassRoom.ModifiedOn,
       ClassRoom.DepartmentId,
       Departments.DepartmentName, 
       ClassRoom.InsertedBy,
       ClassRoom.ModifiedBy ,
       Users.Username as InsertedByName,
       Users_1.Username as ModifiedByName,
       Organizations.OrganizationId,
       Organizations.OrganizationName
from ClassRoom join Organizations on ClassRoom.OrganizationId = Organizations.OrganizationId
join Departments on ClassRoom.DepartmentId=Departments.DepartmentId 
join Users on ClassRoom.InsertedBy=Users.UserId 
left join Users as Users_1 on  ClassRoom.ModifiedBy=Users_1.UserId
where (@organizationId is null or Departments.OrganizationId =@organizationId)
and (@classRoomId is null or ClassRoomId=@classRoomId)
