alter table [dbo].[ContractSpecifications]
add RouteId bigint;

go

ALTER TABLE [dbo].[ContractSpecifications]  WITH CHECK ADD  CONSTRAINT [FK_Routes_Specifications] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Routes] ([Id])
GO
ALTER TABLE [dbo].[ContractSpecifications] CHECK CONSTRAINT [FK_Routes_Specifications]
GO

alter table [dbo].[Organization]
add CountryId int;

go

ALTER TABLE [dbo].[Organization]  WITH CHECK ADD  CONSTRAINT [FK_Organization_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([?ode])
GO
ALTER TABLE [dbo].[Organization] CHECK CONSTRAINT [FK_Organization_Countries]
GO

alter table [dbo].[Organization]
add IsTruck bit;

go