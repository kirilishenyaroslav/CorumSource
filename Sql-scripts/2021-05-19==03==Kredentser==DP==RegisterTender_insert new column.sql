use [Corum.Prod-2021-01-30]
go
alter table [dbo].RegisterTenders add processValue NVARCHAR(100) null;
go 
alter table [dbo].RegisterTenders add resultsTender NVARCHAR(1000) null;
go
select*from [dbo].RegisterTenders;