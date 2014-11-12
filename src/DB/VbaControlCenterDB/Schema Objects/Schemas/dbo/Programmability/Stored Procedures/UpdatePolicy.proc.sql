CREATE PROCEDURE [dbo].[UpdatePolicy]
	@TypeName nvarchar(128),
	@Params ntext,
	@Comment nvarchar(128)
WITH ENCRYPTION
AS
	DECLARE @ID smallint
	SET @ID = (SELECT [ID] FROM PolicyTypes WHERE [TypeName] = @TypeName)
	
	UPDATE [PolicyTypes]
	SET    [TypeName] = @TypeName, [Params] = @Params, [Comment] = @Comment 
	WHERE [ID] = @ID