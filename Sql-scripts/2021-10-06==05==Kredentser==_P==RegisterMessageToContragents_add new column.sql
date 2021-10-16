use [uh417455_db2]
go
alter table [dbo].[RegisterMessageToContragents] add flagCreate bit not null;
go
select*from [dbo].[RegisterMessageToContragents];