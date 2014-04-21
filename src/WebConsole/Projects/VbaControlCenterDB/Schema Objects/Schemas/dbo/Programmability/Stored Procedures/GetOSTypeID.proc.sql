CREATE PROCEDURE [GetOSTypeID](
	@OSName nvarchar(128),
	@InsertIfNotExists tinyint = 0)
WITH ENCRYPTION
AS
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a OS type
		IF NOT EXISTS (SELECT [ID] FROM [OSTypes] WHERE [OSName] = @OSName)
			INSERT INTO [OSTypes](OSName) VALUES (@OSName);
	END
	RETURN (SELECT [ID] FROM [OSTypes] WHERE [OSName] = @OSName)