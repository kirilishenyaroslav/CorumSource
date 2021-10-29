use [Corum.Prod-2021-01-30]
go
alter table [dbo].RegisterTenders add tenderOwnerPath NVARCHAR(200) not null;
go
select*from [dbo].RegisterTenders;