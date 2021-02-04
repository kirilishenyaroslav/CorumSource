ALTER TABLE dbo.OrderCompetitiveList
ALTER COLUMN NDS decimal(16, 2)
go

ALTER TABLE dbo.OrderCompetitiveList
ADD Prepayment2 int
go

ALTER TABLE dbo.OrderCompetitiveList
ADD PrepaymentEffect2 decimal(16, 2)
go

ALTER TABLE dbo.OrderCompetitiveList
ALTER COLUMN PrepaymentEffect decimal(16, 2)
go