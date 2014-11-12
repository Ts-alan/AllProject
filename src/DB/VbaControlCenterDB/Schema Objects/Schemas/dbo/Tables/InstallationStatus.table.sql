CREATE TABLE [dbo].[InstallationStatus] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[Status] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL
)