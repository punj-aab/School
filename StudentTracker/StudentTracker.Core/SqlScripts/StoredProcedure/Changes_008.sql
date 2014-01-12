 alter procedure [dbo].[usp_AssignGroupToUser]  
(  
            @UserId as bigint  
           ,@GroupId as bigint  
           ,@InsertedOn as datetime  
           ,@InsertedBy as bigint 
           ,@StudentId as bigint
           ,@StaffId as bigint 
)  
as  
BEGIN  
INSERT INTO [dbo].[UserGroup]  
           ([UserId]  
           ,[GroupId]  
           ,[InsertedOn]  
           ,[InsertedBy]  
           ,[UpdatedOn]  
           ,[UpdatedBy]
           ,StudentId
           ,StaffId)  
     VALUES  
           (@UserId  
           ,@GroupId  
           ,@InsertedOn  
           ,@InsertedBy  
           ,null  
           ,null
           ,@StudentId
           ,@StaffId)  
           END  



Go
Create procedure usp_getUserProfile
(
@UserId as bigint
)
as
BEGIN
		SELECT TOP 1000 [ProfileId]
			  ,P.[UserId]
			  ,[Address1]
			  ,[Address2]
			  ,[City]
			  ,[StateId]
			  ,[ZipCode]
			  ,[Phone1]
			  ,[Phone2]
			  ,[EmailAddress1]
			  ,[EmailAddress2]
			  ,P.[InsertedOn]
			  ,[ModifiedOn]
			  ,[Title]
			  ,[DateOfBirth]
			  ,[MobileNumber]
			  ,[HomeTelephoneNumber]
			  ,[SecurityQuestionId]
			  ,[SecurityAnswer]
			  ,U.Username
			  ,U.FirstName
			  ,U.LastName
		  FROM [Profile] as P
			   join users as U on P.UserId = U.UserId
		  Where P.UserId = @UserId
  END