CREATE PROCEDURE [dbo].[UpdateDevice]
	@ID smallint,
	@Comment nvarchar(256)
WITH ENCRYPTION
AS
	UPDATE [Devices]
	SET    [Comment] = @Comment
	WHERE [ID] = @ID