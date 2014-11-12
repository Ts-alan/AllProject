CREATE PROCEDURE [dbo].[GetEventsCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (e.[ID]) FROM Events AS e
					INNER JOIN Computers AS c ON e.[ComputerID] = c.[ID]
					INNER JOIN ComponentTypes AS ct ON e.[ComponentID] = ct.[ID]
					INNER JOIN EventTypes AS et ON e.[EventID] = et.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query