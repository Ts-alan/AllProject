CREATE PROCEDURE [dbo].[AddDevicePolicyToGroup]
	@GroupID int,
	@SerialNo nvarchar(256),
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DeviceClassMode WHERE [ModeName] = @StateName)

	DECLARE @DeviceID smallint
	SET @DeviceID = (SELECT [ID] FROM Devices WHERE [SerialNo] = @SerialNo)

	IF @DeviceID IS NOT NULL
	BEGIN
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
		LEFT JOIN DevicesPolicies AS dp ON dp.[ComputerID] = g.[ComputerID] AND dp.[DeviceID] = @DeviceID
		WHERE dp.[DeviceID] IS NULL

		IF NOT EXISTS(SELECT [ID] FROM @ComputerPage)
			RETURN

		INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
		SELECT [ID], @DeviceID, @StateID FROM @ComputerPage
		
		SELECT d.[ID], d.[SerialNo], d.[Comment], dt.TypeName FROM Devices AS d
		INNER JOIN DeviceTypes AS dt ON d.DeviceTypeID = dt.ID
		WHERE d.[ID]=@DeviceID
	END