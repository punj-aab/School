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


-- new 26-12-2013

CREATE Procedure usp_GetStaff
(
@StaffId as BIGINT = null,
@OrganizationId as BIGINT = null
)
AS
BEGIN
		SELECT 
		      S.StaffId
			  ,S.UserId
			  ,P.Title
			  ,U.FirstName +' '+U.LastName as FullName
			  ,S.StaffTypeId
			  ,[ImportId]
			  ,[Remarks]
			  ,S.Email
			  ,S.InsertedOn
			  ,[InsertedBy]
			  ,[ModifieBy]
			  ,S.ModifiedOn
			  ,S.OrganizationId
			  ,U.Username
			  ,P.ProfileId
			  ,ST.StaffTypeName
			  ,O.OrganizationName
			  ,U_1.Username as InsertedByName
			  ,U_2.Username as ModifiedByName
		 FROM staff as S
		JOIN Users as U on S.UserId = U.UserId
		JOIN [Profile] as P on U.UserId = P.UserId
		JOIN StaffTypes as ST on S.StaffTypeId = ST.StaffTypeId
		JOIN Organizations as O on S.OrganizationId =O.OrganizationId
		JOIN Users as U_1 on S.InsertedBy = U_1.UserId
		LEFT JOIN Users as U_2 on S.ModifieBy = U_2.UserId
         WHERE (@StaffId is null or	S.StaffId = @StaffId) 
           AND (@OrganizationId	is null or S.OrganizationId = @OrganizationId)
		
END

GO

-- Get Teacher Subjects
create procedure usp_GetTeacherSubjects 
(
@UserId as BIGINT
)
AS
BEGIN
		SELECT Id
			  ,T.[UserId]
			  ,T.[CourseId]
			  ,T.[DepartmentId]
			  ,T.[ClassId]
			  ,T.[SectionId]
			  ,T.[SubjectId]
			  ,U.Username,
			  C.CourseName,
			  D.DepartmentName,
			  CL.ClassName,
			  S.SectionName,
			  SB.SubjectName
		  FROM TeacherSubjects as T
		  join Users as U on T.UserId = U.UserId
		  join Courses as C on T.CourseId = C.CourseId
		  join Departments as D on T.DepartmentId = D.DepartmentId
		  join Classes as CL on T.ClassId = CL.ClassId
		  join Sections as S on T.SectionId = S.SectionId
		  Join Subjects as SB on T.SubjectId = SB.SubjectId
		  WHERE T.UserId = @UserId
  END