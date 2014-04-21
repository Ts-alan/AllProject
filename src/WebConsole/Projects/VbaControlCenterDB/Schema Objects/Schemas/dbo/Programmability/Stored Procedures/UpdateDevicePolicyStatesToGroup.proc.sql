CREATE PROCEDURE [dbo].[UpdateDevicePolicyStatesToGroup]
	@DeviceID smallint,
	@GroupID int,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)

	DECLARE @GroupPage TABLE(
				[RecID] int IDENTITY(1, 1) NOT NULL,									
				[ID] int
			);

	WITH TreeGroups AS
	(
		SELECT [ID], [ParentID] FROM GroupTypes
		WHERE [ID] = @GroupID

		UNION ALL

		SELECT gt.[ID], gt.[ParentID] FROM GroupTypes AS gt
		INNER JOIN TreeGroups AS tg ON tg.[ID] = gt.[ParentID]
	)
	INSERT INTO @GroupPage([ID])
	SELECT [ID] FROM TreeGroups

	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @StateID
	WHERE	[DeviceID] = @DeviceID AND 
			[ComputerID] IN (
					SELECT [ComputerID] FROM Groups
					WHERE [GroupID] IN (
								SELECT [ID] FROM @GroupPage
					)
			)