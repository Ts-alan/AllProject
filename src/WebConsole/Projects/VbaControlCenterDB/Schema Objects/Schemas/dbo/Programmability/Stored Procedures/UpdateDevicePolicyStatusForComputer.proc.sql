CREATE PROCEDURE [dbo].[UpdateDevicePolicyStatusForComputer]
	@ID smallint,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @ID1 smallint
	SET @ID1 = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)
	
	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @ID1  
	WHERE [ID] = @ID