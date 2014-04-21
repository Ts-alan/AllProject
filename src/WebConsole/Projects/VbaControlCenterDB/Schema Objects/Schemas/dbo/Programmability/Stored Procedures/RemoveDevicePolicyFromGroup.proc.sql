CREATE PROCEDURE [dbo].[RemoveDevicePolicyFromGroup]
	@GroupID int,
	@DeviceID smallint
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

	DELETE FROM DevicesPolicies 
	WHERE [DeviceID] = @DeviceID AND ([ComputerID] IN (SELECT [ID] FROM @ComputerPage))