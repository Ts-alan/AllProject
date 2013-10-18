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
	dp.[ID], c.[ID],
	[ComputerName] = CASE
		WHEN tmp.[DP_LatestInsert] IS NULL THEN NULL
		ELSE c.[ComputerName]
	END, 
	d.[ID],
	StateName=CASE tmp.[CountStates]
		WHEN 1 THEN dps.[StateName]
		ELSE 'Undefined'
    END,
	d.[SerialNo], dt.[TypeName], d.[Comment], tmp.[DP_LatestInsert],
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
	dp.[ID], c.[ID], 
	[ComputerName] = CASE
		WHEN tmp.[DP_LatestInsert] IS NULL THEN NULL
		ELSE c.[ComputerName]
	END,
	d.[ID], 
	StateName=CASE tmp.[CountStates]
		WHEN 1 THEN dps.[StateName]
		ELSE 'Undefined'
    END,
	d.[SerialNo], dt.[TypeName], d.[Comment], tmp.[DP_LatestInsert],
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

		IF NOT EXISTS(SELECT [ID] FROM @ComputerPage)
			RETURN

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

		IF NOT EXISTS(SELECT [ID] FROM @ComputerPage)
			RETURN

		INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
		SELECT [ID], @DeviceID, @StateID FROM @ComputerPage
		
		SELECT [ID],[SerialNo],[Comment] AS DevicePolicyID FROM Devices WHERE [ID]=@DeviceID
	END
GO

-- Remove device policies from group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RemoveDevicePolicyFromGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveDevicePolicyFromGroup]
GO

CREATE PROCEDURE [dbo].[RemoveDevicePolicyFromGroup]
	@GroupID int,
	@DeviceID smallint
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

	DELETE FROM DevicesPolicies 
	WHERE [DeviceID] = @DeviceID AND ([ComputerID] IN (SELECT [ID] FROM @ComputerPage))
GO

-- Remove device policies from without group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RemoveDevicePolicyFromWithoutGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveDevicePolicyFromWithoutGroup]
GO

CREATE PROCEDURE [dbo].[RemoveDevicePolicyFromWithoutGroup]	
	@DeviceID smallint
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

	DELETE FROM DevicesPolicies 
	WHERE [DeviceID] = @DeviceID AND ([ComputerID] IN (SELECT [ID] FROM @ComputerPage))
GO


--  Get devicePolicy state list
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicePolicyStates]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicePolicyStates]
GO

CREATE PROCEDURE [GetDevicePolicyStates]
WITH ENCRYPTION
AS
	SELECT [StateName] FROM DevicePolicyStates
	ORDER BY [StateName] ASC
GO


-- Returns a page from Devices table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicesPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicesPage]
GO

CREATE PROCEDURE [dbo].[GetDevicesPage]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] smallint,
			[SerialNo] nvarchar(256) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS
		)
	
		INSERT INTO @DevicesPage(
			[ID], [SerialNo], [Comment])

		SELECT
			d.[ID], d.[SerialNo], d.[Comment]
		FROM Devices AS d
		LEFT JOIN DevicesPolicies AS dp ON d.[ID] = dp.[DeviceID]
		LEFT JOIN DevicePolicyStates AS dps ON dps.[ID] = dp.[DevicePolicyStateID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N' GROUP BY d.[ID], d.[SerialNo], d.[Comment]'
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [SerialNo], [Comment]
		FROM @DevicesPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'	

	EXEC sp_executesql @Query
GO

-- Returns a count from Devices table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetCountDevices]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetCountDevices]
GO

CREATE PROCEDURE [dbo].[GetCountDevices]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] smallint,
			[SerialNo] nvarchar(256) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS
		)
	
		INSERT INTO @DevicesPage(
			[ID], [SerialNo], [Comment])

		SELECT
			d.[ID], d.[SerialNo], d.[Comment]
		FROM Devices AS d
		LEFT JOIN DevicesPolicies AS dp ON d.[ID] = dp.[DeviceID]
		LEFT JOIN DevicePolicyStates AS dps ON dps.[ID] = dp.[DevicePolicyStateID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N' GROUP BY d.[ID], d.[SerialNo], d.[Comment]'
	SET @Query = @Query + N';
		SELECT COUNT(*) FROM @DevicesPage'	

	EXEC sp_executesql @Query
GO


-- Get ComputerListByDeviceID
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerListByDeviceID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerListByDeviceID]
GO

