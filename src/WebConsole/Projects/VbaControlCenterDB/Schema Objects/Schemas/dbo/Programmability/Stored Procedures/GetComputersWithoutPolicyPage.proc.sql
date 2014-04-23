﻿CREATE PROCEDURE [dbo].[GetComputersWithoutPolicyPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL
WITH ENCRYPTION
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersWithoutPolicyPage TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[IPAddress] nvarchar(16) NOT NULL,
			[ControlCenter] bit NOT NULL,
			[DomainName] nvarchar(256) NULL,
			[UserLogin] nvarchar(256) NULL,
			[OSName] nvarchar(128) NULL,
			[RAM] smallint NULL,
			[CPUClock] smallint NULL,
			[RecentActive] smalldatetime NOT NULL,
			[LatestUpdate] smalldatetime NULL,
			[Vba32Version] nvarchar(256) NULL,
			[LatestInfected] smalldatetime NULL,
			[LatestMalware] nvarchar(256) NULL,
			[Vba32Integrity] bit NULL,
			[Vba32KeyValid] bit NULL,
			[Description] nvarchar(64) NULL
		)
	
		INSERT INTO @ComputersWithoutPolicyPage(
			[ID], [ComputerName], [IPAddress], [ControlCenter],
			[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
			[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Computers as c
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		WHERE p.PolicyID IS NULL'
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
			   [DomainName], [UserLogin], [OSName], [RAM], [CPUClock], 
			   [RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			   [LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description]
		FROM @ComputersWithoutPolicyPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query