-- USE [Student]
GO
/****** Object:  StoredProcedure [dbo].[getOrganizations]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getOrganizations]
(
@organizationId bigint = null
)
as
select OrganizationId,
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
/****** Object:  StoredProcedure [dbo].[addOrganization]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[addOrganization]
(
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

--select @@IDENTITY as Id
GO
/****** Object:  StoredProcedure [dbo].[addCourse]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[addCourse]
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
/****** Object:  StoredProcedure [dbo].[getDepartments]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getDepartments] --@departmentId=2
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
/****** Object:  StoredProcedure [dbo].[getCourses]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getCourses]
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
/****** Object:  StoredProcedure [dbo].[sp_AddDepartment]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_AddDepartment]  
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
/****** Object:  StoredProcedure [dbo].[getClassRooms]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getClassRooms] --@organizationId=18
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
/****** Object:  StoredProcedure [dbo].[getClasses]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getClasses]
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
/****** Object:  StoredProcedure [dbo].[addClassRoom]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- USE [Student1]
GO
Create procedure [dbo].[addClassRoom]
(
@Name as varchar(100),              
@Description as varchar(100),
@Location as varchar(100),   
@InsertedOn as datetime, 
@InsertedBy as bigint, 
@DepartmentId as bigint
)
as 
BEGIN
     insert into ClassRoom values
     (
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
/****** Object:  StoredProcedure [dbo].[addClass]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[addClass]
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
/****** Object:  StoredProcedure [dbo].[getSubjects]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getSubjects] --@organizationId=17
(
@organizationId bigint=null,
--@courseId bigint=null,
@subjectId bigint=null
)
as
select SubjectId,SubjectName,SubjectDescription,Subjects.CourseId,Courses.CourseName, Subjects.InsertedOn,Subjects.CreatedBy,Subjects.ModifiedBy,Subjects.ModifiedOn,Subjects.ModifiedBy,Subjects.ClassId,Classes.ClassName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName
from Subjects join Courses on Subjects.CourseId=Courses.CourseId join Classes on Subjects.ClassId = Classes.ClassId join Users on Subjects.CreatedBy=Users.UserId left join Users as Users_1 on  Subjects.ModifiedBy=Users_1.UserId
where (@organizationId is null or Courses.OrganisationId =@organizationId)
and (@subjectId is null or SubjectId=@subjectId)
GO
/****** Object:  StoredProcedure [dbo].[getSchedules]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[getSchedules]
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
/****** Object:  StoredProcedure [dbo].[addSubject]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[addSubject]
(
@SubjectName as varchar(100),       
@SubjectDescription as varchar(100),
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
@ClassId
)
GO
/****** Object:  StoredProcedure [dbo].[addSection]    Script Date: 10/08/2013 21:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[addSection]
(
@SectionName as varchar(100),        
@SectionDescription as varchar(100),
@ClassId as bigint,            
@CreatedBy as bigint,         
@InsertedOn as datetime         
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
null
)
END
GO

create procedure [dbo].[sp_GetGroups]
(
@groupId as bigint =null
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
        User_1.Username as ModifiedByName
from [Group] as G
 join Users on G.InsertedBy = Users.UserId
 left join Users as User_1 on G.ModifieBy = User_1.UserId
  where @groupId is null or GroupId=@groupId
 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteGroup]    Script Date: 10/11/2013 22:36:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_DeleteGroup]
(
@GroupId as bigint
)
as
BEGIN
Delete from [Group] where GroupId=@GroupId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddGroups]    Script Date: 10/11/2013 22:36:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_AddGroups]
(
@GroupName as varchar(100),  
@Description as varchar(500),
@InsertedOn  as datetime,
@InsertedBy  as bigint
)
as
BEGIN
insert into [Group] values(
@GroupName   ,
@Description, 
@InsertedOn,  
@InsertedBy,
null,
null
)  
END
GO


-- USE [Student2]
GO

/****** Object:  StoredProcedure [dbo].[SP_AddRegistrationToken]    Script Date: 10/17/2013 22:01:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[SP_AddRegistrationToken]
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
create procedure sp_GetOrganization_Courses --5
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
create procedure sp_GetOrganization_Departments --2
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
Create procedure sp_AddTemplates
(
            @Name as varchar(100)
           ,@Description as varchar(500)
           ,@TemplateText as nvarchar(max)
           ,@IsActive as bit
           ,@InsertedOn as datetime
           ,@InsertedBy as bigint
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
           ,[UpdatedBy])
     VALUES
           (@Name
           ,@Description
           ,@TemplateText
           ,@IsActive
           ,@InsertedOn
           ,@InsertedBy
           ,null
           ,null
           )
 END
GO


-- GET TEMPLATES
Create procedure sp_GetTemplates
(
@TemplateId as bigint=null,
@OrganizationId as bigint = null
)
as
Begin
select 
TemplateId,
Name,
Description,
TemplateText ,
IsActive,
Template.InsertedOn,
InsertedBy,
UpdatedOn,
UpdatedBy,
Users.Username as InsertedByName,
User_1.Username as UpdatedByName

from Template
join Users on Template.InsertedBy = Users.UserId
left join Users as User_1 on Template.UpdatedBy = User_1.UserId
where (@TemplateId is null or Template.TemplateId = @TemplateId)
and (@OrganizationId is null or Users.OrgainzationId=@OrganizationId)
END