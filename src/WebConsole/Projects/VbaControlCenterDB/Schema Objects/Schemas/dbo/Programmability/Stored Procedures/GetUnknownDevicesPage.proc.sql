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