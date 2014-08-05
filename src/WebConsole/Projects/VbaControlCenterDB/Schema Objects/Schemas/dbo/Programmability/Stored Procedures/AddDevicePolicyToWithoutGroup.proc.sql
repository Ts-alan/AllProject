CREATE PROCEDURE [dbo].[AddDevicePolicyToWithoutGroup]	
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
		DECLARE @ComputerPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,									
			[ID] smallint
		)

		INSERT INTO @ComputerPage([ID])
		SELECT c.[ID] FROM Computers AS c
		LEFT JOIN Groups AS g ON g.[ComputerID] = c.[ID]
		LEFT JOIN DevicesPolicies AS dp ON dp.[ComputerID] = c.[ID] AND dp.[DeviceID] = @DeviceID
		WHERE g.[ID] IS NULL AND dp.[DeviceID] IS NULL

		IF NOT EXISTS(SELECT [ID] FROM @ComputerPage)
			RETURN

		INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
		SELECT [ID], @DeviceID, @StateID FROM @ComputerPage
		
		SELECT [ID],[SerialNo],[Comment] AS DevicePolicyID FROM Devices WHERE [ID]=@DeviceID
	END