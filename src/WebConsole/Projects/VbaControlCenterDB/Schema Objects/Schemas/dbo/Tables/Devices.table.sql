CREATE TABLE [dbo].[Devices] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[SerialNo] nvarchar(1024) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[DeviceTypeID] smallint NOT NULL,
	[Comment] nvarchar(256) COLLATE Cyrillic_General_CI_AS
)