CREATE TABLE [dbo].[UpdateLog] 
( 
	[BuildId]           nvarchar(50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[DeployDatetime]    smalldatetime NOT NULL,
	[StateID]			smallint NOT NULL,
	[Description]		nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL
)