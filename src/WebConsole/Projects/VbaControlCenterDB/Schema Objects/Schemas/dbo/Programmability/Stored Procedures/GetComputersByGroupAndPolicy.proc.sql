CREATE PROCEDURE [dbo].[GetComputersByGroupAndPolicy]
	@GroupID int = NULL,
	@PolicyID smallint = NULL	
WITH ENCRYPTION
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT * FROM ComputersExtended'

		IF @GroupID IS NOT NULL
			SET @Query = @Query + ' WHERE [GroupID] = ' + CAST(@GroupID AS nvarchar(32));
		ELSE
			SET @Query = @Query + ' WHERE [GroupID] IS NULL';

		IF @PolicyID IS NOT NULL
			SET @Query = @Query + ' AND [PolicyID] = ' + CAST(@PolicyID AS nvarchar(32));
		ELSE
			SET @Query = @Query + ' AND [PolicyID] IS NULL';

	SET @Query = @Query + N' ORDER BY [ComputerName] ASC'

	EXEC sp_executesql @Query