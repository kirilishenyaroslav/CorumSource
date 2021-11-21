use [uh417455_db2]
go
alter table [dbo].[RegisterMessageToContragents] add tenderTureNumber int null;
go
select*from [dbo].[RegisterMessageToContragents];