use [uh417455_db2_2021-08-18]
go
alter table [dbo].[RegisterMessageToContragents] add flag bit null;
go
select*from [dbo].[RegisterMessageToContragents];