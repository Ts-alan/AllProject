CREATE TABLE [dbo].[TaskTypes] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[TaskName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL
)