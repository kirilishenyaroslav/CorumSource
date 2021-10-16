use [uh417455_db2_2021-08-18]
go
alter table [dbo].[RegisterFormFromContragents] add modelTC text null;
go
alter table [dbo].[RegisterFormFromContragents] add typeTC text null;
go
alter table [dbo].[RegisterFormFromContragents] add fullMassTC int null;
go
alter table [dbo].[RegisterFormFromContragents] add dimensionsTR text null;
go
alter table [dbo].[RegisterFormFromContragents] add fullMassTR int null;
go
alter table [dbo].[RegisterFormFromContragents] add weightWithoutLoad int null;
go
select*from [dbo].[RegisterFormFromContragents];