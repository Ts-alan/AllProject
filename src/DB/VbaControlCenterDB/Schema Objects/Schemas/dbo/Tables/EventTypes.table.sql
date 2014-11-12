CREATE TABLE [dbo].[EventTypes] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[EventName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Color] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL,
	[Send] bit NOT NULL,
	[NoDelete] bit NOT NULL,
	[Notify] bit NOT NULL
)