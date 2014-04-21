CREATE PROCEDURE [InsertInstallationTask]
	@ComputerName nvarchar(64),
	@IPAddress nvarchar(16),
	@TaskType nvarchar(64),
	@Vba32Version nvarchar(64),
	@Status nvarchar(64),
	@Date datetime,
	@Exitcode smallint
WITH ENCRYPTION
AS
	-- Retrieving TaskTypeID
	DECLARE @TaskTypeID smallint
	EXEC @TaskTypeID = dbo.GetInstallationTaskTypeID @TaskType

	-- Retrieving StatusID
	DECLARE @StatusID smallint
	EXEC @StatusID = dbo.GetStatusID @Status

	-- Retrieving Vba32VersionID
	DECLARE @Vba32VersionID smallint
	EXEC @Vba32VersionID = dbo.GetVba32VersionID @Vba32Version
	
	INSERT INTO [InstallationTasks]
	([ComputerName], [IPAddress], [TaskTypeID], [Vba32VersionID], [StatusID], [InstallationDate], [ExitCode])
	VALUES
	(@ComputerName,	@IPAddress, @TaskTypeID, @Vba32VersionID, @StatusID,	@Date, @Exitcode)

	SELECT SCOPE_IDENTITY()