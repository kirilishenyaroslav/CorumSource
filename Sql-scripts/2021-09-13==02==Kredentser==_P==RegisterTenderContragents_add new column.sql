use [uh417455_db2]
go
alter table [dbo].[RegisterTenderContragents] add cargoWeight varchar(50) null;
go
select*from [dbo].[RegisterTenderContragents];