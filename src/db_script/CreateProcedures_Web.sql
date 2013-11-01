--------------------------------------------------
-- Stored procedures for Vba32 CC Web interface --
--------------------------------------------------


-- Returns Computer ID by its name (for web)
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerIDWeb]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerIDWeb]
GO

CREATE PROCEDURE [dbo].[GetComputerIDWeb]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	SELECT [ID] FROM [dbo].[Computers] WHERE [ComputerName] = @ComputerName
GO


-- Updates Description in Computers table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateComputerDescription]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateComputerDescription]
GO

CREATE PROCEDURE [dbo].[UpdateComputerDescription]
	@ID smallint,
	@Description nvarchar(64)
WITH ENCRYPTION
AS
	UPDATE [Computers]
	SET [Description] = @Description
	WHERE [ID] = @ID
GO


-- Updates Color in EventTypes table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateEventColor]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateEventColor]
GO

CREATE PROCEDURE [dbo].[UpdateEventColor]
	@ID smallint,
	@Color nvarchar(32)
WITH ENCRYPTION
AS
	UPDATE [EventTypes]
	SET [Color] = @Color
	WHERE [ID] = @ID
GO


-- Updates Send in EventTypes table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateEventSend]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateEventSend]
GO

CREATE PROCEDURE [dbo].[UpdateEventSend]
	@ID smallint,
	@Send bit
WITH ENCRYPTION
AS
	UPDATE [EventTypes]
	SET [Send] = @Send 
	WHERE [ID] = @ID
GO


-- Updates NoDelete in EventTypes table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateEventNoDelete]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateEventNoDelete]
GO

CREATE PROCEDURE [dbo].[UpdateEventNoDelete]
	@ID smallint,
	@NoDelete bit
WITH ENCRYPTION
AS
	UPDATE [EventTypes]
	SET [NoDelete] = @NoDelete 
	WHERE [ID] = @ID
GO


-- Updates NOtify in EventTypes table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateEventNotify]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateEventNotify]
GO

CREATE PROCEDURE [dbo].[UpdateEventNotify]
	@ID smallint,
	@Notify bit
WITH ENCRYPTION
AS
	UPDATE [EventTypes]
	SET [Notify] = @Notify 
	WHERE [ID] = @ID
GO

-- Returns a list of Task names
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetTaskTypesList]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTaskTypesList]
GO

CREATE PROCEDURE [GetTaskTypesList]
WITH ENCRYPTION
AS
	SELECT [ID],[TaskName] FROM [TaskTypes]
GO

-- Returns a list of Task states
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetTaskStatesList]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTaskStatesList]
GO

CREATE PROCEDURE [GetTaskStatesList]
WITH ENCRYPTION
AS
	SELECT [TaskState] FROM [TaskStates]
GO

-- Returns a list of Component states
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComponentStateList]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComponentStateList]
GO

CREATE PROCEDURE [GetComponentStateList]
WITH ENCRYPTION
AS
	SELECT [ComponentState] FROM [ComponentStates]
GO


-- Returns a list of Component names
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComponentTypeList]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComponentTypeList]
GO

CREATE PROCEDURE [GetComponentTypeList]
WITH ENCRYPTION
AS
	SELECT [ComponentName] FROM [ComponentTypes]
GO


-- Returns Task by its ID
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetTaskByID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTaskByID]
GO

CREATE PROCEDURE [GetTaskByID]
	@ID bigint
WITH ENCRYPTION
AS
	SELECT t.[ID], t.[ComputerID], t.[StateID], t.[DateComplete], t.[DateUpdated], t.[TaskParams], tt.[TaskName], t.[TaskUser] 
	FROM [Tasks] as t
	INNER JOIN [TaskTypes] as tt ON t.[TaskID] = tt.[ID]
	WHERE t.[ID] = @ID
GO


-- Returns IP address of computer which performs task with specified ID
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetIPAddressByTaskID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetIPAddressByTaskID]
GO

CREATE PROCEDURE [GetIPAddressByTaskID]
	@ID bigint
WITH ENCRYPTION
AS
	SELECT
		c.[IPAddress]
	FROM
		[Computers] AS c INNER JOIN [Tasks] AS t ON c.ID = t.ComputerID
	WHERE t.[ID] = @ID
GO


-- Adds information about a new task into the database
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[CreateTask]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[CreateTask]
GO

