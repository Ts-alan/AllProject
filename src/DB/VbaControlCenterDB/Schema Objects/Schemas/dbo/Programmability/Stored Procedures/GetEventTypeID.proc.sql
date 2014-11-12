CREATE PROCEDURE [GetEventTypeID]
	@EventName nvarchar(128),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such an event type
		IF NOT EXISTS (SELECT [ID] FROM [EventTypes] WHERE [EventName] = @EventName)
			INSERT INTO [EventTypes](EventName) VALUES (@EventName);
	END
	RETURN (SELECT [ID] FROM [EventTypes] WHERE [EventName] = @EventName)