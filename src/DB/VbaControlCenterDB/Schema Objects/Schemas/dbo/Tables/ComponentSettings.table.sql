CREATE TABLE [dbo].[ComponentSettings] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[Name] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL,
	[Settings] ntext COLLATE Cyrillic_General_CI_AS NOT NULL
)
