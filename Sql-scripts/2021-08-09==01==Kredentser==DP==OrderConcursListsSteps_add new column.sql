use [uh417455_db2]
go
alter table [dbo].[OrderConcursListsSteps] add tenderNumber bigint null;
go
select*from [dbo].[OrderConcursListsSteps];