CREATE PROCEDURE [dbo].[GetComputersWithoutGroupPageCount]
WITH ENCRYPTION
AS
		SELECT COUNT(ID) FROM [ComputersExtended]
		WHERE GroupID IS NULL