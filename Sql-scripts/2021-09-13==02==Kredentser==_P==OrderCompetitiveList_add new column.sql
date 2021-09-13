use [uh417455_db2]
go
alter table [dbo].[OrderCompetitiveList] add cargoWeight varchar(50) null;
go
select*from [dbo].[OrderCompetitiveList];