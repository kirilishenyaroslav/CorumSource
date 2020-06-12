CREATE TABLE dbo.OrderUnloadingTypes (
  Id INT IDENTITY
 ,UnloadingTypeName VARCHAR(50) NOT NULL
 ,CONSTRAINT PK_table1_Id PRIMARY KEY CLUSTERED (Id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.OrderLoadingTypes (
  Id INT IDENTITY
 ,LoadingTypeName VARCHAR(50) NOT NULL
 ,CONSTRAINT PK_OrderLoadingTypes_Id PRIMARY KEY CLUSTERED (Id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.OrderTruckTypes (
  Id INT IDENTITY
 ,TruckTypeName VARCHAR(50) NOT NULL
 ,CONSTRAINT PK_OrderTruckTypes_Id PRIMARY KEY CLUSTERED (Id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.OrderVehicleTypes (
  Id INT IDENTITY
 ,VehicleTypeName VARCHAR(100) NOT NULL
 ,CONSTRAINT PK_OrderVehicleTypes_Id PRIMARY KEY CLUSTERED (Id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.OrderTruckTransport (
  Id BIGINT IDENTITY
 ,OrderId BIGINT NOT NULL
 ,TruckTypeId INT NOT NULL
 ,TruckDescription VARCHAR(500) NOT NULL
 ,Weight DECIMAL NULL
 ,Volume DECIMAL NULL
 ,BoxingDescription VARCHAR(100) NULL
 ,DimenssionL DECIMAL NULL
 ,DimenssionW DECIMAL NULL
 ,DimenssionH DECIMAL NULL
 ,VehicleTypeId INT NULL
 ,LoadingTypeId INT NULL
 ,Shipper VARCHAR(500) NOT NULL
 ,Consignee VARCHAR(500) NOT NULL
 ,FromShipperDatetime DATETIME2 NULL
 ,ToConsigneeDatetime DATETIME2 NULL
 ,UnloadingTypeId INT NULL
 ,ShipperCountryId INT NULL
 ,ConsigneeCountryId INT NULL
 ,ShipperCity VARCHAR(100) NULL
 ,ConsigneeCity VARCHAR(100) NULL
 ,ShipperAdress VARCHAR(500) NULL
 ,ConsigneeAdress VARCHAR(500) NULL
 ,TripType INT NULL
 ,CONSTRAINT PK_OrderTruckTransport_Id PRIMARY KEY CLUSTERED (Id)
 ,CONSTRAINT FK_OrderTruckTransport FOREIGN KEY (TruckTypeId) REFERENCES dbo.OrderTruckTypes (Id)
 ,CONSTRAINT FK_OrderTruckTransport2 FOREIGN KEY (VehicleTypeId) REFERENCES dbo.OrderVehicleTypes (Id)
 ,CONSTRAINT FK_OrderTruckTransport3 FOREIGN KEY (LoadingTypeId) REFERENCES dbo.OrderLoadingTypes (Id)
 ,CONSTRAINT FK_OrderTruckTransport4 FOREIGN KEY (UnloadingTypeId) REFERENCES dbo.OrderUnloadingTypes (Id)
 ,CONSTRAINT FK_OrderTruckTransport5 FOREIGN KEY (OrderId) REFERENCES dbo.OrdersBase (Id)
) ON [PRIMARY]
GO