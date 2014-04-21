CREATE PROCEDURE [dbo].[RemoveGroupByName]
	@GroupName nvarchar(128)
WITH ENCRYPTION
AS
	DECLARE @GroupID int
	SET @GroupID = (SELECT [ID] FROM GroupTypes WHERE [GroupName] = @GroupName);

	WITH x(ID) 
	AS (
		SELECT @GroupID FROM [GroupTypes]
		UNION ALL
		SELECT gt.ID
		FROM [GroupTypes] as gt
		INNER JOIN x ON gt.ParentID = x.ID
	)
	DELETE [GroupTypes]
	FROM x
	INNER JOIN [GroupTypes] ON [GroupTypes].ID = x.ID