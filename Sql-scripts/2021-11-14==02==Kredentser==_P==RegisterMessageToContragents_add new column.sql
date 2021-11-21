use [uh417455_db2]
go
alter table [dbo].[RegisterMessageToContragents] add isSelected bit null;
go
select*from [dbo].[RegisterMessageToContragents];