CREATE TABLE [dbo].[DeviceClassPolicy] (
	[ID] [int] IDENTITY(1, 1) NOT NULL ,
	[ComputerID] smallint NOT NULL,
	[DeviceClassID] smallint NOT NULL,
	[DeviceClassModeID] smallint NOT NULL
)