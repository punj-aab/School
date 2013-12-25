Insert into StaffTypes(StaffTypeName,Description)Values('Teacher','Teacher'),('Clerk','Clerk'),('Coach','Coach')

GO

CREATE procedure usp_GetImportedStaff --'635206550878328371'  
(  
@ImportId as varchar(50)  
)  
as  
BEGIN  
SELECT [StaffId],  
S.FullName  
      ,[UserId]  
      ,S.OrganizationId  
      
      ,S.[InsertedOn]  
      ,S.[InsertedBy]  
      ,S.[ModifiedOn]  
      
      ,[ImportId]  
      ,[Remarks]  
      ,S.[Email]  
      ,O.OrganizationName  
      
     
  FROM Staff as S  
  JOIN Organizations As O on S.OrganizationId = O.OrganizationId  
  where S.ImportId = @ImportId  
END  
GO
