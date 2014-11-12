CREATE PROCEDURE [dbo].[DeletePolicy]
	@ID smallint
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[PolicyTypes]
	WHERE
		[ID] = @ID