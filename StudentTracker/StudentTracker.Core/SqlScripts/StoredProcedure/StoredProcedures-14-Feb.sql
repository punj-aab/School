CREATE procedure [dbo].[usp_getClassRooms] --@organizationId=18  
(  
@organizationId bigint=null,  
--@courseId bigint=null,  
@classRoomId bigint=null  
)  
as  
select ClassRoomId,  
       Name,  
       Description,  
       Location,  
       ClassRoom.InsertedOn,  
       ClassRoom.ModifiedOn,  
       ClassRoom.DepartmentId,  
       Departments.DepartmentName,   
       ClassRoom.InsertedBy,  
       ClassRoom.ModifiedBy ,  
       Users.Username as InsertedByName,  
       Users_1.Username as ModifiedByName,  
       Organizations.OrganizationId,  
       Organizations.OrganizationName  
from ClassRoom join Organizations on ClassRoom.OrganizationId = Organizations.OrganizationId  
join Departments on ClassRoom.DepartmentId=Departments.DepartmentId   
join Users on ClassRoom.InsertedBy=Users.UserId   
left join Users as Users_1 on  ClassRoom.ModifiedBy=Users_1.UserId  
where (@organizationId is null or Departments.OrganizationId =@organizationId)  
and (@classRoomId is null or ClassRoomId=@classRoomId)  

GO

CREATE procedure [dbo].[usp_getClasses]    
(    
@organizationId bigint=null,    
--@courseId bigint=null,    
@classId bigint=null    
)    
as    
select Classes.ClassId,Classes.ClassName,Classes.Description,Classes.InsertedOn,  
Classes.ModifiedOn,Courses.CourseName,Organizations.OrganizationName,  
Classes.InsertedBy,Classes.ModifiedBy,Users.Username as InsertedByName,  
Users_1.Username as ModifiedByName , s.UserCount  
from Classes join Organizations on Classes.OrganizationId=Organizations.OrganizationId   
join Courses on Classes.CourseId = Courses.CourseId join Users on Classes.InsertedBy=Users.UserId  
 left join Users as Users_1 on  Classes.ModifiedBy=Users_1.UserId    
 left join  (select classid, count(userid) as UserCount from student group by classid ) s   
 on Classes.classid=s.classid  
where (@organizationId is null or Classes.OrganizationId =@organizationId)    
and (@classId is null or Classes.ClassId=@classId ) 


GO

CREATE procedure [dbo].[usp_GetTemplates]  
(  
@TemplateId as bigint=null,  
@OrganizationId as bigint = null  
)  
as  
Begin  
select   
TemplateId,  
Template.Name,  
Template.Description,  
Template.OrganizationId,  
Template.TemplateTypeId,  
TemplateText ,  
IsActive,  
Template.InsertedOn,  
Template.InsertedBy,  
UpdatedOn,  
UpdatedBy,  
Users.Username as InsertedByName,  
User_1.Username as UpdatedByName,  
TemplateType.name as TemplateTypeName,  
Organizations.OrganizationName as OrganizationName  
  
  
from Template  
join Organizations on Template.OrganizationId = Organizations.OrganizationId  
join TemplateType on Template.TemplateTypeId = TemplateType.TemplateTypeId  
join Users on Template.InsertedBy = Users.UserId  
left join Users as User_1 on Template.UpdatedBy = User_1.UserId  
where (@TemplateId is null or Template.TemplateId = @TemplateId)  
and (@OrganizationId is null or Users.OrganizationId=@OrganizationId)  
END  

GO

CREATE Procedure [dbo].[usp_GetStaff]    
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

GO

CREATE procedure [dbo].[usp_GetGroupUsers]     
(    
@GroupId as bigint    
)    
as    
BEGIN    
  select DISTINCT     
      UG.UserId,     
      U.Username,     
      U.FirstName,    
      U.LastName    
         
  from UserGroup as UG     
  join Users as U on UG.UserId = U.UserId    
     Where UG.GroupId = @GroupId    
END  

GO

CREATE procedure [dbo].[usp_GetGroups] --@organizationId=1    
(    
@groupId as bigint = null,    
@organizationId as bigint = null     
)    
as    
BEGIN    
 Select G.GroupId,     
        GroupName,     
        Description,    
        G.InsertedOn,    
        InsertedBy,    
        ModifieBy,    
        ModifiedOn,    
        Users.Username as InsertedByName,    
        User_1.Username as ModifiedByName,    
        O.OrganizationId,    
        O.OrganizationName,  
        UserCount    
