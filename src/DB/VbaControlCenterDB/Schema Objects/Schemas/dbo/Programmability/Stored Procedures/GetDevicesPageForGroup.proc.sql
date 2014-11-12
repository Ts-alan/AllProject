CREATE PROCEDURE [dbo].[GetDevicesPageForGroup]
	@GroupID int,
	@DeviceType nvarchar(256)
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
	INNER JOIN DeviceTypes AS dt ON dt.[ID] = d.[DeviceTypeID]
	WHERE dp.[ComputerID] IN (SELECT [ID] FROM @ComputerPage) AND dt.TypeName = @DeviceType
	GROUP BY d.[ID]

	SELECT 
		dp.[ID], c.[ID],
		[ComputerName] = CASE
			WHEN tmp.[DP_LatestInsert] IS NULL THEN NULL
			ELSE c.[ComputerName]
		END, 
		d.[ID],
		StateName=CASE tmp.[CountStates]
			WHEN 1 THEN dps.[ModeName]
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
	LEFT JOIN DeviceClassMode AS dps ON dps.[ID] = dp.[DevicePolicyStateID]
	LEFT JOIN DeviceTypes AS dt ON dt.[ID] = d.[DeviceTypeID]
	LEFT JOIN Computers AS c ON c.[ID] = dp.[ComputerID]
	ORDER BY c.[ComputerName] ASC, dp.[LatestInsert] DESC