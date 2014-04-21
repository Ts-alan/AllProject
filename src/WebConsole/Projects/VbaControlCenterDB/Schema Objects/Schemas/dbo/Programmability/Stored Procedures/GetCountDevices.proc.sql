CREATE PROCEDURE [dbo].[GetCountDevices]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @DevicesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] smallint,
			[SerialNo] nvarchar(256) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS
		)
	
		INSERT INTO @DevicesPage(
			[ID], [SerialNo], [Comment])

		SELECT
			d.[ID], d.[SerialNo], d.[Comment]
		FROM Devices AS d
		LEFT JOIN DevicesPolicies AS dp ON d.[ID] = dp.[DeviceID]
		LEFT JOIN DevicePolicyStates AS dps ON dps.[ID] = dp.[DevicePolicyStateID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N' GROUP BY d.[ID], d.[SerialNo], d.[Comment]'
	SET @Query = @Query + N';
		SELECT COUNT(*) FROM @DevicesPage'	

	EXEC sp_executesql @Query