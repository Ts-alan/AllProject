CREATE PROCEDURE [UpdateProcessInfo]
	@ComputerName nvarchar(64),
	@ProcessName nvarchar(260),
	@MemorySize int,
	@Date datetime
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
	-- Insert
	INSERT INTO [Processes](ComputerID, ProcessName, MemorySize, LastDate)
		VALUES(@ComputerID, @ProcessName, @MemorySize, @Date)