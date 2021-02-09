alter table [dbo].[Projects]
add ManufacturingEnterprise varchar(500)

go

alter table [dbo].[Projects]
add NumOrder int

go

alter table [dbo].[Projects]
add DateOpenOrder datetime2(7)

go

alter table [dbo].[Projects]
add PlanPeriodForMP datetime2(7)

go

alter table [dbo].[Projects]
add PlanPeriodForComponents datetime2(7)

go

alter table [dbo].[Projects]
add PlanPeriodForSGI datetime2(7)

go
	
alter table [dbo].[Projects]
add PlanPeriodForTransportation datetime2(7)

go
	
alter table [dbo].[Projects]
add PlanDeliveryToConsignee datetime2(7)

go
	
alter table [dbo].[Projects]
add DeliveryBasic int

go	

alter table [dbo].[Projects]
add Shipper bigint

go	

alter table [dbo].[Projects]
add Consignee bigint

go		


ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Organization] FOREIGN KEY([Shipper])
REFERENCES [dbo].[Organization] ([Id])
GO

ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Organization]
GO

ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Organization1] FOREIGN KEY([Consignee])
REFERENCES [dbo].[Organization] ([Id])
GO

ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Organization1]
GO
