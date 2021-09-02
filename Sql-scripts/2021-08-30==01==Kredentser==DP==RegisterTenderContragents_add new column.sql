use [uh417455_db2]
go
alter table [dbo].[RegisterTenderContragents] add note text null;
go
select*from [dbo].[RegisterTenderContragents];