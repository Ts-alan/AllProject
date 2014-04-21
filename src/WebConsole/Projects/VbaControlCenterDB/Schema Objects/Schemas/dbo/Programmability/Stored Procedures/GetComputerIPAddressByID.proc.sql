CREATE PROCEDURE [dbo].[GetComputerIPAddressByID]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT
		[IPAddress]
	FROM
		[dbo].[Computers]
	WHERE
		[ID] = @ID