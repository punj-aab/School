ALTER procedure [dbo].[usp_GetStudents] --1--1--1,3  
(  
@OrganizationId as bigint,  
@StudentId as bigint = null  
)  
as  
BEGIN  
  SELECT [StudentId]  
        ,S.[UserId]  
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
        ,S.[Email]  
        ,S.[OrganizationId]  
        ,S.FullName,  
        C.CourseId,  
        C.CourseName,  
        D.DepartmentId,  
        D.DepartmentName,  
        CL.ClassId,  
        CL.ClassName,  
        SC.SectionId,  
        SC.SectionName  
        ,P.Title
        ,U.FirstName
        ,U.LastName
        ,P.DateOfBirth
        ,P.MobileNumber
        ,P.HomeTelephoneNumber
        ,U_1.Username as InsertedByName
        ,U_2.Username as ModifiedByName
    FROM Student as S  
    left join Courses As C on S.CourseId = C.CourseId  
    left join Departments as D on S.DepartmentId = D.DepartmentId  
    left join Classes as CL on S.ClassId = CL.ClassId  
    left join Sections as SC on S.SectionId = SC.SectionId  
    left join Users as U on S.UserId = U.UserId
    left join [Profile] as P on S.UserId = P.UserId
    left join Users as U_1 on S.InsertedBy = U_1.UserId
    left join Users as U_2 on S.ModifiedBy = U_2.UserId
        Where S.OrganizationId = @OrganizationId and (@StudentId is null or S.StudentId = @StudentId)      
END  

GO

ALTER Procedure [dbo].[usp_GetStaff]  
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
     ,S.FullName  
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
     ,U.FirstName  
     ,U.LastName
     ,P.ProfileId  
     ,P.DateOfBirth
     ,P.MobileNumber
     ,P.HomeTelephoneNumber
     ,ST.StaffTypeName  
     ,O.OrganizationName  
     ,U_1.Username as InsertedByName  
     ,U_2.Username as ModifiedByName  
   FROM staff as S  
 left JOIN Users as U on S.UserId = U.UserId  
 left JOIN [Profile] as P on U.UserId = P.UserId  
 left JOIN StaffTypes as ST on S.StaffTypeId = ST.StaffTypeId  
 left JOIN Organizations as O on S.OrganizationId =O.OrganizationId  
 left JOIN Users as U_1 on S.InsertedBy = U_1.UserId  
  LEFT JOIN Users as U_2 on S.ModifieBy = U_2.UserId  
         WHERE (@StaffId is null or S.StaffId = @StaffId)   
           AND (@OrganizationId is null or S.OrganizationId = @OrganizationId)  
    
END  











