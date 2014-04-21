CREATE PROCEDURE [dbo].[InsertSettings]
	@MAC nvarchar(64),
	@ComponentName nvarchar(64),
	@Settings ntext
WITH ENCRYPTION
AS
	DECLARE @ComputerID smallint
	SET @ComputerID =  (SELECT [ID] FROM Computers WHERE [MACAddress] = @MAC)

	DECLARE @ComponentID smallint
	SET @ComponentID =  (SELECT [ID] FROM ComponentTypes WHERE [ComponentName] = @ComponentName)
	
	IF (@ComponentID IS NULL) OR (@ComputerID IS NULL)
		RETURN

	DECLARE @ComponentSettingsID smallint
	SET @ComponentSettingsID =  (SELECT [SettingsID] FROM Components WHERE [ComponentID] = @ComponentID AND [ComputerID] = @ComputerID)
	IF @ComponentSettingsID IS NOT NULL
	BEGIN
		IF @ComponentSettingsID = 1
		BEGIN
			INSERT INTO ComponentSettings([Settings])
			VALUES (@Settings)
		
			UPDATE Components
			SET [SettingsID] = @@IDENTITY
			WHERE [ComponentID] = @ComponentID AND [ComputerID] = @ComputerID
		END
		ELSE BEGIN
			UPDATE ComponentSettings
			SET [Settings] = @Settings
			WHERE [ID] = @ComponentSettingsID
		END
	END