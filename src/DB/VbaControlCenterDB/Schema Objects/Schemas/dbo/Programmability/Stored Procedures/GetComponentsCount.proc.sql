CREATE PROCEDURE [dbo].[GetComponentsCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (cmnt.[ComputerID]) FROM Components AS cmnt
					INNER JOIN Computers AS c ON cmnt.[ComputerID] = c.[ID]
					INNER JOIN ComponentStates AS cst ON cmnt.[StateID] = cst.[ID]
					INNER JOIN ComponentSettings AS cse ON cmnt.[SettingsID] = cse.[ID]
					INNER JOIN ComponentTypes AS cty ON cmnt.[ComponentID] = cty.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query