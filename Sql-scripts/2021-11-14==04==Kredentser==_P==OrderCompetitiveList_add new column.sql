use [uh417455_db2]
go
alter table [dbo].[OrderCompetitiveList] add tenderTureNumber int null;
go
select*from [dbo].[OrderCompetitiveList];