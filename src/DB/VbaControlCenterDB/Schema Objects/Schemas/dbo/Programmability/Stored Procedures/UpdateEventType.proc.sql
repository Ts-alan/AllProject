CREATE PROCEDURE [dbo].[UpdateEventType]
	@ID smallint, 
	@EventName nvarchar(128), 
	@Color nvarchar(32) 
WITH ENCRYPTION
AS
	UPDATE [dbo].[EventTypes]
	SET
		[EventName] = @EventName,
		[Color] = @Color
	WHERE
		[ID] = @ID