CREATE PROCEDURE [dbo].[GetComputersWithoutGroupPage]
WITH ENCRYPTION
AS	
		SELECT * FROM ComputersExtended
		WHERE GroupID IS NULL	
		ORDER BY ComputerName ASC