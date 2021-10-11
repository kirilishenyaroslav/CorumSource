use [uh417455_db2_2021-08-18]
go
alter table [dbo].[RegisterFormFromContragents] add transportDimensions nvarchar(350) null;
go
select*from [dbo].[RegisterFormFromContragents];