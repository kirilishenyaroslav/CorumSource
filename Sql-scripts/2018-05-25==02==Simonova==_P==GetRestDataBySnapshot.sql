USE [uh417455_db]
GO
/****** Object:  StoredProcedure [dbo].[GetRestDataBySnapshot]    Script Date: 25.05.2018 22:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetRestDataBySnapshot] 

	@snapShotId int,
	
	@FilterStorageId varchar(500)=null,
    @FilterCenterId varchar(500)=null,
    @FilterRecieverPlanId varchar(500)=null,
    @FilterRecieverFactId varchar(500)=null,
    @FilterKeeperId varchar(500)=null,
    @FilterProducerId varchar(500)=null,
	@FilterOrderProjectId varchar(500)=null,

    @UseStorageFilter int=0,
    @UseCenterFilter int=0,
    @UseRecieverPlanFilter int=0,
    @UseRecieverFactFilter int=0,
    @UseKeeperFilter int=0,
    @UseProducerFilter int=0,
	@UseOrderProjectFilter int=0

    
AS
BEGIN
     --SET FMTONLY ON;

	 WITH GroupQuantityQuery /*(Product, Shifr, Figure, Measure, Storage, QuantityAfter, QuantityBefore,
	                          PE_Before, PF_Before,  PCP_Before, PCPC_Before,  FCP_Before, FCPC_Before,  BP_Before,
	                          PE_After,  PF_After,   PCP_After,  PCPC_After,   FCP_After,  FCPC_After,   BP_After, idrow0)*/
     AS
     (
             SELECT Product, Shifr, Figure, Measure, Storage, min(idrow) as idrow0,
			        Sum(QuantityAfter) AS QuantityAfter, Sum(QuantityBefore) AS QuantityBefore,
			        SUM(PE_Before)     AS PE_Before,     SUM(PF_Before)      AS PF_Before,  
					SUM(PCP_Before)    AS PCP_Before,    SUM(PCPC_Before)    AS PCPC_Before,  
					SUM(FCP_Before)    AS FCP_Before,    SUM(FCPC_Before)    AS FCPC_Before,  
					SUM(BP_Before)     AS BP_Before,     SUM(PE_After)       AS PE_After,  
					SUM(PF_After)      AS PF_After,      SUM(PCP_After)      AS PCP_After,  
					SUM(PCPC_After)    AS PCPC_After,    SUM(FCP_After)      AS FCP_After,  
					SUM(FCPC_After)    AS FCPC_After,    SUM(BP_After)       AS BP_After
               FROM [dbo].[RestsSnapshot] DC
              WHERE (DC.id_snapshot=@snapShotId)
                AND ((@UseStorageFilter=0)        OR ((@UseStorageFilter=1)       and (DC.Storage=@FilterStorageId)))
                AND ((@UseCenterFilter=0)         OR ((@UseCenterFilter=1)        and (DC.Сenter=@FilterCenterId))) 
                AND ((@UseRecieverPlanFilter=0)   OR ((@UseRecieverPlanFilter=1)  and (DC.pRecieverPlan=@FilterRecieverPlanId)))
                AND ((@UseRecieverFactFilter=0)   OR ((@UseRecieverFactFilter=1)  and (DC.pRecieverFact=@FilterRecieverFactId)))
                AND ((@UseKeeperFilter=0)         OR ((@UseKeeperFilter=1)        and (DC.BalanceKeeper=@FilterKeeperId))) 
                AND ((@UseProducerFilter=0)       OR ((@UseProducerFilter=1)      and (DC.Producer=@FilterProducerId)))
				AND ((@UseOrderProjectFilter=0)   OR ((@UseOrderProjectFilter=1)  and (DC.InnerOrderNum=@FilterOrderProjectId)))
		   GROUP BY Product, Shifr, Figure, Measure, Storage
     )


	 SELECT 
			idrow0,
			GroupQuantityQuery.Product, 
	        GroupQuantityQuery.Shifr, 
			GroupQuantityQuery.Figure, 
			GroupQuantityQuery.Measure, 
			GroupQuantityQuery.Storage, 
			GroupQuantityQuery.QuantityAfter, 
			GroupQuantityQuery.QuantityBefore,
			GroupQuantityQuery.PE_Before,
	        GroupQuantityQuery.PF_Before,
	        GroupQuantityQuery.PCP_Before,
	        GroupQuantityQuery.PCPC_Before,
	        GroupQuantityQuery.FCP_Before,
	        GroupQuantityQuery.FCPC_Before,
	        GroupQuantityQuery.BP_Before,
	        GroupQuantityQuery.PE_After,
	        GroupQuantityQuery.PF_After,
	        GroupQuantityQuery.PCP_After,
	        GroupQuantityQuery.PCPC_After,
	        GroupQuantityQuery.FCP_After,
	        GroupQuantityQuery.FCPC_After,
	        GroupQuantityQuery.BP_After,
			RI.Producer,
	        RI.Weight,
	        RI.pType ,
	        RI.pGroup,
	        RI.pRecieverPlan,
	        RI.pRecieverFact,
	        RI.RecieverGroupPlan,
	        RI.InnerOrderNum,
	        RI.OrderedBy,
	        RI.OrderNum,
	        RI.StorageCity,
	        RI.StorageCountry,
	        RI.Сenter,
	        RI.BalanceKeeper,
	        RI.ReadyForSaleStatus,
	        RI.ReserveStatus,
	        RI.ProduceDate,
	        RI.ReconcervationDate,
	        RI.TermOnStorage,
	        RI.PrihodDocType,
	        RI.PrihodDocNum,
	        RI.PrihodDocDate,
	        RI.BalanceCurrency,
	        RI.CurrencyIndexToUAH,
			RI.InnerPartyKey
            
       FROM GroupQuantityQuery
	  /*OUTER APPLY [dbo].GetRestInfoByRestMultiKey(@snapShotId, 
	   GroupQuantityQuery.Product,
	   GroupQuantityQuery.Shifr,
	   GroupQuantityQuery.Figure, 
	   GroupQuantityQuery.Measure, 
	   GroupQuantityQuery.Storage) RI  */
	  inner join [RestsSnapshot] ri on GroupQuantityQuery.idrow0 = ri.idrow
		
        
END
