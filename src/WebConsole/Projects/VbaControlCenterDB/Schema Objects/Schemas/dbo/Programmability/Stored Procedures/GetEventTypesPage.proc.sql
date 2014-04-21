CREATE PROCEDURE [dbo].[GetEventTypesPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @EventTypesPage TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL,
			[EventName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Color] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL,
			[Send] bit NOT NULL,
			[NoDelete] bit NOT NULL,
			[Notify] bit NOT NULL
		)
	
		INSERT INTO @EventTypesPage(
			[ID], [EventName], [Color], [Send], [NoDelete], [Notify])
		SELECT
			[ID], [EventName], [Color], [Send], [NoDelete], [Notify]
		FROM EventTypes'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [EventName], [Color], [Send], [NoDelete], [Notify]
		FROM @EventTypesPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query