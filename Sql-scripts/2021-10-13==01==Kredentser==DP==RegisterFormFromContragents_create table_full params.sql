USE [uh417455_db2_2021-08-18]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegisterFormFromContragents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegisterMessageToContragentId] [int] NOT NULL,
	[carBrand] [nvarchar](350) NULL,
	[modelTC] [text] NULL,
	[typeTC] [text] NULL,
	[stateNumberCar] [nvarchar](100) NULL,
	[dimensionsTC] [nvarchar](350) NULL,
	[dimensionsTC_long] [nvarchar](350) NULL,
	[dimensionsTC_width] [nvarchar](350) NULL,
	[dimensionsTC_height] [nvarchar](350) NULL,
	[fullMassTC] [int] NULL,
	[weightWithoutLoadTC] [int] NULL,
	[trailerNumber] [nvarchar](100) NULL,
	[TRBrand] [nvarchar](350) NULL,
	[modelTR] [text] NULL,
	[typeTR] [text] NULL,
	[dimensionsTR] [text] NULL,
	[dimensionsTR_long] [nvarchar](350) NULL,
	[dimensionsTR_width] [nvarchar](350) NULL,
	[dimensionsTR_height] [nvarchar](350) NULL,
	[fullMassTR] [int] NULL,
	[weightWithoutLoadTR] [int] NULL,
	[loadCapacity] [float] NULL,
	[distance] [float] NULL,
	[fullNameOfDriver] [nvarchar](350) NULL,
	[phoneNumber] [nvarchar](350) NULL,
	[drivingLicenseNumber] [nvarchar](350) NULL,
	[contragentName] [nvarchar](350) NULL,
	[note] [text] NULL,
	[stateBorderCrossingPoint] [text] NULL,
	[seriesPassportNumber] [nvarchar](350) NULL,
	[scannedCopyOfSignedOrder] [bit] NULL,
	[scannedCopyOfRegistrationCertificate] [bit] NULL,
	[scanCopyOfPassport] [bit] NULL,
	[scannedCopyOfAdmissionToTransportation] [bit] NULL,
	[scannedCopyOfCivilPassport] [bit] NULL,
	[IsUpdate] [bit] NULL,
	[dateCreate] [datetime2](7) NOT NULL,
	[dateUpdate] [datetime2](7) NOT NULL,
	[formUuid] [uniqueidentifier] NOT NULL,
	[ipAddressContragent] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[RegisterFormFromContragents]  WITH CHECK ADD  CONSTRAINT [FK_RegisterFormFromContragents_RegisterMessageToContragents] FOREIGN KEY([RegisterMessageToContragentId])
REFERENCES [dbo].[RegisterMessageToContragents] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RegisterFormFromContragents] CHECK CONSTRAINT [FK_RegisterFormFromContragents_RegisterMessageToContragents]
GO
select*from [dbo].[RegisterFormFromContragents];
