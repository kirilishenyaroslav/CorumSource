use [Corum.Prod-2021-01-30]
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