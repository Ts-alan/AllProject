CREATE PROCEDURE [dbo].[GetDeviceClassPolicyListByComputerID]
	@ComputerID smallint		
WITH ENCRYPTION
AS
	SELECT dcp.[ID],  dc.[ID], dc.[UID], dc.[Class], dc.[ClassName], dcm.[ModeName] FROM DeviceClassPolicy AS dcp
	INNER JOIN DeviceClass AS dc ON dc.[ID] = dcp.[DeviceClassID]
	INNER JOIN DeviceClassMode AS dcm ON dcm.[ID] = dcp.[DeviceClassModeID]
	WHERE dcp.[ComputerID] = @ComputerID
	ORDER BY [UID] ASC