CREATE PROCEDURE [dbo].[CreateTask]
	@ComputerName nvarchar(64),
	@TaskName nvarchar(64),
	@TaskParams ntext,
	@TaskUser nvarchar(128)
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID = 0
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END

	-- Retrieving TaskNameID
	DECLARE @TaskNameID smallint
	EXEC @TaskNameID = dbo.GetTaskTypeID @TaskName, 1
	
	-- Retrieving TaskStateID
	DECLARE @TaskStateID smallint
	EXEC @TaskStateID = dbo.GetTaskStateID N'Delivery', 1
	
	-- Inserting data
	INSERT INTO [Tasks](TaskID, ComputerID, StateID, DateIssued, DateUpdated, TaskParams, TaskUser)
		VALUES(@TaskNameID, @ComputerID, @TaskStateID, GETDATE(), GETDATE(), @TaskParams, @TaskUser)
	
	SELECT SCOPE_IDENTITY()
GO


--------------------------
--	Paging procedures	--
--------------------------

-- Returns count of computers that match the criteria
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersCount]
GO

CREATE PROCEDURE [dbo].[GetComputersCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (c.[ID]) FROM Computers AS c
					INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query
GO


-- Returns a page from Computers table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersPage]
GO

CREATE PROCEDURE [dbo].[GetComputersPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersPage TABLE(
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
	
		INSERT INTO @ComputersPage(
			[ID], [ComputerName], [IPAddress], [ControlCenter],
			[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
			[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Computers AS c
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
			   [DomainName], [UserLogin], [OSName], [RAM], [CPUClock], 
			   [RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			   [LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description]
		FROM @ComputersPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO


-- Returns count of events that match the criteria
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventsCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventsCount]
GO

CREATE PROCEDURE [dbo].[GetEventsCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (e.[ID]) FROM Events AS e
					INNER JOIN Computers AS c ON e.[ComputerID] = c.[ID]
					INNER JOIN ComponentTypes AS ct ON e.[ComponentID] = ct.[ID]
					INNER JOIN EventTypes AS et ON e.[EventID] = et.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query
GO


-- Returns a page from Events table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventsPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventsPage]
GO

CREATE PROCEDURE [dbo].[GetEventsPage]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @EventsPage TABLE(
			[ID] int IDENTITY(1, 1) NOT NULL,
			[EventTime] datetime NOT NULL,
			[Object] nvarchar(260) COLLATE Cyrillic_General_CI_AS NULL,
			[Comment] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL,
			[Color] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[EventName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ComponentName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Description] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL
		)
	
        DECLARE @QuerySize int
        SET @QuerySize = @Page * @RowCount
        SET ROWCOUNT @QuerySize
		INSERT INTO @EventsPage(
			[ComputerName], [EventName], [Color],
			[ComponentName], [EventTime], [Object], [Comment], [IPAddress], [Description])
		SELECT c.[ComputerName], et.[EventName], et.[Color],
			   ct.[ComponentName], e.[EventTime], e.[Object], e.[Comment], c.[IPAddress], c.[Description]
		FROM Events AS e
		INNER JOIN Computers AS c ON e.[ComputerID] = c.[ID]
		INNER JOIN ComponentTypes AS ct ON e.[ComponentID] = ct.[ID]
		INNER JOIN EventTypes AS et ON e.[EventID] = et.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N'
        SET ROWCOUNT 0
		SELECT [ComputerName], [EventName], [Color],
			   [ComponentName], [EventTime], [Object], [Comment], [IPAddress], [Description]
		FROM @EventsPage WHERE [ID] BETWEEN (@RowCount * (@Page - 1) + 1) AND
        (@RowCount * @Page)'
	DECLARE @Params nvarchar(128)
	SET @Params = N'@Page int, @RowCount int'
	EXEC sp_executesql @Query, @Params, @Page, @RowCount
GO


-- Returns count of event types that match the criteria
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventTypesCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventTypesCount]
GO

CREATE PROCEDURE [dbo].[GetEventTypesCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT ([ID]) FROM EventTypes'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query
GO


-- Returns a page from EventTypes table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventTypesPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventTypesPage]
GO

CREATE PROCEDURE [dbo].[GetEventTypesPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @EventTypesPage TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL,
			[EventName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Color] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL,
			[Send] bit NOT NULL,
			[NoDelete] bit NOT NULL,
			[Notify] bit NOT NULL
		)
	
		INSERT INTO @EventTypesPage(
			[ID], [EventName], [Color], [Send], [NoDelete], [Notify])
		SELECT
			[ID], [EventName], [Color], [Send], [NoDelete], [Notify]
		FROM EventTypes'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [EventName], [Color], [Send], [NoDelete], [Notify]
		FROM @EventTypesPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO


-- Returns count of tasks that match the criteria
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetTasksCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTasksCount]
GO

CREATE PROCEDURE [dbo].[GetTasksCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (t.[ID]) FROM Tasks AS t
					INNER JOIN Computers AS c ON t.[ComputerID] = c.[ID]
					INNER JOIN TaskTypes AS tt ON t.[TaskID] = tt.[ID]
					INNER JOIN TaskStates AS ts ON t.[StateID] = ts.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query
GO


-- Returns a page from Tasks table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetTasksPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTasksPage]
GO

CREATE PROCEDURE [dbo].[GetTasksPage]
	@Page bigint,
	@RowCount bigint,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @TasksPage TABLE(
			[RecID] bigint IDENTITY(1, 1) NOT NULL,
			[ID] bigint NOT NULL,
			[TaskName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[TaskState] nvarchar(32) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[DateIssued] datetime NOT NULL,
			[DateComplete] datetime NULL,
			[DateUpdated] datetime NOT NULL,
			[TaskParams] ntext COLLATE Cyrillic_General_CI_AS NULL,
			[TaskUser] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[TaskDescription] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL
		)
	
		INSERT INTO @TasksPage(
			[ID], [TaskName], [ComputerName], [TaskState],
			[DateIssued], [DateComplete], [DateUpdated], [TaskParams], [TaskUser], [TaskDescription])
		SELECT
			t.[ID], tt.[TaskName], c.[ComputerName], ts.[TaskState],
			t.[DateIssued], t.[DateComplete], t.[DateUpdated], t.[TaskParams], t.[TaskUser], t.[TaskDescription]
		FROM Tasks AS t
		INNER JOIN Computers AS c ON t.[ComputerID] = c.[ID]
		INNER JOIN TaskTypes AS tt ON t.[TaskID] = tt.[ID]
		INNER JOIN TaskStates AS ts ON t.[StateID] = ts.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT  [ID], [TaskName], [ComputerName], [TaskState],
				[DateIssued], [DateComplete], [DateUpdated], [TaskParams], [TaskUser], [TaskDescription]
		FROM @TasksPage	WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO

-- Returns count of components that match the criteria
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComponentsCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComponentsCount]
GO

CREATE PROCEDURE [dbo].[GetComponentsCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (cmnt.[ComputerID]) FROM Components AS cmnt
					INNER JOIN Computers AS c ON cmnt.[ComputerID] = c.[ID]
					INNER JOIN ComponentStates AS cst ON cmnt.[StateID] = cst.[ID]
					INNER JOIN ComponentSettings AS cse ON cmnt.[SettingsID] = cse.[ID]
					INNER JOIN ComponentTypes AS cty ON cmnt.[ComponentID] = cty.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query
GO


-- Returns a page from Components table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComponentsPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComponentsPage]
GO

CREATE PROCEDURE [dbo].[GetComponentsPage]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @ComponentsPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ComponentName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ComponentState] nvarchar(32) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Version] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL,
			[SettingsName] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL
		)
	
		INSERT INTO @ComponentsPage(
			[ComputerName], [ComponentName], [ComponentState],
			[Version], [SettingsName])
		SELECT
			c.[ComputerName], cty.[ComponentName], cst.[ComponentState],
			cmnt.[Version], cse.[Name]
		FROM Components AS cmnt
		INNER JOIN Computers AS c ON cmnt.[ComputerID] = c.[ID]
		INNER JOIN ComponentStates AS cst ON cmnt.[StateID] = cst.[ID]
		INNER JOIN ComponentSettings AS cse ON cmnt.[SettingsID] = cse.[ID]
		INNER JOIN ComponentTypes AS cty ON cmnt.[ComponentID] = cty.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT
			[ComputerName], [ComponentName], [ComponentState], [Version], [SettingsName]
		FROM @ComponentsPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO


