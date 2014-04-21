CREATE PROCEDURE [dbo].[GetComputerEx]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description], pt.[TypeName]		
		FROM Computers as c
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		LEFT JOIN PolicyTypes AS pt ON pt.[ID] = p.[PolicyID]
	WHERE c.[ID] = @ID