from [Group] as G    
 join Organizations as O on G.OrganizationId = O.OrganizationId    
 join Users on G.InsertedBy = Users.UserId    
 left join Users as User_1 on G.ModifieBy = User_1.UserId    
 left join(  
 select GroupId, count(*) 'UserCount' from UserGroup group by GroupId  
 ) UG on G.GroupId=UG.GroupId  
  where (@groupId is null or G.GroupId=@groupId)    
  and (@organizationId is null or Users.OrganizationId = @organizationId)    
     
END 
GO
CREATE procedure [dbo].[usp_getOrganizations]    
(    
@organizationId bigint = null    
)    
as    
select Organizations.OrganizationId,    
       OrganizationName,    
       OrganizationDesc,    
       OrganizationTypeId,    
       RegisterationNumber,    
       CountryId,    
       Organizations.StateId,    
       Address1,    
       Address2,    
       Organizations.Email,    
       Phone1,    
       Phone2,    
       CreatedBy,    
       CreatedDate,    
       ModifiedBy,    
       ModifiedDate,    
       Deletedby,    
       DeletedDate,    
       Users.Username as InsertedByName,    
       Users_1.Username as ModifiedByName,    
       Countries.CountryName as CountryName,    
       States.StateName as StateName,    
       City    
 from Organizations     
 join Countries on Organizations.CountryId=Countries.Id     
 join States on organizations.StateId=States.Id    
 join Users on Organizations.CreatedBy=Users.UserId     
 left join Users as Users_1 on  Organizations.ModifiedBy=Users_1.UserId    
 where (@organizationId is null or Organizations.OrganizationId=@organizationId)  

 GO

 create procedure [dbo].[usp_GetOrganization_Departments] --2  
(  
@OrganizationId as bigint  
)  
as  
BEGIN  
  select O.OrganizationId,   
      D.DepartmentId,  
      CR.ClassRoomId  
  from Organizations as O  
   left join Departments as D on O.OrganizationId=D.OrganizationId  
   left join ClassRoom as CR on D.DepartmentId = CR.DepartmentId  
  where O.OrganizationId=@OrganizationId  
END  

GO
create procedure [dbo].[usp_GetOrganization_Courses] --5  
(  
@OrganizationId as bigint  
)  
as  
BEGIN  
  select O.OrganizationId,   
      C.CourseId,  
      CS.ClassId,  
      S.SubjectId,  
      SC.SectionId  
  from Organizations as O  
  left join Courses as C on O.OrganizationId = C.OrganisationId  
  left join Classes as CS on C.CourseId = CS.CourseId  
  left join Subjects as S on CS.ClassId = S.ClassId  
  left join Sections as SC on CS.ClassId = SC.ClassId  
  where OrganisationId=@OrganizationId  
END  

GO
CREATE procedure [dbo].[usp_GetSections]   
(  
@organizationId as bigint = null,  
@sectionId as bigint = null  
)  
as   
  
BEGIN  
  select SectionId,  
         SectionName,  
         SectionDescription,  
         Sections.ClassId,  
         Sections.InsertedOn,  
         sections.CreatedBy,  
         Sections.ModifiedBy,  
         Sections.ModifiedOn,  
         Users.Username as InsertedByName,  
         Users_1.Username as ModifiedByName,   
         Classes.ClassName,  
         Organizations.OrganizationId,  
         organizations.OrganizationName,  
         Courses.CourseId,  
         Courses.CourseName  
  
 from Sections   
 join Organizations on Sections.OrganizationId = Organizations.OrganizationId  
 join Courses on Sections.CourseId = Courses.CourseId  
 join Classes on Sections.ClassId=Classes.ClassId   
 join Users on Sections.CreatedBy=Users.UserId   
 left join Users as Users_1 on  Sections.ModifiedBy=Users_1.UserId  
  
 where (@sectionId is null or Sections.SectionId = @sectionId)  
   and (@organizationId is null or Users.OrganizationId = @organizationId)  
END  

