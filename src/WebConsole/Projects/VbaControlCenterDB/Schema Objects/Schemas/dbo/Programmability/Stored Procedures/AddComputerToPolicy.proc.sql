CREATE PROCEDURE [dbo].[AddComputerToPolicy]
	@ComputerName nvarchar(64),
	@PolicyID smallint

WITH ENCRYPTION
AS
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM Computers WHERE [ComputerName] = @ComputerName)

	INSERT INTO [Policies](ComputerID, PolicyID) VALUES (@ComputerID, @PolicyID);