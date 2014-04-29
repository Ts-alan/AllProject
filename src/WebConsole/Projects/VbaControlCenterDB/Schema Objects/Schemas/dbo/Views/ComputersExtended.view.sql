CREATE VIEW [dbo].[ComputersExtended]
	AS SELECT 
	c.ID,
	c.ComputerName,
	c.MACAddress,
	c.IPAddress,
	c.ControlCenter,
	c.DomainName,
	c.UserLogin,
	c.OSTypeID,
	c.RAM,
	c.CPUClock,
	c.RecentActive,
	c.LatestUpdate,
	c.Vba32Version,
	c.LatestInfected,
	c.LatestMalware,
	c.Vba32Integrity,
	c.Vba32KeyValid,
	c.[Description],
	o.OSName,
	cai.IsControllable,
	cai.IsRenamed,
	cai.PreviousComputerName,	
	cdt.ControlName,
	gt.ID AS GroupID,
	gt.GroupName,
	pt.ID AS PolicyID,
	pt.TypeName AS PolicyName
	FROM [Computers] AS c
	INNER JOIN OSTypes AS o ON o.ID = c.OSTypeID
	INNER JOIN ComputerAdditionalInfo AS cai ON c.ID = cai.ComputerID
	INNER JOIN ControlDeviceType AS cdt ON cdt.ID = cai.ControlDeviceTypeID
	LEFT JOIN Groups AS g ON c.ID = g.ComputerID
	LEFT JOIN GroupTypes AS gt ON gt.ID = g.GroupID
	LEFT JOIN Policies AS p ON p.ComputerID = c.ID
	LEFT JOIN PolicyTypes AS pt ON pt.ID = p.PolicyID