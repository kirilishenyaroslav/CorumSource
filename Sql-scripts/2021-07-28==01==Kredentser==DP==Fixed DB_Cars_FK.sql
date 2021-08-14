use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FKCars_CarOwners] FOREIGN KEY([CarOwnersId])
REFERENCES [dbo].[CarOwners] ([Id])
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FKCars_CarOwners]
GO
ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FKCars_CarsFuelType] FOREIGN KEY([FuelTypeId])
REFERENCES [dbo].[CarsFuelType] ([Id])
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FKCars_CarsFuelType]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ìàðêà' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Model'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ãîñíîìåð' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ÔÈÎ âîäèòåëÿ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'Driver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ñåðèÿ ïðàâ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'DriverLicenseSeria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Âèä òîïëèâà' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'FuelTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ðàñõîä íà 100êì â ðåæèìå ãîðîä' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'ConsumptionCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ðàñõîä íà 100êì â ðåæèìå çà ãîðîä' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'ConsumptionHighway'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Êîëè÷åñòâî ïàññàæèðîâ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'PassNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'íîìåð ïðàâ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'DriverLicenseNumber'
GO
select*from dbo.Cars;