CREATE PROCEDURE [dbo].[GetEventsPage]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @EventsPage TABLE(
			[ID] int IDENTITY(1, 1) NOT NULL,
			[EventTime] datetime NOT NULL,
			[Object] nvarchar(260) COLLATE Cyrillic_General_CI_AS NULL,
			[Comment] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL,
			[Color] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[EventName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ComponentName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Description] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL
		)
	
        DECLARE @QuerySize int
        SET @QuerySize = @Page * @RowCount
        SET ROWCOUNT @QuerySize
		INSERT INTO @EventsPage(
			[ComputerName], [EventName], [Color],
			[ComponentName], [EventTime], [Object], [Comment], [IPAddress], [Description])
		SELECT c.[ComputerName], et.[EventName], et.[Color],
			   ct.[ComponentName], e.[EventTime], e.[Object], e.[Comment], c.[IPAddress], c.[Description]
		FROM Events AS e
		INNER JOIN Computers AS c ON e.[ComputerID] = c.[ID]
		INNER JOIN ComponentTypes AS ct ON e.[ComponentID] = ct.[ID]
		INNER JOIN EventTypes AS et ON e.[EventID] = et.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N'
        SET ROWCOUNT 0
		SELECT [ComputerName], [EventName], [Color],
			   [ComponentName], [EventTime], [Object], [Comment], [IPAddress], [Description]
		FROM @EventsPage WHERE [ID] BETWEEN (@RowCount * (@Page - 1) + 1) AND
        (@RowCount * @Page)'
	DECLARE @Params nvarchar(128)
	SET @Params = N'@Page int, @RowCount int'
	EXEC sp_executesql @Query, @Params, @Page, @RowCount