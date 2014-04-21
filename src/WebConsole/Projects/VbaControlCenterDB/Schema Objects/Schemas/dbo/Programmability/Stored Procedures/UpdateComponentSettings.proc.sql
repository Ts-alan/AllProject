CREATE PROCEDURE [UpdateComponentSettings]
	@ComputerName nvarchar(64),
	@ComponentName nvarchar(64),
	@ComponentSettings ntext
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID = 0
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END
	
	-- Retrieving ComponentID
	DECLARE @ComponentID smallint
	EXEC @ComponentID = dbo.GetComponentTypeID @ComponentName, 1
	
	IF EXISTS (SELECT [StateID] FROM [Components] WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID)
	BEGIN
		-- Inserting/updating data
		DECLARE @SettingsID smallint
		SET @SettingsID = (SELECT [SettingsID] FROM [Components] WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID)
		IF @SettingsID IS NULL
		BEGIN
			-- Insert
			INSERT INTO [ComponentSettings](Settings)
				VALUES(@ComponentSettings)
			DECLARE @IDD smallint
			UPDATE [Components]
			SET	[SettingsID] = @@IDENTITY
			WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID
		END
		ELSE
		BEGIN
			-- Update
			-- Спорный момент!
			UPDATE [ComponentSettings]
			SET	[Settings] = @ComponentSettings
			WHERE [ID] = @SettingsID
		END
	END