IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[ControlDeviceType]')
					   AND OBJECTPROPERTY(id, N'IsTable') = 1)
DROP TABLE [dbo].[ControlDeviceType]
GO

-- Create table for control device types
CREATE TABLE [dbo].[ControlDeviceType] (
	[ID] smallint IDENTITY(1, 1) NOT NULL ,
	[ControlName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	
	CONSTRAINT [PK_ControlDeviceType]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_ControlDeviceType_ControlName]
		UNIQUE NONCLUSTERED ([ControlName])
) 
GO

INSERT INTO ControlDeviceType ([ControlName]) VALUES ('Unknown')
INSERT INTO ControlDeviceType ([ControlName]) VALUES ('Loader')
INSERT INTO ControlDeviceType ([ControlName]) VALUES ('Vsis')
INSERT INTO ControlDeviceType ([ControlName]) VALUES ('RCSUpdateService')
GO

ALTER TABLE ComputerAdditionalInfo
ADD [ControlDeviceTypeID] smallint NOT NULL
DEFAULT(1)
GO

ALTER TABLE ComputerAdditionalInfo ADD
CONSTRAINT [FK_ComputerAdditionalInfo_ControlDeviceType]
		FOREIGN KEY ([ControlDeviceTypeID]) REFERENCES ControlDeviceType([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
GO