create procedure usp_GetImportedStudents
(
@ImportId as varchar(50)
)
as
BEGIN
SELECT [StudentId]
      ,[UserId]
      ,S.OrganizationId
      ,S.[CourseId]
      ,S.[DepartmentId]
      ,S.[ClassId]
      ,[SectionId]
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
  FROM Student as S
  JOIN Organizations As O on S.OrganizationId = O.OrganizationId
  JOIN Courses as C on S.CourseId = C.CourseId
  JOIN Classes as CL on S.ClassId = S.ClassId
  LEFT JOIN Departments AS D on S.DepartmentId = D.DepartmentId
  where S.ImportId = @ImportId
END
GO


