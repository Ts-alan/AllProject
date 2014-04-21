CREATE PROCEDURE [dbo].[UpdateComputerDescription]
	@ID smallint,
	@Description nvarchar(64)
WITH ENCRYPTION
AS
	UPDATE [Computers]
	SET [Description] = @Description
	WHERE [ID] = @ID