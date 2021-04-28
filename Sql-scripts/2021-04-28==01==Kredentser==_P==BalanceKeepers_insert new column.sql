use [uh417455_db2]
go

alter table dbo.BalanceKeepers
add subCompanyId_Test int null
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = 6 where Id = 2
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 3
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = 13 where Id = 5
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = 10 where Id = 6
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = 9 where Id = 8
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = 8 where Id = 9
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = 11 where Id = 10
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 11
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 13
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 15
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 25
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 38
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 41
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 42
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = 7 where Id = 43
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = 5 where Id = 44
GO
update [dbo].[BalanceKeepers] set subCompanyId_Test = NULL where Id = 45
GO

select*from dbo.BalanceKeepers;