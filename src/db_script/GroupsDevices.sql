-- Returns a page from Devices table for group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicesPageForGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicesPageForGroup]
GO

CREATE PROCEDURE [dbo].[GetDevicesPageForGroup]
	@GroupID int
WITH ENCRYPTION
AS
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

DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[D_ID] smallint,
			[DP_LatestInsert] smalldatetime,
			[CountStates] smallint
		)
	
INSERT INTO @DevicesPage([D_ID], [DP_LatestInsert], [CountStates])

SELECT
	d.[ID], MAX(dp.[LatestInsert]), COUNT(DISTINCT(dp.[DevicePolicyStateID]))
FROM @GroupPage AS gp
INNER JOIN Groups AS g ON g.[GroupID] = gp.[ID]
INNER JOIN DevicesPolicies AS dp ON dp.[ComputerID] = g.[ComputerID]
INNER JOIN Devices AS d ON d.[ID] = dp.[DeviceID]
GROUP BY d.[ID]

SELECT 
	dp.[ID], c.[ID], c.[ComputerName], d.[ID], StateName=CASE tmp.[CountStates]
	WHEN 1 THEN dps.[StateName]
	ELSE 'Undefined'
    END,
	d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert]
FROM @DevicesPage AS tmp
INNER JOIN Devices AS d ON d.[ID] = tmp.[D_ID]
LEFT JOIN DevicesPolicies AS dp ON dp.[DeviceID] = tmp.[D_ID] AND (dp.[LatestInsert] = tmp.[DP_LatestInsert] OR (dp.[LatestInsert] IS NULL AND tmp.[DP_LatestInsert] IS NULL))
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
DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[D_ID] smallint,
			[DP_LatestInsert] smalldatetime
		)
	
INSERT INTO @DevicesPage([D_ID], [DP_LatestInsert])

SELECT
	d.[ID], MAX(dp.[LatestInsert])
FROM Computers AS c
INNER JOIN DevicesPolicies AS dp ON dp.[ComputerID] = c.[ID]
INNER JOIN Devices AS d ON d.[ID] = dp.[DeviceID]
LEFT JOIN Groups AS g ON g.[ComputerID] = c.[ID]
WHERE g.[ID] IS NULL
GROUP BY d.[ID]

SELECT 
	dp.[ID], c.[ID], c.[ComputerName], d.[ID], dps.[StateName],
	d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert]
FROM @DevicesPage AS tmp
INNER JOIN Devices AS d ON d.[ID] = tmp.[D_ID]
LEFT JOIN DevicesPolicies AS dp ON dp.[DeviceID] = tmp.[D_ID] AND (dp.[LatestInsert] = tmp.[DP_LatestInsert] OR (dp.[LatestInsert] IS NULL AND tmp.[DP_LatestInsert] IS NULL))
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