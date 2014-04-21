CREATE PROCEDURE [dbo].[GetComputersStatistic]
	@Field nvarchar (64),
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT ' + @Field + N', c.[ComputerName]
					INTO #vba_temp FROM Computers AS c
					INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N'; SELECT DISTINCT(' + @Field + N'), count([ComputerName]) as [Count]
						FROM #vba_temp GROUP BY ' + @Field +
						N' ORDER BY [Count] DESC'
	EXEC sp_executesql @Query