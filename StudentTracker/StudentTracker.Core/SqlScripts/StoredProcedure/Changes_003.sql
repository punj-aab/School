CREATE procedure usp_GetImportedStudents --'635206550878328371'  
(  
@ImportId as varchar(50)  
)  
as  
BEGIN  
SELECT [StudentId],  
S.FullName  
      ,[UserId]  
      ,S.OrganizationId  
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
      ,O.OrganizationName  
      ,C.CourseName  
      ,CL.ClassName  
      ,D.DepartmentName  
      ,SL.SectionName  
  FROM Student as S  
  JOIN Organizations As O on S.OrganizationId = O.OrganizationId  
  JOIN Courses as C on S.CourseId = C.CourseId  
  JOIN Classes as CL on CL.ClassId = S.ClassId  
  JOIN Sections as SL on SL.SectionId = S.SectionId  
    
  LEFT JOIN Departments AS D on S.DepartmentId = D.DepartmentId  
  where S.ImportId = @ImportId  
END  