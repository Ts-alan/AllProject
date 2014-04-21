CREATE PROCEDURE [DeleteProcessInfo]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID IS NULL
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END
	
	DELETE FROM [Processes] WHERE [ComputerID] = @ComputerID