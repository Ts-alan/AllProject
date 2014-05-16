CREATE PROCEDURE [dbo].[GetDeviceClassCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] smallint
		)
	
		INSERT INTO @DevicesPage([ID])
		SELECT d.[ID]
		FROM DeviceClass AS d
		LEFT JOIN DeviceClassPolicy AS dp ON d.[ID] = dp.[DeviceClassID]
		LEFT JOIN DeviceClassMode AS dcm ON dcm.[ID] = dp.[DeviceClassModeID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N' GROUP BY d.[ID]'
	SET @Query = @Query + N';
		SELECT COUNT(*) FROM @DevicesPage'	

	EXEC sp_executesql @Query