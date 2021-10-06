use [uh417455_db2_2021-08-18]
go
alter table [dbo].[RegisterMessageToContragents] add flagCreate bit not null;
go
select*from [dbo].[RegisterMessageToContragents];