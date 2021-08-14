use [Corum.Prod-2021-07-27_remote server]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'не пустое для остатков' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNotNullForRest'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'если движение, то должно быть при QuantityRashod<>0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNotZeroForQPrihodNotZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'если движение, то должно быть пустое если=0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForQPrihodZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'если движение, то должно быть ПУСТО при QuantityPrihod<>0 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForQPrihodNotZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Пустое для остатков' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForRest'
GO
select*from dbo.ImportConfig;