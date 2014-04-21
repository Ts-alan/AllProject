CREATE PROCEDURE [GetTaskTypeID]
	@TaskName nvarchar(64),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a task type
		IF NOT EXISTS (SELECT [ID] FROM [TaskTypes] WHERE [TaskName] = @TaskName)
			INSERT INTO [TaskTypes]([TaskName]) VALUES (@TaskName);
	END
	RETURN (SELECT [ID] FROM [TaskTypes] WHERE [TaskName] = @TaskName)