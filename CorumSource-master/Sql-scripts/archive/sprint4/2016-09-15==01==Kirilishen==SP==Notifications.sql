

CREATE TABLE dbo.OrderNotificationTypes (
  Id INT IDENTITY
 ,Name VARCHAR(50) NOT NULL
 ,CONSTRAINT PK_OrderNotificationTypes_Id PRIMARY KEY CLUSTERED (Id)
) ON [PRIMARY]
GO



CREATE TABLE dbo.OrderNotifications (
  Id BIGINT IDENTITY
 ,Datetime DATETIME2 NOT NULL DEFAULT (GETDATE())
 ,TypeId INT NOT NULL
 ,CreatedBy NVARCHAR(128) NOT NULL
 ,Body VARCHAR(MAX) NULL
 ,OrderId BIGINT NOT NULL
 ,Reciever NVARCHAR(128) NOT NULL
 ,CONSTRAINT PK_OrderNotifications_Id PRIMARY KEY CLUSTERED (Id)
 ,CONSTRAINT FK_OrderNotificationsOrders FOREIGN KEY (OrderId) REFERENCES dbo.OrdersBase (Id)
 ,CONSTRAINT FK_OrderNotificationsTypes FOREIGN KEY (TypeId) REFERENCES dbo.OrderNotificationTypes (Id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE dbo.OrderNotifications
  ADD CONSTRAINT FK_OrderNotificationsCreator FOREIGN KEY (CreatedBy) REFERENCES dbo.AspNetUsers (Id)
GO


--
ALTER TABLE dbo.OrderNotifications
  ADD CONSTRAINT FK_OrderNotifications FOREIGN KEY (Reciever) REFERENCES dbo.AspNetUsers (Id)
GO