GO
create procedure [dbo].[usp_getSchedules]  
(  
@userRole as varchar(50)=null,  
@organizationId as bigint=-1,  
@courseId as bigint=-1,  
@subjectId as bigint=-1,  
@departmentId as bigint=-1,  
@classId as bigint=-1,  
@createdBy as bigint=-1  
)  
as  
if(@userRole='SiteAdmin')  
BEGIN  
select * from Organizations  
END  
else  
BEGIN  
select * from Organizations where CreatedBy = @createdBy  
END  
select * from Courses where OrganisationId = @organizationId  
select * from Classes where CourseId = @courseId  
select * from Subjects where ClassId = @classId  
select * from Departments where OrganizationId = @organizationId  
select * from ClassRoom where DepartmentId = @departmentId  

GO
CREATE procedure [dbo].[usp_addSection]  
(  
@SectionName as varchar(100),          
@SectionDescription as varchar(100),  
@ClassId as bigint,              
@CreatedBy as bigint,           
@InsertedOn as datetime,  
@OrganizationId as bigint,  
@CourseId as bigint           
)  
as  
BEGIN  
insert into Sections values(  
@SectionName        ,  
@SectionDescription ,  
@ClassId            ,  
@InsertedOn         ,  
@CreatedBy          ,  
null,  
null,  
@OrganizationId,  
@CourseId  
)  
END  

