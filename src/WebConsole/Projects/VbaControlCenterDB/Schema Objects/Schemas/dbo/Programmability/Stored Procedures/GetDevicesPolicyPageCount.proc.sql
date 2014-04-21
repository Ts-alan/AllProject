CREATE PROCEDURE [dbo].[GetDevicesPolicyPageCount]
	@Where nvarchar(2000) = NULL
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT COUNT(*)
		 FROM DevicesPolicies as dp
		 INNER JOIN Computers as c ON c.[ID] = dp.[ComputerID]
		 INNER JOIN Devices as d ON dp.[DeviceID] = d.[ID]
		 INNER JOIN DevicePolicyStates as dps ON dps.[ID] = dp.[DevicePolicyStateID]
		 INNER JOIN DeviceTypes as dt ON dt.[ID] = d.[DeviceTypeID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where	
	EXEC sp_executesql @Query