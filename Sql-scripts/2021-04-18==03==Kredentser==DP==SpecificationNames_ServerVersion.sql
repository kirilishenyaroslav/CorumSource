USE [uh417455_db2]
GO
/****** Object:  Table [dbo].[SpecificationNames]    Script Date: 18.04.2021 23:56:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpecificationNames](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SpecCode] [int] NOT NULL,
	[SpecName] [nvarchar](500) NOT NULL,
	[nmcTestId] [int] NULL,
	[nmcWorkId] [int] NULL,
	[industryId] [int] NULL,
	[industryId_Test] [int] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SpecificationNames] ON 
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (1, 311, N'3.1.1. ������� ����������� ������� �������� - ������ � ��������� ������������� �� ������ �������� (Q =�� 5 �� 20��.) � ������ � ���
', 827, 34011, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (4, 312, N'3.1.2. ������� ����������� ������� ����������� (���� ��� ����� �� 5 �� 9�; Q = �� 15��)
', 829, 34012, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (5, 313, N'3.1.3. ������� ����������� ������� ����������� (���� ��� ����� �� 10 �� 12�; Q = �� 15��)', 830, 34013, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (6, 314, N'3.1.4. ������� ����������� ������� ����������� (������� �� 10 �� 12�; Q = �� 15��)', 831, 34014, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (7, 322, N'3.2.2. ������� ����������� ������� �������� ���������� (Q = 1,6-3,5��)
', 832, 34015, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (8, 321, N'3.2.1. ������� ��������� - ������������� (Q = �� 1,5��)

', 833, 34016, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (9, 411, N'4.1.1 ������� ��������� (Q = 10-15��.)
', 834, 34017, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (10, 412, N'4.1.2. ������� ��������� (Q = 16-35��.)
', 835, 34018, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (11, 421, N'4.2.1. ������� ������������ ��� ����������� ���� (�� ������) (�� 2��)
', 836, 34019, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (12, 422, N'4.2.2. ������� ������������ ��� ����������� ���� (�� ������) (�� 3 �� 5��)', 837, 34020, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (13, 423, N'4.2.3. ������� ������������ ��� ����������� ���� (�� ������) (�� 6 �� 15��)', 838, 34021, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (14, 431, N'�4.3.1. ������� ����� / ����������� ��������� (����������� ����������: L = �� 13,3�; W = �� 2,55; H = 3,1-3,4�, ����� �� 20��)', 839, 34022, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (15, 432, N'�4.3.2. ������� ����� / ����������� ��������� (����������� ��������������: L = �� 15�; W = �� 3,5 �; H = �� 4,5 �, ����� �� 30��)', 840, 34023, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (16, 433, N'�4.3.3. ������� ����� / ����������� ��������� (����������� �������������� �������� ���������: L = ����� 15�; W = ����� 3,5; H = ����� 4,5 �, ����� ���� 30��)', 841, 34024, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (17, 511, N'5.1.1. ������� ��������� (� / � 10 ��. � / � ����. 14-15 �)', 842, 34025, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (18, 512, N'5.1.2.������� ��������� (� / � 20-25 ��. � / � ����. 21-22 �)', 843, 34026, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (19, 513, N'5.1.3.������� ��������� (� / � �� 50 ��. � / � ����. 35 �)', 844, 34027, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (20, 514, N'5.1.4.������� ��������� (� / � 100 ��. � / � ����. 52 �)', 845, 34028, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (21, 521, N'5.2.1. ������� ��������� (������ ������ 17-21�):', 846, 34029, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (22, 522, N'5.2.2. ������� ��������� (������ ������ 22�):', 847, 34030, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (23, 523, N'5.2.3. ������� ��������� (������ ������ 37�):', 848, 34031, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (24, 524, N'5.2.4. ������� ��������� (������ ������ 40�):', 849, 34032, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (25, 531, N'5.3.1. ������� ������������� ���������', 850, 34033, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (26, 532, N'5.3.2. ������� ������������� ������������ � ������', 851, 34034, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (27, 541, N'5.4.1 ������� ���������� ', 852, 34035, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (28, 551, N'5.5.1 ������� �����������', 853, 34036, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (29, 561, N'5.6.1 ������� ��������', 854, 34037, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (30, 571, N'5.7.1. ������� �������� ������������ (�� �����) �� 4 �3', 855, 34038, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (31, 572, N'5.7.2.������� �������� ������������ (�� �����) ����� 4 �3', 856, 34039, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (32, 581, N'5.8.1.������� �����-����������� (� / � 2��; � / � 6�; ���� 4�� / 6�)', 857, 34040, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (33, 582, N'5.8.2.������� �����-����������� (� / � 2��; � / � 8�; ���� 10�� / 7�)', 858, 34041, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (34, 583, N'5.8.3.������� �����-����������� (� / � 5��; � / � 10�; ���� 16�� / 7,8�)', 859, 34042, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (35, 611, N'6.1.1.  ������� ��������-������������ ���������� ���������� ������� �� 1,5�� (������ �Ӫ�)', 860, 34043, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (36, 612, N'6.1.2.  ������� ��������-������������ ���������� ���������� ������� �� 3,5�� (����� �Ӫ�)', 861, 34044, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (37, 111, N'1.1.1. ������� ����.��������� �������� ����������� � ������������ ��䳺� ���������� ����� E, F (��� ����������� ��������� ������ ������������ ���������)', 862, 34045, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (38, 112, N'1.1.2. ������� ����.��������� �������� ����������� � ������������ ��䳺� ���������� ����� D(������ ����.��������� �������� ����������� � ������������ ��������� ��� ����������� ��������� ������ ���������� �����������)', 863, 34046, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (39, 121, N'1.2.1. ������� ����.��������� �������� ����������� ���''������ ��������� (��� ����������) ���������� ����� � (���� ������ �������������� ����������, ��������������� � �������� ��� ������������ � ��������� ��������. ����� ����� ����� �� ������ ��������� 3,8 ������. ��������� ��������������� ����� ������� ���������� "���", "Daewoo Matiz", "Peugeot 107".)', 864, 34047, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (45, 127, N'1.2.7. ������� ����.��������� �������� ����������� ���''������ ��������� (��� ����������) ���������� ����� J �������������', 870, 34053, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (46, 128, N'1.2.8. ������� ����.��������� �������� ����������� ���''������ ��������� (��� ����������) ���������� ����� ϳ���', 871, 34054, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (47, 211, N'2.1.1.  ������� ����.���������� �������������� ������� �� 8 ����', 872, 34055, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (48, 212, N'2.1.2.  ������� ����.���������� �������������� ������� �� 8 �� 16 ����', 873, 34056, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (40, 122, N'1.2.2. ������� ����.��������� �������� ����������� ���''������ ��������� (��� ����������) ���������� ����� � (��� ���������� ���������� � ������ ����� �����, ������������ ����� ������� ����� ����� ������� � �������� ������. �������� ����������� ������ �: ����� � 3,8 - 4,2 �; ������ � 1,5�1,7 �. �������� �������������: "���� ������, ������, ������", "Renault Logan", "Hyundai Solaris".)', 865, 34048, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (41, 123, N'1.2.3. ������� ����.��������� �������� ����������� ���''������ ��������� (��� ����������) ���������� ����� �(��� ���������� ������ ������� �����, ��������� ��� "�����-�������". ����� ���������� "�����-������" � 4,2 � 4,5 �, ������ � 1,6�1,75 �. �������� �������������: "Ford Focus", "Toyota Corolla", "Opel Astra J".)', 866, 34049, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (42, 124, N'1.2.4. ������� ����.��������� �������� ����������� ���''������ ��������� (��� ����������) ���������� ����� D (������� �����. ���� �� �������� ��������� ������������� ������� �����������, ������������� �������� ��� ���� ����������� � �������� ���������� ������ �. � D ����� ������ ���������� ������ 4,6 � 4,85 �. �������� �������������: "VW Passat", "Audi A4", "Mazda 6", "Toyota Camry".)', 867, 34050, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (43, 125, N'1.2.5. ������� ����.��������� �������� ����������� ���''������ ��������� (��� ����������) ���������� ����� E (������ ������� �����. ��������� ����� �-������: ����� � �� 4,85 �� 5,05 ������. �������� �������������: "Audi A6", "Mercedes-Benz E-������", "BMW" 5-�����.)', 868, 34051, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (44, 126, N'1.2.6. ������� ����.��������� �������� ����������� ���''������ ��������� (��� ����������) ���������� ����� F (������������ � ���� ��������������� ������ ����������, � ������ ���������� ��� "����" ��� "����������������� �������". ����� ����� ����� ������ ����� 5 ������. �������� �������������: "BMW" ������� �����, "Jaguar XJ8", "Mercedes-Benz S-������", "Audi A8.)', 869, 34052, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (49, 221, N'2.2.1.  ������� ����.��������� ���������� ������� �� 16 �� 22 ����', 874, 34057, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (50, 222, N'2.2.2.  ������� ����.��������� ���������� ������� �� 22 �� 45 ����', 875, 34058, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (51, 323, N'3.2.3. ������� ����������� ������� �������� ���������� (Q = 3,6-5��)', 876, 34059, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (52, 213, N'2.1.3.  ������� ����.���������� �������������� ������� �� 8 ���� (�������)', 877, 34060, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (53, 214, N'2.1.4.  ������� ����.���������� �������������� ������� �� 8 �� 16 ����(�������)', 878, 34061, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (54, 16111, N'1.1.1 ������� ���������� �� �� �����', 879, 34062, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (55, 16112, N'1.1.2 ������� ���������� �� ��������', 880, 34063, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (56, 16113, N'1.1.3 ������� ���������� ��������� �� �����', NULL, 34064, 54, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (57, 16114, N'1.1.4 ������� ���������� ��������� ��������
', 881, 34065, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (58, 16115, N'1.1.5 ������� ���������� ������ ����� �� �����', 882, 34066, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (59, 16116, N'1.1.6 ������� ���������� ������ ����� ��������', 883, 34067, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (60, 2221, N'2.2.2.1 ������� ����.��������� ���������� ������� �� 22 �� 45 ����(������ �� �.���������) ��
', 884, 34068, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (61, 2222, N'2.2.2.2 ������� ����.��������� ���������� ������� �� 22 �� 45 ����(������ �� �.���������) ��
', 885, 34069, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (62, 5511, N'5.5.1.1 ���������� � �������������� �������', 886, 34070, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (63, 0, N'���������� �������� ������������ �������', 887, 34071, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (64, 1, N'���������� �������� �������������', NULL, 34072, 54, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (65, 715, N'715. ������� ��������.����������� ������� ���������� (�� 1,5��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (66, 735, N'735. ������� ��������.����������� ������� ���������� (1,6-3,5��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (67, 750, N'750. ������� ��������.����������� ������� ���������� (3,6-5��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (68, 710, N'710. ������� ��������.����������� ������� ���������� (6-10��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (69, 720, N'720. ������� ��������.����������� ������� ���������� (20��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (70, 721, N'721.  ������� ��������.����������� ������� ���������� (������� ����)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (71, 722, N'722. ������� ��������.����������� ������� ���������� (�������������)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (72, 315, N'315. ������� ������������ ����������� ������� ���������� (�� 1,5��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (73, 335, N'335. ������� ������������ ����������� ������� ���������� (1,6-3,5��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (74, 350, N'350. ������� ������������ ����������� ������� ���������� (3,6-5��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (75, 310, N'310. ������� ������������ ����������� ������� ���������� (6-10��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (76, 320, N'320. ������� ������������ ����������� ������� ���������� (20��)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (77, 321, N'321.  ������� ������������ ����������� ������� ���������� (������� ����)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (78, 322, N'322. ������� ������������ ����������� ������� ���������� (�������������)', NULL, 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[SpecificationNames] OFF
GO
