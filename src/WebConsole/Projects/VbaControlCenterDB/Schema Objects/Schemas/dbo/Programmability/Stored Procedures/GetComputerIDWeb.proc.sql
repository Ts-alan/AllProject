CREATE PROCEDURE [dbo].[GetComputerIDWeb]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	SELECT [ID] FROM [dbo].[Computers] WHERE [ComputerName] = @ComputerName