CREATE PROCEDURE [dbo].[GetDevicesPageCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (*) FROM Devices as d					
					INNER JOIN DeviceTypes AS dt ON d.[DeviceTypeID] = dt.[ID]
					LEFT JOIN DevicesPolicies AS dp ON d.[ID] = dp.[DeviceID]
					LEFT JOIN Computers AS c ON dp.[ComputerID] = c.[ID]
					WHERE (dp.[LatestInsert] = (SELECT MAX([LatestInsert]) FROM DevicesPolicies WHERE [DeviceID] = d.[ID]) OR (0 = (SELECT COUNT([ID]) FROM DevicesPolicies WHERE [DeviceID] = d.[ID])))'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' AND ' + @Where
	
	EXEC sp_executesql @Query