USE [Student2]
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




