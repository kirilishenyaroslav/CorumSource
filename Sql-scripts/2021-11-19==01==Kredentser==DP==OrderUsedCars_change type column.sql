use [uh417455_db2]
go
alter table [dbo].OrderUsedCars alter column CarCapacity float null;
go
select*from [dbo].OrderUsedCars;