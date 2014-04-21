CREATE PROCEDURE [dbo].[UpdateEventColor]
	@ID smallint,
	@Color nvarchar(32)
WITH ENCRYPTION
AS
	UPDATE [EventTypes]
	SET [Color] = @Color
	WHERE [ID] = @ID