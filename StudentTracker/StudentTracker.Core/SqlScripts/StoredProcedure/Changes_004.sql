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


