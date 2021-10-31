use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[PictureBinary]  WITH NOCHECK ADD  CONSTRAINT [FK_PictureBinary_Picture_PictureId] FOREIGN KEY([PictureId])
REFERENCES [dbo].[Picture] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PictureBinary] NOCHECK CONSTRAINT [FK_PictureBinary_Picture_PictureId]
GO
select*from dbo.PictureBinary;