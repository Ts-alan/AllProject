CREATE TABLE [dbo].[DeviceClass] (
	[ID] [smallint] IDENTITY(1, 1) NOT NULL ,
	[UID] nvarchar(38) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Class] nvarchar(128) COLLATE Cyrillic_General_CI_AS,	
	[ClassName] nvarchar(128) COLLATE Cyrillic_General_CI_AS
)