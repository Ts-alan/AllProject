CREATE PROCEDURE [dbo].[GetDevicesPage]
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
			[SerialNo] nvarchar(1024) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Comment] nvarchar(256) COLLATE Cyrillic_General_CI_AS
		)
	
		INSERT INTO @DevicesPage(
			[ID], [SerialNo], [Comment])

		SELECT
			d.[ID], d.[SerialNo], d.[Comment]
		FROM Devices AS d
		INNER JOIN DeviceTypes AS dt ON d.[DeviceTypeID] = dt.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where	
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [SerialNo], [Comment]
		FROM @DevicesPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'	

	EXEC sp_executesql @Query