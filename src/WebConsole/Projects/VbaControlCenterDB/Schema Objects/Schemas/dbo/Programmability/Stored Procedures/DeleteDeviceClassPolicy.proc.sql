CREATE PROCEDURE [dbo].[DeleteDeviceClassPolicy]	
	@ComputerID smallint,
	@DeviceClassID smallint
WITH ENCRYPTION
AS
	DELETE FROM DeviceClassPolicy
	WHERE [ComputerID] = @ComputerID AND [DeviceClassID] = @DeviceClassID