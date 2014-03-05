IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComponentsPageByComputerID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComponentsPageByComputerID]
GO

CREATE PROCEDURE [dbo].[GetComponentsPageByComputerID]
	@ID smallint
WITH ENCRYPTION
AS
SELECT comps.[ComputerName],ct.[ComponentName],cs.[ComponentState],c.[Version],cset.[Name]  
		FROM Components AS c 
			INNER JOIN ComponentStates as cs ON c.[ComponentID] = cs.[ID]
			INNER JOIN Computers as comps ON comps.[ID] = c.[ComputerID]
			INNER JOIN ComponentSettings AS cset ON cset.[ID] = c.[SettingsID]
			INNER JOIN ComponentTypes AS ct ON ct.[ID] = c.[ComponentID]
		 WHERE c.[ComputerID]=@ID;
GO

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerEx]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerEx]
GO

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
GO

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetSelectionComputerForTask]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetSelectionComputerForTask]
GO

CREATE PROCEDURE [dbo].[GetSelectionComputerForTask]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersPage TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[IPAddress] nvarchar(16) NOT NULL			
		)
	
		INSERT INTO @ComputersPage([ComputerName], [IPAddress])
				SELECT	 c.[ComputerName], c.[IPAddress] 
				FROM Computers AS c'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N';
		SELECT [ComputerName], [IPAddress]
		FROM @ComputersPage '
	EXEC sp_executesql @Query
GO