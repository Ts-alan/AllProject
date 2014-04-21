CREATE TABLE [dbo].[Processes] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] smallint NOT NULL,
	[ProcessName] nvarchar(260) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[MemorySize] int NULL,
	[LastDate] datetime NOT NULL
)