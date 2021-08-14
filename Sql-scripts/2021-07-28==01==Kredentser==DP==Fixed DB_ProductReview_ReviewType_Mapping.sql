use [Corum.Prod-2021-07-27_remote server]
go
alter table dbo.ProductReview_ReviewType_Mapping
add CONSTRAINT [PK_ProductReview_ReviewType_Mapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
go
select*from dbo.ProductReview_ReviewType_Mapping;