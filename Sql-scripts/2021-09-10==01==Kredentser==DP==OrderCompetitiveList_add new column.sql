use [uh417455_db2_2021-08-18]
go
alter table [dbo].[OrderCompetitiveList] add itemDescription text null;
go
select*from [dbo].[OrderCompetitiveList];