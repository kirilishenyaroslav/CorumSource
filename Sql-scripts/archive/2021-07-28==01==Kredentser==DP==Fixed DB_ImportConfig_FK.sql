use [Corum.Prod-2021-07-27_remote server]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�� ������ ��� ��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNotNullForRest'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ��������, �� ������ ���� ��� QuantityRashod<>0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNotZeroForQPrihodNotZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ��������, �� ������ ���� ������ ����=0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForQPrihodZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ��������, �� ������ ���� ����� ��� QuantityPrihod<>0 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForQPrihodNotZeroForDocs'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ ��� ��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportConfig', @level2type=N'COLUMN',@level2name=N'isNullForRest'
GO
select*from dbo.ImportConfig;