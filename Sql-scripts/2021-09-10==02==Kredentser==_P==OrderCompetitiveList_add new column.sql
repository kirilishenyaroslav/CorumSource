use [uh417455_db2]
go
alter table [dbo].[OrderCompetitiveList] add itemDescription text null;
go
select*from [dbo].[OrderCompetitiveList];