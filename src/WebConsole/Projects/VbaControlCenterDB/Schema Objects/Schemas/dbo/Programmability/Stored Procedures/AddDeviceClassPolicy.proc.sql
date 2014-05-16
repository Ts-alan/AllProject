CREATE PROCEDURE [dbo].[AddDeviceClassPolicy]
	@ComputerID smallint,
	@DeviceClassID smallint,
	@DeviceClassMode nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @DeviceClassModeID smallint
	SET @DeviceClassModeID = (SELECT [ID] FROM DeviceClassMode WHERE [ModeName] = @DeviceClassMode)
	
	IF @DeviceClassModeID IS NULL
		SELECT 0
	ELSE BEGIN
		DECLARE @ID int
		SET @ID = (SELECT [ID] FROM DeviceClassPolicy WHERE [ComputerID] = @ComputerID AND [DeviceClassID] = @DeviceClassID)
		IF @ID IS NOT NULL
		BEGIN
			UPDATE DeviceClassPolicy
			SET [DeviceClassModeID] = @DeviceClassModeID
			WHERE [ComputerID] = @ComputerID AND [DeviceClassID] = @DeviceClassID
		END
		ELSE BEGIN 
			INSERT INTO DeviceClassPolicy ([ComputerID], [DeviceClassID], [DeviceClassModeID])
			VALUES (@ComputerID, @DeviceClassID, @DeviceClassModeID)

			SET @ID = @@IDENTITY
		END

		SELECT @ID
	END