
GO
/****** Object:  StoredProcedure [dbo].[getOrganizations]    Script Date: 10/03/2013 23:45:52 ******/
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
/****** Object:  StoredProcedure [dbo].[getDepartments]    Script Date: 10/03/2013 23:45:52 ******/
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
/****** Object:  StoredProcedure [dbo].[getCourses]    Script Date: 10/03/2013 23:45:52 ******/
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
/****** Object:  StoredProcedure [dbo].[getClassRooms]    Script Date: 10/03/2013 23:45:52 ******/
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
/****** Object:  StoredProcedure [dbo].[getClasses]    Script Date: 10/03/2013 23:45:52 ******/
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
/****** Object:  StoredProcedure [dbo].[getSubjects]    Script Date: 10/03/2013 23:45:52 ******/
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
/****** Object:  StoredProcedure [dbo].[getSchedules]    Script Date: 10/03/2013 23:45:52 ******/
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
--select @@IDENTITY as Id  