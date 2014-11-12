CREATE PROCEDURE [dbo].[GetTasksCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (t.[ID]) FROM Tasks AS t
					INNER JOIN Computers AS c ON t.[ComputerID] = c.[ID]
					INNER JOIN TaskTypes AS tt ON t.[TaskID] = tt.[ID]
					INNER JOIN TaskStates AS ts ON t.[StateID] = ts.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query