use [uh417455_db2]
go
alter table [dbo].RegisterTenders add processValue NVARCHAR(100) null;
go 
alter table [dbo].RegisterTenders add resultsTender NVARCHAR(1000) null;
go
select*from [dbo].RegisterTenders;