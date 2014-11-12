CREATE TABLE [dbo].[GroupTypes] (
	[ID] [int] IDENTITY(1, 1) NOT NULL ,
	[GroupName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[GroupComment] nvarchar(128) COLLATE Cyrillic_General_CI_AS,
	[ParentID] int NULL
)