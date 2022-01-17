use [uh417455_db2]
go
alter table [dbo].OrderUsedCars alter column CarCapacity int null;
select*from [dbo].OrderUsedCars;