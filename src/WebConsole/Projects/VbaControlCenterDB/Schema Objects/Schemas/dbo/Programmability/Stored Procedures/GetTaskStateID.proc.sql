CREATE PROCEDURE [GetTaskStateID]
	@TaskState nvarchar(32),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a task state
		IF NOT EXISTS (SELECT [ID] FROM [TaskStates] WHERE [TaskState] = @TaskState)
			INSERT INTO [TaskStates]([TaskState]) VALUES (@TaskState);
	END
	RETURN (SELECT [ID] FROM [TaskStates] WHERE [TaskState] = @TaskState)