-- Get Statistics
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetStatistics]')
        AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetStatistics]
GO

CREATE PROCEDURE [dbo].[GetStatistics]
 @OrderBy nvarchar(128),
 @Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
 DECLARE @Query nvarchar(4000)
 SET @Query =  N'SELECT ' + @OrderBy + N', COUNT(*) AS [Count] FROM Events AS e
  INNER JOIN Computers AS c ON c.[ID] = e.[ComputerID]
  INNER JOIN EventTypes AS et ON et.[ID] = e.[EventID]'
 IF @Where IS NOT NULL
  SET @Query = @Query + N' WHERE ' + @Where
 SET @Query = @Query + N' GROUP BY ' + @OrderBy + N' ORDER BY [Count] DESC'
 EXEC sp_executesql @Query
GO