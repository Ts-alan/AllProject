-- Returns a page from Devices table for group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicesPageForGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicesPageForGroup]
GO

CREATE PROCEDURE [dbo].[GetDevicesPageForGroup]
	@GroupID int
WITH ENCRYPTION
AS

DECLARE @ComputerPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[ID] smallint
		);

WITH TreeGroups AS
(
	SELECT [ID], [ParentID] FROM GroupTypes
	WHERE [ID] = @GroupID

	UNION ALL

	SELECT gt.[ID], gt.[ParentID] FROM GroupTypes AS gt
	INNER JOIN TreeGroups AS tg ON tg.[ID] = gt.[ParentID]
)
INSERT INTO @ComputerPage([ID])
SELECT g.[ComputerID] FROM Groups AS g
INNER JOIN TreeGroups AS tg ON tg.[ID] = g.[GroupID]

DECLARE @ComputerCount int
SET @ComputerCount = (SELECT COUNT([ID]) FROM @ComputerPage)
	
DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[D_ID] smallint,
			[DP_LatestInsert] smalldatetime,
			[CountStates] smallint,
			[Count] int
		)

INSERT INTO @DevicesPage([D_ID], [DP_LatestInsert], [CountStates], [Count])
SELECT
	d.[ID], MAX(dp.[LatestInsert]), COUNT(DISTINCT(dp.[DevicePolicyStateID])), COUNT(dp.[DevicePolicyStateID])
FROM DevicesPolicies AS dp
INNER JOIN Devices AS d ON d.[ID] = dp.[DeviceID]
WHERE dp.[ComputerID] IN (SELECT [ID] FROM @ComputerPage)
GROUP BY d.[ID]

SELECT 
	dp.[ID], c.[ID], c.[ComputerName], d.[ID], StateName=CASE tmp.[CountStates]
	WHEN 1 THEN dps.[StateName]
	ELSE 'Undefined'
    END,
	d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert],
	[All] = CASE tmp.[Count]
		WHEN @ComputerCount THEN '1'
		ELSE '0'
		END
FROM @DevicesPage AS tmp
INNER JOIN Devices AS d ON d.[ID] = tmp.[D_ID]
LEFT JOIN DevicesPolicies AS dp 
	ON dp.[ID] = (SELECT TOP(1) [ID] FROM DevicesPolicies WHERE [DeviceID] = tmp.[D_ID] AND 
		([LatestInsert] = tmp.[DP_LatestInsert] OR ([LatestInsert] IS NULL AND tmp.[DP_LatestInsert] IS NULL))
		AND [ComputerID] IN (SELECT [ID] FROM @ComputerPage))
LEFT JOIN DevicePolicyStates AS dps ON dps.[ID] = dp.[DevicePolicyStateID]
LEFT JOIN DeviceTypes AS dt ON dt.[ID] = d.[DeviceTypeID]
LEFT JOIN Computers AS c ON c.[ID] = dp.[ComputerID]
ORDER BY c.[ComputerName] ASC, dp.[LatestInsert] DESC

GO


-- Returns a page from Devices table without group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicesPageWithoutGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicesPageWithoutGroup]
GO

CREATE PROCEDURE [dbo].[GetDevicesPageWithoutGroup]	
WITH ENCRYPTION
AS
DECLARE @ComputerPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[ID] smallint
		)

INSERT INTO @ComputerPage([ID])
SELECT c.[ID] FROM Computers AS c
LEFT JOIN Groups AS g ON g.[ComputerID] = c.[ID]
WHERE g.[ID] IS NULL

DECLARE @ComputerCount int
SET @ComputerCount = (SELECT COUNT([ID]) FROM @ComputerPage)
	
DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[D_ID] smallint,
			[DP_LatestInsert] smalldatetime,
			[CountStates] smallint,
			[Count] int
		)

INSERT INTO @DevicesPage([D_ID], [DP_LatestInsert], [CountStates], [Count])
SELECT
	d.[ID], MAX(dp.[LatestInsert]), COUNT(DISTINCT(dp.[DevicePolicyStateID])), COUNT(dp.[DevicePolicyStateID])
FROM DevicesPolicies AS dp
INNER JOIN Devices AS d ON d.[ID] = dp.[DeviceID]
WHERE dp.[ComputerID] IN (SELECT [ID] FROM @ComputerPage)
GROUP BY d.[ID]

