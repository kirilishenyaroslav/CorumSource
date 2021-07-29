use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[DocsSnapshot]  WITH NOCHECK ADD  CONSTRAINT [FK_DocsSnapshot_LogisticSnapshots] FOREIGN KEY([id_snapshot])
REFERENCES [dbo].[LogisticSnapshots] ([id_spanshot])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocsSnapshot] NOCHECK CONSTRAINT [FK_DocsSnapshot_LogisticSnapshots]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Внутренний идентификатор, который однозначно идентифицирует партию' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'InnerPartyKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Производитель' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Producer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Наименование номенклатуры товара' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Product'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Артикул (шифр) товара' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Shifr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Информация о чертеже товара/изделия' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Figure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ед измерения' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Measure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Вес единицы товара в кг.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип товара (товар или услуга)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Группа товара (группа номенклатуры)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Грузополучатель (плановый)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pRecieverPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Грузополучатель (фактический) ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pRecieverFact'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Группа грузополучателя (клиент)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RecieverGroupPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Внутренний номер заказа ПП ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'InnerOrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Заказчик изготовления продукции' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'OrderedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер заказа покупателя в базе ПП' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'OrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество товара в партии на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'QuantityPrihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для конечного потребителя  на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PE_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для первого покупателя на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PF_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая полная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая переменная единицы товара  на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCPC_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая полная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая переменная единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCPC_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансовая стоимость единицы товара на начало периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для конечного потребителя на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PE_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Стоимость товара из партии для первого покупателя на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PF_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая полная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость плановая переменная единицы товара  на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCPC_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая полная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Себестоимость фактическая переменная единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCPC_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансовая стоимость единицы товара на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество товара в партии на конец периода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'QuantityRashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Наименование склада' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Storage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расположение склада город' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'StorageCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расположение склада страна' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'StorageCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Информация о центре финансовой ответственности' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Сenter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Балансодержатель' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BalanceKeeper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Признак ограничения к отгрузке' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReadyForSaleStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Статус резерва' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReserveStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата производства' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ProduceDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата реконсервации' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReconcervationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Количество дней фактического хранения партии на складе' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'TermOnStorage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Документ прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Документ прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата документа прихода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Валюта балансовая' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BalanceCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Курс валюты к  грн' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'CurrencyIndexToUAH'
GO
select*from dbo.DocsSnapshot;