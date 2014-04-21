CREATE PROCEDURE [AddEvent]
	@ComputerName nvarchar(64),
	@EventName nvarchar(128),
	@EventTime datetime,
	@ComponentName nvarchar(64),
	@Object nvarchar(260) = NULL,
	@Comment nvarchar(256) = NULL
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID = 0
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END

	-- Retrieving EventID
	DECLARE @EventID smallint
	EXEC @EventID = dbo.GetEventTypeID @EventName, 1

	-- Retrieving ComponentID
	DECLARE @ComponentID smallint
	EXEC @ComponentID = dbo.GetComponentTypeID @ComponentName, 1

	-- Inserting data
	INSERT INTO [Events](ComputerID, EventID, EventTime, ComponentID, Object, Comment)
		VALUES(@ComputerID, @EventID, @EventTime, @ComponentID, @Object, @Comment)

	-- Checking for events which require additional action
	-- Update
	IF (@EventName LIKE N'%update.success%')
		UPDATE [Computers]
		SET [LatestUpdate] = @EventTime
		WHERE [ID] = @ComputerID

	-- Virus
	IF (@EventName LIKE N'%virus.found')
		UPDATE [Computers]
		SET [LatestInfected] = @EventTime,
			[LatestMalware] = @Comment
		WHERE [ID] = @ComputerID

	-- Recent activity time
	UPDATE [Computers]
	SET [RecentActive] = GETDATE()
	WHERE [ID] = @ComputerID