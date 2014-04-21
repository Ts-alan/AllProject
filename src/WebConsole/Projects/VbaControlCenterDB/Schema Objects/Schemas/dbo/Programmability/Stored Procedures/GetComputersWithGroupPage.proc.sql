CREATE PROCEDURE [dbo].[GetComputersWithGroupPage]
WITH ENCRYPTION
AS	
		SELECT	c.[ID], c.[ComputerName], g.[GroupID]
		FROM Computers as c
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]