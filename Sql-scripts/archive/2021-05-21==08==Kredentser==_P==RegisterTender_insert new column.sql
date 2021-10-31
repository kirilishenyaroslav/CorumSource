use [uh417455_db2]
go
alter table [dbo].RegisterTenders add remainingTime NVARCHAR(100) not null;
go
select*from [dbo].RegisterTenders;