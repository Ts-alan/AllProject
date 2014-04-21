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