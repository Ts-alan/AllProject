CREATE PROCEDURE [GetPolicyID]
	@TypeName nvarchar(128),
	@Params ntext,
	@Comment nvarchar(128),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such an device
		IF NOT EXISTS (SELECT [ID] FROM [PolicyTypes] WHERE [TypeName] = @TypeName)
			INSERT INTO [PolicyTypes](TypeName, Params, Comment) VALUES (@TypeName, @Params, @Comment);
	END
	SELECT [ID] FROM [PolicyTypes] WHERE [TypeName] = @TypeName