SELECT 
	dp.[ID], c.[ID], c.[ComputerName], d.[ID], StateName=CASE tmp.[CountStates]
	WHEN 1 THEN dps.[StateName]
	ELSE 'Undefined'
    END,
	d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert],
	[All] = CASE tmp.[Count]
		WHEN @ComputerCount THEN '1'
		ELSE '0'
		END
FROM @DevicesPage AS tmp
INNER JOIN Devices AS d ON d.[ID] = tmp.[D_ID]
LEFT JOIN DevicesPolicies AS dp 
	ON dp.[ID] = (SELECT TOP(1) [ID] FROM DevicesPolicies WHERE [DeviceID] = tmp.[D_ID] AND 
		([LatestInsert] = tmp.[DP_LatestInsert] OR ([LatestInsert] IS NULL AND tmp.[DP_LatestInsert] IS NULL))
		AND [ComputerID] IN (SELECT [ID] FROM @ComputerPage))
LEFT JOIN DevicePolicyStates AS dps ON dps.[ID] = dp.[DevicePolicyStateID]
LEFT JOIN DeviceTypes AS dt ON dt.[ID] = d.[DeviceTypeID]
LEFT JOIN Computers AS c ON c.[ID] = dp.[ComputerID]
ORDER BY c.[ComputerName] ASC, dp.[LatestInsert] DESC

GO


-- Update device policies state to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateDevicePolicyStatesToComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateDevicePolicyStatesToComputer]
GO

CREATE PROCEDURE [dbo].[UpdateDevicePolicyStatesToComputer]
	@DeviceID smallint,
	@ComputerID smallint,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @StateID
	WHERE [DeviceID] = @DeviceID AND [ComputerID] = @ComputerID
GO


-- Update device policies state to group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateDevicePolicyStatesToGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateDevicePolicyStatesToGroup]
GO

CREATE PROCEDURE [dbo].[UpdateDevicePolicyStatesToGroup]
	@DeviceID smallint,
	@GroupID int,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	DECLARE @GroupPage TABLE(
				[RecID] int IDENTITY(1, 1) NOT NULL,									
				[ID] int
			);

	WITH TreeGroups AS
	(
		SELECT [ID], [ParentID] FROM GroupTypes
		WHERE [ID] = @GroupID

		UNION ALL

		SELECT gt.[ID], gt.[ParentID] FROM GroupTypes AS gt
		INNER JOIN TreeGroups AS tg ON tg.[ID] = gt.[ParentID]
	)
	INSERT INTO @GroupPage([ID])
	SELECT [ID] FROM TreeGroups

	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @StateID
	WHERE	[DeviceID] = @DeviceID AND 
			[ComputerID] IN (
					SELECT [ComputerID] FROM Groups
					WHERE [GroupID] IN (
								SELECT [ID] FROM @GroupPage
					)
			)

GO


-- Update device policies state to without group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateDevicePolicyStatesToWithoutGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateDevicePolicyStatesToWithoutGroup]
GO

CREATE PROCEDURE [dbo].[UpdateDevicePolicyStatesToWithoutGroup]
	@DeviceID smallint,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	DECLARE @ComputerPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[ID] smallint
		)

	INSERT INTO @ComputerPage([ID])
	SELECT c.[ID] FROM Computers AS c
	LEFT JOIN Groups AS g ON g.[ComputerID] = c.[ID]
	WHERE g.[ID] IS NULL

	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @StateID
	WHERE	[DeviceID] = @DeviceID AND 
			[ComputerID] IN (SELECT [ID] FROM @ComputerPage)		
GO


-- Update all device policies state to group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateAllDevicePolicyStatesToGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateAllDevicePolicyStatesToGroup]
GO

CREATE PROCEDURE [dbo].[UpdateAllDevicePolicyStatesToGroup]	
	@GroupID int,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	DECLARE @GroupPage TABLE(
				[RecID] int IDENTITY(1, 1) NOT NULL,									
				[ID] int
			);

	WITH TreeGroups AS
	(
		SELECT [ID], [ParentID] FROM GroupTypes
		WHERE [ID] = @GroupID

		UNION ALL

		SELECT gt.[ID], gt.[ParentID] FROM GroupTypes AS gt
		INNER JOIN TreeGroups AS tg ON tg.[ID] = gt.[ParentID]
	)
	INSERT INTO @GroupPage([ID])
	SELECT [ID] FROM TreeGroups

	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @StateID
	WHERE  [ComputerID] IN (
					SELECT [ComputerID] FROM Groups
					WHERE [GroupID] IN (
								SELECT [ID] FROM @GroupPage
					)
			)

GO


