CREATE FUNCTION [dbo].[GetCarRegNumFilter]( 
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
   select DC.CarRegNum, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.CarRegNum LIKE  '%' + @searchTerm + '%') AND
			  (DC.CarRegNum <> '')
			  group by DC.CarRegNum
			  order by DC.CarRegNum Asc     

   RETURN
END
go

CREATE FUNCTION [dbo].[GetCarCapacityFilter]( 
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
   select DC.CarCapacity, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.CarCapacity LIKE  '%' + @searchTerm + '%') AND
			  (DC.CarCapacity <> '')
			  group by DC.CarCapacity
			  order by DC.CarCapacity Asc     

   RETURN
END
go

CREATE FUNCTION [dbo].[GetCarDriverInfoFilter]( 
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
   select DC.CarDriverInfo, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.CarDriverInfo LIKE  '%' + @searchTerm + '%') AND
			  (DC.CarDriverInfo <> '')
			  group by DC.CarDriverInfo
			  order by DC.CarDriverInfo Asc     

   RETURN
END
go

CREATE FUNCTION [dbo].[GetDriverCardInfoFilter]( 
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
   select DC.DriverCardInfo, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.DriverCardInfo LIKE  '%' + @searchTerm + '%') AND
			  (DC.DriverCardInfo <> '')
			  group by DC.DriverCardInfo
			  order by DC.DriverCardInfo Asc     

   RETURN
END
go

CREATE FUNCTION [dbo].[GetDriverContactInfoFilter]( 
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
   select DC.DriverContactInfo, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.DriverContactInfo LIKE  '%' + @searchTerm + '%') AND
			  (DC.DriverContactInfo <> '')
			  group by DC.DriverContactInfo
			  order by DC.DriverContactInfo Asc     

   RETURN
END
go

CREATE FUNCTION [dbo].[GetCommentsFilter]( 
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
   select DC.Comments, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.Comments LIKE  '%' + @searchTerm + '%') AND
			  (DC.Comments <> '')
			  group by DC.Comments
			  order by DC.Comments Asc     

   RETURN
END
go


CREATE FUNCTION [dbo].[GetCommentsFilter]( 
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
   select DC.Comments, min(DC.Id)		 
		   FROM [dbo].[OrderUsedCars] DC
              WHERE (DC.Comments LIKE  '%' + @searchTerm + '%') AND
			  (DC.Comments <> '')
			  group by DC.Comments
			  order by DC.Comments Asc     

   RETURN
END
go


