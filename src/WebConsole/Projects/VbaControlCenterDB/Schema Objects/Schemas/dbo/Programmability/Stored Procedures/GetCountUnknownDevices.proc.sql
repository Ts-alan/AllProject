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