-- GetComponentCurrentSettings
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComponentCurrentSettings]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComponentCurrentSettings]
GO

CREATE PROCEDURE [dbo].[GetComponentCurrentSettings]	
	@compId smallint,
	@name nvarchar(64)
WITH ENCRYPTION
AS
	SELECT cs.[Name], cs.[Settings] FROM Components AS c
	INNER JOIN ComponentTypes AS ct ON ct.[ID] = c.[ComponentID]
	INNER JOIN ComponentSettings AS cs ON cs.[ID] = c.[SettingsID]
	WHERE c.[ComputerID] = @compId AND ct.[ComponentName] = @name
GO


-- Insert component settings
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[InsertSettings]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[InsertSettings]
GO

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
GO