use [uh417455_db2]
go
alter table [dbo].[RegisterFormFromContragents] add tenderTureNumber int null;
go
select*from [dbo].[RegisterFormFromContragents];