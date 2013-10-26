-- USE [Student]
GO

/****** Object:  StoredProcedure [dbo].[addOrganization]    Script Date: 10/07/2013 19:56:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[addOrganization]
(
@Address1 as varchar(200),            
@Address2 as varchar(200),                       
@City  as varchar(200),                          
@CountryId as bigint,          
@CreatedBy as int,          
@CreatedDate as datetime,        
@Email   as varchar(200),                        
@OrganizationDesc  as varchar(200),              
@OrganizationName as varchar(200),               
@OrganizationTypeId as bigint, 
@Phone1 as varchar(200),             
@Phone2 as varchar(200),             
@RegisterationNumber as varchar(200),
@StateId as bigint             
)
as
insert into dbo.Organizations values(
@OrganizationName,
@OrganizationDesc,   
@OrganizationTypeId,
@RegisterationNumber ,
@CountryId,         
@StateId,
@City,
@Address1,            
@Address2 ,           
@Email,                
@Phone1,
@Phone2,
@CreatedBy,        
@CreatedDate,
null,
null,
null,
null
)

--select @@IDENTITY as Id
GO

