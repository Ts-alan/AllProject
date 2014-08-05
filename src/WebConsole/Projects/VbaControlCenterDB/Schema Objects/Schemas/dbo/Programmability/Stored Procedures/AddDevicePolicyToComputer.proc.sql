CREATE PROCEDURE [dbo].[AddDevicePolicyToComputer]
	@ComputerID smallint,
	@SerialNo nvarchar(256),
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DeviceClassMode WHERE [ModeName] = @StateName)

	DECLARE @DeviceID smallint
	SET @DeviceID = (SELECT [ID] FROM Devices WHERE [SerialNo] = @SerialNo)

	IF @DeviceID IS NOT NULL
	BEGIN
		IF NOT EXISTS (SELECT [ID]FROM DevicesPolicies WHERE [ComputerID] = @ComputerID AND [DeviceID] = @DeviceID)
		BEGIN
			INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
			VALUES    (@ComputerID, @DeviceID, @StateID)
			
			SELECT [ID],[SerialNo],[Comment], @@IDENTITY AS DevicePolicyID FROM Devices WHERE [ID]=@DeviceID
		END
	END