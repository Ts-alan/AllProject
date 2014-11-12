CREATE PROCEDURE [dbo].[UpdateEventSend]
	@ID smallint,
	@Send bit
WITH ENCRYPTION
AS
	UPDATE [EventTypes]
	SET [Send] = @Send 
	WHERE [ID] = @ID