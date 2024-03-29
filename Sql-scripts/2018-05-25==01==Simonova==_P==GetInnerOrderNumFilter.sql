USE [uh417455_db]
GO
/****** Object:  UserDefinedFunction [dbo].[GetInnerOrderNumFilter]    Script Date: 25.05.2018 19:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetInnerOrderNumFilter]( 
    @searchTerm VARCHAR(50),
	@snapShot bigint
	)
RETURNS @InnerOrderNumTable TABLE
   (
       [InnerOrderNum] VARCHAR(50),
	   [idrow] bigint
   )
AS
BEGIN
 INSERT @InnerOrderNumTable
   select DC.InnerOrderNum, min(DC.idrow)		 
		   FROM [dbo].[RestsSnapshot] DC
              WHERE (DC.InnerOrderNum LIKE  '%' + @searchTerm + '%') AND
			  (DC.InnerOrderNum <> '')
			  AND (DC.id_snapshot = @snapShot)
			  group by DC.InnerOrderNum
			  order by DC.InnerOrderNum Asc     

   RETURN
END