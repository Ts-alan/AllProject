CREATE TABLE [dbo].[Components] (
	[ComputerID] smallint NOT NULL,
	[ComponentID] smallint NOT NULL,
	[StateID] smallint NOT NULL,
	[Version] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL,
	[SettingsID] smallint NOT NULL
)