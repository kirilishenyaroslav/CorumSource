USE [uh417455_db]
GO
/****** Object:  StoredProcedure [dbo].[GetTruckReport]    Script Date: 23.07.2018 19:28:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetTruckReport] @userId NVARCHAR(128),
@isAdmin BIT,
@UseOrderTypeFilter BIT,
@FilterOrderTypeId NVARCHAR(128),
@FilterOrderDate DATETIME2,
@UseOrderDateFilter BIT
AS
  IF (@isAdmin = 1)
  BEGIN
    SELECT 
	  ob.OrderType,
	  ott.Shipper,	  
      ott.Consignee,
	  ott.ShipperId,
	  ott.ConsigneeId,
	  ott.ToConsigneeDatetime,
	  ott.FromShipperDatetime,
	  orgShipper.IsSystemOrg as ShipperIsSystemOrg,
	  orgConsignee.IsSystemOrg as ConsigneeIsSystemOrg,
	  countryConsignee.Name as ConsigneeCountry,
	  ott.ConsigneeCity as ConsigneeCity,
	  ott.ConsigneeAdress as ConsigneeAdress,
	  countryShipper.Name as ShipperCountry,
	  ott.ShipperCity as ShipperCity,
	  ott.ShipperAdress as ShipperAdress,
	  countryConsignee.Сode as 	ConsigneeCountryId,
	  countryShipper.Сode as 	ShipperCountryId
	FROM dbo.OrdersBase ob
    LEFT JOIN dbo.OrderTypesBase otb
      ON ob.OrderType = otb.Id
	INNER JOIN dbo.OrderTruckTransport ott
      ON ob.Id = ott.OrderId
	LEFT JOIN dbo.Organization orgShipper
	on orgShipper.Id = ott.ShipperId
	LEFT JOIN dbo.Organization orgConsignee
	on orgConsignee.Id = ott.ConsigneeId
	LEFT JOIN dbo.Countries countryConsignee
	on countryConsignee.Сode = ott.ConsigneeCountryId
	LEFT JOIN dbo.Countries countryShipper
	on countryShipper.Сode = ott.ShipperCountryId
    WHERE (otb.IsTransportType = 1)
    AND (((@UseOrderTypeFilter = 0) AND (ob.OrderType in (4,5,7)))
    OR ((@UseOrderTypeFilter = 1)   
    AND (ob.OrderType in (SELECT *  
                                   FROM dbo.SplitString(@FilterOrderTypeId)))))			

	AND ((@UseOrderDateFilter = 0)
    OR ((@UseOrderDateFilter = 1)
    AND (((select convert(date, ott.ToConsigneeDatetime)) = @FilterOrderDate )
	OR ((select convert(date, ott.FromShipperDatetime)) = @FilterOrderDate ))))

	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1))	
    END
  ELSE
  BEGIN

   SELECT 
	  ob.OrderType,
	  ott.Shipper,	  
      ott.Consignee,
	  ott.ShipperId,
	  ott.ConsigneeId,
	  ott.ToConsigneeDatetime,
	  ott.FromShipperDatetime,
	  orgShipper.IsSystemOrg as IsSystemOrgShipper,
	  orgConsignee.IsSystemOrg as IsSystemOrgConsignee,
	  countryConsignee.Name as ConsigneeCountry,
	  ott.ConsigneeCity as ConsigneeCity,
	  ott.ConsigneeAdress as ConsigneeAdress,
	  countryShipper.Name as ShipperCountry,
	  ott.ShipperCity as ShipperCity,
	  ott.ShipperAdress as ShipperAdress,
	  countryConsignee.Сode as 	ConsigneeCountryId,
	  countryShipper.Сode as 	ShipperCountryId
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
	LEFT JOIN dbo.Organization orgShipper
	on orgShipper.Id = ott.ShipperId
	LEFT JOIN dbo.Organization orgConsignee
	on orgConsignee.Id = ott.ConsigneeId
	LEFT JOIN dbo.Countries countryConsignee
	on countryConsignee.Сode = ott.ConsigneeCountryId
	LEFT JOIN dbo.Countries countryShipper
	on countryShipper.Сode = ott.ShipperCountryId
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
  
    AND (((@UseOrderTypeFilter = 0) AND (ob.OrderType in (4,5,7)))
    OR ((@UseOrderTypeFilter = 1)   
    AND (ob.OrderType in (SELECT *  
                                   FROM dbo.SplitString(@FilterOrderTypeId)))))			   
	AND ((@UseOrderDateFilter = 0)
    OR ((@UseOrderDateFilter = 1)
    AND (((select convert(date, ott.ToConsigneeDatetime)) = @FilterOrderDate )
	OR ((select convert(date, ott.FromShipperDatetime)) = @FilterOrderDate ))))

	AND (exists(SELECT *  
                FROM OrderPipelineSteps ops
                WHERE ops.OrderTypeId = ob.OrderType
                AND ops.ToStatus = ob.CurrentOrderStatus
                AND ops.FinishOfTheProcess=1))	
          END

SET ANSI_NULLS ON