GO
CREATE procedure [dbo].[usp_GetTeacherSubjects]    
(    
@UserId as BIGINT =null,  
@StaffId as bigint = null    
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
    left join Departments as D on T.DepartmentId = D.DepartmentId    
    join Classes as CL on T.ClassId = CL.ClassId    
    join Sections as S on T.SectionId = S.SectionId    
    Join Subjects as SB on T.SubjectId = SB.SubjectId    
    WHERE (@UserId is null or T.UserId = @UserId)  
      AND (@StaffId is null or T.StaffId = @StaffId)     
  END  
  GO
  CREATE procedure [dbo].[usp_getSubjects] --@organizationId=17  
(  
@organizationId bigint=null,  
--@courseId bigint=null,  
@subjectId bigint=null  
)  
as  
select SubjectId,SubjectName,  
       SubjectDescription,  
       Subjects.CourseId,  
       Courses.CourseName,   
       Subjects.InsertedOn,  
       Subjects.CreatedBy,  
       Subjects.ModifiedBy,  
       Subjects.ModifiedOn,  
       Subjects.ModifiedBy,  
       Subjects.ClassId,  
       Classes.ClassName,   
       Users.Username as InsertedByName,  
       Users_1.Username as ModifiedByName,  
       Organizations.OrganizationId,  
       Organizations.OrganizationName  
from Subjects   
join Organizations on Subjects.OrganizationId = Organizations.OrganizationId  
join Courses on Subjects.CourseId=Courses.CourseId   
join Classes on Subjects.ClassId = Classes.ClassId   
join Users on Subjects.CreatedBy=Users.UserId   
left join Users as Users_1 on  Subjects.ModifiedBy=Users_1.UserId  
where (@organizationId is null or Courses.OrganisationId =@organizationId)  
and (@subjectId is null or SubjectId=@subjectId)  

GO

CREATE procedure [dbo].[usp_addSubject]  
(  
@SubjectName as varchar(100),         
@SubjectDescription as varchar(100),  
@OrganizationId as bigint,  
@CourseId as bigint,            
@ClassId  as bigint,            
@CreatedBy as bigint,           
@InsertedOn as datetime  
)  
as  
insert into Subjects values(  
@SubjectName,  
@SubjectDescription,  
@CourseId ,  
@InsertedOn,  
@CreatedBy ,  
null,  
null,  
@ClassId,  
@OrganizationId  
)  
GO

CREATE procedure [dbo].[usp_AddStudent]    
(    
 @UserId as bigint    
,@CourseId as bigint    
,@DepartmentId as bigint    
,@ClassId as bigint    
,@SectionId as bigint    
,@RollNo as nvarchar(100)    
,@InsertedOn as datetime    
,@InsertedBy as bigint    
,@Email as varchar(100)    
,@OrganizationId as bigint    
,@FullName as varchar(200)  
)    
as    
BEGIN    
  INSERT INTO Student    
       ([UserId]    
       ,[OrganizationId]  
       ,[CourseId]    
       ,[DepartmentId]    
       ,[ClassId]    
       ,[SectionId]    
       ,[RollNo]    
       ,[InsertedOn]    
       ,[InsertedBy]    
       ,[ModifiedOn]    
       ,[ModifiedBy]    
       ,[ImportId]    
       ,[Remarks]    
       ,[Email]    
       )    
    VALUES    
       (@UserId    
       ,@OrganizationId        
       ,@CourseId    
       ,@DepartmentId    
       ,@ClassId    
       ,@SectionId    
       ,@RollNo    
       ,@InsertedOn    
       ,@InsertedBy    
       ,null    
       ,null    
       ,null    
       ,null    
       ,@Email  
       )  
    
END  

GO

-- ADD Events  
create procedure [dbo].[usp_AddEvent]  
(  
            @EventTypeId as bigint  
           ,@EventName as varchar(250)  
           ,@Description as varchar(250)  
           ,@StartDate as datetime  
           ,@EndDate as datetime  
           ,@StartTime as varchar(250)  
           ,@EndTime as varchar(250)  
           ,@IsActive as bit  
           ,@NotificationTypeId as int  
           ,@OrganizationId as bigint  
           ,@CourseId as bigint  
           ,@ClassId as bigint  
           ,@SectionId as int  
           ,@InsertedOn as datetime  
           ,@InsertedBy as bigint  
             
)  
as  
BEGIN  
INSERT INTO [Event]  
           (  
            [EventTypeId]  
           ,[EventName]  
           ,[Description]  
           ,[StartDate]  
           ,[EndDate]  
           ,[StartTime]  
           ,[EndTime]  
           ,[IsActive]  
           ,[NotificationTypeId]  
           ,[OrganizationId]  
           ,[CourseId]  
           ,[ClassId]  
           ,[SectionId]  
           ,[InsertedOn]  
           ,[InsertedBy]  
           ,[ModifiedOn]  
           ,[ModifiedBy]  
           )  
     VALUES  
           (@EventTypeId  
           ,@EventName  
           ,@Description   
           ,@StartDate   
           ,@EndDate   
           ,@StartTime   
           ,@EndTime   
           ,@IsActive  
           ,@NotificationTypeId   
           ,@OrganizationId   
           ,@CourseId   
           ,@ClassId   
           ,@SectionId   
           ,@InsertedOn  
           ,@InsertedBy  
           ,null  
           ,null)  
 END  
 GO

 -- Get User Events  
CREATE procedure [dbo].[usp_GetUsersForEvent]    
(  
@EventId as bigint  
)  
as  
BEGIN  
  select   
      Users.UserId,  
      EventId,  
      Template.TemplateId,  
      E.OrganizationId  
      from [Event] as E  
  join RegistrationToken on E.ClassId = RegistrationToken.ClassId  
     join Users on RegistrationToken.Token = Users.RegistrationToken  
  join Template on E.EventTypeId = Template.TemplateTypeId  
  where E.EventId = @EventId  
  END  

  GO
  CREATE procedure [dbo].[usp_GetStudents] --1--1--1,3    
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

CREATE procedure [dbo].[usp_GetImportedStudents] --'635206550878328371'    
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

GO

-- GET Events  
CREATE procedure [dbo].[usp_GetEvents]  
(  
@EventId as bigint = null,  
@OrganizationId as bigint = null  
)  
as  
BEGIN  
SELECT [EventId]  
      ,E.[EventTypeId]  
      ,E.[EventName]  
      ,E.[Description]  
      ,[StartDate]  
      ,[EndDate]  
      ,[StartTime]  
      ,[EndTime]  
      ,E.[IsActive]  
      ,[NotificationTypeId]  
      ,E.[OrganizationId]  
      ,E.[CourseId]  
      ,E.[ClassId]  
      ,E.[SectionId]  
      ,E.[InsertedOn]  
      ,E.[InsertedBy]  
      ,E.[ModifiedOn]  
      ,E.[ModifiedBy]  
      ,O.OrganizationName  
      ,CR.CourseName  
      ,CL.ClassName  
      ,SC.SectionName  
      ,ET.Name as EventTypeName,  
      Users.Username as InsertedByName,  
      Users_1.Username as UpdatedByName  
  FROM [Event] as E  
  join Organizations as O on E.OrganizationId = O.OrganizationId  
  join Courses as CR on E.CourseId = CR.CourseId  
  join Classes as CL on E.ClassId = CL.ClassId  
  join Sections as SC on E.SectionId = Sc.SectionId  
  join TemplateType as ET on E.EventTypeId = ET.TemplateTypeId  
  join Users on E.InsertedBy = Users.UserId  
  left join Users as Users_1 on E.ModifiedBy = Users_1.UserId     
  WHERE (@EventId is null or E.EventId = @EventId)  
    AND (@OrganizationId is null or E.OrganizationId = @OrganizationId)  
END  

GO

-- Get Eletters  
CREATE procedure [dbo].[usp_GetEletters] --@UserId=1  
(    
@UserId as bigint,  
@EletterId as bigint = null    
)    
as     
BEGIN    
  SELECT [EletterId]    
     ,EL.[UserId]    
     ,EL.[EventId]    
     ,EL.[TemplateId]  
     ,TL.TemplateText    
     ,EL.[OrganizationId]    
     ,[IsRead]    
     ,EL.[InsertedOn]    
     ,EL.[InsertedBy]    
     ,EL.[ModifiedOn]    
     ,EL.[ModifiedBy]    
     ,US.Username    
     ,E.EventName    
     ,TL.Name as TemplateName    
     ,O.OrganizationName    
     ,U.Username as InsertedByName    
     ,U_1.Username as UpdatedByName    
    
    FROM [ELetter] as EL    
    join Users as US on EL.UserId = US.UserId    
    join [Event] as E on EL.EventId = E.EventId    
    join Template as TL on El.TemplateId = TL.TemplateId  
    join Organizations as O on EL.OrganizationId = O.OrganizationId    
    join Users as U on EL.InsertedBy = U.UserId    
    left join Users as U_1 on EL.ModifiedBy = U_1.UserId    
        
  Where (EL.UserId = @UserId)  
    and (@EletterId is null or El.EletterId = @EletterId)  
order by EL.[InsertedOn] desc    
END  

GO

-- Add Eletters  
CREATE procedure [dbo].[usp_AddEletters]  
(  
            @UserId as bigint  
           ,@EventId as bigint  
           ,@TemplateId as bigint  
           ,@OrganizationId as bigint  
           ,@IsRead as bit  
           ,@InsertedOn as datetime  
           ,@InsertedBy as bigint  
             
)  
as  
BEGIN  
  INSERT INTO [ELetter]  
       ([UserId]  
       ,[EventId]  
       ,[TemplateId]  
       ,[OrganizationId]  
       ,[IsRead]  
       ,[InsertedOn]  
       ,[InsertedBy]  
       ,[ModifiedOn]  
       ,[ModifiedBy])  
    VALUES  
       (@UserId  
       ,@EventId  
       ,@TemplateId  
       ,@OrganizationId  
       ,@IsRead  
       ,@InsertedOn  
       ,@InsertedBy  
       ,null  
       ,null)  
END  

GO

CREATE procedure usp_getUserProfile    
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

  GO

    
CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]  
(  
    @Application NVARCHAR(60),  
    @ErrorId UNIQUEIDENTIFIER  
)  
AS  
  
    SET NOCOUNT ON  
  
    SELECT   
        [AllXml]  
    FROM   
        [ELMAH_Error]  
    WHERE  
        [ErrorId] = @ErrorId  
    AND  
        [Application] = @Application  
  

  GO

    
