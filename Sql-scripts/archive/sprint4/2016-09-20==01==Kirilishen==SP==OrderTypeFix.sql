alter table [dbo].[OrderTypesBase]
add DefaultExecuterId nvarchar(128) null;

go

alter table [dbo].[OrderTypesBase]
add ShortName nvarchar(128) null;

go

alter table [dbo].[OrderTypesBase]
add UserForAnnonymousForm nvarchar(128) null;

go


ALTER TABLE [dbo].[OrderTypesBase]  WITH CHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetUsers] FOREIGN KEY([DefaultExecuterId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[OrderTypesBase] CHECK CONSTRAINT [FK_OrderTypesBase_AspNetUsers]
GO

ALTER TABLE [dbo].[OrderTypesBase]  WITH CHECK ADD  CONSTRAINT [FK_OrderTypesBase_AspNetAnnonymousUsers] FOREIGN KEY([UserForAnnonymousForm])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[OrderTypesBase] CHECK CONSTRAINT [FK_OrderTypesBase_AspNetAnnonymousUsers]

