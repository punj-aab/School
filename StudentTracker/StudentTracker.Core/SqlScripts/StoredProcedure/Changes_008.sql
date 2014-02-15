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
create procedure usp_getUserProfile  --1
(  
@UserId as bigint  
)  
as  
BEGIN  
  SELECT [ProfileId]  
     ,U.[UserId] 
	 ,P.ProfileImageUrl 
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
    FROM users as U 
     left join [profile] as P on U.UserId = P.UserId  
    Where U.UserId = @UserId  
  END