CREATE PROCEDURE [dbo].[GetComputersByPolicy]
	@PolicyID smallint
WITH ENCRYPTION
AS
	SELECT * FROM ComputersExtended	
	WHERE PolicyID = @PolicyID