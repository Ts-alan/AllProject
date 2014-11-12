CREATE PROCEDURE [ClearInstallationTasks]
	@ComputerName nvarchar(64) = NULL,
	@IPAddress nvarchar(16) = NULL
WITH ENCRYPTION
AS
	DELETE FROM InstallationTasks
	WHERE [ComputerName] = @ComputerName OR [IPAddress] = @IPAddress