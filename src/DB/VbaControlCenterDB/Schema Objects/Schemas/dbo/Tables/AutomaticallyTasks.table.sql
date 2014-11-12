CREATE TABLE [dbo].[AutomaticallyTasks] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[EventID] smallint NOT NULL,
	[TaskID] smallint NOT NULL,	
	[Params] ntext COLLATE Cyrillic_General_CI_AS NULL,
	[IsAllowed] bit NOT NULL
)
