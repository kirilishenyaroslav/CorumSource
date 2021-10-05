use [uh417455_db2]
go
alter table [dbo].[RegisterMessageToContragents] add flag bit null;
go
select*from [dbo].[RegisterMessageToContragents];