use [uh417455_db2]
go
alter table [dbo].RegisterTenders add dateCreate datetime2(7) null;
go
select*from [dbo].RegisterTenders;