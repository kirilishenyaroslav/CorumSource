use [uh417455_db2]
go
alter table [dbo].[OrdersBase] add LastTenderNumber int null;
go
select*from [dbo].[OrdersBase];