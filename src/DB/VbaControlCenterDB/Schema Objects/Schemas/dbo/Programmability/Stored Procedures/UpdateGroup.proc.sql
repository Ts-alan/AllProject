CREATE PROCEDURE [dbo].[UpdateGroup]
	@GroupName nvarchar(128),
	@NewGroupName nvarchar(128) = NULL,
	@NewComment nvarchar(128) = NULL,
	@NewParentID int = NULL
WITH ENCRYPTION
AS
	IF (SELECT COUNT(*) FROM [GroupTypes] WHERE GroupName = @GroupName) = 1
	BEGIN
		IF @NewGroupName IS NOT NULL
		BEGIN
			UPDATE [GroupTypes]
			SET GroupName   = @NewGroupName
			WHERE GroupName = @GroupName
		END
		IF @NewComment IS NOT NULL
		BEGIN
			UPDATE [GroupTypes]
			SET GroupComment   = @NewComment
			WHERE GroupName = @GroupName
		END
		IF @NewParentID IS NOT NULL
		BEGIN
			IF @NewParentID = 0
			BEGIN
				SET @NewParentID = NULL
			END
			UPDATE [GroupTypes]
				SET ParentID   = @NewParentID
				WHERE GroupName = @GroupName
		END
	END