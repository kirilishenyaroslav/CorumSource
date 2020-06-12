alter table [dbo].[OrderUsedCars]
ALTER COLUMN [ContractId] int NULL;

GO

alter table [dbo].[OrderUsedCars]
Add [ContractInfo] varchar(100) NULL;

GO

alter table [dbo].[OrderUsedCars]
Add [ExpeditorName] varchar(255) NULL;

GO

alter table [dbo].[OrderUsedCars]
Add [Summ] decimal(16,2) NULL;

GO

alter table [dbo].[OrderUsedCars]
Add [DriverCardInfo] varchar(100) NULL;

GO

alter table [dbo].[OrderUsedCars]
Add [Comments] varchar(255) NULL;

GO