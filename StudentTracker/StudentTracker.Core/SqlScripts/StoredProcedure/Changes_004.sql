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
GO

CREATE procedure [dbo].[usp_AddStudent]
(
 @UserId as bigint
,@CourseId as bigint
,@DepartmentId as bigint
,@ClassId as bigint
,@SectionId as bigint
,@RollNo as nvarchar(100)
,@InsertedOn as datetime
,@InsertedBy as bigint
,@Email as varchar(100)
,@OrganizationId as bigint
)
as
BEGIN
		INSERT INTO Student
				   ([UserId]
				   ,[CourseId]
				   ,[DepartmentId]
				   ,[ClassId]
				   ,[SectionId]
				   ,[RollNo]
				   ,[InsertedOn]
				   ,[InsertedBy]
				   ,[ModifiedOn]
				   ,[ModifiedBy]
				   ,[ImportId]
				   ,[Remarks]
				   ,[Email]
				   ,[OrganizationId])
			 VALUES
				   (@UserId
				   ,@CourseId
				   ,@DepartmentId
				   ,@ClassId
				   ,@SectionId
				   ,@RollNo
				   ,@InsertedOn
				   ,@InsertedBy
				   ,null
				   ,null
				   ,null
				   ,null
				   ,@Email
				   ,@OrganizationId)
END

GO

GO

create procedure [dbo].[usp_AssignGroupToUser]
(
            @UserId as bigint
           ,@GroupId as bigint
           ,@InsertedOn as datetime
           ,@InsertedBy as bigint
)
as
BEGIN
INSERT INTO [Student5].[dbo].[UserGroup]
           ([UserId]
           ,[GroupId]
           ,[InsertedOn]
           ,[InsertedBy]
           ,[UpdatedOn]
           ,[UpdatedBy])
     VALUES
           (@UserId
           ,@GroupId
           ,@InsertedOn
           ,@InsertedBy
           ,null
           ,null)
           END

GO

ALTER procedure usp_GetStudents --1,3
(
@OrganizationId as bigint,
@StudentId as bigint = null
)
as
BEGIN
		SELECT [StudentId]
		      ,[UserId]
		      ,S.[CourseId]
		      ,S.[DepartmentId]
		      ,S.[ClassId]
		      ,S.[SectionId]
		      ,[RollNo]
		      ,S.[InsertedOn]
		      ,S.[InsertedBy]
		      ,S.[ModifiedOn]
		      ,S.[ModifiedBy]
		      ,[ImportId]
		      ,[Remarks]
		      ,[Email]
		      ,S.[OrganizationId]
		      ,[FullName],
		      C.CourseId,
		      C.CourseName,
		      D.DepartmentId,
		      D.DepartmentName,
		      CL.ClassId,
		      CL.ClassName,
		      SC.SectionId,
		      SC.SectionName
		  FROM Student as S
		  join Courses As C on S.CourseId = C.CourseId
		  left join Departments as D on S.DepartmentId = D.DepartmentId
		  join Classes as CL on S.ClassId = CL.ClassId
		  join Sections as SC on S.SectionId = SC.SectionId
        Where S.OrganizationId = @OrganizationId and (@StudentId is null or S.StudentId = @StudentId)		  
END
GO
