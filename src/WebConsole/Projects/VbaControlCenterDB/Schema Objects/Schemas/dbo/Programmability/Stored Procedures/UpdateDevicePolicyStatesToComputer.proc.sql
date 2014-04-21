CREATE PROCEDURE [dbo].[UpdateDevicePolicyStatesToComputer]
	@DeviceID smallint,
	@ComputerID smallint,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @StateID
	WHERE [DeviceID] = @DeviceID AND [ComputerID] = @ComputerID