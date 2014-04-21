CREATE PROCEDURE [dbo].[GetComputerNameListFromComponents]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Components AS cm
        INNER JOIN Computers AS c ON c.[ID] = cm.[ComputerID]
        INNER JOIN ComponentSettings AS cms ON cm.[SettingsID] = cms.[ID]
        INNER JOIN ComponentStates AS cmst ON cm.[StateID] = cmst.[ID]
        INNER JOIN ComponentTypes AS cmt ON cm.[ComponentID] = cmt.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query