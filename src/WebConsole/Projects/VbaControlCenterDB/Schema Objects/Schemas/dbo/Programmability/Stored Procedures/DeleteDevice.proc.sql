CREATE PROCEDURE [dbo].[DeleteDevice]
	@ID smallint
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[Devices]
	WHERE
		[ID] = @ID