-- Returns events statistics
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventStatistic]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)

DROP PROCEDURE [dbo].[GetEventStatistic]
GO

CREATE  PROCEDURE [dbo].[GetEventStatistic]
	@Field nvarchar (64),
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT ' + @Field + N', [EventName]
					INTO #vba_temp FROM Events AS e
					INNER JOIN Computers AS c ON e.[ComputerID] = c.[ID]
					INNER JOIN ComponentTypes AS ct ON e.[ComponentID] = ct.[ID]
					INNER JOIN EventTypes AS et ON e.[EventID] = et.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N'; SELECT DISTINCT(' + @Field + N'), count([EventName]) as [Count]
						FROM #vba_temp GROUP BY '+ @Field +
						N' ORDER BY [Count] DESC'
	EXEC sp_executesql @Query
GO


-- Returns computer statistics
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersStatistic]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersStatistic]
GO

CREATE PROCEDURE [dbo].[GetComputersStatistic]
	@Field nvarchar (64),
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT ' + @Field + N', c.[ComputerName]
					INTO #vba_temp FROM Computers AS c
					INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	SET @Query = @Query + N'; SELECT DISTINCT(' + @Field + N'), count([ComputerName]) as [Count]
						FROM #vba_temp GROUP BY ' + @Field +
						N' ORDER BY [Count] DESC'
	EXEC sp_executesql @Query
GO
