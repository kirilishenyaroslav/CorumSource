use [Corum.Prod-2021-07-27_remote server]
go
alter table dbo.Forums_Forum
add CONSTRAINT [PK_Forums_Forum] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
go
select*from dbo.Forums_Forum;