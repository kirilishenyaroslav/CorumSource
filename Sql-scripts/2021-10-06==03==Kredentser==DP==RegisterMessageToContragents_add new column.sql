use [uh417455_db2_2021-08-18]
go
alter table [dbo].[RegisterMessageToContragents] add industryName nvarchar(350) null;
go
alter table [dbo].[RegisterMessageToContragents] add routeShort text null;
go
alter table [dbo].[RegisterMessageToContragents] add nameCargo text null;
go
alter table [dbo].[RegisterMessageToContragents] add weightCargo float null;
go
alter table [dbo].[RegisterMessageToContragents] add dataDownload datetime2(7) null;
go
alter table [dbo].[RegisterMessageToContragents] add dataUnload datetime2(7) null;
go
alter table [dbo].[RegisterMessageToContragents] add DelayPayment int null;
go
select*from [dbo].[RegisterMessageToContragents];