CREATE PROCEDURE [dbo].[DeleteEventType]
	@ID smallint
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[EventTypes]
	WHERE
		[ID] = @ID