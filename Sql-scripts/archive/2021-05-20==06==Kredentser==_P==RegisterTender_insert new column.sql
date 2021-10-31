use [uh417455_db2]
go
alter table [dbo].RegisterTenders add tenderOwnerPath NVARCHAR(200) not null;
go
select*from [dbo].RegisterTenders;