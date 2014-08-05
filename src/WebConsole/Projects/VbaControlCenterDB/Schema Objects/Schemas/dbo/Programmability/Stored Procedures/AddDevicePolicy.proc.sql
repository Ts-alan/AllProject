CREATE PROCEDURE [dbo].[AddDevicePolicy]
	@ComputerID smallint,
	@DeviceID smallint,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @ID smallint
	SET @ID = (SELECT [ID] FROM DeviceClassMode WHERE [ModeName] = @StateName)
	
	INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
	VALUES    (@ComputerID, @DeviceID, @ID)