SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetOrderProjects] @OrderId bigint,
  @searchTerm VARCHAR(50)
AS
BEGIN 
    SELECT  p.Id, p.Code   
    FROM dbo.OrderBaseProjects ob
    INNER JOIN dbo.Projects p
            ON ob.ProjectId = p.Id   
    WHERE ob.OrderId = @OrderId
	    AND (p.Code LIKE  '%' + @searchTerm + '%') AND
			  (p.Code <> '')
	order by p.Code Asc     
END
