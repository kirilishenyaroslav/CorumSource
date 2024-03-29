/****** Object:  UserDefinedFunction [dbo].[GetDocsSummaryByInnerKey]    Script Date: 14.04.2017 21:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create FUNCTION [dbo].[GetSpecificationById]( 
    @SpecificationsId int
)
RETURNS @SpecificationsTable TABLE
   (
       UsedRateName     nvarchar(100),
	   UsedRateId       int,
	   RateValue        nvarchar(100)	   
   )
AS
BEGIN
DECLARE @RateKm decimal(16, 2)
 DECLARE @RateHour decimal(16, 2)
 DECLARE @RateMachineHour decimal(16, 2) 

select @RateKm = 0;
select @RateHour = 0;
select @RateMachineHour = 0;

   SELECT @RateKm = (select cs.RateKm  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)
   
   SELECT @RateHour = (select cs.RateHour  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)

	SELECT @RateMachineHour = (select cs.RateMachineHour  FROM [dbo].[ContractSpecifications] cs
         where cs.Id = @SpecificationsId)

	 if (@RateKm > 0)		    
         INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES ('Стоимость за 1 км', 1, @RateKm)
   
	  if (@RateHour > 0)		    
         INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES ('Стоимость за 1 час', 2, @RateHour)
      
	    if (@RateMachineHour > 0)		    
         INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES ('Стоимость за 1 машино/час', 3, @RateMachineHour)
                   
		if ((@RateKm = 0) and (@RateHour = 0) and (@RateMachineHour = 0))		    
		    INSERT INTO @SpecificationsTable (UsedRateName, UsedRateId, RateValue) VALUES ('Стоимость за 1 км', 1, @RateKm)

   RETURN
END

go

/****** Object:  StoredProcedure [dbo].[GetSpecifications]    Script Date: 14.04.2017 20:39:25 ******/
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
 CROSS APPLY dbo.GetSpecificationById(cs.Id) si

SET ANSI_NULLS ON
