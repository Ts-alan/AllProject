CREATE PROCEDURE [dbo].[GetComputersWithoutGroupPageCount]
WITH ENCRYPTION
AS
		SELECT COUNT(*) FROM [Computers] as c
		LEFT JOIN [Groups] as g ON c.ID = g.ComputerID
		WHERE g.GroupID IS NULL