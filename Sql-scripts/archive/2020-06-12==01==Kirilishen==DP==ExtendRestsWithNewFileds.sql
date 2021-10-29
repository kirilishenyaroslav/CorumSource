alter table [dbo].[RestsSnapshot]
  add BacodeProduct nvarchar(50);

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Штрихкод номенклатуры' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BacodeProduct'
GO

alter table [dbo].[RestsSnapshot]
  add BacodeConsignment nvarchar(50);

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Штрихкод партии' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BacodeConsignment'
GO

alter table [dbo].[RestsSnapshot]
  add BacodesAll nvarchar(max);

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Штрихкоды все' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'BacodesAll'
GO

alter table [dbo].[RestsSnapshot]
  add Shifr_MDM nvarchar(50);

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Артикул МДМ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RestsSnapshot', @level2type=N'COLUMN',@level2name=N'Shifr_MDM'
GO