CREATE PROCEDURE [dbo].[AddDeviceClassPolicyWithoutGroup]	
	@DeviceClassID smallint,
	@DeviceClassMode nvarchar(64)
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
	GROUP BY c.[ID]

	DELETE FROM @ComputerPage 
	WHERE [ID] IN (	SELECT [ComputerID] FROM DeviceClassPolicy 
					WHERE [DeviceClassID] = @DeviceClassID 
					GROUP BY [ComputerID])

	DECLARE @ComputerID smallint
	DECLARE cur CURSOR LOCAL FOR
   	SELECT [ID] FROM @ComputerPage

	OPEN cur

	FETCH NEXT FROM cur INTO @ComputerID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC	[dbo].[AddDeviceClassPolicy] @ComputerID, @DeviceClassID, @DeviceClassMode
		FETCH NEXT FROM cur INTO @ComputerID
	END

	CLOSE cur
	DEALLOCATE cur

	SELECT COUNT(*) FROM @ComputerPage