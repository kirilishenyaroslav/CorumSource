
alter table [dbo].OrderUsedCars
add PlanDistance decimal(16,2);

GO
alter table [dbo].OrderUsedCars
add PlanTimeWorkDay int;

Go
alter table [dbo].OrderUsedCars
add PlanTimeHoliday int;

GO
alter table [dbo].OrderUsedCars
add BaseRate decimal(16,2);

GO
alter table [dbo].OrderUsedCars
add BaseRateWorkDay decimal(16,2);

GO

alter table [dbo].OrderUsedCars
add BaseRateHoliday decimal(16,2);

GO
alter table [dbo].OrderUsedCars
add DelayDays int;
GO

CREATE TABLE [dbo].Settings(
  [NDS] decimal(16,2)
)
