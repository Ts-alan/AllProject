﻿CREATE PROCEDURE [dbo].[GetComputersExWithoutGroupPageWithFilter]
@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @TmpList TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[IPAddress] nvarchar(16) NOT NULL,
			[ControlCenter] bit NOT NULL,
			[DomainName] nvarchar(256) NULL,
			[UserLogin] nvarchar(256) NULL,
			[OSName] nvarchar(128) NOT NULL,
			[RAM] smallint NULL,
			[CPUClock] smallint NULL,
			[RecentActive] smalldatetime NOT NULL,
			[LatestUpdate] smalldatetime NULL,
			[Vba32Version] nvarchar(256) NULL,
			[LatestInfected] smalldatetime NULL,
			[LatestMalware] nvarchar(256) NULL,
			[Vba32Integrity] bit NULL,
			[Vba32KeyValid] bit NULL,
			[Description] nvarchar(64) NULL,
			[PolicyName] nvarchar(128) NULL
		)
	
		INSERT INTO @TmpList([ID], [ComputerName], [IPAddress], [ControlCenter],
				[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
				[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
				[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description], [PolicyName])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description], pt.[TypeName]		
		FROM Computers as c
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		LEFT JOIN PolicyTypes AS pt ON pt.[ID] = p.[PolicyID]
		WHERE g.[GroupID] IS NULL'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' AND ' + @Where
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
				[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
				[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
				[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description], [PolicyName]
		FROM @TmpList
		ORDER BY [ComputerName] ASC'
	EXEC sp_executesql @Query