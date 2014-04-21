CREATE PROCEDURE [dbo].[RemoveComputerFromGroup]
	@ComputerID smallint
WITH ENCRYPTION
AS
	DELETE FROM [Groups] WHERE ComputerID = @ComputerID