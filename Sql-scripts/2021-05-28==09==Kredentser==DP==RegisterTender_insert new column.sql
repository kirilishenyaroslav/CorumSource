use [Corum.Prod-2021-01-30]
go
alter table [dbo].RegisterTenders add dateUpdateStatus datetime2(7) null;
go
select*from [dbo].RegisterTenders;