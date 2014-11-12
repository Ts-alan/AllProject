CREATE PROCEDURE [dbo].[DeleteDeviceClassPolicyByDeviceClassID]
	@DeviceClassID smallint
WITH ENCRYPTION
AS
	DELETE FROM DeviceClassPolicy
	WHERE [DeviceClassID] = @DeviceClassID