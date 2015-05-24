use [CCPDb-dev]

ALTER TABLE [dbo].[User] ALTER COLUMN [InternalUserId] bigint NULL
GO

ALTER TABLE [dbo].[User] ALTER COLUMN [RoleId] bigint NULL
GO

ALTER TABLE [dbo].[Contract] ALTER COLUMN [TDGA] int NULL
GO

ALTER TABLE [dbo].[Contract] ALTER COLUMN [StartDate] datetime2(7) NULL
GO

ALTER TABLE [dbo].[Contract] ALTER COLUMN [EndDate] datetime2(7) NULL
GO