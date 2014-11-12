CREATE PROCEDURE [dbo].[GetComputerListByDeviceClassID]
	@DeviceClassID smallint		
WITH ENCRYPTION
AS
	SELECT 
		dcp.[DeviceClassID], c.[ID], c.[ComputerName], dcm.[ModeName], g.[GroupID]
	FROM DeviceClassPolicy AS dcp
	INNER JOIN DeviceClassMode AS dcm ON dcm.[ID] = dcp.[DeviceClassModeID]
	INNER JOIN Computers AS c ON c.[ID] = dcp.[ComputerID]
	LEFT JOIN Groups AS g ON g.[ComputerID] = dcp.[ComputerID]	
	WHERE dcp.[DeviceClassID] = @DeviceClassID
	ORDER BY c.[ComputerName] ASC