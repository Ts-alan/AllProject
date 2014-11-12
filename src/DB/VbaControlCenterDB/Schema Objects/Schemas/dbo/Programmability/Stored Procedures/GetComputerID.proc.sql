CREATE PROCEDURE [dbo].[GetComputerID]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	RETURN (SELECT [ID] FROM [dbo].[Computers] WHERE [ComputerName] = @ComputerName)