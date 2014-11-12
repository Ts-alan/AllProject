CREATE PROCEDURE [dbo].[RemoveComputerFromAllPolicies]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM Computers WHERE [ComputerName] = @ComputerName)

	DELETE FROM [Policies] WHERE ComputerID = @ComputerID