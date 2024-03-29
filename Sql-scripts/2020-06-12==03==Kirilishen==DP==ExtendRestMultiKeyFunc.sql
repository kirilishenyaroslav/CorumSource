USE [CorumSource.DevNew]
GO
/****** Object:  UserDefinedFunction [dbo].[GetRestInfoByRestMultiKey]    Script Date: 12.06.2020 16:11:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetRestInfoByRestMultiKey]( 
    @snapShotId int,
    @Product varchar(500),
	@Shifr varchar(50),
	@Figure varchar(50), 
	@Measure varchar(10), 
	@Storage varchar(150),
	@Shifr_MDM varchar(50),
	@BacodeProduct varchar(50),
	@BacodeConsignment varchar(50)
)
RETURNS @RestInfoTable TABLE
   (
       [InnerPartyKey] [varchar](500) NULL,
       [Producer] [varchar](500) NULL,
	   [Weight] [decimal](16, 2) NULL,
	   [pType] [varchar](100) NULL,
	   [pGroup] [varchar](500) NULL,
	   [pRecieverPlan] [varchar](500) NULL,
	   [pRecieverFact] [varchar](500) NULL,
	   [RecieverGroupPlan] [varchar](100) NULL,
	   [InnerOrderNum] [varchar](50) NULL,
	   [OrderedBy] [varchar](500) NULL,
	   [OrderNum] [varchar](50) NULL,
	   [StorageCity] [varchar](50) NULL,
	   [StorageCountry] [varchar](10) NULL,
	   [Сenter] [varchar](100) NULL,
	   [BalanceKeeper] [varchar](100) NULL,
	   [ReadyForSaleStatus] [varchar](50) NULL,
	   [ReserveStatus] [varchar](50) NULL,
	   [ProduceDate] [varchar](50) NULL,
	   [ReconcervationDate] [varchar](50) NULL,
	   [TermOnStorage] [int] NULL,
	   [PrihodDocType] [varchar](100) NULL,
	   [PrihodDocNum] [varchar](50) NULL,
	   [PrihodDocDate] [varchar](50) NULL,
	   [BalanceCurrency] [varchar](50) NULL,
	   [CurrencyIndexToUAH] [decimal](18, 2) NULL
   )
AS
BEGIN

   

   INSERT @RestInfoTable
        SELECT TOP 1 [InnerPartyKey],
	                 [Producer],
	                 [Weight],
	                 [pType],
	                 [pGroup],
	                 [pRecieverPlan],
	                 [pRecieverFact],
	                 [RecieverGroupPlan],
	                 [InnerOrderNum],
	                 [OrderedBy],
	                 [OrderNum],
	                 [StorageCity],
	                 [StorageCountry],
	                 [Сenter],
	                 [BalanceKeeper],
	                 [ReadyForSaleStatus],
	                 [ReserveStatus],
	                 [ProduceDate],
	                 [ReconcervationDate],
	                 [TermOnStorage],
	                 [PrihodDocType],
	                 [PrihodDocNum],
	                 [PrihodDocDate],
	                 [BalanceCurrency],
	                 [CurrencyIndexToUAH]
			    FROM [dbo].[RestsSnapshot] DC 
		       where @Product           = DC.Product 
		         and @Shifr             = DC.Shifr
		         and @Figure            = DC.Figure
		         and @Measure           = DC.Measure
		         and @Storage           = DC.Storage
				 and @Shifr_MDM         = DC.Shifr_MDM
				 and @BacodeProduct     = DC.BacodeProduct
				 and @BacodeConsignment = DC.BacodeConsignment
				 and DC.id_snapshot=@snapShotId
   
   
      
           
   RETURN
END