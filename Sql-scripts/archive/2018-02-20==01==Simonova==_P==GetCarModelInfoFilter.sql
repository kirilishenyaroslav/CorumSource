USE [uh417455_db]
GO
/****** Object:  UserDefinedFunction [dbo].[GetRestsOnDate]    Script Date: 22.02.2018 21:01:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetCarModelInfoFilter]( 
    @searchTerm NVARCHAR(255)
	)
RETURNS @RestInfoTable TABLE
   (
       [ResultFilter]     NVARCHAR(255),
	   [Id] int
   )
AS
BEGIN
 INSERT @RestInfoTable
   select DC.CarModelInfo, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.CarModelInfo LIKE  '%' + @searchTerm + '%') AND
			  (DC.CarModelInfo <> '')
			  group by DC.CarModelInfo
			  order by DC.CarModelInfo Asc     

   RETURN
END
