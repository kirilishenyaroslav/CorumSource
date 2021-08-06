USE [uh417455_db2]
GO
/****** Object:  Table [dbo].[CarOwners]    Script Date: 04.08.2021 16:56:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarOwners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CarrierName] [varchar](100) NULL,
	[Address] [nvarchar](500) NULL,
	[Phone] [nvarchar](20) NULL,
	[ContactPerson] [varchar](100) NULL,
	[parentId] [int] NULL,
	[IsForwarder] [bit] NULL,
	[edrpou_aps] [bigint] NULL,
	[email_aps] [nvarchar](500) NULL,
 CONSTRAINT [PK_CarOwners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CarOwners] ON 
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (1, N'КОРУМ', NULL, NULL, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (2, N'ООО "ТК "ДАВ ТРАНС"', NULL, N'093-198-57-57', N'Дмитрий ', NULL, 1, 34364539, N'office@davtrans.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (3, N'ООО "КОРУМ ГРУП" Киев', N'Лейпцигская, 15А', N'095-231-61-27', N'Шевчук Анна', 7, 0, NULL, NULL)
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (5, N'ООО "КОСМОС"', N'г.Днепр', NULL, N'Кузниченко Александр', NULL, 1, 20197631, N'a.kuznichenko@kosmos.dp.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (6, N'ООО "КОРУМ ГРУП" УРМ Харьков', N'ул.Свет Шахтера, 4/6', N'095-231-67-33', N'Рябоконь Е.Г.', 7, 0, NULL, NULL)
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (7, N'ООО "КОРУМ ГРУП"', N'г. Киев, ул. Лейпцигская, 15 а', NULL, N'Шевчук Анна', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (9, N'ООО "КОРУМ ГРУП" УРМ г. Дружковка', N'ул. Соборная, 7', N'095-231-74-77', N'Юдин В.В.', 7, 0, NULL, NULL)
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (10, N'ООО "Космос"', N'г. Днепр, проспект Кирова, 129Р', N'067-566-11-78', N'Александр Кузниченко', NULL, 1, 20197631, N'a.kuznichenko@kosmos.dp.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (11, N'ООО "ПУВК"', N'г. Днепр, ул. Мечникова, 11, каб 212', N'067-631-76-46', N'Руслан', NULL, 1, 38955754, N'r.karpenko@neolit.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (13, N'ФЛП Леонова А.', N'г. Дружковка, ул. Орджоникидзе, 7', N'0504281662', N'Александр', NULL, 1, 2758516123, N'Leontrans2008@gmail.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (14, N'АТЦ АО "Свет Шахтера" ', N'ул. Свет шахтера, 4/6', N'067-626-54-98', N'Шевелева Ирина', 27, 0, 33206228, N'alexleon3@rambler.ru')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (15, N'ФЛП Гриндей Р.В.', N'г. Славянск, пер. Волочаевкий, 8', N'050-919-86-81', N'Владимир Степанович', NULL, 1, 3172501251, N'alina_2009.09@ukr.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (16, N'ООО "Жефко Украина"', N'г. Киев, ул. Жилянская, 110, 7 этаж', N'-', N'Ген. директор О.А. Яковлева', NULL, 1, 35894081, N'Nataliya.Krasnopolska@gefco.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (17, N'ООО "Телс Украина"', N'г. Киев, ул. Рижская, строение 8-а', N'+ 38 044 454 72 31 ', N'Домащенко Андрей', NULL, 1, 35625192, N'nikolayko@telsgroup.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (18, N'ООО" Глобалинк Лоджистикс групп"', NULL, NULL, NULL, NULL, 1, 34800081, N'kiev@globalinkllc.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (19, N'ООО "Укртиртранс"', NULL, NULL, NULL, NULL, 1, 37471636, N'info@ukrtirtrans.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (21, N'Людовик-14', N'г. Каменец-Подольский, ул. Хмельницкое шоссе,36', N'067-381-12-60', N'-', NULL, 1, 35605595, N'akivakiv@i.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (22, N'ООО "Мико Транс-1"', N'г. Днепр, ул. Данила Галицького, д.12, кв. 125', N'+38-067-196-00-93', N'Дарья Волошина ', NULL, 1, 38113552, N'd.voloshyna@miko-transport.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (23, N'ООО "Конвой транс"', N'г. Киев, пр-т Воздухофлотский, д.54', N'(044)-338-35-07', N'Ястреб Д.Н.', NULL, 1, 39352706, N'convoytrans@ukr.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (25, N'ТОВ «ЄВРО ЛОДЖІСТИК ТРЕЙД»', N'м. Київ, вул. Саксаганского 133-А', N'+380966881444', N'Мигель Мирослав', NULL, 1, 39127875, N'info@eurologic.kiev.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (26, N'ООО "БОНУС ТРАНС"', N'г. Киев', NULL, N'Андрей', NULL, 1, 38619773, N'bonustrans1@gmail.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (27, N'АО "СВЕТ ШАХТЕРА"', N'Харьков', NULL, NULL, NULL, 1, 33206228, N'koroleva@shaht.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (28, N'АТЦ ДАВ ТРАНС', NULL, NULL, NULL, 2, 0, 34364539, N'office@davtrans.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (29, N'АТЦ Леонов', NULL, NULL, NULL, 13, 0, 2758516123, N'alexleon3@rambler.ru')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (30, N'ООО "Метинвест-СМЦ"', N'г. Киев, ул. Лейпцигская, 15А', N'(044) 581-44-37', N'Аскеров Вакил', NULL, 1, 32036829, N'kirill.chikmarev@metinvestholding.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (31, N'ФЛП Резцова И.В.', N'93000, Луганская обл., г. Рубежное, ул. Пушкина, д.19 кв.1                                            ', N'0952240214', N'Игорь', NULL, 1, 2661600881, N'din-trans@ukr.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (32, N'МЕТІНВЕСТ- ШІППІНГ ТОВ', NULL, NULL, NULL, NULL, 1, 31158623, N' 	olga.ivanova@metinvestholding.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (33, N'ФЛП Некравцев С.Н.', N'Харьков, пр. Тракторостроителей, 112А', N'0503009498', N'Некравцев С.Н.', NULL, 1, 2998716259, N'vboyko2018@gmail.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (34, N'ТНТ УКРАЇНА ТОВ', N'Украина', NULL, NULL, NULL, 1, 39500321, N'tatyana.shulga@tnt.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (35, N'ФЛП Доценко', N'Луганская обл, г. Лисичанск, ул. Ленинградская, 54/98', N'0506628088 ', N'Доценко Александр Леонидович ', NULL, 1, 2639000110, N'docenkoal@ukr.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (36, N'ФЛП Вороной Д.Л.', N'Днепр, ж.м Тополь-2, дом 22, корпус 8, кв.33', N'0668620802', N'Вороной Денис Леонидович', NULL, 1, 14360570, N'vadim@nstrans.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (37, N'ФЛП Щабло', N'г. Краматорск, бул. Машиностроителей, д. 31/13', N'094444', N'Щабло А.В.', NULL, 1, 2798713755, N'as0509373355@ukr.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (38, N'ДП «Интегрированные логистические системы»', N'Одесская обл., Белявский р-йн', N'050 490-24-43', N'Андрияшин Алексей ', NULL, 1, 32012483, N'chaal@mpc-ua.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (39, N'ФЛП Леонов А.', N'г. Дружковка, ул. Орджоникидзе, 7', N'---', N'Леонов Александр', NULL, 1, 2758516123, N'alexleon3@rambler.ru')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (40, N'ООО "Астерикс груп Украина"', N'г. Киев, ул. Евгения Сверстюка, 2а', NULL, N'Василевский Д.П.', NULL, 1, 40415402, N'denis.vasilevskiy@asterix-group.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (41, N'ЧТЭП «Автотрансэкспедиция»', N'г. Львов, ул. Городоцкая, д.291', N'---', N'---', NULL, 1, 25544907, N'оlena@avtotranz.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (42, N'Ин-Транс ЛТД', N'г. Киев, ул. Попудренка, 52-А', N'-', N'-', NULL, 1, 39446364, N'den_in-trans@mail.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (43, N'ЧП «БЕРЕГИНЯ»', N'г. Запорожье, ул. Испытателей, 2 В', N'--', N'---', NULL, 1, 37573450, N'paltrans2011@gmeil.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (44, N'ООО "Статус Строй"', N'г. Харьков, ул. Ольгинская,11', N'--', N'--', NULL, 1, 41072024, N'StatusTrans2016@gmail.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (45, N'ФЛП Мехедок', N'--', N'0982597709', N'Галина Мехедок', NULL, 1, 2711815484, N'19743103@ukr.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (46, N'Гермес Транс ПП', N'г. Харьков, пр. Юбилейный, 56', N'---', N'--', NULL, 1, 40700318, N'а0955049402@gmail.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (47, N'ФТЛ Сервис Украина', N'г. Киев, ул. Евгения Сверстюка,11', N'0678294323', N'Эллина Константинова', NULL, 1, 40826406, N'operator12@ltlservice.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (48, N'ЧП Данкир', N'------', N'+380675726971 ', N'Сергей Горбунов', NULL, 1, 36429388, N'dankir@ukr.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (49, N'ООО "ДИАД ЛОГИСТИК"', N'Г. КИЕВ, УЛ. ТБИЛИССКАЯ, 4/10, КАБ.301', N'--', NULL, NULL, 1, 39425648, N'sales@diad.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (50, N'ФЛП Лепетуха ', N'Харьков, ул. Николая Бажана, 10/178', N'--', N'Лепетуха ', NULL, 1, 2832116970, N'vboyko2018@gmail.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (51, N'ФОП Гайтан', N'пгт. Сартана, Донецкая обл.', N'096 791-04-63', N'Гайтан Андрей Дмитриевич ', NULL, 1, 3039704955, N'andreigaitan@mail.ru')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (52, N'ООО «Крафтер ЛТД»', N'г.Тернополь, ул. С. Бандеры 36/41', N'067175-19-91', N'Педорич Лилия ', NULL, 1, 42049258, N'liliia.pedorych@krafterltd.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (53, N'Павлов В.В. ФОП', N'---', N'тел. 0509398204', N'Елена', NULL, 1, NULL, NULL)
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (54, N'ГУД ЛОГИСТИК ООО', N'--', N'380675586007', N'Oksana Danilishyna ', NULL, 1, 39353364, N't.zahoriichuk@goodlogistics.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (55, N'ДЕВИК ЧП', N'---', N'+38(067) 670-59-52', N'Пазинич Тетяна Керівник відділу продажу', NULL, 1, 19322456, N'office@devik.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (56, N'Укравтологистика ООО', N'--', N'050-582-48-75', N'Дьяченко Виктория ', NULL, 1, 39427556, N'dva@ukrautologistic.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (57, N'ДСВ Логистика', N'г. Киев, ул. Семьи Сосниных,7', NULL, NULL, NULL, 1, 38857571, N'oleg.serpionov@ua.dsv.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (58, N'Адонис Украина', N'---', N'067-010-18-28', N'Вера Бурыкина ', NULL, 1, 39214847, N'tasima.dnepr@gmail.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (59, N'Бахмут Бетон Комплект', N'---', N' (050) 567 56 50', N'Иванин Константин 	', NULL, 1, 41596788, NULL)
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (60, N'Шевченко ФЛП', N'--', N'0677303076', N'Шевченко Елена ', NULL, 1, 2943107861, N'shevchenko.avto1@gmail.com')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (61, N'Острищенко Ю.В. ФЛП', N'---', N'80675058499', N'Паржицкая Олеся Валерьевна ', NULL, 1, 2547712851, N'stolica_kiev_@ukr.net')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (62, N'Мико Транс-1 ООО', N'---', N'+38-067-196-00-93', N'Дарья Волошина ', NULL, 1, 38113552, N'd.voloshyna@miko-transport.com.ua')
GO
INSERT [dbo].[CarOwners] ([Id], [CarrierName], [Address], [Phone], [ContactPerson], [parentId], [IsForwarder], [edrpou_aps], [email_aps]) VALUES (63, N'ПРАЙМ ЛОГИСТИК ГРУП', N'м. Київ,вул. Миколи Василенка, буд. 7-А', N'+(38)044-303-91-87', N'Владислав Владиславович Коваль', NULL, NULL, 40417623, N'info@prime-lg.com.ua')
GO
SET IDENTITY_INSERT [dbo].[CarOwners] OFF
GO
