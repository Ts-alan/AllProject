CREATE PROCEDURE [dbo].[AddGroup]
	@GroupName nvarchar(128),
	@Comment nvarchar(128) = NULL,
	@ParentID int = NULL

WITH ENCRYPTION
AS
	IF (SELECT COUNT(*) FROM [GroupTypes] WHERE GroupName = @GroupName) = 0
	BEGIN
		INSERT INTO [GroupTypes] (GroupName, GroupComment, ParentID)
		VALUES (@GroupName, @Comment, @ParentID)

		SELECT SCOPE_IDENTITY()
	END
	ELSE 
		SELECT [ID] FROM GroupTypes WHERE GroupName = @GroupName