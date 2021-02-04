ALTER TABLE dbo.ContractSpecifications
ADD IsTruck bit
go

ALTER TABLE dbo.CarCarryCapacity
ADD MaxCapacity decimal(16, 2)
go


ALTER TABLE dbo.RouteIntervalType
ADD MaxInterval int
go


/****** Object:  StoredProcedure [dbo].[GetSpecifications]    Script Date: 20.04.2017 20:39:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetSpecifications] 
AS
select co.CarrierName, cow.CarrierName as CarrierName1, co.IsForwarder, c.BalanceKeeperId, cgs.NameGroupSpec, 
cgs.DaysDelay,
cs.NameSpecification, cs.IsFreight, 
     (case 
	  when (cs.IsFreight = 1)  then 'Фрахт'
	  else 'Фиксированный'
	  end) as FreightName,
cc.CarryCapacity,
r.NameIntervalType,
cs.MovingType,
     (case 
	  when (cs.MovingType = 1)  then 'Фиксированный'
	  else 'Свободный'
	  end) as MovingTypeName,
cs.Id,
cs.DeparturePoint,
cs.ArrivalPoint,
cs.IsTruck,
si.UsedRateName,
si.UsedRateId,
si.RateValue
from [dbo].[Contracts] c
left join dbo.CarOwners co 
 on c.ExpeditorId = co.Id
left join dbo.CarOwners cow 
 on c.CarOwnersId = cow.Id
 inner join dbo.ContractGroupesSpecifications cgs
 on c.Id = cgs.ContractId
 inner join dbo.ContractSpecifications cs
 on cgs.Id = cs.GroupeSpecId
 left join dbo.CarCarryCapacity cc
 on cs.CarryCapacityId = cc.Id
 left join dbo.RouteIntervalType r
 on cs.IntervalTypeId = r.Id
 CROSS APPLY [dbo].GetSpecificationById(cs.Id) si

SET ANSI_NULLS ON
