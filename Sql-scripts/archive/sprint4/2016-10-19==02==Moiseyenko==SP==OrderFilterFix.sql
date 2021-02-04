delete from [dbo].[OrderFilterSettings];
Go

alter table [dbo].[OrderFilterSettings]
add IdCurrentUser nvarchar(128) not null;

GO

ALTER TABLE [dbo].OrderFilterSettings  WITH CHECK ADD  CONSTRAINT [FK_OrderFilterSettings_AspNetUsersCur] FOREIGN KEY([IdCurrentUser])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].OrderFilterSettings CHECK CONSTRAINT [FK_OrderFilterSettings_AspNetUsersCur]
GO