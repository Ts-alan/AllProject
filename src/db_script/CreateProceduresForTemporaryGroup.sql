-- Processes
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerNameListFromProcesses]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerNameListFromProcesses]
GO

CREATE PROCEDURE [dbo].[GetComputerNameListFromProcesses]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Processes AS p
		INNER JOIN Computers AS c ON p.[ComputerID] = c.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query
GO

-- Events
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerNameListFromEvents]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerNameListFromEvents]
GO

CREATE PROCEDURE [dbo].[GetComputerNameListFromEvents]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Events AS e
        INNER JOIN Computers AS c ON c.[ID] = e.[ComputerID]
        INNER JOIN EventTypes AS et ON e.[EventID] = et.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query
GO


--Tasks
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerNameListFromTasks]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerNameListFromTasks]
GO

CREATE PROCEDURE [dbo].[GetComputerNameListFromTasks]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Tasks AS t
        INNER JOIN Computers AS c ON c.[ID] = t.[ComputerID] 
        INNER JOIN TaskStates AS ts ON t.[StateID] = ts.[ID]
        INNER JOIN TaskTypes AS tt ON t.[TaskID] = tt.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query
GO


--Components
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerNameListFromComponents]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerNameListFromComponents]
GO

CREATE PROCEDURE [dbo].[GetComputerNameListFromComponents]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Components AS cm
        INNER JOIN Computers AS c ON c.[ID] = cm.[ComputerID]
        INNER JOIN ComponentSettings AS cms ON cm.[SettingsID] = cms.[ID]
        INNER JOIN ComponentStates AS cmst ON cm.[StateID] = cmst.[ID]
        INNER JOIN ComponentTypes AS cmt ON cm.[ComponentID] = cmt.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query
GO


--Computers
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerNameListFromComputers]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerNameListFromComputers]
GO

CREATE PROCEDURE [dbo].[GetComputerNameListFromComputers]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT c.[ComputerName] FROM Computers AS c
        INNER JOIN OSTypes AS os ON c.[OSTypeID] = os.[ID]'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	SET @Query = @Query + N' GROUP BY c.[ComputerName]'

	EXEC sp_executesql @Query
GO