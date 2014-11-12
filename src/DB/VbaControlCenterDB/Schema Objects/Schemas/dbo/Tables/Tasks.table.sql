CREATE TABLE [dbo].[Tasks] (
	[ID] bigint IDENTITY(1, 1) NOT NULL,
	[TaskID] smallint NOT NULL,
	[ComputerID] smallint NOT NULL,
	[StateID] smallint NOT NULL,
	[DateIssued] datetime NOT NULL,
	[DateComplete] datetime NULL,
	[DateUpdated] datetime NOT NULL,
	[TaskParams] ntext COLLATE Cyrillic_General_CI_AS NULL,
	[TaskUser] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[TaskDescription] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL
)