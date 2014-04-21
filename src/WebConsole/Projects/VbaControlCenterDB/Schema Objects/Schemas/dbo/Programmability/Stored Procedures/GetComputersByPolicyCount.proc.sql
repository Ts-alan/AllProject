CREATE PROCEDURE [dbo].[GetComputersByPolicyCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (c.[ID]) FROM Policies AS p
			INNER JOIN Computers AS c ON c.[ID] = p.[ComputerID]
			INNER JOIN PolicyTypes AS pt ON  pt.[ID] = p.[PolicyID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query