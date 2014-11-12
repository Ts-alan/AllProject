CREATE TABLE [dbo].[TaskStates] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[TaskState] nvarchar(32) COLLATE Cyrillic_General_CI_AS NOT NULL
)