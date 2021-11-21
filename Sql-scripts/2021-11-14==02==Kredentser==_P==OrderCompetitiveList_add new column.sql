use [uh417455_db2]
go
alter table [dbo].[OrderCompetitiveList] add formUuid uniqueidentifier null;
go
select*from [dbo].[OrderCompetitiveList];