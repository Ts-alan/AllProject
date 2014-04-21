CREATE PROCEDURE [dbo].[GetComputersByGroupCount]
	@GroupID int
WITH ENCRYPTION
AS
	SELECT COUNT (*) FROM Groups AS g			
		WHERE g.[GroupID] = @GroupID