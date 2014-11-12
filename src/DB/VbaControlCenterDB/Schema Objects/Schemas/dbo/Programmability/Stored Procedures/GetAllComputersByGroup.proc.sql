CREATE PROCEDURE [dbo].[GetAllComputersByGroup]
	@GroupID int
WITH ENCRYPTION
AS

WITH TreeGroups AS
(
	SELECT [ID], [ParentID] FROM GroupTypes
	WHERE [ID] = @GroupID

	UNION ALL

	SELECT gt.[ID], gt.[ParentID] FROM GroupTypes AS gt
	INNER JOIN TreeGroups AS tg ON tg.[ID] = gt.[ParentID]
)

SELECT c.[ComputerName] FROM Groups AS g
INNER JOIN TreeGroups AS tg ON tg.[ID] = g.[GroupID]
INNER JOIN Computers AS c ON c.[ID] = g.[ComputerID]