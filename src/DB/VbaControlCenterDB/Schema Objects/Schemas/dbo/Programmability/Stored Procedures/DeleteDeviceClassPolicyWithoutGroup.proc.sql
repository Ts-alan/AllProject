CREATE PROCEDURE [dbo].[DeleteDeviceClassPolicyWithoutGroup]	
	@DeviceClassID smallint
WITH ENCRYPTION
AS
	DECLARE @ComputerPage TABLE(
				[RecID] int IDENTITY(1, 1) NOT NULL,									
				[ID] smallint
			);
	
	INSERT INTO @ComputerPage([ID])
	SELECT c.[ID] FROM Computers AS c
	LEFT JOIN Groups AS g ON g.[ComputerID] = c.[ID]
	WHERE g.[ID] IS NULL

	DELETE FROM DeviceClassPolicy
	WHERE [DeviceClassID] = @DeviceClassID AND ([ComputerID] IN (SELECT [ID] FROM @ComputerPage))