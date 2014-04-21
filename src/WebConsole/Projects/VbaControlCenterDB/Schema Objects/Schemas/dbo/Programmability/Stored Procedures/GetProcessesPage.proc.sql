CREATE PROCEDURE [dbo].[GetProcessesPage]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @ProcessesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ProcessName] nvarchar(260) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[MemorySize] int NULL,
			[LastDate] datetime NOT NULL
		)
	
		INSERT INTO @ProcessesPage(
			[ComputerName], [ProcessName], [MemorySize], [LastDate])
		SELECT
			c.[ComputerName], p.[ProcessName], p.[MemorySize], p.[LastDate]
		FROM Processes AS p
		INNER JOIN Computers AS c ON p.[ComputerID] = c.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ComputerName], [ProcessName], [MemorySize], [LastDate]
		FROM @ProcessesPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query