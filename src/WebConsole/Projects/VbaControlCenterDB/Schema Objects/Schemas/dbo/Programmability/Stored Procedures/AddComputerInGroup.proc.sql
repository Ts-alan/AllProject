CREATE PROCEDURE [dbo].[AddComputerInGroup]
	@ComputerID smallint,
	@GroupID int

WITH ENCRYPTION
AS
	INSERT INTO [Groups] (ComputerID, GroupID)
	VALUES (@ComputerID, @GroupID)