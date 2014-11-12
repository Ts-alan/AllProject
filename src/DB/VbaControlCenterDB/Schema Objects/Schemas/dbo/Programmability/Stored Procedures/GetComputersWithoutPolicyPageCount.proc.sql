CREATE PROCEDURE [dbo].[GetComputersWithoutPolicyPageCount]
WITH ENCRYPTION
AS
	SELECT COUNT(ID) FROM ComputersExtended	
	WHERE PolicyID IS NULL