CREATE TABLE [dbo].[Events] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] smallint NOT NULL,
	[EventID] smallint NOT NULL,
	[EventTime] datetime NOT NULL,
	[ComponentID] smallint NOT NULL,
	[Object] nvarchar(260) COLLATE Cyrillic_General_CI_AS NULL,
	[Comment] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL
)