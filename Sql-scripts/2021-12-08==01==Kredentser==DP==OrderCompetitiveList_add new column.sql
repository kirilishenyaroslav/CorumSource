use [uh417455_db2]
go
alter table [dbo].[OrderCompetitiveList] add edrpou_aps bigint null;
go
select*from [dbo].[OrderCompetitiveList];