CREATE PROCEDURE [dbo].[GetComputerNameListFromEvents]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Events AS e
        INNER JOIN Computers AS c ON c.[ID] = e.[ComputerID]
        INNER JOIN EventTypes AS et ON e.[EventID] = et.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query