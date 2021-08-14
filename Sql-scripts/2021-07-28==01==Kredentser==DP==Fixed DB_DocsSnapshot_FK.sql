use [Corum.Prod-2021-07-27_remote server]
GO
ALTER TABLE [dbo].[DocsSnapshot]  WITH NOCHECK ADD  CONSTRAINT [FK_DocsSnapshot_LogisticSnapshots] FOREIGN KEY([id_snapshot])
REFERENCES [dbo].[LogisticSnapshots] ([id_spanshot])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocsSnapshot] NOCHECK CONSTRAINT [FK_DocsSnapshot_LogisticSnapshots]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� �������������, ������� ���������� �������������� ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'InnerPartyKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Producer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������ ������������ ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Product'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������� (����) ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Shifr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� � ������� ������/�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Figure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�� ���������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Measure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������� ������ � ��.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������ (����� ��� ������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ ������ (������ ������������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������������� (��������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pRecieverPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������������� (�����������) ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'pRecieverFact'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ ��������������� (������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RecieverGroupPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ����� ������ �� ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'InnerOrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������� ������������ ���������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'OrderedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� ������ ���������� � ���� ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'OrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ������ � ������ �� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'QuantityPrihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������� ������ �� ������ ��� ��������� �����������  �� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PE_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������� ������ �� ������ ��� ������� ���������� �� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PF_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� �������� ������ ������� ������ �� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� �������� ���������� ������� ������  �� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCPC_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� ����������� ������ ������� ������ �� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� ����������� ���������� ������� ������ �� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCPC_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ��������� ������� ������ �� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BP_Prihod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������� ������ �� ������ ��� ��������� ����������� �� ����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PE_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������� ������ �� ������ ��� ������� ���������� �� ����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PF_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� �������� ������ ������� ������ �� ����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� �������� ���������� ������� ������  �� ����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PCPC_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� ����������� ������ ������� ������ �� ����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������� ����������� ���������� ������� ������ �� ����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'FCPC_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ��������� ������� ������ �� ����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BP_Rashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ������ � ������ �� ����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'QuantityRashod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������ ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'Storage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������ ������ �����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'StorageCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������������ ������ ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'StorageCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� � ������ ���������� ���������������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'�enter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BalanceKeeper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������� ����������� � ��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReadyForSaleStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReserveStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ������������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ProduceDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� �������������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'ReconcervationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������� ���� ������������ �������� ������ �� ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'TermOnStorage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� ��������� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ��������� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'PrihodDocDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� ��������� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ��������� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'RashodDocDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ ����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'BalanceCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ������ �  ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DocsSnapshot', @level2type=N'COLUMN',@level2name=N'CurrencyIndexToUAH'
GO
select*from dbo.DocsSnapshot;