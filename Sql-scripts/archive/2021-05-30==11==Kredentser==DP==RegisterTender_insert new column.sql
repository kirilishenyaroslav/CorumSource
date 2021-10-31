use [Corum.Prod-2021-01-30]
go
alter table [dbo].RegisterTenders add dateCreate datetime2(7) null;
go
select*from [dbo].RegisterTenders;