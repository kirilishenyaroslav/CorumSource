SET IDENTITY_INSERT [dbo].[CarCarryCapacity] ON 
GO

INSERT [dbo].[CarCarryCapacity] ([Id], [CarryCapacity]) VALUES (1, N'1.5')
GO

INSERT [dbo].[CarCarryCapacity] ([Id], [CarryCapacity]) VALUES (2, N'3')
GO

INSERT [dbo].[CarCarryCapacity] ([Id], [CarryCapacity]) VALUES (3, N'5')
GO

SET IDENTITY_INSERT [dbo].[CarCarryCapacity] OFF
GO

SET IDENTITY_INSERT [dbo].[RouteIntervalType] ON 
GO

INSERT [dbo].[RouteIntervalType] ([Id], [NameIntervalType]) VALUES (1, N'0-100')
GO

INSERT [dbo].[RouteIntervalType] ([Id], [NameIntervalType]) VALUES (2, N'101-200')
GO


SET IDENTITY_INSERT [dbo].[RouteIntervalType] OFF
GO

