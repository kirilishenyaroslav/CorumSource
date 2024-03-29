
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetProductBarcodeFilter]( 
    @searchTerm VARCHAR(50),
	@snapShot bigint
	)
RETURNS @ProductBarcodeTable TABLE
   (
       [ProductBarcode] VARCHAR(50),
	   [idrow] bigint
   )
AS
BEGIN
 INSERT @ProductBarcodeTable
   select DC.BacodesAll, min(DC.idrow)		 
		   FROM [dbo].[RestsSnapshot] DC
              WHERE (DC.BacodesAll LIKE  '%' + @searchTerm + '%') AND
			  (DC.BacodesAll <> '')
			  AND (DC.id_snapshot = @snapShot)
			  group by DC.BacodesAll
			  order by DC.BacodesAll Asc     

   RETURN
END