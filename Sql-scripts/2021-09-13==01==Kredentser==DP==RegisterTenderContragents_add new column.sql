use [uh417455_db2_2021-08-18]
go
alter table [dbo].[RegisterTenderContragents] add cargoWeight varchar(50) null;
go
select*from [dbo].[RegisterTenderContragents];