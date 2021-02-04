alter table [dbo].[Cars]
add DriverLicenseNumber int null;

GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'номер прав' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'DriverLicenseNumber'
GO


EXEC sp_RENAME '[dbo].[Cars].DriverLicense', 'DriverLicenseSeria', 'COLUMN'
GO

EXEC sys.sp_updateextendedproperty  @name=N'MS_Description', @value=N'серия прав' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cars', @level2type=N'COLUMN',@level2name=N'DriverLicenseSeria'
GO


EXEC sp_RENAME '[dbo].[CarOwners].OwnerAddress', 'Address', 'COLUMN'
GO

