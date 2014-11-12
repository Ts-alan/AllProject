CREATE PROCEDURE [dbo].[GetPoliciesToComputer]
	@ComputerName nvarchar(64)

WITH ENCRYPTION
AS
	SELECT pt.[ID], [TypeName], [Params], [Comment]
	FROM ComputersExtended AS c
	INNER JOIN PolicyTypes AS pt ON pt.ID = c.PolicyID
	WHERE c.ComputerName = @ComputerName