USE [uh417455_db]
GO
/****** Object:  UserDefinedFunction [dbo].[GetDriverContactInfoFilter]    Script Date: 25.02.2018 22:02:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetCarrierInfoFilter]( 
    @searchTerm NVARCHAR(255)
	)
RETURNS @RestInfoTable TABLE
   (
       [ResultFilter] NVARCHAR(255),
	   [Id] int
   )
AS
BEGIN
 INSERT @RestInfoTable
   select DC.CarrierInfo, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.CarrierInfo LIKE  '%' + @searchTerm + '%') AND
			  (DC.CarrierInfo <> '')
			  group by DC.CarrierInfo
			  order by DC.CarrierInfo Asc     

   RETURN
END
