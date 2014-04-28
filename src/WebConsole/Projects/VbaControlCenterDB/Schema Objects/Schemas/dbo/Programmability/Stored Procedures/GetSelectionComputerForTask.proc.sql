CREATE PROCEDURE [dbo].[GetSelectionComputerForTask]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT [ComputerName], [IPAddress]
		FROM ComputersExtended'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	
	EXEC sp_executesql @Query