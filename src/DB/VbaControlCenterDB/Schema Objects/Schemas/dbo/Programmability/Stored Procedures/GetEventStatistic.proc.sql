CREATE  PROCEDURE [dbo].[GetEventStatistic]
	@Field nvarchar (64),
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT ' + @Field + N', [EventName]
					INTO #vba_temp FROM Events AS e
					INNER JOIN Computers AS c ON e.[ComputerID] = c.[ID]
					INNER JOIN ComponentTypes AS ct ON e.[ComponentID] = ct.[ID]
					INNER JOIN EventTypes AS et ON e.[EventID] = et.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N'; SELECT DISTINCT(' + @Field + N'), count([EventName]) as [Count]
						FROM #vba_temp GROUP BY '+ @Field +
						N' ORDER BY [Count] DESC'
	EXEC sp_executesql @Query