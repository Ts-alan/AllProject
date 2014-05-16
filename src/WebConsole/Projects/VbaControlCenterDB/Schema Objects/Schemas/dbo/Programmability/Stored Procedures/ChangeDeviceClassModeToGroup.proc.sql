CREATE PROCEDURE [dbo].[ChangeDeviceClassModeToGroup]
	@GroupID int,
	@DeviceClassID smallint,
	@DeviceClassMode nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @ComputerPage TABLE(
				[RecID] int IDENTITY(1, 1) NOT NULL,									
				[ID] smallint
			);

	WITH TreeGroups AS
	(
		SELECT [ID], [ParentID] FROM GroupTypes
		WHERE [ID] = @GroupID

		UNION ALL

		SELECT gt.[ID], gt.[ParentID] FROM GroupTypes AS gt
		INNER JOIN TreeGroups AS tg ON tg.[ID] = gt.[ParentID]
	)
	INSERT INTO @ComputerPage([ID])
	SELECT g.[ComputerID] FROM Groups AS g
	INNER JOIN TreeGroups AS tg ON tg.[ID] = g.[GroupID]
	
	DECLARE @ComputerID smallint
	DECLARE cur CURSOR LOCAL FOR
   	SELECT [ID] FROM @ComputerPage

	OPEN cur

	FETCH NEXT FROM cur INTO @ComputerID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC	[dbo].[ChangeModeToDeviceClassPolicy] @ComputerID, @DeviceClassID, @DeviceClassMode
		FETCH NEXT FROM cur INTO @ComputerID
	END

	CLOSE cur
	DEALLOCATE cur