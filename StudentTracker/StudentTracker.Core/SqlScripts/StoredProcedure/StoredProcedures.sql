
GO
/****** Object:  StoredProcedure [dbo].[getOrganizations]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[usp_getOrganizations]  
(  
@organizationId bigint = null  
)  
as  
select Organizations.OrganizationId,  
       OrganizationName,  
       OrganizationDesc,  
       OrganizationTypeId,  
       RegisterationNumber,  
       CountryId,  
       Organizations.StateId,  
       Address1,  
       Address2,  
       Organizations.Email,  
       Phone1,  
       Phone2,  
       CreatedBy,  
       CreatedDate,  
       ModifiedBy,  
       ModifiedDate,  
       Deletedby,  
       DeletedDate,  
       Users.Username as InsertedByName,  
       Users_1.Username as ModifiedByName,  
       Countries.CountryName as CountryName,  
       States.StateName as StateName,  
       City  
 from Organizations   
 join Countries on Organizations.CountryId=Countries.Id   
 join States on organizations.StateId=States.Id  
 join Users on Organizations.CreatedBy=Users.UserId   
 left join Users as Users_1 on  Organizations.ModifiedBy=Users_1.UserId  
 where (@organizationId is null or Organizations.OrganizationId=@organizationId)
GO

CREATE procedure [dbo].[usp_addOrganization](
@Address1 as varchar(200),            
@Address2 as varchar(200),                       
@City  as varchar(200),                          
@CountryId as bigint,          
@CreatedBy as int,          
@CreatedDate as datetime,        
@Email   as varchar(200),                        
@OrganizationDesc  as varchar(200),              
@OrganizationName as varchar(200),               
@OrganizationTypeId as bigint, 
@Phone1 as varchar(200),             
@Phone2 as varchar(200),             
@RegisterationNumber as varchar(200),
@StateId as bigint             
)
as
insert into dbo.Organizations values(
@OrganizationName,
@OrganizationDesc,   
@OrganizationTypeId,
@RegisterationNumber ,
@CountryId,         
@StateId,
@City,
@Address1,            
@Address2 ,           
@Email,                
@Phone1,
@Phone2,
@CreatedBy,        
@CreatedDate,
null,
null,
null,
null
)

GO
create procedure [dbo].[usp_addCourse]
(
@CourseName as varchar(100),       
@CourseDescription as varchar(100),
@OrganisationId as bigint,   
@CreatedBy as bigint,       
@InsertedOn as datetime      
)
as
insert into Courses values(
@CourseName,
@CourseDescription,
@OrganisationId   ,
@CreatedBy        ,
@InsertedOn ,
 null,
 null,
 null     
)
GO

CREATE procedure [dbo].[usp_getDepartments] --@departmentId=2
(
@organizationId bigint=null,
--@courseId bigint=null,
@departmentId bigint=null
)
as
select Departments.DepartmentId,Departments.DepartmentName,Departments.DepartmentDesc,Departments.CreatedBy,Departments.CreatedDate,Departments.UpdatedBy,Departments.UpdatedDate, Organizations.OrganizationName,Users.Username as InsertedByName,Users_1.Username as ModifiedByName,Departments.OrganizationId
from Departments join Organizations on Departments.OrganizationId=Organizations.OrganizationId join Users on Departments.CreatedBy=Users.UserId left join Users as Users_1 on  Departments.UpdatedBy=Users_1.UserId

where (@departmentId is null or DepartmentId=@departmentId) and
(@organizationId is null or Departments.OrganizationId = @organizationId)
GO

CREATE procedure [dbo].[usp_getCourses]
(
@courseId bigint=null,
@organizationId bigint=null
)
as
select CourseId,CourseName,CourseDescription,Courses.CreatedBy,Courses.InsertedOn,Courses.ModifiedBy,ModifiedOn,Courses.OrganisationId,Organizations.OrganizationName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName
from Courses join Organizations on Courses.OrganisationId=Organizations.OrganizationId join Users on Courses.CreatedBy=Users.UserId left join Users as Users_1 on  Courses.ModifiedBy=Users_1.UserId
where (@courseId is null or CourseId=@courseId )
and (@organizationId is null or Courses.OrganisationId=@organizationId)
GO

create procedure [dbo].[usp_AddDepartment]  
(  
@DepartmentName varchar(200),              
@DepartmentDesc as varchar(200),                         
@OrganizationId  as int,                            
@CreatedDate as datetime,            
@CreatedBy as bigint           
              
)  
as  
insert into dbo.Departments values(  
@DepartmentName,  
@DepartmentDesc,     
@OrganizationId,  
@CreatedDate ,  
@CreatedBy,  
null,  
null,  
null,  
null  
)
GO

CREATE procedure [dbo].[usp_getClassRooms] --@organizationId=18
(
@organizationId bigint=null,
--@courseId bigint=null,
@classRoomId bigint=null
)
as
select ClassRoomId,Name,Description,Location,ClassRoom.InsertedOn,ClassRoom.ModifiedOn,ClassRoom.DepartmentId,Departments.DepartmentName, ClassRoom.InsertedBy,ClassRoom.ModifiedBy ,Users.Username as InsertedByName,Users_1.Username as ModifiedByName
from ClassRoom join Departments on ClassRoom.DepartmentId=Departments.DepartmentId join Users on ClassRoom.InsertedBy=Users.UserId left join Users as Users_1 on  ClassRoom.ModifiedBy=Users_1.UserId
where (@organizationId is null or Departments.OrganizationId =@organizationId)
and (@classRoomId is null or ClassRoomId=@classRoomId)
GO

CREATE procedure [dbo].[usp_getClasses]
(
@organizationId bigint=null,
--@courseId bigint=null,
@classId bigint=null
)
as
select Classes.ClassId,Classes.ClassName,Classes.Description,Classes.InsertedOn,Classes.ModifiedOn,Courses.CourseName,Organizations.OrganizationName,Classes.InsertedBy,Classes.ModifiedBy,Users.Username as InsertedByName,Users_1.Username as ModifiedByName
from Classes join Organizations on Classes.OrganizationId=Organizations.OrganizationId join Courses on Classes.CourseId = Courses.CourseId join Users on Classes.InsertedBy=Users.UserId left join Users as Users_1 on  Classes.ModifiedBy=Users_1.UserId
where (@organizationId is null or Classes.OrganizationId =@organizationId)
and (@classId is null or Classes.ClassId=@classId )
GO

create procedure [dbo].[usp_addClassRoom]
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

GO

CREATE procedure [dbo].[usp_addClass]
(
@ClassName as varchar(100)       ,      
@Description as varchar(100)     ,
@OrganizationId as bigint  ,
@CourseId as bigint       ,
@InsertedOn as datetime     ,
@InsertedBy as bigint     
)
as
insert into Classes values
(
@ClassName       ,      
@Description     ,
@InsertedOn      ,
@InsertedBy  ,
null,
null,
@OrganizationId  ,
@CourseId        


)
GO


CREATE procedure [dbo].[usp_getSubjects] --@organizationId=17
(
@organizationId bigint=null,
--@courseId bigint=null,
@subjectId bigint=null
)
as
select SubjectId,SubjectName,
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
       Organizations.OrganizationName
from Subjects 
join Organizations on Subjects.OrganizationId = Organizations.OrganizationId
join Courses on Subjects.CourseId=Courses.CourseId 
join Classes on Subjects.ClassId = Classes.ClassId 
join Users on Subjects.CreatedBy=Users.UserId 
left join Users as Users_1 on  Subjects.ModifiedBy=Users_1.UserId
where (@organizationId is null or Courses.OrganisationId =@organizationId)
and (@subjectId is null or SubjectId=@subjectId)
go

create procedure [dbo].[usp_getSchedules]
(
@userRole as varchar(50)=null,
@organizationId as bigint=-1,
@courseId as bigint=-1,
@subjectId as bigint=-1,
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
GO

CREATE procedure [dbo].[usp_addSubject]
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
GO

create procedure [dbo].[usp_addSection]
(
@SectionName as varchar(100),        
@SectionDescription as varchar(100),
@ClassId as bigint,            
@CreatedBy as bigint,         
@InsertedOn as datetime,
@OrganizationId as bigint,
@CourseId as bigint         
)
as
BEGIN
insert into Sections values(
@SectionName        ,
@SectionDescription ,
@ClassId            ,
@InsertedOn         ,
@CreatedBy          ,
null,
null,
@OrganizationId,
@CourseId
)
END
GO

CREATE procedure [dbo].[usp_GetGroups] --@organizationId=1
(
@groupId as bigint = null,
@organizationId as bigint = null 
)
as
BEGIN
 Select GroupId, 
        GroupName, 
        Description,
        G.InsertedOn,
        InsertedBy,
        ModifieBy,
        ModifiedOn,
        Users.Username as InsertedByName,
        User_1.Username as ModifiedByName,
        O.OrganizationId,
        O.OrganizationName
from [Group] as G
 join Organizations as O on G.OrganizationId = O.OrganizationId
 join Users on G.InsertedBy = Users.UserId
 left join Users as User_1 on G.ModifieBy = User_1.UserId
  where (@groupId is null or GroupId=@groupId)
  and (@organizationId is null or Users.OrgainzationId = @organizationId)
 
END



GO
create procedure [dbo].[usp_DeleteGroup]
(
@GroupId as bigint
)
as
BEGIN
Delete from [Group] where GroupId=@GroupId
END

GO
Create procedure [dbo].[usp_AddGroups]
(
@GroupName as varchar(100),  
@Description as varchar(500),
@InsertedOn  as datetime,
@InsertedBy  as bigint,
@OrganizationId as bigint
)
as
BEGIN
insert into [Group] values(
@GroupName   ,
@Description, 
@InsertedOn,  
@InsertedBy,
null,
null,
@OrganizationId
)  
END
GO


CREATE procedure [dbo].[usp_AddRegistrationToken]
(
@Token as nvarchar(max),           
@OrganizationId as bigint,  
@DepartmentId as int = 0,     
@CourseId as int = 0,
@ClassId as int =0,
@SectionId as int =0,
@RoleId as int,          
@CreatedBy as bigint       

)
as 
BEGIN
		INSERT INTO RegistrationToken values(
		@Token ,
		@OrganizationId ,
		@DepartmentId ,
		@CourseId ,
		@ClassId,
		@SectionId,
		@RoleId,
		@CreatedBy
		)
END
GO

GO
create procedure usp_GetOrganization_Courses --5
(
@OrganizationId as bigint
)
as
BEGIN
		select O.OrganizationId, 
			   C.CourseId,
			   CS.ClassId,
			   S.SubjectId,
			   SC.SectionId
		from Organizations as O
		left join Courses as C on O.OrganizationId = C.OrganisationId
		left join Classes as CS on C.CourseId = CS.CourseId
		left join Subjects as S on CS.ClassId = S.ClassId
		left join Sections as SC on CS.ClassId = SC.ClassId
		where OrganisationId=@OrganizationId
END

GO
create procedure usp_GetOrganization_Departments --2
(
@OrganizationId as bigint
)
as
BEGIN
		select O.OrganizationId, 
			   D.DepartmentId,
			   CR.ClassRoomId
		from Organizations as O
		 left join Departments as D on O.OrganizationId=D.OrganizationId
		 left join ClassRoom as CR on D.DepartmentId = CR.DepartmentId
		where O.OrganizationId=@OrganizationId
END
GO

-- Add templates
create procedure [dbo].[usp_AddTemplates]
(
            @Name as varchar(100)
           ,@Description as varchar(500)
           ,@TemplateText as nvarchar(max)
           ,@IsActive as bit
           ,@InsertedOn as datetime
           ,@InsertedBy as bigint
           ,@OrganizationId as bigint
           ,@TemplateTypeId as bigint
)
as
BEGIN
INSERT INTO [Template]
           ([Name]
           ,[Description]
           ,[TemplateText]
           ,[IsActive]
           ,[InsertedOn]
           ,[InsertedBy]
           ,[UpdatedOn]
           ,[UpdatedBy]
           ,[OrganizationId]
           ,[TemplateTypeId])
     VALUES
           (@Name
           ,@Description
           ,@TemplateText
           ,@IsActive
           ,@InsertedOn
           ,@InsertedBy
           ,null
           ,null
           ,@OrganizationId
           ,@TemplateTypeId
           )
 END
GO
-- GET TEMPLATES
create procedure usp_GetTemplates
(
@TemplateId as bigint=null,
@OrganizationId as bigint = null
)
as
Begin
select 
TemplateId,
Template.Name,
Template.Description,
Template.OrganizationId,
Template.TemplateTypeId,
TemplateText ,
IsActive,
Template.InsertedOn,
Template.InsertedBy,
UpdatedOn,
UpdatedBy,
Users.Username as InsertedByName,
User_1.Username as UpdatedByName,
TemplateType.name as TemplateTypeName,
Organizations.OrganizationName as OrganizationName


from Template
join Organizations on Template.OrganizationId = Organizations.OrganizationId
join TemplateType on Template.TemplateTypeId = TemplateType.TemplateTypeId
join Users on Template.InsertedBy = Users.UserId
left join Users as User_1 on Template.UpdatedBy = User_1.UserId
where (@TemplateId is null or Template.TemplateId = @TemplateId)
and (@OrganizationId is null or Users.OrgainzationId=@OrganizationId)
END

GO
Create Procedure usp_getSchedules
(
@userRole as varchar(50)=null,
@organizationId as bigint=-1,
@courseId as bigint=-1,
@departmentId as bigint=-1,
@classId as bigint=-1,
@createdBy as bigint=-1
)
as
BEGIN
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
  END
  GO
create procedure [dbo].[usp_GetSections] 
(
@organizationId as bigint = null,
@sectionId as bigint = null
)
as 

BEGIN
  select SectionId,
         SectionName,
         SectionDescription,
         Sections.ClassId,
         Sections.InsertedOn,
         sections.CreatedBy,
         Sections.ModifiedBy,
         Sections.ModifiedOn,
         Users.Username as InsertedByName,
         Users_1.Username as ModifiedByName, 
         Classes.ClassName,
         Organizations.OrganizationId,
         organizations.OrganizationName,
         Courses.CourseId,
         Courses.CourseName

 from Sections 
 join Organizations on Sections.OrganizationId = Organizations.OrganizationId
 join Courses on Sections.CourseId = Courses.CourseId
 join Classes on Sections.ClassId=Classes.ClassId 
 join Users on Sections.CreatedBy=Users.UserId 
 left join Users as Users_1 on  Sections.ModifiedBy=Users_1.UserId

 where (@sectionId is null or Sections.SectionId = @sectionId)
   and (@organizationId is null or Users.OrganizationId = @organizationId)
END

GO
-- ADD Events
create procedure [dbo].[usp_AddEvent]
(
            @EventTypeId as bigint
           ,@EventName as varchar(250)
           ,@Description as varchar(250)
           ,@StartDate as datetime
           ,@EndDate as datetime
           ,@StartTime as varchar(250)
           ,@EndTime as varchar(250)
           ,@IsActive as bit
           ,@NotificationTypeId as int
           ,@OrganizationId as bigint
           ,@CourseId as bigint
           ,@ClassId as bigint
           ,@SectionId as int
           ,@InsertedOn as datetime
           ,@InsertedBy as bigint
           
)
as
BEGIN
INSERT INTO [Event]
           (
            [EventTypeId]
           ,[EventName]
           ,[Description]
           ,[StartDate]
           ,[EndDate]
           ,[StartTime]
           ,[EndTime]
           ,[IsActive]
           ,[NotificationTypeId]
           ,[OrganizationId]
           ,[CourseId]
           ,[ClassId]
           ,[SectionId]
           ,[InsertedOn]
           ,[InsertedBy]
           ,[ModifiedOn]
           ,[ModifiedBy]
           )
     VALUES
           (@EventTypeId
           ,@EventName
           ,@Description 
           ,@StartDate 
           ,@EndDate 
           ,@StartTime 
           ,@EndTime 
           ,@IsActive
           ,@NotificationTypeId 
           ,@OrganizationId 
           ,@CourseId 
           ,@ClassId 
           ,@SectionId 
           ,@InsertedOn
           ,@InsertedBy
           ,null
           ,null)
 END

GO
-- GET Events
CREATE procedure usp_GetEvents
(
@EventId as bigint = null,
@OrganizationId as bigint = null
)
as
BEGIN
SELECT [EventId]
      ,E.[EventTypeId]
      ,E.[EventName]
      ,E.[Description]
      ,[StartDate]
      ,[EndDate]
      ,[StartTime]
      ,[EndTime]
      ,E.[IsActive]
      ,[NotificationTypeId]
      ,E.[OrganizationId]
      ,E.[CourseId]
      ,E.[ClassId]
      ,E.[SectionId]
      ,E.[InsertedOn]
      ,E.[InsertedBy]
      ,E.[ModifiedOn]
      ,E.[ModifiedBy]
      ,O.OrganizationName
      ,CR.CourseName
      ,CL.ClassName
      ,SC.SectionName
      ,ET.Name as EventTypeName,
      Users.Username as InsertedByName,
      Users_1.Username as UpdatedByName
  FROM [Event] as E
  join Organizations as O on E.OrganizationId = O.OrganizationId
  join Courses as CR on E.CourseId = CR.CourseId
  join Classes as CL on E.ClassId = CL.ClassId
  join Sections as SC on E.SectionId = Sc.SectionId
  join TemplateType as ET on E.EventTypeId = ET.TemplateTypeId
  join Users on E.InsertedBy = Users.UserId
  left join Users as Users_1 on E.ModifiedBy = Users_1.UserId   
  WHERE (@EventId is null or E.EventId = @EventId)
    AND (@OrganizationId is null or E.OrganizationId = @OrganizationId)
END
GO

-- Get User Events
CREATE procedure usp_GetUsersForEvent  
(
@EventId as bigint
)
as
BEGIN
		select 
			   Users.UserId,
			   EventId,
			   Template.TemplateId,
			   E.OrganizationId
			   from [Event] as E
		join RegistrationToken on E.ClassId = RegistrationToken.ClassId
	    join Users on RegistrationToken.Token = Users.RegistrationToken
		join Template on E.EventTypeId = Template.TemplateTypeId
		where E.EventId = @EventId
		END
GO

-- Add Eletters
CREATE procedure usp_AddEletters
(
            @UserId as bigint
           ,@EventId as bigint
           ,@TemplateId as bigint
           ,@OrganizationId as bigint
           ,@IsRead as bit
           ,@InsertedOn as datetime
           ,@InsertedBy as bigint
           
)
as
BEGIN
		INSERT INTO [ELetter]
				   ([UserId]
				   ,[EventId]
				   ,[TemplateId]
				   ,[OrganizationId]
				   ,[IsRead]
				   ,[InsertedOn]
				   ,[InsertedBy]
				   ,[ModifiedOn]
				   ,[ModifiedBy])
			 VALUES
				   (@UserId
				   ,@EventId
				   ,@TemplateId
				   ,@OrganizationId
				   ,@IsRead
				   ,@InsertedOn
				   ,@InsertedBy
				   ,null
				   ,null)
END

GO
-- Get Eletters
CREATE procedure [dbo].[usp_GetEletters] --@UserId=1
(  
@UserId as bigint,
@EletterId as bigint = null  
)  
as   
BEGIN  
  SELECT [EletterId]  
     ,EL.[UserId]  
     ,EL.[EventId]  
     ,EL.[TemplateId]
     ,TL.TemplateText  
     ,EL.[OrganizationId]  
     ,[IsRead]  
     ,EL.[InsertedOn]  
     ,EL.[InsertedBy]  
     ,EL.[ModifiedOn]  
     ,EL.[ModifiedBy]  
     ,US.Username  
     ,E.EventName  
     ,TL.Name as TemplateName  
     ,O.OrganizationName  
     ,U.Username as InsertedByName  
     ,U_1.Username as UpdatedByName  
  
    FROM [ELetter] as EL  
    join Users as US on EL.UserId = US.UserId  
    join [Event] as E on EL.EventId = E.EventId  
    join Template as TL on El.TemplateId = TL.TemplateId
    join Organizations as O on EL.OrganizationId = O.OrganizationId  
    join Users as U on EL.InsertedBy = U.UserId  
    left join Users as U_1 on EL.ModifiedBy = U_1.UserId  
      
  Where (EL.UserId = @UserId)
    and (@EletterId is null or El.EletterId = @EletterId)
order by EL.[InsertedOn] desc  
END  

GO
-- 28-11-2013
create procedure usp_AssignSubjectToUser
( 
@UserId as bigint,
@SubjectId  as bigint,
@InsertedOn as datetime,
@InsertedBy as bigint
)
as
BEGIN
		INSERT INTO [Student5].[dbo].[UserSubjects]
				   ([UserId]
				   ,[SubjectId]
				   ,[InsertedOn]
				   ,[InsertedBy]
				   ,[UpdatedOn]
				   ,[UpdatedBy])
			 VALUES
				   (@UserId
				   ,@SubjectId
				   ,@InsertedOn
				   ,@InsertedBy
				   ,null
				   ,null)
END













