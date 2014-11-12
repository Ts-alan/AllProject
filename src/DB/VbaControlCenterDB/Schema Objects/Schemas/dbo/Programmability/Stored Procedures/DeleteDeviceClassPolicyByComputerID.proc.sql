CREATE PROCEDURE [dbo].[DeleteDeviceClassPolicyByComputerID]	
	@ComputerID smallint
WITH ENCRYPTION
AS
	DELETE FROM DeviceClassPolicy
	WHERE [ComputerID] = @ComputerID