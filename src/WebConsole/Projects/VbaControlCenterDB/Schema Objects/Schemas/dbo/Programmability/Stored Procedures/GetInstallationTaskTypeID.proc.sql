CREATE PROCEDURE [GetInstallationTaskTypeID]
	@TaskType nvarchar(64)
WITH ENCRYPTION
AS
	RETURN (SELECT [ID] FROM [InstallationTaskType] WHERE [TaskType] = @TaskType)