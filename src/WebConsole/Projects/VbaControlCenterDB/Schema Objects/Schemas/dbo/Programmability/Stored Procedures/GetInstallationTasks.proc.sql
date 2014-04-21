CREATE PROCEDURE [dbo].[GetInstallationTasks]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @TasksPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] int,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[TaskType] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Vba32Version] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Status] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[InstallationDate] smalldatetime NOT NULL,
			[ExitCode] smallint,
			[Error] ntext
		)
	
		INSERT INTO @TasksPage(
			[ID], [ComputerName], [IPAddress], [TaskType], [Vba32Version], [Status], [InstallationDate], [ExitCode], [Error])
		SELECT
			t.[ID], t.[ComputerName], t.[IPAddress], tt.[TaskType], v.[Vba32Version], s.[Status], t.[InstallationDate], t.[ExitCode], t.[Error]
		FROM InstallationTasks AS t
		INNER JOIN InstallationStatus AS s ON t.[StatusID] = s.[ID]
		INNER JOIN Vba32Versions AS v ON t.[Vba32VersionID] = v.[ID]
		INNER JOIN InstallationTaskType AS tt ON t.[TaskTypeID] = tt.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [TaskType], [Vba32Version], [Status], [InstallationDate], [ExitCode], [Error]
		FROM @TasksPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query