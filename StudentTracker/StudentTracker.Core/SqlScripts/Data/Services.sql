Insert into Services(ServiceName,ServiceDescription) 
values('Email','Email Service'),
('SMS','SMS Service'),
('Reports','Reports Service'),
('Payments/Fees',' Fees Service'),
('Calendar','Calendar Service'),
('E Letters',' E Letters Service')


Go
Insert into StaffTypes(StaffTypeName,Description)Values('Teacher','Teacher'),
('Clerk','Clerk'),
('Coach','Coach')

GO

insert into SecurityQuestion values('What is your mother''s born place.',GETDATE(),0)
insert into SecurityQuestion values('What is your favourite pet''s name.',GETDATE(),0)
insert into SecurityQuestion values('What is your father''s name.',GETDATE(),0)
insert into SecurityQuestion values('What is your favourite game.',GETDATE(),0)
insert into SecurityQuestion values('What is your uncle''s home place.',GETDATE(),0)

GO
