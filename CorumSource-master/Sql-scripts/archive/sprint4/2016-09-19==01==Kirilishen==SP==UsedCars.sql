CREATE TABLE dbo.OrderUsedCars (
  Id bigint IDENTITY,
  OrderId bigint NOT NULL,
  ContractId int NOT NULL,
  CarOwnerInfo varchar(100) NULL,
  CarModelInfo varchar(100) NULL,
  CarRegNum varchar(20) NULL,
  CarDriverInfo varchar(100) NULL,
  DriverContactInfo varchar(100) NULL,
  CarrierInfo varchar(100) NULL,
  CarId int NULL,
  CONSTRAINT PK_OrderUsedCars_Id PRIMARY KEY CLUSTERED (Id),
  CONSTRAINT FK_OrderUsedCarsOrder FOREIGN KEY (OrderId) REFERENCES dbo.OrdersBase (Id) ON DELETE CASCADE,
  CONSTRAINT FK_OrderUsedCarsContract FOREIGN KEY (ContractId) REFERENCES dbo.Contracts (Id),
  CONSTRAINT FK_OrderUsedCars FOREIGN KEY (CarId) REFERENCES dbo.Cars (Id)
)
ON [PRIMARY]
GO