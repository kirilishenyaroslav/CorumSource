--
-- Скрипт сгенерирован Devart dbForge Studio for SQL Server, Версия 5.2.177.0
-- Домашняя страница продукта: http://www.devart.com/ru/dbforge/sql/studio
-- Дата скрипта: 9/12/16 20:40:46
-- Версия сервера: 11.00.3128
-- Версия клиента: 
-- Пожалуйста, сохраните резервную копию вашей базы перед запуском этого скрипта
--

USE uh218479_db5
GO

IF DB_NAME() <> N'uh218479_db5' SET NOEXEC ON
GO

--
-- Начать транзакцию
--
BEGIN TRANSACTION
GO

--
-- Удалить внешний ключ "FK_OrderTypesBase_AspNetRoles" для объекта типа таблица "dbo.OrderTypesBase"
--
ALTER TABLE dbo.OrderTypesBase
  DROP CONSTRAINT FK_OrderTypesBase_AspNetRoles
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Удалить внешний ключ "FK_OrderTypesBase_AspNetRoles1" для объекта типа таблица "dbo.OrderTypesBase"
--
ALTER TABLE dbo.OrderTypesBase
  DROP CONSTRAINT FK_OrderTypesBase_AspNetRoles1
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Переименовать объект типа таблица из "dbo.OrderTypesBase" в "dbo.tmp_devart_OrderTypesBase"
--
DECLARE @res int
EXEC @res = sp_rename N'dbo.OrderTypesBase', N'tmp_devart_OrderTypesBase', 'OBJECT'
IF @res <> 0
  RAISERROR ('Error while Переименовать объект типа таблица из "dbo.OrderTypesBase" в "dbo.tmp_devart_OrderTypesBase"', 11, 1 );
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Переименовать объект типа индекс из "PK_OrderTypesBase" в "tmp_devart_PK_OrderTypesBase" для объекта типа таблица "dbo.OrderTypesBase"
--
DECLARE @res int
EXEC @res = sp_rename N'dbo.PK_OrderTypesBase', N'tmp_devart_PK_OrderTypesBase', 'OBJECT'
IF @res <> 0
  RAISERROR ('Error while Переименовать объект типа индекс из "PK_OrderTypesBase" в "tmp_devart_PK_OrderTypesBase" для объекта типа таблица "dbo.OrderTypesBase"', 11, 1 );
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу "dbo.OrderTypesBase"
--
CREATE TABLE dbo.OrderTypesBase (
  Id int NOT NULL,
  TypeName varchar(100) NOT NULL,
  UserRoleIdForClientData nvarchar(128) NULL,
  UserRoleIdForExecuterData nvarchar(128) NULL,
  IsActive bit NOT NULL DEFAULT (0)
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Восстановить данные в таблице "dbo.OrderTypesBase"
--
INSERT dbo.OrderTypesBase(Id, TypeName, UserRoleIdForClientData, UserRoleIdForExecuterData, IsActive)
  SELECT Id, TypeName, UserRoleIdForClientData, UserRoleIdForExecuterData, IsActive FROM dbo.tmp_devart_OrderTypesBase WITH (NOLOCK)

--if @@ERROR = 0
--  DROP TABLE dbo.tmp_devart_OrderTypesBase
--GO
--IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать первичный ключ "PK_OrderTypesBase" для объекта типа таблица "dbo.OrderTypesBase"
--
ALTER TABLE dbo.OrderTypesBase
  ADD CONSTRAINT PK_OrderTypesBase PRIMARY KEY CLUSTERED (Id)
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ "FK_OrderTypesBase_AspNetRoles" для объекта типа таблица "dbo.OrderTypesBase"
--
ALTER TABLE dbo.OrderTypesBase
  ADD CONSTRAINT FK_OrderTypesBase_AspNetRoles FOREIGN KEY (UserRoleIdForClientData) REFERENCES dbo.AspNetRoles (Id)
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ "FK_OrderTypesBase_AspNetRoles1" для объекта типа таблица "dbo.OrderTypesBase"
--
ALTER TABLE dbo.OrderTypesBase
  ADD CONSTRAINT FK_OrderTypesBase_AspNetRoles1 FOREIGN KEY (UserRoleIdForExecuterData) REFERENCES dbo.AspNetRoles (Id)
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Фиксировать транзакцию
--
IF @@TRANCOUNT>0 COMMIT TRANSACTION
GO

--
-- Установить NOEXEC в состояние off
--
SET NOEXEC OFF
GO