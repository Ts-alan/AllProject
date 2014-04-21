CREATE PROCEDURE [dbo].[UpdateDevicePolicyStatesToWithoutGroup]
	@DeviceID smallint,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	DECLARE @ComputerPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[ID] smallint
		)

	INSERT INTO @ComputerPage([ID])
	SELECT c.[ID] FROM Computers AS c
	LEFT JOIN Groups AS g ON g.[ComputerID] = c.[ID]
	WHERE g.[ID] IS NULL

	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @StateID
	WHERE	[DeviceID] = @DeviceID AND 
			[ComputerID] IN (SELECT [ID] FROM @ComputerPage)