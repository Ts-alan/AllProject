CREATE PROCEDURE [dbo].[GetComponentsPage]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @ComponentsPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ComponentName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ComponentState] nvarchar(32) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Version] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL,
			[SettingsName] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL
		)
	
		INSERT INTO @ComponentsPage(
			[ComputerName], [ComponentName], [ComponentState],
			[Version], [SettingsName])
		SELECT
			c.[ComputerName], cty.[ComponentName], cst.[ComponentState],
			cmnt.[Version], cse.[Name]
		FROM Components AS cmnt
		INNER JOIN Computers AS c ON cmnt.[ComputerID] = c.[ID]
		INNER JOIN ComponentStates AS cst ON cmnt.[StateID] = cst.[ID]
		INNER JOIN ComponentSettings AS cse ON cmnt.[SettingsID] = cse.[ID]
		INNER JOIN ComponentTypes AS cty ON cmnt.[ComponentID] = cty.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT
			[ComputerName], [ComponentName], [ComponentState], [Version], [SettingsName]
		FROM @ComponentsPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query