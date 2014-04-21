CREATE TABLE [dbo].[DevicesPolicies] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] smallint NOT NULL,
	[DeviceID] smallint NOT NULL,
	[DevicePolicyStateID] smallint NOT NULL,
	[LatestInsert] smalldatetime NULL
)