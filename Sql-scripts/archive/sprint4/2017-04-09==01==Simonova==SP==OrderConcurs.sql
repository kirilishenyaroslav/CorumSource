ALTER TABLE dbo.OrderCompetitiveList
ADD ExpeditorName varchar(100);
go


ALTER TABLE dbo.OrderCompetitiveList
ADD CarryCapacity decimal(16, 2);
go

sp_RENAME 'dbo.OrderCompetitiveList.PaymentsDeferment', 'DaysDelay' , 'COLUMN'
GO

ALTER TABLE dbo.OrderCompetitiveList
ADD IsSelectedId bit;
go