CREATE PROCEDURE [dbo].[ELMAH_LogError]  
(  
    @ErrorId UNIQUEIDENTIFIER,  
    @Application NVARCHAR(60),  
    @Host NVARCHAR(30),  
    @Type NVARCHAR(100),  
    @Source NVARCHAR(60),  
    @Message NVARCHAR(500),  
    @User NVARCHAR(50),  
    @AllXml NTEXT,  
    @StatusCode INT,  
    @TimeUtc DATETIME  
)  
AS  
  
    SET NOCOUNT ON  
  
    INSERT  
    INTO  
        [ELMAH_Error]  
        (  
            [ErrorId],  
            [Application],  
            [Host],  
            [Type],  
            [Source],  
            [Message],  
            [User],  
            [AllXml],  
            [StatusCode],  
            [TimeUtc]  
        )  
    VALUES  
        (  
            @ErrorId,  
            @Application,  
            @Host,  
            @Type,  
            @Source,  
            @Message,  
            @User,  
            @AllXml,  
            @StatusCode,  
            @TimeUtc  
        )  
  
  GO

  CREATE procedure [dbo].[usp_AssignSubjectToUser]    
(     
@UserId as bigint,    
@SubjectId  as bigint,    
@InsertedOn as datetime,    
@InsertedBy as bigint,  
@StudentId as bigint    
)    
as    
BEGIN    
  INSERT INTO [dbo].[UserSubjects]    
       ([UserId]    
       ,[SubjectId]    
       ,[InsertedOn]    
       ,[InsertedBy]    
       ,[UpdatedOn]    
       ,[UpdatedBy],  
       [StudentId] )   
    VALUES    
       (@UserId    
       ,@SubjectId    
       ,@InsertedOn    
       ,@InsertedBy    
       ,null    
       ,null  
       ,@StudentId  
       )    
