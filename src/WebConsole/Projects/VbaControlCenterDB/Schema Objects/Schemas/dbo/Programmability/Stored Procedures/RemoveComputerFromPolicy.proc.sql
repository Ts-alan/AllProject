CREATE PROCEDURE [dbo].[RemoveComputerFromPolicy]
	@ComputerName nvarchar(64),
	@PolicyID smallint
WITH ENCRYPTION
AS
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM Computers WHERE [ComputerName] = @ComputerName)

	DELETE FROM [Policies] WHERE ComputerID = @ComputerID AND PolicyID = @PolicyID