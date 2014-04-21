CREATE PROCEDURE [dbo].[GetComputersWithoutPolicyPageCount]
WITH ENCRYPTION
AS
	SELECT COUNT(*) FROM [Computers] as c
	LEFT JOIN [Policies] as p ON c.ID = p.ComputerID
	WHERE p.PolicyID IS NULL