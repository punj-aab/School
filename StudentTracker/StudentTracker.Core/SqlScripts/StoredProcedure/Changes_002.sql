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

go 
alter procedure [dbo].[usp_addSection]
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
go
ALTER procedure [dbo].[usp_GetSections] 
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
   and (@organizationId is null or Users.OrgainzationId = @organizationId)
END

GO
ALTER procedure [dbo].[usp_AddGroups]
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
ALTER procedure [dbo].[usp_GetGroups] --@organizationId=1
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
-- Add templates
ALTER procedure [dbo].[usp_AddTemplates]
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
 ALTER procedure usp_GetTemplates
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
