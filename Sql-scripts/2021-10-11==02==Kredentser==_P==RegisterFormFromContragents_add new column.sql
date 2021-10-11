use [uh417455_db2]
go
alter table [dbo].[RegisterFormFromContragents] add transportDimensions nvarchar(350) null;
go
select*from [dbo].[RegisterFormFromContragents];