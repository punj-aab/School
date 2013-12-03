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


CREATE procedure [dbo].[usp_GetUsersForEvent]  
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