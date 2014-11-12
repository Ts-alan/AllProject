CREATE PROCEDURE [UpdateInstallationTask]
	@ID int,
	@Status nvarchar(64),
	@Exitcode smallint,
	@Error ntext
WITH ENCRYPTION
AS
	-- Retrieving StatusID
	DECLARE @StatusID smallint
	EXEC @StatusID = dbo.GetStatusID @Status

	
	UPDATE [InstallationTasks]
	SET 	[StatusID] = @StatusID,
		[ExitCode] = @Exitcode,
		[Error] = @Error
	WHERE [ID] = @ID