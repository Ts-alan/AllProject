CREATE TABLE [dbo].[InstallationTaskType] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[TaskType] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL
)