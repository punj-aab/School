Create Procedure usp_GetOrganizationServices
(
@organizationId bigint
)
AS   
Begin  
select S.*, isnull(OS.Id,0) Id, (case when OS.ServiceId is null then 0 else 1 end)IsAdded, 0 as Modified  from Services s  
left outer join   
(select ServiceId, Id from OrganizationServices where OrganizationId=@organizationId and StatusId=1)OS  
 on s.ServiceId = OS.ServiceId  
End

GO
ALTER procedure [dbo].[usp_AddRegistrationToken]
(
@Token as nvarchar(max),           
@OrganizationId as bigint,  
@DepartmentId as int = 0,     
@CourseId as int = 0,
@ClassId as int =0,
@SectionId as int =0,
@RoleId as int,          
@CreatedBy as bigint,       
@StudentId as bigint=0
)
as 
BEGIN
		INSERT INTO RegistrationToken values(
		@Token ,
		@OrganizationId ,
		@DepartmentId ,
		@CourseId ,
		@ClassId,
		@SectionId,
		@RoleId,
		@StudentId,
		@CreatedBy
		)
END
GO
