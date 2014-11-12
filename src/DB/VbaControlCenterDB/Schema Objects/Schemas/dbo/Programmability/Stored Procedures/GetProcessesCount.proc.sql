CREATE PROCEDURE [dbo].[GetProcessesCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (p.[ID]) FROM Processes AS p
					INNER JOIN Computers AS c ON p.[ComputerID] = c.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query