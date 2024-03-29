USE [uh417455_db]
GO
/****** Object:  StoredProcedure [dbo].[GetSummaryDataBySnapshot]    Script Date: 26.05.2018 20:58:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetSummaryDataBySnapshot] 

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
     SET FMTONLY OFF
	 CREATE TABLE #RestInfoQuery
     (
           QuantityPrihod decimal(16,2) null,
           MassPrihod decimal(16,2) null,
           PE_Prihod decimal(16,2) null,
           PF_Prihod decimal(16,2) null,
           PCP_Prihod decimal(16,2) null,
           PCPC_Prihod decimal(16,2) null,
           FCP_Prihod decimal(16,2) null,
           FCPC_Prihod decimal(16,2) null,
           BP_Prihod decimal(16,2) null,
	       QuantityRashod decimal(16,2) null,
           MassRashod decimal(16,2) null,
           PE_Rashod decimal(16,2) null,
           PF_Rashod decimal(16,2) null, 
           PCP_Rashod decimal(16,2) null,
           PCPC_Rashod decimal(16,2) null,
           FCP_Rashod decimal(16,2) null,
           FCPC_Rashod decimal(16,2) null,
           BP_Rashod decimal(16,2) null,
		   InnerPartyKey varchar(500) null
     )

	 insert into #RestInfoQuery
     SELECT SUM(D.QuantityPrihod)     AS QuantityPrihod, 
            SUM(D.Weight*D.QuantityPrihod) AS MassPrihod,
            SUM(D.PE_Prihod)               AS PE_Prihod,
            SUM(D.PF_Prihod)               AS PF_Prihod,
            SUM(D.PCP_Prihod)              AS PCP_Prihod,
            SUM(D.PCPC_Prihod)             AS PCPC_Prihod,
            SUM(D.FCP_Prihod)              AS FCP_Prihod,
            SUM(D.FCPC_Prihod)             AS FCPC_Prihod,
            SUM(D.BP_Prihod)               AS BP_Prihod,
	        SUM(D.QuantityRashod)          AS QuantityRashod, 
            SUM(D.Weight*D.QuantityRashod) AS MassRashod,
            SUM(D.PE_Rashod)               AS PE_Rashod,
            SUM(D.PF_Rashod)               AS PF_Rashod,
            SUM(D.PCP_Rashod)              AS PCP_Rashod,
            SUM(D.PCPC_Rashod)             AS PCPC_Rashod,
            SUM(D.FCP_Rashod)              AS FCP_Rashod,
            SUM(D.FCPC_Rashod)             AS FCPC_Rashod,
            SUM(D.BP_Rashod)               AS BP_Rashod,
		    D.InnerPartyKey	      
          FROM [dbo].[DocsSnapshot] D
         where D.id_snapshot = @snapShotId
         GROUP BY D.InnerPartyKey;
     
	 SELECT R.[idrow],               R.[id_snapshot],       R.[InnerPartyKey],         R.[Producer],            R.[Product],            R.[Shifr],
	        R.[Figure],              R.[Measure],           R.[Weight],                R.[pType],               R.[pGroup],             R.[pRecieverPlan],
	        R.[pRecieverFact],       R.[RecieverGroupPlan], R.[InnerOrderNum],         R.[OrderedBy],           R.[OrderNum],           R.[QuantityBefore],
	        R.[PE_Before],           R.[PF_Before],         R.[PCP_Before],            R.[PCPC_Before],         R.[FCP_Before],         R.[FCPC_Before],
	        R.[BP_Before],           R.[PE_After],          R.[PF_After],              R.[PCP_After],           R.[PCPC_After],         R.[FCP_After],
	        R.[FCPC_After],          R.[BP_After],          R.[QuantityAfter],         R.[Storage],             R.[StorageCity],        R.[StorageCountry],
	        R.[Сenter],              R.[BalanceKeeper],     R.[ReadyForSaleStatus],    R.[ReserveStatus],       R.[ProduceDate],        R.[ReconcervationDate],
	        R.[TermOnStorage],       R.[PrihodDocType],     R.[PrihodDocNum],          R.[PrihodDocDate],       R.[BalanceCurrency],    R.[CurrencyIndexToUAH],	        
	        coalesce(D.QuantityPrihod,0) as QuantityPrihod, 
	        coalesce(D.MassPrihod,0) as MassPrihod,   
	        coalesce(D.PE_Prihod,0) as PE_Prihod, 
	        coalesce(D.PF_Prihod,0) as PF_Prihod, 
	        coalesce(D.PCP_Prihod,0) as PCP_Prihod, 
	        coalesce(D.PCPC_Prihod,0) as PCPC_Prihod, 
            coalesce(D.FCP_Prihod,0) as FCP_Prihod,           
            coalesce(D.FCPC_Prihod,0) as FCPC_Prihod, 
            coalesce(D.BP_Prihod,0) as BP_Prihod,             
            coalesce(D.QuantityRashod,0) as QuantityRashod, 
            coalesce(D.MassRashod,0) as MassRashod, 
            coalesce(D.PE_Rashod,0) as PE_Rashod, 
            coalesce(D.PF_Rashod,0) as PF_Rashod, 
            coalesce(D.PCP_Rashod,0) as PCP_Rashod, 
            coalesce(D.PCPC_Rashod,0) as PCPC_Rashod, 
            coalesce(D.FCP_Rashod,0) as FCP_Rashod, 
            coalesce(D.FCPC_Rashod,0) as FCPC_Rashod, 
            coalesce(D.BP_Rashod,0) as BP_Rashod
            
       FROM [dbo].[RestsSnapshot] R
     left join #RestInfoQuery D on R.InnerPartyKey = D.InnerPartyKey
      WHERE (R.id_snapshot=@snapShotId)
        AND ((@UseStorageFilter=0)     OR ((@UseStorageFilter=1)     and(R.Storage=@FilterStorageId)))
        AND ((@UseCenterFilter=0)      OR ((@UseCenterFilter=1)      and(R.Сenter=@FilterCenterId))) 
        AND ((@UseRecieverPlanFilter=0)OR ((@UseRecieverPlanFilter=1)and(R.pRecieverPlan=@FilterRecieverPlanId)))
        AND ((@UseRecieverFactFilter=0)OR ((@UseRecieverFactFilter=1)and(R.pRecieverFact=@FilterRecieverFactId)))
        AND ((@UseKeeperFilter=0)      OR ((@UseKeeperFilter=1)      and(R.BalanceKeeper=@FilterKeeperId))) 
        AND ((@UseProducerFilter=0)    OR ((@UseProducerFilter=1)    and(R.Producer=@FilterProducerId)))
    	AND ((@UseOrderProjectFilter=0) OR ((@UseOrderProjectFilter=1) and(R.InnerOrderNum=@FilterOrderProjectId)))
    
     
        SET FMTONLY ON
END


/****** Object:  StoredProcedure [dbo].[RestsSnapshotInsert]    Script Date: 04.07.2016 15:09:43 ******/
SET ANSI_NULLS ON