CREATE PROCEDURE [dbo].[GetComputerListByDeviceID]
	@DeviceID smallint		
WITH ENCRYPTION
AS

	SELECT 
		dp.[ID], c.[ID], c.[ComputerName], dps.[StateName], dp.[LatestInsert], g.[GroupID]
	FROM DevicesPolicies AS dp
	INNER JOIN DevicePolicyStates AS dps ON dps.[ID] = dp.[DevicePolicyStateID]
	INNER JOIN Computers AS c ON c.[ID] = dp.[ComputerID]
	LEFT JOIN Groups AS g ON g.[ComputerID] = dp.[ComputerID]	
	WHERE dp.[DeviceID] = @DeviceID
	ORDER BY c.[ComputerName] ASC

GO


-- Returns a page from Devices table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetUnknownDevicesPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetUnknownDevicesPage]
GO

CREATE PROCEDURE [dbo].[GetUnknownDevicesPage]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[PolicyID] int,
			[ID] smallint,
			[SerialNo] nvarchar(256) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[LatestInsert] smalldatetime
		)
	
		INSERT INTO @DevicesPage(
			[PolicyID], [ID], [SerialNo], [Comment], [ComputerName], [LatestInsert])

		SELECT
			dp.[ID], d.[ID], d.[SerialNo], d.[Comment], c.[ComputerName], dp.[LatestInsert]
		FROM Devices AS d
		INNER JOIN DevicesPolicies AS dp ON d.[ID] = dp.[DeviceID]
		INNER JOIN DevicePolicyStates AS dps ON dps.[ID] = dp.[DevicePolicyStateID]
		INNER JOIN Computers AS c ON c.[ID] = dp.[ComputerID]
		WHERE (dp.[LatestInsert] IS NOT NULL) AND dps.[StateName] = ''Undefined'''
	IF @Where IS NOT NULL
		SET @Query = @Query + N' AND ' + @Where	
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [PolicyID], [ID], [SerialNo], [Comment], [ComputerName], [LatestInsert]
		FROM @DevicesPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'	

	EXEC sp_executesql @Query
GO

-- Returns a count from Devices table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetCountUnknownDevices]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetCountUnknownDevices]
GO

CREATE PROCEDURE [dbo].[GetCountUnknownDevices]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] smallint,
			[SerialNo] nvarchar(256) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[LatestInsert] smalldatetime
		)
	
		INSERT INTO @DevicesPage(
			[ID], [SerialNo], [Comment], [ComputerName], [LatestInsert])

		SELECT
			d.[ID], d.[SerialNo], d.[Comment], c.[ComputerName], dp.[LatestInsert]
		FROM Devices AS d
		INNER JOIN DevicesPolicies AS dp ON d.[ID] = dp.[DeviceID]
		INNER JOIN DevicePolicyStates AS dps ON dps.[ID] = dp.[DevicePolicyStateID]
		INNER JOIN Computers AS c ON c.[ID] = dp.[ComputerID]
		WHERE (dp.[LatestInsert] IS NOT NULL) AND dps.[StateName] = ''Undefined'''
	IF @Where IS NOT NULL
		SET @Query = @Query + N' AND ' + @Where
	
	SET @Query = @Query + N';
		SELECT COUNT(*) FROM @DevicesPage'	

	EXEC sp_executesql @Query
GO


-- [GetSubgroupTypes]
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetSubgroupTypes]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetSubgroupTypes]
GO

CREATE PROCEDURE [dbo].[GetSubgroupTypes]
	@ParentID int
WITH ENCRYPTION
AS
	SELECT * FROM GroupTypes
	WHERE [ParentID] = @ParentID
	ORDER BY [GroupName]
GO