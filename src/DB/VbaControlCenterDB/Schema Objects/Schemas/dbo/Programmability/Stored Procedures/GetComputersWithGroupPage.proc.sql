CREATE PROCEDURE [dbo].[GetComputersWithGroupPage]
WITH ENCRYPTION
AS	
		SELECT	[ID], [ComputerName], [GroupID] FROM ComputersExtended