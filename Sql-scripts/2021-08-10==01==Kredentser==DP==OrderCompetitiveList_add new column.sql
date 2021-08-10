use [uh417455_db2]
go
alter table [dbo].[OrderCompetitiveList] add tenderNumber int null;
go
select*from [dbo].[OrderCompetitiveList];