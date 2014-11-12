CREATE PROCEDURE [dbo].[GetComputer]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT * FROM [dbo].[ComputersExtended]
	WHERE [ID] = @ID