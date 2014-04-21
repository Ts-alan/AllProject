CREATE PROCEDURE [dbo].[GetGroupToComputer]
	@ComputerName nvarchar(64)

WITH ENCRYPTION
AS
	SELECT gt.[ID], [GroupName], [GroupComment]
	FROM [GroupTypes] AS gt
	INNER JOIN [Groups] AS g ON g.[GroupID] = gt.[ID]
	INNER JOIN [Computers] AS c ON c.[ID] = g.[ComputerID]
	WHERE c.[ComputerName] = @ComputerName