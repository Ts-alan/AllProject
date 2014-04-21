CREATE PROCEDURE [GetComputerNamesAndIP]
WITH ENCRYPTION
AS
	SELECT [ComputerName], [IPAddress] FROM [Computers]