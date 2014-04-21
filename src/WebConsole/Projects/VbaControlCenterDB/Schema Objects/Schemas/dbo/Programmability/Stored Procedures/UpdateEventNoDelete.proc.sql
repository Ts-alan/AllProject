CREATE PROCEDURE [dbo].[UpdateEventNoDelete]
	@ID smallint,
	@NoDelete bit
WITH ENCRYPTION
AS
	UPDATE [EventTypes]
	SET [NoDelete] = @NoDelete 
	WHERE [ID] = @ID