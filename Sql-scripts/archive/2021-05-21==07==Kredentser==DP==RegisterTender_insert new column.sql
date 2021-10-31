use [Corum.Prod-2021-01-30]
go
alter table [dbo].RegisterTenders add remainingTime NVARCHAR(100) not null;
go
select*from [dbo].RegisterTenders;