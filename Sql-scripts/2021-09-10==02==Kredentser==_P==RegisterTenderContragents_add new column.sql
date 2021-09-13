use [uh417455_db2]
go
alter table [dbo].[RegisterTenderContragents] add itemDescription text null;
go
select*from [dbo].[RegisterTenderContragents];