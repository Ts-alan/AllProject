CREATE PROCEDURE [dbo].[GetComputerNameListFromProcesses]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Processes AS p
		INNER JOIN Computers AS c ON p.[ComputerID] = c.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query