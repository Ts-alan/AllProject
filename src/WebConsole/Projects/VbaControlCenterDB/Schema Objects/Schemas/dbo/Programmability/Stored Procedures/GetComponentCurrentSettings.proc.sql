CREATE PROCEDURE [dbo].[GetComponentCurrentSettings]	
	@compId smallint,
	@name nvarchar(64)
WITH ENCRYPTION
AS
	SELECT cs.[Name], cs.[Settings] FROM Components AS c
	INNER JOIN ComponentTypes AS ct ON ct.[ID] = c.[ComponentID]
	INNER JOIN ComponentSettings AS cs ON cs.[ID] = c.[SettingsID]
	WHERE c.[ComputerID] = @compId AND ct.[ComponentName] = @name