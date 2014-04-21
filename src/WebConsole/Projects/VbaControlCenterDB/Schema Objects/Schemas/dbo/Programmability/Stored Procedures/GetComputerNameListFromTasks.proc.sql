CREATE PROCEDURE [dbo].[GetComputerNameListFromTasks]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Tasks AS t
        INNER JOIN Computers AS c ON c.[ID] = t.[ComputerID] 
        INNER JOIN TaskStates AS ts ON t.[StateID] = ts.[ID]
        INNER JOIN TaskTypes AS tt ON t.[TaskID] = tt.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query