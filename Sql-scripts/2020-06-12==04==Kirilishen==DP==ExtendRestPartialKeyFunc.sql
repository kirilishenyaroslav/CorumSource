
/****** Object:  StoredProcedure [dbo].[GetRestDataByInnerPartyKey]    Script Date: 12.06.2020 16:04:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetRestDataByInnerPartyKey] 

	@snapShotId int,
	@InnerPartyKey varchar(500)
	
    
AS
BEGIN
     --SET FMTONLY ON;

	 WITH GroupQuantityQuery (Product, Shifr, Figure, Measure, Storage, Shifr_MDM, BarcodeProduct, BacodeConsignment, 
	                          QuantityAfter, QuantityBefore,
	                          PE_Before, PF_Before,  PCP_Before, PCPC_Before,  FCP_Before, FCPC_Before,  BP_Before,
	                          PE_After,  PF_After,   PCP_After,  PCPC_After,   FCP_After,  FCPC_After,   BP_After)
     AS
     (
             SELECT Product,
			        Shifr,
					Figure,
					Measure,
					Storage,
					Shifr_MDM,
					BacodeProduct,
					BacodeConsignment,
			        Sum(QuantityAfter) AS QuantityAfter, 
					Sum(QuantityBefore) AS QuantityBefore,
			        SUM(PE_Before) AS PE_Before, 
					SUM(PF_Before) AS PF_Before,  
					SUM(PCP_Before) AS PCP_Before, 
					SUM(PCPC_Before) AS PCPC_Before,  
					SUM(FCP_Before) AS FCP_Before, 
					SUM(FCPC_Before) AS FCPC_Before,  
					SUM(BP_Before) AS BP_Before,
	                SUM(PE_After) AS PE_After,  
					SUM(PF_After) AS PF_After,   
					SUM(PCP_After) AS PCP_After,  
					SUM(PCPC_After) AS PCPC_After,   
					SUM(FCP_After) AS FCP_After,  
					SUM(FCPC_After) AS FCPC_After,   
					SUM(BP_After) AS BP_After
               FROM [dbo].[RestsSnapshot] DC
              WHERE (DC.id_snapshot=@snapShotId)
                AND (DC.InnerPartyKey =@InnerPartyKey)
		      GROUP BY Product, Shifr, Figure, Measure, Storage, Shifr_MDM, BacodeProduct, BacodeConsignment
     )


	 SELECT GroupQuantityQuery.Product, 
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
	  OUTER APPLY [dbo].GetRestInfoByRestMultiKey(@snapShotId, 
	   GroupQuantityQuery.Product,
	   GroupQuantityQuery.Shifr,
	   GroupQuantityQuery.Figure, 
	   GroupQuantityQuery.Measure, 
	   GroupQuantityQuery.Storage,
	   GroupQuantityQuery.Shifr_MDM,
	   GroupQuantityQuery.BarcodeProduct,
	   GroupQuantityQuery.BacodeConsignment) RI  
        
END


/****** Object:  StoredProcedure [dbo].[GetSummaryDataBySnapshot]    Script Date: 04.07.2016 15:07:47 ******/
SET ANSI_NULLS ON