use [uh417455_db2]
go
alter table [dbo].[RegisterFormFromContragents] add filesTTH_CMR bit null;
go
alter table [dbo].[RegisterFormFromContragents] add filesInvoice bit null;
go
alter table [dbo].[RegisterFormFromContragents] add filesActOfCompletion bit null;
go
select*from [dbo].[RegisterFormFromContragents];