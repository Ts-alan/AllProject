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
	DECLARE @ComponentSettingsID smallint
	SET @ComponentSettingsID =  (SELECT cmpt.[SettingsID] FROM Components AS cmpt
									INNER JOIN Computers AS c ON c.[ID] = cmpt.[ComputerID]
									INNER JOIN ComponentTypes AS ct ON ct.[ID] = cmpt.[ComponentID]
									WHERE c.[MACAddress] = @MAC AND ct.[ComponentName] = @ComponentName)
	
	UPDATE ComponentSettings
	SET [Settings] = @Settings
	WHERE [ID] = @ComponentSettingsID
GO