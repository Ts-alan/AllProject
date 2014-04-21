CREATE PROCEDURE [dbo].[GetComputer]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT
		c.[ID],
		c.[ComputerName],
		c.[IPAddress],
		c.[ControlCenter],
		c.[DomainName],
		c.[UserLogin],
		c.[OSTypeID],
		c.[RAM],
		c.[CPUClock],
		c.[RecentActive],
		c.[LatestUpdate],
		c.[Vba32Version],
		c.[LatestInfected],
		c.[LatestMalware],
		c.[Vba32Integrity],
		c.[Vba32KeyValid],
		c.[Description],
		o.[OSName],
		cdt.[ControlName]
	FROM [dbo].[Computers] AS c
	INNER JOIN OSTypes AS o ON o.[ID] = c.[OSTypeID]
	INNER JOIN ComputerAdditionalInfo AS cai ON c.[ID] = cai.[ComputerID]
	INNER JOIN ControlDeviceType AS cdt ON cdt.[ID] = cai.[ControlDeviceTypeID]
	WHERE c.[ID] = @ID