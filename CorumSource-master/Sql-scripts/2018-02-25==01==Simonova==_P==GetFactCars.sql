USE [uh417455_db]
GO
/****** Object:  UserDefinedFunction [dbo].[GetDriverContactInfoFilter]    Script Date: 25.02.2018 22:02:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetContractInfoFilter]( 
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
   select DC.ContractInfo, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.ContractInfo LIKE  '%' + @searchTerm + '%') AND
			  (DC.ContractInfo <> '')
			  group by DC.ContractInfo
			  order by DC.ContractInfo Asc     

   RETURN
END
