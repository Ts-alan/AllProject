CREATE PROCEDURE [GetComponentTypeID](
	@ComponentName nvarchar(64),
	@InsertIfNotExists tinyint = 0)
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a component type
		IF NOT EXISTS (SELECT [ID] FROM [ComponentTypes] WHERE [ComponentName] = @ComponentName)
			INSERT INTO [ComponentTypes](ComponentName) VALUES (@ComponentName);
	END
	RETURN (SELECT [ID] FROM [ComponentTypes] WHERE [ComponentName] = @ComponentName)
END