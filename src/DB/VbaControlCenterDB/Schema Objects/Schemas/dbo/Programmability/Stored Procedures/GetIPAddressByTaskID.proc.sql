CREATE PROCEDURE [GetIPAddressByTaskID]
	@ID bigint
WITH ENCRYPTION
AS
	SELECT
		c.[IPAddress]
	FROM
		[Computers] AS c INNER JOIN [Tasks] AS t ON c.ID = t.ComputerID
	WHERE t.[ID] = @ID