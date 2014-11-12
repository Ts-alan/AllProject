CREATE PROCEDURE [dbo].[GetComputersWithoutPolicyPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersPage TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL
		)
	
		INSERT INTO @ComputersPage([ID])
		SELECT [ID] FROM ComputersExtended
		WHERE [PolicyID] IS NULL'
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT c.*
		FROM @ComputersPage AS cp
		INNER JOIN ComputersExtended AS c ON c.[ID] = cp.[ID]
		WHERE cp.[RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query