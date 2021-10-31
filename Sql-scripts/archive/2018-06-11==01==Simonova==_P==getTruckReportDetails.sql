
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetTruckReportDetails] @userId NVARCHAR(128),
@isAdmin BIT,
@OrgId INT,
@ReportDate DATETIME2
AS
  IF (@isAdmin = 1)
  BEGIN
    SELECT 
	  ob.Id,
	  ob.OrderType,
	  ott.Shipper,	  
      ott.Consignee,
	  ott.ShipperId,
	  ott.ConsigneeId,
	  ott.TruckDescription,
	  u.ExpeditorName,
	  u.ExpeditorId,
	  u.CarModelInfo,
	  u.CarRegNum,
	  u.DriverCardInfo,
      u.FactConsigneeDateTime,
	  u.FactShipperDateTime,
	  u.CarCapacity,
	  ott.ToConsigneeDatetime,
	  ott.FromShipperDatetime
    FROM dbo.OrdersBase ob
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
	INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
	  INNER JOIN dbo.OrderUsedCars u
	  ON u.OrderId = ob.Id  
    WHERE (otb.IsTransportType = 1)
   /* AND (((@UseOrderTypeFilter = 0) AND (ob.OrderType in (4,5,7)))
    //OR ((@UseOrderTypeFilter = 1)   
   // AND (ob.OrderType in (SELECT *  
    //                               FROM dbo.SplitString(@FilterOrderTypeId)))))	*/		
	AND (ob.OrderType in (4,5,7))
    AND (((select convert(date, ott.ToConsigneeDatetime)) = @ReportDate )
	OR ((select convert(date, ott.FromShipperDatetime)) = @ReportDate ))

	AND ((ott.ShipperId = @OrgId) Or (ott.ConsigneeId = @OrgId))

	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1))	
    END
  ELSE
  BEGIN

   SELECT 
	  ob.Id,
	  ob.OrderType,
	  ott.Shipper,	  
      ott.Consignee,
	  ott.ShipperId,
	  ott.ConsigneeId,
	  ott.TruckDescription,
	  u.ExpeditorName,
	  u.ExpeditorId,
	  u.CarModelInfo,
	  u.CarRegNum,
	  u.DriverCardInfo,
      u.FactConsigneeDateTime,
	  u.FactShipperDateTime,
	  u.CarCapacity,
	  ott.ToConsigneeDatetime,
	  ott.FromShipperDatetime
    FROM dbo.OrdersBase ob
    LEFT JOIN dbo.OrderClients oc
      ON ob.ClientId = oc.Id
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
    LEFT JOIN dbo.AspNetRoles ro
      ON oc.AccessRoleId = ro.Id
    LEFT JOIN dbo.AspNetRoles rt
      ON otb.TypeAccessGroupId = rt.Id
	INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
	INNER JOIN dbo.OrderUsedCars u
	  ON u.OrderId = ob.Id  
    WHERE (EXISTS (SELECT
        *
      FROM dbo.AspNetUsers anu
      LEFT JOIN dbo.AspNetUserRoles anur
        ON anu.Id = anur.UserId
      WHERE anur.RoleId = ro.Id
      AND anur.UserId = @userId)
    )
    AND (EXISTS (SELECT
        *
      FROM dbo.AspNetUsers anu
      LEFT JOIN dbo.AspNetUserRoles anur
        ON anu.Id = anur.UserId
      WHERE anur.RoleId = rt.Id
      AND anur.UserId = @userId)
    )
    AND (otb.IsTransportType = 1)
  
  /* AND (((@UseOrderTypeFilter = 0) AND (ob.OrderType in (4,5,7)))
  //  OR ((@UseOrderTypeFilter = 1)   
  //  AND (ob.OrderType in (SELECT *  
                                   FROM dbo.SplitString(@FilterOrderTypeId)))))			   */
	
    AND (ob.OrderType in (4,5,7))
    AND (((select convert(date, ott.ToConsigneeDatetime)) = @ReportDate )
	OR ((select convert(date, ott.FromShipperDatetime)) = @ReportDate ))

	AND ((ott.ShipperId = @OrgId) Or (ott.ConsigneeId = @OrgId))

	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1))	
          END

SET ANSI_NULLS ON
