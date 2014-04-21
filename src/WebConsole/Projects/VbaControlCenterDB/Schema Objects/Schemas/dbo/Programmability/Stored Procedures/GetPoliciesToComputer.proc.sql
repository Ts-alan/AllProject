CREATE PROCEDURE [dbo].[GetPoliciesToComputer]
	@ComputerName nvarchar(64)

WITH ENCRYPTION
AS
	SELECT pt.[ID], [TypeName], [Params], [Comment]
	FROM [PolicyTypes] AS pt
	INNER JOIN [Policies] AS p ON p.[PolicyID] = pt.[ID]
	INNER JOIN [Computers] AS c ON c.[ID] = p.[ComputerID]
	WHERE c.[ComputerName] = @ComputerName