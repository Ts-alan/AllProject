CREATE PROCEDURE [dbo].[GetComputersByPolicy]
	@PolicyID smallint
WITH ENCRYPTION
AS
	SELECT p.[ComputerID], c.[ComputerName] FROM Policies as p
	INNER JOIN Computers AS c ON c.[ID] = p.[ComputerID]
	WHERE p.[PolicyID] = @PolicyID