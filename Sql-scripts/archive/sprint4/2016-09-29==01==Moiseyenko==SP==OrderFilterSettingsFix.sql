alter table [dbo].[OrderFilterSettings]
add UseStatusFilter bit not null;

GO

alter table [dbo].[OrderFilterSettings]
add UseCreatorFilter bit not null;

GO

alter table [dbo].[OrderFilterSettings]
add UseExecuterFilter bit not null;

GO

alter table [dbo].[OrderFilterSettings]
add UseTypeFilter bit not null;

GO

alter table [dbo].[OrderFilterSettings]
add UseClientFilter bit not null;

GO

alter table [dbo].[OrderFilterSettings]
add UsePriorityFilter bit not null;

GO

alter table [dbo].[OrderFilterSettings]
add UseDateFilter bit not null;

GO

alter table [dbo].[OrderFilterSettings]
add UseExDateFilter bit not null;

GO