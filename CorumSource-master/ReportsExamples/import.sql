delete from [dbo].[RestsSnapshot];

insert into [dbo].[RestsSnapshot]
([id_snapshot],           [InnerPartyKey],     [Producer],           [Product],       [Shifr],
 [Figure],                [Measure],           [Weight],             [pType],         [pGroup],
 [pRecieverPlan],         [pRecieverFact],     [RecieverGroup],      [InnerOrderNum], [OrderedBy],
 [OrderNum],              [QuantityBefore],    [PE_Before],          [PF_Before],     [PCP_Before],
 [PCPC_Before],           [FCP_Before],        [FCPC_Before],        [BP_Before],     [PE_After],
 [PF_After],              [PCP_After],         [PCPC_After],         [FCP_After],     [FCPC_After],
 [BP_After],              [QuantityAfter],     [Storage],            [StorageCity],   [StorageCountry],
 [Сenter],                [BalanceKeeper],     [ReadyForSaleStatus], [ReserveStatus], [ProduceDate],
 [ReconcervationDate],    [TermOnStorage],     [CreateByDoc],        [CreateByDocNum],[CreateByDocDate],
 [BalanceCurrency],       [CurrencyIndexToUAH])
 select 
 1,                       '',                  [producer],           [product],       [shifr],   
 [figure],                'кг.',               [width],              'Тип товаров',   'Группа товаров',
 [p_reciever_plan],       [p_reciever_fact],   'Группа получателей', '',              [Orderer],
 [OrderNum],              [quantity],          [PE],                 [PF],            [PCP], 
 [PCPC],                  [FCP],               [FCPC],               [BP],            [PE],
 [PF],                    [PCP],               [PCPC],               [FCP],           [FCPC],
 [BP],                    [quantity],          [storage],            '',              'Украина',
 [center],                [Balance_keeper],    [liquid_status],      [reserve_status],[produce_date], 
 [reconcervation_date],   0,                   [DocType],            [DocNumber],      [DocDate], 
 'UAH',                   1
 

 from [dbo].[test_import];


	 
	 
	 
	