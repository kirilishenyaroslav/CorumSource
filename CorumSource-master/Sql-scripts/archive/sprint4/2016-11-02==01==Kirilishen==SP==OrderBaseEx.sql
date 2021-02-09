ALTER TABLE OrdersBase
  ADD DistanceDescription VARCHAR(MAX) null;

GO

ALTER TABLE OrdersBase
  ADD TotalPrice DECIMAL(16,2) DEFAULT(0);
GO

ALTER TABLE OrdersBase
  ADD TotalDistanceLength DECIMAL(16,2) DEFAULT(0);

GO