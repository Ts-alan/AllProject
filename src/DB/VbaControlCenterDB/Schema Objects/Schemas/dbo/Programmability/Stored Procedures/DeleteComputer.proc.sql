CREATE PROCEDURE [dbo].[DeleteComputer]
	@ID smallint
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[Computers]
	WHERE [ID] = @ID