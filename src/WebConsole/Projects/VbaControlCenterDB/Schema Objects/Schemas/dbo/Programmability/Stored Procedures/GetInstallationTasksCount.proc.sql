CREATE PROCEDURE [dbo].[GetInstallationTasksCount]
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
			[ExitCode] smallint
		)
	
		INSERT INTO @TasksPage(
			[ID], [ComputerName], [IPAddress], [TaskType], [Vba32Version], [Status], [InstallationDate], [ExitCode])
		SELECT
			t.[ID], t.[ComputerName], t.[IPAddress], tt.[TaskType], v.[Vba32Version], s.[Status], t.[InstallationDate], t.[ExitCode]
		FROM InstallationTasks AS t
		INNER JOIN InstallationStatus AS s ON t.[StatusID] = s.[ID]
		INNER JOIN Vba32Versions AS v ON t.[Vba32VersionID] = v.[ID]
		INNER JOIN InstallationTaskType AS tt ON t.[TaskTypeID] = tt.[ID]'

	SET @Query = @Query + N';
		SELECT COUNT(*)
		FROM @TasksPage'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	EXEC sp_executesql @Query