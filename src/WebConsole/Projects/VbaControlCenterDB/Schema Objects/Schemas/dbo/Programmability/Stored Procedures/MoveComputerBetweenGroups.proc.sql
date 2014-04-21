CREATE PROCEDURE [dbo].[MoveComputerBetweenGroups]
	@ComputerID smallint,
	@GroupID int
WITH ENCRYPTION
AS
	UPDATE [Groups]
	SET GroupID = @GroupID
	WHERE ComputerID = @ComputerID