-- Add device policies to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddDevicePolicyToComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddDevicePolicyToComputer]
GO

CREATE PROCEDURE [dbo].[AddDevicePolicyToComputer]
	@ComputerID smallint,
	@SerialNo nvarchar(256),
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	DECLARE @DeviceID smallint
	SET @DeviceID = (SELECT [ID] FROM Devices WHERE [SerialNo] = @SerialNo)

	IF @DeviceID IS NOT NULL
	BEGIN
		IF NOT EXISTS (SELECT [ID]FROM DevicesPolicies WHERE [ComputerID] = @ComputerID AND [DeviceID] = @DeviceID)
		BEGIN
			INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
			VALUES    (@ComputerID, @DeviceID, @StateID)
			
			SELECT [ID],[SerialNo],[Comment], @@IDENTITY AS DevicePolicyID FROM Devices WHERE [ID]=@DeviceID
		END
	END
GO



-- Get device policies to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDeviceEntitiesFromComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDeviceEntitiesFromComputer]
GO

CREATE PROCEDURE [dbo].[GetDeviceEntitiesFromComputer]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS

	 SELECT dp.[ID], dp.[ComputerID], c.[ComputerName], dp.[DeviceID], dps.[StateName],
	 d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert]
	 FROM DevicesPolicies as dp
	 INNER JOIN Computers as c ON c.[ID] = dp.[ComputerID]
	 INNER JOIN Devices as d ON dp.[DeviceID] = d.[ID]
	 INNER JOIN DevicePolicyStates as dps ON dps.[ID] = dp.[DevicePolicyStateID]
	 INNER JOIN DeviceTypes as dt ON dt.[ID] = d.[DeviceTypeID]
	 WHERE c.[ComputerName] = @ComputerName
GO


-- Add device policies to group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddDevicePolicyToGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddDevicePolicyToGroup]
GO

CREATE PROCEDURE [dbo].[AddDevicePolicyToGroup]
	@GroupID int,
	@SerialNo nvarchar(256),
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	DECLARE @DeviceID smallint
	SET @DeviceID = (SELECT [ID] FROM Devices WHERE [SerialNo] = @SerialNo)

	IF @DeviceID IS NOT NULL
	BEGIN
		DECLARE @ComputerPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[ID] smallint
		);

		WITH TreeGroups AS
		(
			SELECT [ID], [ParentID] FROM GroupTypes
			WHERE [ID] = @GroupID

			UNION ALL

			SELECT gt.[ID], gt.[ParentID] FROM GroupTypes AS gt
			INNER JOIN TreeGroups AS tg ON tg.[ID] = gt.[ParentID]
		)
		INSERT INTO @ComputerPage([ID])
		SELECT g.[ComputerID] FROM Groups AS g		
		INNER JOIN TreeGroups AS tg ON tg.[ID] = g.[GroupID]
		LEFT JOIN DevicesPolicies AS dp ON dp.[ComputerID] = g.[ComputerID] AND dp.[DeviceID] = @DeviceID
		WHERE dp.[DeviceID] IS NULL

		INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
		SELECT [ID], @DeviceID, @StateID FROM @ComputerPage
		
		SELECT [ID],[SerialNo],[Comment] AS DevicePolicyID FROM Devices WHERE [ID]=@DeviceID
	END
GO


-- Add device policies to without group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddDevicePolicyToWithoutGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddDevicePolicyToWithoutGroup]
GO

CREATE PROCEDURE [dbo].[AddDevicePolicyToWithoutGroup]	
	@SerialNo nvarchar(256),
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	DECLARE @DeviceID smallint
	SET @DeviceID = (SELECT [ID] FROM Devices WHERE [SerialNo] = @SerialNo)

	IF @DeviceID IS NOT NULL
	BEGIN
		DECLARE @ComputerPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[ID] smallint
		)

		INSERT INTO @ComputerPage([ID])
		SELECT c.[ID] FROM Computers AS c
		LEFT JOIN Groups AS g ON g.[ComputerID] = c.[ID]
		LEFT JOIN DevicesPolicies AS dp ON dp.[ComputerID] = c.[ID] AND dp.[DeviceID] = @DeviceID
		WHERE g.[ID] IS NULL AND dp.[DeviceID] IS NULL

		INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
		SELECT [ID], @DeviceID, @StateID FROM @ComputerPage
		
		SELECT [ID],[SerialNo],[Comment] AS DevicePolicyID FROM Devices WHERE [ID]=@DeviceID
	END
GO