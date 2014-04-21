CREATE PROCEDURE [dbo].[DeleteEvent]
	@ID int
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[Events]
	WHERE
		[ID] = @ID