END  

GO

 CREATE procedure [dbo].[usp_AssignGroupToUser]    
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
  
  
  GO

  CREATE procedure [dbo].[usp_AddRegistrationToken]      
(      
@Token as nvarchar(max),                 
@OrganizationId as bigint,        
@DepartmentId as int = 0,           
@CourseId as int = 0,      
@ClassId as int =0,      
@SectionId as int =0,      
@RoleId as int,                
@CreatedBy as bigint,             
@StudentId as bigint=0,      
@StaffId as bigint = null  ,  
 @Email as nvarchar(100)=null     
)      
as       
BEGIN      
  INSERT INTO RegistrationToken
  (Token,OrganizationId,DepartmentId,CourseId,ClassId,
  SectionId,RoleId,StudentId,StaffId,CreatedBy,InsertedOn,ImportId,Email)
   values(      
  @Token ,      
  @OrganizationId ,      
  @DepartmentId ,      
  @CourseId ,      
  @ClassId,      
  @SectionId,      
  @RoleId,      
  @StudentId,      
  @StaffId,      
  @CreatedBy,    
  GETDATE(),  
  NULL,  
  @Email   
  )      
END 

GO

CREATE procedure [dbo].[usp_addOrganization](  
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

GO

CREATE procedure [dbo].[usp_GetImportedStaff] --'635206550878328371'    
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

CREATE Procedure [dbo].[usp_GetOrganizationServices]  --1  
(    
@organizationId bigint    
)    
AS       
Begin      
select S.*,sc.CategoryName, isnull(OS.Id,0) Id, (case when OS.ServiceId is null then 0 else 1 end)IsAdded, 0 as Modified  from Services s      
left outer join       
(select ServiceId, Id from OrganizationServices where OrganizationId=@organizationId and StatusId=1)OS      
 on s.ServiceId = OS.ServiceId    
 inner join servicecategory sc on sc.id=s.servicecategoryid    
End    

GO

create procedure [dbo].[usp_DeleteGroup]  
(  
@GroupId as bigint  
)  
as  
BEGIN  
Delete from [Group] where GroupId=@GroupId  
END  

GO

-- Add templates  
CREATE procedure [dbo].[usp_AddTemplates]  
(  
            @Name as varchar(100)  
           ,@Description as varchar(500)  
           ,@TemplateText as nvarchar(max)  
           ,@IsActive as bit  
           ,@InsertedOn as datetime  
           ,@InsertedBy as bigint  
           ,@OrganizationId as bigint  
           ,@TemplateTypeId as bigint  
)  
as  
BEGIN  
INSERT INTO [Template]  
           ([Name]  
           ,[Description]  
           ,[TemplateText]  
           ,[IsActive]  
           ,[InsertedOn]  
           ,[InsertedBy]  
           ,[UpdatedOn]  
           ,[UpdatedBy]  
           ,[OrganizationId]  
           ,[TemplateTypeId])  
     VALUES  
           (@Name  
           ,@Description  
           ,@TemplateText  
           ,@IsActive  
           ,@InsertedOn  
           ,@InsertedBy  
           ,null  
           ,null  
           ,@OrganizationId  
           ,@TemplateTypeId  
           )  
 END  
 GO

 CREATE procedure [dbo].[usp_AddGroups]  
(  
@GroupName as varchar(100),    
@Description as varchar(500),  
@InsertedOn  as datetime,  
@InsertedBy  as bigint,  
@OrganizationId as bigint  
)  
as  
BEGIN  
insert into [Group] values(  
@GroupName   ,  
@Description,   
@InsertedOn,    
@InsertedBy,  
null,  
null,  
@OrganizationId  
)    
END  

GO

create procedure [dbo].[usp_AddDepartment]    
(    
@DepartmentName varchar(200),                
@DepartmentDesc as varchar(200),                           
@OrganizationId  as int,                              
@CreatedDate as datetime,              
@CreatedBy as bigint             
                
)    
as    
insert into dbo.Departments values(    
@DepartmentName,    
@DepartmentDesc,       
@OrganizationId,    
@CreatedDate ,    
@CreatedBy,    
null,    
null,    
null,    
null    
)  

GO

create procedure [dbo].[usp_addCourse]  
(  
@CourseName as varchar(100),         
@CourseDescription as varchar(100),  
@OrganisationId as bigint,     
@CreatedBy as bigint,         
@InsertedOn as datetime        
)  
as  
insert into Courses values(  
@CourseName,  
@CourseDescription,  
@OrganisationId   ,  
@CreatedBy        ,  
@InsertedOn ,  
 null,  
 null,  
 null       
)  

GO

CREATE procedure [dbo].[usp_addClassRoom]  
(  
@Name as varchar(100),                
@Description as varchar(100),  
@Location as varchar(100),     
@InsertedOn as datetime,   
@InsertedBy as bigint,   
@DepartmentId as int,  
@OrganizationId as int  
)  
as   
BEGIN  
     insert into ClassRoom(OrganizationId,DepartmentId,Name,Description,Location,InsertedOn,InsertedBy,ModifiedOn,ModifiedBy) values  
     (  
  @OrganizationId,  
  @DepartmentId,  
  @Name,          
  @Description,   
  @Location,      
  @InsertedOn,    
  @InsertedBy,    
  null,  
  null  
)  
END  

GO

CREATE procedure [dbo].[usp_addClass]  
(  
@ClassName as varchar(100)       ,        
@Description as varchar(100)     ,  
@OrganizationId as bigint  ,  
@CourseId as bigint       ,  
@InsertedOn as datetime     ,  
@InsertedBy as bigint       
)  
as  
insert into Classes values  
(  
@ClassName       ,        
@Description     ,  
@InsertedOn      ,  
@InsertedBy  ,  
null,  
null,  
@OrganizationId  ,  
@CourseId          
  
  
)  

GO

CREATE procedure [dbo].[usp_getDepartments] --@departmentId=2  
(  
@organizationId bigint=null,  
--@courseId bigint=null,  
@departmentId bigint=null  
)  
as  
select Departments.DepartmentId,Departments.DepartmentName,Departments.DepartmentDesc,Departments.CreatedBy,Departments.CreatedDate,Departments.UpdatedBy,Departments.UpdatedDate, 
Organizations.OrganizationName,Users.Username as InsertedByName,Users_1.Username as ModifiedByName,Departments.OrganizationId  
from Departments join Organizations on Departments.OrganizationId=Organizations.OrganizationId join Users on Departments.CreatedBy=Users.UserId left join Users as Users_1 on  Departments.UpdatedBy=Users_1.UserId  
  
where (@departmentId is null or DepartmentId=@departmentId) and  
(@organizationId is null or Departments.OrganizationId = @organizationId)  

GO

CREATE procedure [dbo].[usp_getCourses]  
(  
@courseId bigint=null,  
@organizationId bigint=null  
)  
as  
select CourseId,CourseName,CourseDescription,Courses.CreatedBy,Courses.InsertedOn,Courses.ModifiedBy,ModifiedOn,Courses.OrganisationId,Organizations.OrganizationName, Users.Username as InsertedByName,Users_1.Username as ModifiedByName  
from Courses join Organizations on Courses.OrganisationId=Organizations.OrganizationId join Users on Courses.CreatedBy=Users.UserId left join Users as Users_1 on  Courses.ModifiedBy=Users_1.UserId  
where (@courseId is null or CourseId=@courseId )  
and (@organizationId is null or Courses.OrganisationId=@organizationId)  

GO

