CREATE TABLE [dbo].[InstallationTasks] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[StatusID] smallint NOT NULL,
	[InstallationDate] smalldatetime NOT NULL,
	[ExitCode] smallint NULL,
	[Error] ntext COLLATE Cyrillic_General_CI_AS NULL
)