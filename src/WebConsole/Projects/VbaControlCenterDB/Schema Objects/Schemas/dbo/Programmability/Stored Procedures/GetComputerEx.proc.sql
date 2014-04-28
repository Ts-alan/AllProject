CREATE PROCEDURE [dbo].[GetComputerEx]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT	* FROM ComputersExtended as c
	WHERE c.[ID] = @ID