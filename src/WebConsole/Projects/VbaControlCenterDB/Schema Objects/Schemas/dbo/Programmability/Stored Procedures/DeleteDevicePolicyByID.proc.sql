CREATE PROCEDURE [dbo].[DeleteDevicePolicyByID]
	@ID int
WITH ENCRYPTION
AS
	 DELETE	 FROM DevicesPolicies WHERE [ID] = @ID