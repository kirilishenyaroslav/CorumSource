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
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (1, 311, N'3.1.1. Послуги перевезення вантажів довгоміри - Тягачі з бортовими напівпричепами та бортові автомобілі (Q =от 5 до 20тн.) З тентом і без
', 827, 34011, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (4, 312, N'3.1.2. Послуги перевезення вантажів майданчиком (авто без бортів від 5 до 9м; Q = до 15тн)
', 829, 34012, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (5, 313, N'3.1.3. Послуги перевезення вантажів майданчиком (авто без бортів від 10 до 12м; Q = до 15тн)', 830, 34013, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (6, 314, N'3.1.4. Послуги перевезення вантажів майданчиком (трубовіз від 10 до 12м; Q = до 15тн)', 831, 34014, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (7, 322, N'3.2.2. Послуги перевезення вантажів бортовим автомобілем (Q = 1,6-3,5тн)
', 832, 34015, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (8, 321, N'3.2.1. Послуги бортового - малотоннажніка (Q = до 1,5тн)

', 833, 34016, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (9, 411, N'4.1.1 Послуги самоскида (Q = 10-15тн.)
', 834, 34017, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (10, 412, N'4.1.2. Послуги самоскида (Q = 16-35тн.)
', 835, 34018, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (11, 421, N'4.2.1. Послуги автоцистерни для перевезення рідин (за видами) (до 2тн)
', 836, 34019, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (12, 422, N'4.2.2. Послуги автоцистерни для перевезення рідин (за видами) (від 3 до 5тн)', 837, 34020, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (13, 423, N'4.2.3. Послуги автоцистерни для перевезення рідин (за видами) (від 6 до 15тн)', 838, 34021, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (14, 431, N' 4.3.1. Послуги трала / низькорамної платформи (перевезення негабариту: L = до 13,3м; W = до 2,55; H = 3,1-3,4м, масою до 20тн)', 839, 34022, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (15, 432, N' 4.3.2. Послуги трала / низькорамної платформи (перевезення крупногабариту: L = до 15м; W = до 3,5 м; H = до 4,5 м, масою до 30тн)', 840, 34023, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (16, 433, N' 4.3.3. Послуги трала / низькорамної платформи (перевезення крупногабариту особливо складного: L = більше 15м; W = понад 3,5; H = більше 4,5 м, масою більш 30тн)', 841, 34024, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (17, 511, N'5.1.1. Послуги автокрана (г / п 10 тн. В / с макс. 14-15 м)', 842, 34025, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (18, 512, N'5.1.2.Послуги автокрана (г / п 20-25 тн. В / с макс. 21-22 м)', 843, 34026, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (19, 513, N'5.1.3.Послуги автокрана (г / п до 50 тн. В / с макс. 35 м)', 844, 34027, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (20, 514, N'5.1.4.Послуги автокрана (г / п 100 тн. В / с макс. 52 м)', 845, 34028, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (21, 521, N'5.2.1. Послуги автовишки (висота підйому 17-21м):', 846, 34029, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (22, 522, N'5.2.2. Послуги автовишки (висота підйому 22м):', 847, 34030, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (23, 523, N'5.2.3. Послуги автовишки (висота підйому 37м):', 848, 34031, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (24, 524, N'5.2.4. Послуги автовишки (висота підйому 40м):', 849, 34032, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (25, 531, N'5.3.1. Послуги навантажувача вилочного', 850, 34033, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (26, 532, N'5.3.2. Послуги навантажувача фронтального з ковшем', 851, 34034, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (27, 541, N'5.4.1 Послуги бульдозера ', 852, 34035, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (28, 551, N'5.5.1 Послуги екскаватора', 853, 34036, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (29, 561, N'5.6.1 Послуги трактора', 854, 34037, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (30, 571, N'5.7.1. Послуги Цистерни Ассінізаторной (АС Бочка) до 4 м3', 855, 34038, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (31, 572, N'5.7.2.Послуги Цистерни Ассінізаторной (АС Бочка) більше 4 м3', 856, 34039, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (32, 581, N'5.8.1.Послуги крана-маніпулятора (г / п 2тн; в / с 6м; борт 4тн / 6м)', 857, 34040, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (33, 582, N'5.8.2.Послуги крана-маніпулятора (г / п 2тн; в / с 8м; борт 10тн / 7м)', 858, 34041, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (34, 583, N'5.8.3.Послуги крана-маніпулятора (г / п 5тн; в / с 10м; борт 16тн / 7,8м)', 859, 34042, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (35, 611, N'6.1.1.  Послуги вантажно-пасажирських перевезень автомобілем місткістю до 1,5тн (Газель ДУЄТ)', 860, 34043, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (36, 612, N'6.1.2.  Послуги вантажно-пасажирських перевезень автомобілем місткістю до 3,5тн (БИЧОК ДУЄТ)', 861, 34044, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (37, 111, N'1.1.1. Послуги пасс.перевозок легковим транспортом з персональним водієм автомобілем класу E, F (для сотрудников категории уровня Генерального Директора)', 862, 34045, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (38, 112, N'1.1.2. Послуги пасс.перевозок легковим транспортом з персональним водієм автомобілем класу D(Услуги пасс.перевозок легковым транспортом с персональным водителем для сатрудников категории уровня Директоров предприятий)', 863, 34046, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (39, 121, N'1.2.1. Послуги пасс.перевозок легковим транспортом роз''їзного характеру (без закріплення) автомобілем класу А (Сюда входят малогабаритные автомобили, предназначенные в основном для эксплуатации в городских условиях. Длина таких машин не должна превышать 3,8 метров. Типичными представителями можно считать автомобили "Ока", "Daewoo Matiz", "Peugeot 107".)', 864, 34047, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (45, 127, N'1.2.7. Послуги пасс.перевозок легковим транспортом роз''їзного характеру (без закріплення) автомобілем класу J Позашляховики', 870, 34053, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (46, 128, N'1.2.8. Послуги пасс.перевозок легковим транспортом роз''їзного характеру (без закріплення) автомобілем класу Пікап', 871, 34054, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (47, 211, N'2.1.1.  Послуги пасс.перевезень мікроавтобусами місткістю до 8 місць', 872, 34055, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (48, 212, N'2.1.2.  Послуги пасс.перевезень мікроавтобусами місткістю від 8 до 16 місць', 873, 34056, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (40, 122, N'1.2.2. Послуги пасс.перевозок легковим транспортом роз''їзного характеру (без закріплення) автомобілем класу В (Это достаточно популярный в Европе класс машин, значительная часть которых имеет кузов хетчбэк и передний привод. Габариты автомобилей класса В: длина — 3,8 - 4,2 м; ширина — 1,5—1,7 м. Типичные представители: "Лада Гранта, Калина, Приора", "Renault Logan", "Hyundai Solaris".)', 865, 34048, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (41, 123, N'1.2.3. Послуги пасс.перевозок легковим транспортом роз''їзного характеру (без закріплення) автомобілем класу С(Так называемый низший средний класс, именуемый еще "гольф-классом". Длина автомобиля "гольф-класса" — 4,2 — 4,5 м, ширина — 1,6—1,75 м. Типичные представители: "Ford Focus", "Toyota Corolla", "Opel Astra J".)', 866, 34049, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (42, 124, N'1.2.4. Послуги пасс.перевозок легковим транспортом роз''їзного характеру (без закріплення) автомобілем класу D (Средний класс. Один из наиболее динамично развивающихся классов автомобилей, представители которого все чаще соперничают с машинами следующего класса Е. В D класс входят автомобили длиной 4,6 — 4,85 м. Типичные представители: "VW Passat", "Audi A4", "Mazda 6", "Toyota Camry".)', 867, 34050, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (43, 125, N'1.2.5. Послуги пасс.перевозок легковим транспортом роз''їзного характеру (без закріплення) автомобілем класу E (Высший средний класс. Параметры машин Е-класса: длина — от 4,85 до 5,05 метров. Типичные представители: "Audi A6", "Mercedes-Benz E-класса", "BMW" 5-серии.)', 868, 34051, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (44, 126, N'1.2.6. Послуги пасс.перевозок легковим транспортом роз''їзного характеру (без закріплення) автомобілем класу F (Сосредоточил в себе комфортабельные мощные автомобили, а потому называется еще "люкс" или "представительским классом". Длина таких машин обычно свыше 5 метров. Типичные представители: "BMW" седьмой серии, "Jaguar XJ8", "Mercedes-Benz S-класса", "Audi A8.)', 869, 34052, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (49, 221, N'2.2.1.  Послуги пасс.перевозок автобусами місткістю від 16 до 22 місць', 874, 34057, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (50, 222, N'2.2.2.  Послуги пасс.перевозок автобусами місткістю від 22 до 45 місць', 875, 34058, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (51, 323, N'3.2.3. Послуги перевезення вантажів бортовим автомобілем (Q = 3,6-5тн)', 876, 34059, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (52, 213, N'2.1.3.  Послуги пасс.перевезень мікроавтобусами місткістю до 8 місць (комфорт)', 877, 34060, 383, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (53, 214, N'2.1.4.  Послуги пасс.перевезень мікроавтобусами місткістю від 8 до 16 місць(комфорт)', 878, 34061, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (54, 16111, N'1.1.1 Послуги перевзеннь ПВ по Україні', 879, 34062, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (55, 16112, N'1.1.2 Послуги перевзеннь ПВ міжнародні', 880, 34063, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (56, 16113, N'1.1.3 Послуги перевзеннь платформа по Україні', NULL, 34064, 54, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (57, 16114, N'1.1.4 Послуги перевзеннь платформа міжнародні
', 881, 34065, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (58, 16115, N'1.1.5 Послуги перевзеннь критий вагон по Україні', 882, 34066, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (59, 16116, N'1.1.6 Послуги перевзеннь критий вагон міжнародні', 883, 34067, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (60, 2221, N'2.2.2.1 Послуги пасс.перевозок автобусами місткістю від 22 до 45 місць(подача из г.Дружковка) РД
', 884, 34068, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (61, 2222, N'2.2.2.2 Послуги пасс.перевозок автобусами місткістю від 22 до 45 місць(подача из г.Дружковка) ВД
', 885, 34069, 384, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (62, 5511, N'5.5.1.1 Экскаватор с гидравлическим молотом', 886, 34070, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (63, 0, N'Курьерские отправки региональные Украина', 887, 34071, 385, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (64, 1, N'Курьерские отправки международные', NULL, 34072, 54, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (65, 715, N'715. Послуги междунар.перевезення вантажів автомобілем (до 1,5тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (66, 735, N'735. Послуги междунар.перевезення вантажів автомобілем (1,6-3,5тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (67, 750, N'750. Послуги междунар.перевезення вантажів автомобілем (3,6-5тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (68, 710, N'710. Послуги междунар.перевезення вантажів автомобілем (6-10тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (69, 720, N'720. Послуги междунар.перевезення вантажів автомобілем (20тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (70, 721, N'721.  Послуги междунар.перевезення вантажів автомобілем (сборный груз)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (71, 722, N'722. Послуги междунар.перевезення вантажів автомобілем (спецтранспорт)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (72, 315, N'315. Послуги регіонального перевезення вантажів автомобілем (до 1,5тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (73, 335, N'335. Послуги регіонального перевезення вантажів автомобілем (1,6-3,5тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (74, 350, N'350. Послуги регіонального перевезення вантажів автомобілем (3,6-5тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (75, 310, N'310. Послуги регіонального перевезення вантажів автомобілем (6-10тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (76, 320, N'320. Послуги регіонального перевезення вантажів автомобілем (20тн)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (77, 321, N'321.  Послуги регіонального перевезення вантажів автомобілем (сборный груз)', NULL, 0, 0, 0)
GO
INSERT [dbo].[SpecificationNames] ([Id], [SpecCode], [SpecName], [nmcTestId], [nmcWorkId], [industryId], [industryId_Test]) VALUES (78, 322, N'322. Послуги регіонального перевезення вантажів автомобілем (спецтранспорт)', NULL, 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[SpecificationNames] OFF
GO
