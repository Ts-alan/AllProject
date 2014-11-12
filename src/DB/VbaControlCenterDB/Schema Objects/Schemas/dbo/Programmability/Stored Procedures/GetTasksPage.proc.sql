CREATE PROCEDURE [dbo].[GetTasksPage]
	@Page bigint,
	@RowCount bigint,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @TasksPage TABLE(
			[RecID] bigint IDENTITY(1, 1) NOT NULL,
			[ID] bigint NOT NULL,
			[TaskName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[TaskState] nvarchar(32) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[DateIssued] datetime NOT NULL,
			[DateComplete] datetime NULL,
			[DateUpdated] datetime NOT NULL,
			[TaskParams] ntext COLLATE Cyrillic_General_CI_AS NULL,
			[TaskUser] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[TaskDescription] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL
		)
	
		INSERT INTO @TasksPage(
			[ID], [TaskName], [ComputerName], [TaskState],
			[DateIssued], [DateComplete], [DateUpdated], [TaskParams], [TaskUser], [TaskDescription])
		SELECT
			t.[ID], tt.[TaskName], c.[ComputerName], ts.[TaskState],
			t.[DateIssued], t.[DateComplete], t.[DateUpdated], t.[TaskParams], t.[TaskUser], t.[TaskDescription]
		FROM Tasks AS t
		INNER JOIN Computers AS c ON t.[ComputerID] = c.[ID]
		INNER JOIN TaskTypes AS tt ON t.[TaskID] = tt.[ID]
		INNER JOIN TaskStates AS ts ON t.[StateID] = ts.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT  [ID], [TaskName], [ComputerName], [TaskState],
				[DateIssued], [DateComplete], [DateUpdated], [TaskParams], [TaskUser], [TaskDescription]
		FROM @TasksPage	WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query