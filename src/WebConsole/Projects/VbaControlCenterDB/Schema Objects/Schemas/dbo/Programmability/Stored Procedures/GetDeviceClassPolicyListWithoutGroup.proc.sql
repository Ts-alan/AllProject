CREATE PROCEDURE [dbo].[GetDeviceClassPolicyListWithoutGroup]
WITH ENCRYPTION
AS
	DECLARE @ComputerPage TABLE(
				[RecID] int IDENTITY(1, 1) NOT NULL,									
				[ID] smallint
			);
	
	INSERT INTO @ComputerPage([ID])
	SELECT c.[ID] FROM Computers AS c
	LEFT JOIN Groups AS g ON g.[ComputerID] = c.[ID]
	WHERE g.[ID] IS NULL
	
	DECLARE @ComputerCount int
	SET @ComputerCount = (SELECT COUNT([ID]) FROM @ComputerPage)

	DECLARE @DeviceClassPage TABLE(
				[RecID] int IDENTITY(1, 1) NOT NULL,									
				[ID] smallint,
				[CountModes] int,
				[Count] int
			)
		
	INSERT INTO @DeviceClassPage([ID], [CountModes], [Count])
	SELECT
		dcp.[DeviceClassID], COUNT(DISTINCT(dcp.[DeviceClassModeID])), COUNT(dcp.[DeviceClassModeID])
	FROM DeviceClassPolicy AS dcp
	INNER JOIN DeviceClassMode AS dcm ON dcm.[ID] = dcp.[DeviceClassModeID]
	WHERE dcp.[ComputerID] IN (SELECT [ID] FROM @ComputerPage)
	GROUP BY dcp.[DeviceClassID]

	SELECT 
		DISTINCT(dc.[ID]), dc.[UID], dc.[Class], dc.[ClassName], ModeName = CASE tmp.[CountModes]
		WHEN 1 THEN dcm.[ModeName]
		ELSE 'Undefined'
		END,
		[All] = CASE tmp.[Count]
		WHEN @ComputerCount THEN 1
		ELSE 0
		END
	FROM @DeviceClassPage AS tmp
	INNER JOIN DeviceClass AS dc ON dc.[ID] = tmp.[ID]
	LEFT JOIN DeviceClassPolicy AS dcp ON dcp.[DeviceClassID] = tmp.[ID] AND dcp.[ComputerID] IN (SELECT [ID] FROM @ComputerPage)
	LEFT JOIN DeviceClassMode AS dcm ON dcm.[ID] = dcp.[DeviceClassModeID]
	ORDER BY dc.[UID] ASC