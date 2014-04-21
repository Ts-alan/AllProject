CREATE TABLE [dbo].[PolicyTypes] (
	[ID] [smallint] IDENTITY(1, 1) NOT NULL ,
	[TypeName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Params] ntext COLLATE Cyrillic_General_CI_AS NULL,
	[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS NULL
)