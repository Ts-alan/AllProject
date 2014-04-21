CREATE PROCEDURE [dbo].[UpdateEventNotify]
	@ID smallint,
	@Notify bit
WITH ENCRYPTION
AS
	UPDATE [EventTypes]
	SET [Notify] = @Notify 
	WHERE [ID] = @ID