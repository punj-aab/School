Create Procedure usp_GetOrganizationServices
(
@organizationId bigint
)
AS 
Begin
select S.*,(case when OS.ServiceId is null then 0 else 1 end)IsAdded  from Services s
left outer join 
(select ServiceId from OrganizationServices where OrganizationId=@organizationId and StatusId=1)OS
 on s.ServiceId = OS.ServiceId 
End