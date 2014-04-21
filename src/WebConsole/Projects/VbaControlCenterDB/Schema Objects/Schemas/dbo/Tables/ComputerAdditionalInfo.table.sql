CREATE TABLE [dbo].[ComputerAdditionalInfo] (
	[ID] smallint IDENTITY(1, 1) NOT NULL ,
	[ComputerID] smallint NOT NULL,
	[IsControllable] bit NOT NULL,
	[IsRenamed] bit NOT NULL,
	[PreviousComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL,
	[ControlDeviceTypeID] smallint NOT NULL
)
