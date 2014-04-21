CREATE PROCEDURE [dbo].[CreateTask]
	@ComputerName nvarchar(64),
	@TaskName nvarchar(64),
	@TaskParams ntext,
	@TaskUser nvarchar(128)
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID = 0
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END

	-- Retrieving TaskNameID
	DECLARE @TaskNameID smallint
	EXEC @TaskNameID = dbo.GetTaskTypeID @TaskName, 1
	
	-- Retrieving TaskStateID
	DECLARE @TaskStateID smallint
	EXEC @TaskStateID = dbo.GetTaskStateID N'Delivery', 1
	
	-- Inserting data
	INSERT INTO [Tasks](TaskID, ComputerID, StateID, DateIssued, DateUpdated, TaskParams, TaskUser)
		VALUES(@TaskNameID, @ComputerID, @TaskStateID, GETDATE(), GETDATE(), @TaskParams, @TaskUser)
	
	SELECT SCOPE_IDENTITY()