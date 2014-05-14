CREATE PROCEDURE [InsertInstallationTask]
	@ComputerName nvarchar(64),
	@IPAddress nvarchar(16),
	@Status nvarchar(64),
	@Date datetime,
	@Exitcode smallint
WITH ENCRYPTION
AS
	-- Retrieving StatusID
	DECLARE @StatusID smallint
	EXEC @StatusID = dbo.GetStatusID @Status
	
	INSERT INTO [InstallationTasks]
	([ComputerName], [IPAddress], [StatusID], [InstallationDate], [ExitCode])
	VALUES
	(@ComputerName,	@IPAddress, @StatusID,	@Date, @Exitcode)

	SELECT SCOPE_IDENTITY()