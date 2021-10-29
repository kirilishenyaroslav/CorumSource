use [uh417455_db2]
go
alter table [dbo].RegisterTenders add uuidFile uniqueidentifier null;
go
alter table [dbo].RegisterTenders add lotStateName text null;
go
alter table [dbo].RegisterTenders add lotResultNote text null;
go
alter table [dbo].RegisterTenders alter column resultsTender text null;
go
select*from [dbo].RegisterTenders;