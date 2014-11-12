CREATE PROCEDURE [dbo].[GetDeviceClassListByFilter]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] smallint,
			[UID] nvarchar(38) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Class] nvarchar(128) COLLATE Cyrillic_General_CI_AS,
			[ClassName] nvarchar(128) COLLATE Cyrillic_General_CI_AS
		)
	
		INSERT INTO @DevicesPage([ID], [UID], [Class], [ClassName])
		SELECT
			d.[ID], d.[UID], d.[Class], d.[ClassName]
		FROM DeviceClass AS d
		LEFT JOIN DeviceClassPolicy AS dp ON d.[ID] = dp.[DeviceClassID]
		LEFT JOIN DeviceClassMode AS dcm ON dcm.[ID] = dp.[DeviceClassModeID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N' GROUP BY d.[ID], d.[UID], d.[Class], d.[ClassName]'
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [UID], [Class], [ClassName]
		FROM @DevicesPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'	

	EXEC sp_executesql @Query