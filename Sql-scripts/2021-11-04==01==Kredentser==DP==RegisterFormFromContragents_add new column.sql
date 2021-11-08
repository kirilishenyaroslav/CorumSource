use [uh417455_db2]
go
alter table [dbo].[RegisterFormFromContragents] add fullMassTC int null;
go
alter table [dbo].[RegisterFormFromContragents] add massWithoutLoadTC1 int null;
go
alter table [dbo].[RegisterFormFromContragents] add fullMassTC2Trailer int null;
go
alter table [dbo].[RegisterFormFromContragents] add massWithoutLoadTC2Trailer int null;
go
select*from [dbo].[RegisterFormFromContragents];