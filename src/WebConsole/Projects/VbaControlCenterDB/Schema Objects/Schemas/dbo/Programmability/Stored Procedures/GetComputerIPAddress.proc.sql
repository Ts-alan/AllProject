CREATE PROCEDURE [dbo].[GetComputerIPAddress]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	SELECT
		[IPAddress]
	FROM
		[dbo].[Computers]
	WHERE
		[ComputerName] = @ComputerName