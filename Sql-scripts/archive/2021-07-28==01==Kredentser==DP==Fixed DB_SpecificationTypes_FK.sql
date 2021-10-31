use [Corum.Prod-2021-07-27_remote server]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - фрахт, 1 - фиксированный' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SpecificationTypes', @level2type=N'COLUMN',@level2name=N'SpecificationType'
GO
select*from dbo.SpecificationTypes;