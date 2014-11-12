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
			[Status] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[InstallationDate] smalldatetime NOT NULL,
			[ExitCode] smallint
		)
	
		INSERT INTO @TasksPage(
			[ID], [ComputerName], [IPAddress], [Status], [InstallationDate], [ExitCode])
		SELECT
			t.[ID], t.[ComputerName], t.[IPAddress], s.[Status], t.[InstallationDate], t.[ExitCode]
		FROM InstallationTasks AS t
		INNER JOIN InstallationStatus AS s ON t.[StatusID] = s.[ID]'

	SET @Query = @Query + N';
		SELECT COUNT(*)
		FROM @TasksPage'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	EXEC sp_executesql @Query