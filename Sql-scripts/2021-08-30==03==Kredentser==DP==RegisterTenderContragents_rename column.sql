USE [uh417455_db2];  
GO  
EXEC sp_rename 'RegisterTenderContragents.costOfCarWithNDS', 'costOfCarWithoutNDSToNull', 'COLUMN';  
GO
SELECT * FROM RegisterTenderContragents;