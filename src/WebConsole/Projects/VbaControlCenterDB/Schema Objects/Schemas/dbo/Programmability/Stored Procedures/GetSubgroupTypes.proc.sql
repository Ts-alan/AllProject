CREATE PROCEDURE [dbo].[GetSubgroupTypes]
	@ParentID int = NULL
WITH ENCRYPTION
AS
	IF @ParentID IS NULL
	BEGIN
		SELECT * FROM GroupTypes
		WHERE [ParentID] IS NULL
		ORDER BY [GroupName]
	END
	ELSE BEGIN
		SELECT * FROM GroupTypes
		WHERE [ParentID] = @ParentID
		ORDER BY [GroupName]
	END