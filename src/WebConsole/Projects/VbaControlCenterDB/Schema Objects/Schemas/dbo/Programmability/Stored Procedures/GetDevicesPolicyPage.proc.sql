CREATE PROCEDURE [dbo].[GetDevicesPolicyPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @UnknownDevicesPolicy TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] int NOT NULL,
			[ComputerID] smallint NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[DeviceID] smallint NOT NULL,
			[StateName] nvarchar(64) NOT NULL,
			[SerialNo] nvarchar(256) NOT NULL ,
			[TypeName] nvarchar(256) NOT NULL,
			[Comment] nvarchar(128) NULL,
			[LatestInsert] smalldatetime NULL
		)
	
		INSERT INTO @UnknownDevicesPolicy([ID], [ComputerID], [ComputerName], [DeviceID], 
			[StateName],[SerialNo], [TypeName], [Comment],[LatestInsert])
		SELECT dp.[ID], dp.[ComputerID], c.[ComputerName], dp.[DeviceID], dps.[StateName],
		 d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert]
		 FROM DevicesPolicies as dp
		 INNER JOIN Computers as c ON c.[ID] = dp.[ComputerID]
		 INNER JOIN Devices as d ON dp.[DeviceID] = d.[ID]
		 INNER JOIN DevicePolicyStates as dps ON dps.[ID] = dp.[DevicePolicyStateID]
		 INNER JOIN DeviceTypes as dt ON dt.[ID] = d.[DeviceTypeID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [ComputerID], [ComputerName], [DeviceID], [StateName],
			[SerialNo], [TypeName], [Comment],[LatestInsert]
		FROM @UnknownDevicesPolicy WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query