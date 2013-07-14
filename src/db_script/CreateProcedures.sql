USE vbaControlCenterDB
GO


----------------------------------------------
-- Stored procedures for 'EventTypes' table --
----------------------------------------------

-- Returns Event Type ID by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventTypeID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventTypeID]
GO

CREATE PROCEDURE [GetEventTypeID]
	@EventName nvarchar(128),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such an event type
		IF NOT EXISTS (SELECT [ID] FROM [EventTypes] WHERE [EventName] = @EventName)
			INSERT INTO [EventTypes](EventName) VALUES (@EventName);
	END
	RETURN (SELECT [ID] FROM [EventTypes] WHERE [EventName] = @EventName)
END
GO

------------------------------------------------
-- Returns Event 'Send' field by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventTypeNotify]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventTypeNotify]
GO

CREATE PROCEDURE [GetEventTypeNotify]
	@EventName nvarchar(128)
WITH ENCRYPTION
AS
BEGIN
	SELECT
		[Notify]
	FROM
		[dbo].[EventTypes]
	WHERE
		[EventName] = @EventName
END
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteEventType]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteEventType]
GO

CREATE PROCEDURE [dbo].[DeleteEventType]
	@ID smallint
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[EventTypes]
	WHERE
		[ID] = @ID
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventType]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventType]
GO

CREATE PROCEDURE [dbo].[GetEventType]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT
		[ID],
		[EventName],
		[Color]
	FROM
		[dbo].[EventTypes]
	WHERE
		[ID] = @ID
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateEventType]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateEventType]
GO

CREATE PROCEDURE [dbo].[UpdateEventType]
	@ID smallint, 
	@EventName nvarchar(128), 
	@Color nvarchar(32) 
WITH ENCRYPTION
AS
	UPDATE [dbo].[EventTypes]
	SET
		[EventName] = @EventName,
		[Color] = @Color
	WHERE
		[ID] = @ID
GO


-------------------------------------------
-- Stored procedures for 'OSTypes' table --
-------------------------------------------

-- Returns OS Type ID by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetOSTypeID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetOSTypeID]
GO

CREATE PROCEDURE [GetOSTypeID](
	@OSName nvarchar(128),
	@InsertIfNotExists tinyint = 0)
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a OS type
		IF NOT EXISTS (SELECT [ID] FROM [OSTypes] WHERE [OSName] = @OSName)
			INSERT INTO [OSTypes](OSName) VALUES (@OSName);
	END
	RETURN (SELECT [ID] FROM [OSTypes] WHERE [OSName] = @OSName)
END
GO

-- Returns OS Name by its ID
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetOSName]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetOSName]
GO

CREATE PROCEDURE [GetOSName]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT [OSName] FROM [OSTypes] WHERE [ID] = @ID
GO

-- Returns OS Name by its ID
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetOSTypesList]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetOSTypesList]
GO

CREATE PROCEDURE [GetOSTypesList]
WITH ENCRYPTION
AS
	SELECT [ID],[OSName] FROM [OSTypes]
GO

--------------------------------------------------
-- Stored procedures for 'ComponentTypes' table --
--------------------------------------------------

-- Returns Component Type ID by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComponentTypeID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComponentTypeID]
GO

CREATE PROCEDURE [GetComponentTypeID](
	@ComponentName nvarchar(64),
	@InsertIfNotExists tinyint = 0)
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a component type
		IF NOT EXISTS (SELECT [ID] FROM [ComponentTypes] WHERE [ComponentName] = @ComponentName)
			INSERT INTO [ComponentTypes](ComponentName) VALUES (@ComponentName);
	END
	RETURN (SELECT [ID] FROM [ComponentTypes] WHERE [ComponentName] = @ComponentName)
END
GO


---------------------------------------------
-- Stored procedures for 'Computers' table --
---------------------------------------------

-- Returns Computer ID by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerID]
GO

CREATE PROCEDURE [dbo].[GetComputerID]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	RETURN (SELECT [ID] FROM [dbo].[Computers] WHERE [ComputerName] = @ComputerName)
GO

---------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerIPAddress]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerIPAddress]
GO

CREATE PROCEDURE [dbo].[GetComputerIPAddress]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	SELECT
		[IPAddress]
	FROM
		[dbo].[Computers]
	WHERE
		[ComputerName] = @ComputerName
GO

---------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerIPAddressByID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerIPAddressByID]
GO

CREATE PROCEDURE [dbo].[GetComputerIPAddressByID]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT
		[IPAddress]
	FROM
		[dbo].[Computers]
	WHERE
		[ID] = @ID
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteComputer]
GO

CREATE PROCEDURE [dbo].[DeleteComputer]
	@ID smallint
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[Computers]
	WHERE [ID] = @ID
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputer]
GO

CREATE PROCEDURE [dbo].[GetComputer]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT
		[ID],
		[ComputerName],
		[IPAddress],
		[ControlCenter],
		[DomainName],
		[UserLogin],
		[OSTypeID],
		[RAM],
		[CPUClock],
		[RecentActive],
		[LatestUpdate],
		[Vba32Version],
		[LatestInfected],
		[LatestMalware],
		[Vba32Integrity],
		[Vba32KeyValid],
		[Description]
	FROM
		[dbo].[Computers]
	WHERE
		[ID] = @ID
GO

-----------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateComputerSystemInfo]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateComputerSystemInfo]
GO

CREATE PROCEDURE [UpdateComputerSystemInfo]
	@ComputerName nvarchar(64),
	@IPAddress nvarchar(16) = NULL,
	@DomainName nvarchar(256) = NULL,
	@UserLogin nvarchar(256) = NULL,
	@OSName nvarchar(128) = NULL,
	@RAM smallint = NULL,
	@CPUClock smallint = NULL,
	@Vba32Version nvarchar(256) = NULL,
	@Vba32Integrity bit = NULL,
	@Vba32KeyValid bit = NULL,
	@ControlCenter bit = NULL	
WITH ENCRYPTION
AS
	-- Retrieving OSID
	DECLARE @OSTypeID smallint
	SET @OSTypeID = NULL
	IF @OSName IS NOT NULL
		EXEC @OSTypeID = dbo.GetOSTypeID @OSName, 1

	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID = 0
	BEGIN
		IF @IPAddress IS NOT NULL
			-- Clearing same IPs in the database
			UPDATE [Computers]
			SET [IPAddress] = '0.0.0.0'
			WHERE [IPAddress] = @IPAddress
		ELSE
			SET @IPAddress = '0.0.0.0'
		-- Checking @ControlCenter param
		IF @ControlCenter IS NULL
			SET @ControlCenter = 0
		-- Checking @OSName param
		IF @OSName IS NULL
			BEGIN
				SET @OSName = '(unknown)'
				EXEC @OSTypeID = dbo.GetOSTypeID @OSName, 1
			END
		-- New computer registration
		INSERT INTO [Computers](ComputerName,
								IPAddress,
								ControlCenter,
								DomainName,
								UserLogin,
								OSTypeID,
								RAM,
								CPUClock,
								RecentActive,
								Vba32Version,
								Vba32Integrity,
								Vba32KeyValid)
		VALUES (@ComputerName,
				@IPAddress,
				@ControlCenter,
				@DomainName,
				@UserLogin,
				@OSTypeID,
				@RAM,
				@CPUClock,
				GETDATE(),
				@Vba32Version,
				@Vba32Integrity,
				@Vba32KeyValid)
		RETURN
	END
	
	-- Updating particular fields
	IF @IPAddress IS NOT NULL
		BEGIN
			-- Clearing same IPs in the database
			UPDATE [Computers]
			SET [IPAddress] = '0.0.0.0'
			WHERE [IPAddress] = @IPAddress
			-- Updating value for current computer
			UPDATE [Computers]
			SET IPAddress = @IPAddress
			WHERE [ID] = @ComputerID
		END
	IF @DomainName IS NOT NULL
		UPDATE [Computers]
		SET DomainName = @DomainName
		WHERE [ID] = @ComputerID
	IF @UserLogin IS NOT NULL
		UPDATE [Computers]
		SET UserLogin = @UserLogin
		WHERE [ID] = @ComputerID
	IF @OSTypeID IS NOT NULL
		UPDATE [Computers]
		SET OSTypeID = @OSTypeID
		WHERE [ID] = @ComputerID
	IF @RAM IS NOT NULL
		UPDATE [Computers]
		SET RAM = @RAM
		WHERE [ID] = @ComputerID
	IF @CPUClock IS NOT NULL
		UPDATE [Computers]
		SET CPUClock = @CPUClock
		WHERE [ID] = @ComputerID
	IF @Vba32Version IS NOT NULL
		UPDATE [Computers]
		SET Vba32Version = @Vba32Version
		WHERE [ID] = @ComputerID
	IF @Vba32Integrity IS NOT NULL
		UPDATE [Computers]
		SET Vba32Integrity = @Vba32Integrity
		WHERE [ID] = @ComputerID
	IF @Vba32KeyValid IS NOT NULL
		UPDATE [Computers]
		SET Vba32KeyValid = @Vba32KeyValid
		WHERE [ID] = @ComputerID
	IF @ControlCenter IS NOT NULL
		UPDATE [Computers]
		SET ControlCenter = @ControlCenter
		WHERE [ID] = @ComputerID
	-- Recent activity time
	UPDATE [Computers]
	SET [RecentActive] = GETDATE()
	WHERE [ID] = @ComputerID
GO


------------------------------------------
-- Stored procedures for 'Events' table --
------------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteEvent]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteEvent]
GO

CREATE PROCEDURE [dbo].[DeleteEvent]
	@ID int
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[Events]
	WHERE
		[ID] = @ID
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEvent]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEvent]
GO

CREATE PROCEDURE [dbo].[GetEvent]
	@ID int
WITH ENCRYPTION
AS
	SELECT
		[ID],
		[ComputerID],
		[EventID],
		[EventTime],
		[ComponentID],
		[Object],
		[Comment]
	FROM
		[dbo].[Events]
	WHERE
		[ID] = @ID
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddEvent]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddEvent]
GO

CREATE PROCEDURE [AddEvent]
	@ComputerName nvarchar(64),
	@EventName nvarchar(128),
	@EventTime datetime,
	@ComponentName nvarchar(64),
	@Object nvarchar(260) = NULL,
	@Comment nvarchar(256) = NULL
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

	-- Retrieving EventID
	DECLARE @EventID smallint
	EXEC @EventID = dbo.GetEventTypeID @EventName, 1

	-- Retrieving ComponentID
	DECLARE @ComponentID smallint
	EXEC @ComponentID = dbo.GetComponentTypeID @ComponentName, 1

	-- Inserting data
	INSERT INTO [Events](ComputerID, EventID, EventTime, ComponentID, Object, Comment)
		VALUES(@ComputerID, @EventID, @EventTime, @ComponentID, @Object, @Comment)

	-- Checking for events which require additional action
	-- Update
	IF (@EventName LIKE N'%update.success%')
		UPDATE [Computers]
		SET [LatestUpdate] = @EventTime
		WHERE [ID] = @ComputerID

	-- Virus
	IF (@EventName LIKE N'%virus.found')
		UPDATE [Computers]
		SET [LatestInfected] = @EventTime,
			[LatestMalware] = @Comment
		WHERE [ID] = @ComputerID

	-- Recent activity time
	UPDATE [Computers]
	SET [RecentActive] = GETDATE()
	WHERE [ID] = @ComputerID
GO


--------------------------------------------------
-- Stored procedures for 'ComponentStates' table --
--------------------------------------------------

-- Returns Component State ID by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComponentStateID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComponentStateID]
GO

CREATE PROCEDURE [GetComponentStateID]
	@ComponentState nvarchar(32),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a component state
		IF NOT EXISTS (SELECT [ID] FROM [ComponentStates] WHERE [ComponentState] = @ComponentState)
			INSERT INTO [ComponentStates](ComponentState) VALUES (@ComponentState);
	END
	RETURN (SELECT [ID] FROM [ComponentStates] WHERE [ComponentState] = @ComponentState)
END
GO


------------------------------------------------
-- Stored procedures for 'ProcessNames' table --
------------------------------------------------

-- Returns Process Name ID by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetProcessNameID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetProcessNameID]
GO

CREATE PROCEDURE [GetProcessNameID]
	@ProcessName nvarchar(260),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such an process type
		IF NOT EXISTS (SELECT [ID] FROM [ProcessNames] WHERE [ProcessName] = @ProcessName)
			INSERT INTO [ProcessNames](ProcessName) VALUES (@ProcessName);
	END
	RETURN (SELECT [ID] FROM [ProcessNames] WHERE [ProcessName] = @ProcessName)
END
GO


----------------------------------------------
-- Stored procedures for 'TaskStates' table --
----------------------------------------------

-- Returns Task State ID by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetTaskStateID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTaskStateID]
GO

CREATE PROCEDURE [GetTaskStateID]
	@TaskState nvarchar(32),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a task state
		IF NOT EXISTS (SELECT [ID] FROM [TaskStates] WHERE [TaskState] = @TaskState)
			INSERT INTO [TaskStates]([TaskState]) VALUES (@TaskState);
	END
	RETURN (SELECT [ID] FROM [TaskStates] WHERE [TaskState] = @TaskState)
END
GO


---------------------------------------------
-- Stored procedures for 'TaskTypes' table --
---------------------------------------------

-- Returns Task Type ID by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetTaskTypeID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTaskTypeID]
GO

CREATE PROCEDURE [GetTaskTypeID]
	@TaskName nvarchar(64),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a task type
		IF NOT EXISTS (SELECT [ID] FROM [TaskTypes] WHERE [TaskName] = @TaskName)
			INSERT INTO [TaskTypes]([TaskName]) VALUES (@TaskName);
	END
	RETURN (SELECT [ID] FROM [TaskTypes] WHERE [TaskName] = @TaskName)
END
GO


---------------------------------------------
-- Stored procedures for 'Processes' table --
---------------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteProcessInfo]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteProcessInfo]
GO

CREATE PROCEDURE [DeleteProcessInfo]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID IS NULL
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END
	
	DELETE FROM [Processes] WHERE [ComputerID] = @ComputerID
GO

-----------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateProcessInfo]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateProcessInfo]
GO

CREATE PROCEDURE [UpdateProcessInfo]
	@ComputerName nvarchar(64),
	@ProcessName nvarchar(260),
	@MemorySize int,
	@Date datetime
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
	
	-- Retrieving ProcessID
	DECLARE @ProcessID smallint
	EXEC @ProcessID = dbo.GetProcessNameID @ProcessName, 1
	
	-- Inserting/updating data
	DECLARE @ID smallint
	SET @ID = (SELECT [ID] FROM [Processes] WHERE [ComputerID] = @ComputerID AND [ProcessID] = @ProcessID)
	IF @ID IS NULL
	BEGIN
		-- Insert
		INSERT INTO [Processes](ComputerID, ProcessID, MemorySize, LastDate)
			VALUES(@ComputerID, @ProcessID, @MemorySize, @Date)
	END
	ELSE
	BEGIN
		-- Update
		UPDATE [Processes]
		SET	[ComputerID] = @ComputerID,
			[ProcessID] = @ProcessID,
			[MemorySize] = @MemorySize,
			[LastDate] = @Date
		WHERE [ID] = @ID
	END
GO


----------------------------------------------
-- Stored procedures for 'Components' table --
----------------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateComponentState]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateComponentState]
GO

CREATE PROCEDURE [UpdateComponentState]
	@ComputerName nvarchar(64),
	@ComponentName nvarchar(64),
	@ComponentState nvarchar(32),
	@Version nvarchar(64) = NULL
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
	
	-- Retrieving ComponentID
	DECLARE @ComponentID smallint
	EXEC @ComponentID = dbo.GetComponentTypeID @ComponentName, 1
	
	-- Retrieving StateID
	DECLARE @StateID smallint
	EXEC @StateID = dbo.GetComponentStateID @ComponentState, 1
	
	-- Inserting/updating data
	IF NOT EXISTS (SELECT [ComputerID] FROM [Components] WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID)
	BEGIN
		-- Insert
		INSERT INTO [Components](ComputerID, ComponentID, StateID, Version)
			VALUES(@ComputerID, @ComponentID, @StateID, @Version)
	END
	ELSE
	BEGIN
		-- Update
		UPDATE [Components]
		SET	[ComputerID] = @ComputerID,
			[ComponentID] = @ComponentID,
			[StateID] = @StateID
		WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID
		-- Updating version if necessary
		IF @Version IS NOT NULL
			UPDATE [Components]
			SET	[Version] = @Version
			WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID
	END
GO

-----------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateComponentSettings]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateComponentSettings]
GO

CREATE PROCEDURE [UpdateComponentSettings]
	@ComputerName nvarchar(64),
	@ComponentName nvarchar(64),
	@ComponentSettings ntext
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
	
	-- Retrieving ComponentID
	DECLARE @ComponentID smallint
	EXEC @ComponentID = dbo.GetComponentTypeID @ComponentName, 1
	
	IF EXISTS (SELECT [StateID] FROM [Components] WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID)
	BEGIN
		-- Inserting/updating data
		DECLARE @SettingsID smallint
		SET @SettingsID = (SELECT [SettingsID] FROM [Components] WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID)
		IF @SettingsID IS NULL
		BEGIN
			-- Insert
			INSERT INTO [ComponentSettings](Settings)
				VALUES(@ComponentSettings)
			DECLARE @IDD smallint
			UPDATE [Components]
			SET	[SettingsID] = @@IDENTITY
			WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID
		END
		ELSE
		BEGIN
			-- Update
			-- Спорный момент!
			UPDATE [ComponentSettings]
			SET	[Settings] = @ComponentSettings
			WHERE [ID] = @SettingsID
		END
	END
GO


-----------------------------------------
-- Stored procedures for 'Tasks' table --
-----------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateTaskState]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateTaskState]
GO

CREATE PROCEDURE [UpdateTaskState]
	@TaskID bigint,
	@TaskState nvarchar(32),
	@Date datetime,
	@Description nvarchar(256) = NULL
WITH ENCRYPTION
AS
	-- Retrieving StateID
	DECLARE @StateID smallint
	EXEC @StateID = dbo.GetTaskStateID @TaskState, 1

	DECLARE @Priority1 smallint
	DECLARE @Priority2 smallint
	
	IF @TaskState IN (N'Completed successfully', N'Execution error', N'Stopped',
					  N'Delivery timeout', N'Execution timeout', N'Error')
			SET @Priority1 = 4
	ELSE
		IF @TaskState IN (N'Execution')
			SET @Priority1 = 3
		ELSE
			IF @TaskState IN (N'Sended')
				SET @Priority1 = 2
			ELSE
				IF @TaskState IN (N'In queue')
					SET @Priority1 = 1
				ELSE
					IF @TaskState IN (N'Delivery')
						SET @Priority1 = 0

	
	DECLARE @TaskStateOld nvarchar(32)
	SET @TaskStateOld = (SELECT ts.[TaskState] FROM Tasks AS t
						 INNER JOIN TaskStates AS ts ON t.[StateID] = ts.[ID]
						 WHERE t.[ID] = @TaskID)
	IF @TaskStateOld IN (N'Completed successfully', N'Execution error', N'Stopped',
					  N'Delivery timeout', N'Execution timeout', N'Error')
			SET @Priority2 = 4
	ELSE
		IF @TaskStateOld IN (N'Execution')
			SET @Priority2 = 3
		ELSE
			IF @TaskStateOld IN (N'Sended')
				SET @Priority2 = 2
			ELSE
				IF @TaskStateOld IN (N'In queue')
					SET @Priority2 = 1
				ELSE
					IF @TaskStateOld IN (N'Delivery')
						SET @Priority2 = 0

	IF(@Priority2 < @Priority1)
	BEGIN
	
		UPDATE [Tasks]
		SET	[StateID] = @StateID,
			[DateUpdated] = @Date
		WHERE [ID] = @TaskID

		IF @Description IS NOT NULL
		BEGIN
			UPDATE [Tasks]
				SET	[TaskDescription] = @Description
				WHERE [ID] = @TaskID
		END
		
		-- Checking particular states
		IF @TaskState = N'Completed successfully'
		BEGIN
			UPDATE [Tasks]
			SET	[DateComplete] = @Date
			WHERE [ID] = @TaskID

			--костыль для удаления компа после успешного завершения задачи на отсоединение агента
			IF (SELECT tt.[TaskName] FROM Tasks AS t
				INNER JOIN TaskTypes AS tt ON t.[TaskID] = tt.[ID]
				WHERE t.[ID] = @TaskID) IN ('Detach agent', 'Отсоединить агент')
			BEGIN
				DELETE FROM Computers
				WHERE [ID] = (SELECT [ComputerID] FROM [Tasks] WHERE [ID] = @TaskID)

				RETURN
			END
		END	
	END

	-- Recent activity time
	UPDATE [Computers]
	SET [RecentActive] = GETDATE()
	WHERE [ID] = (SELECT ComputerID FROM [Tasks] WHERE [ID] = @TaskID)
GO

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateDeliveryState]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateDeliveryState]
GO

CREATE PROCEDURE [UpdateDeliveryState]
	@Date datetime
WITH ENCRYPTION
AS
	-- Retrieving StateID
	DECLARE @StateDelivery smallint
	DECLARE @StateDeliveryTimeOut smallint
	EXEC @StateDelivery = dbo.GetTaskStateID 'Delivery', 1
	EXEC @StateDeliveryTimeOut = dbo.GetTaskStateID 'Delivery timeout', 1

	UPDATE [Tasks]
	SET	[StateID] = @StateDeliveryTimeOut,
	            [DateUpdated] = GETDATE()
	WHERE [StateID] = @StateDelivery AND [DateIssued] < @Date

GO


IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteOldEvents]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteOldEvents]
GO

CREATE PROCEDURE [DeleteOldEvents]
	@Date datetime
WITH ENCRYPTION
AS
	SET ROWCOUNT 10000 

	WHILE (667 = 667)
	BEGIN
		DELETE [Events] FROM [EventTypes] INNER JOIN [Events] ON [EventTypes].[ID] = [Events].[EventID]
			WHERE ([EventTypes].[NoDelete] = 0) AND ([Events].[EventTime] < @Date)
        
		IF @@ROWCOUNT < 10000
		BREAK
	END

	SET ROWCOUNT 0
GO


IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteOldTasks]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteOldTasks]
GO

CREATE PROCEDURE [DeleteOldTasks]
	@Date datetime
WITH ENCRYPTION
AS
	SET ROWCOUNT 10000 

	WHILE (667 = 667)
	BEGIN
		DELETE FROM [Tasks]
			WHERE [Tasks].[DateIssued] < @Date
        
		IF @@ROWCOUNT < 10000
		BREAK
	END

	SET ROWCOUNT 0
GO


-- Stored procedure for local hearth detection
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventsCountByComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventsCountByComputer]
GO

CREATE PROCEDURE [dbo].[GetEventsCountByComputer]
	@ComputerName nvarchar(64),
	@EventName nvarchar(128),
	@EventTime datetime
WITH ENCRYPTION
AS
	DECLARE @Now datetime
	SET @Now = GETDATE()
	SELECT COUNT([Events].[ID])
	FROM [dbo].[Events]
		INNER JOIN [dbo].[Computers] ON [Computers].[ID] = [Events].[ComputerID]
		INNER JOIN [dbo].[EventTypes] ON [EventTypes].[ID] = [Events].[EventID]
	WHERE [ComputerName] = @ComputerName AND [EventName] = @EventName AND [EventTime] BETWEEN @EventTime AND @Now;
GO


-- Stored procedure for epidemic detection
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventsCountByComment]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventsCountByComment]
GO

CREATE PROCEDURE [dbo].[GetEventsCountByComment]
	@Comment nvarchar(64),
	@EventName nvarchar(128),
	@EventTime datetime
WITH ENCRYPTION
AS
	DECLARE @Now datetime
	SET @Now = GETDATE()
	SELECT [ComputerName], COUNT([Events].[ID])
	FROM [dbo].[Events]
		INNER JOIN [dbo].[EventTypes] ON [EventTypes].[ID] = [Events].[EventID]
		INNER JOIN [dbo].[Computers] ON [Computers].[ID] = [Events].[ComputerID]
	WHERE [Comment] = @Comment AND [EventName] = @EventName AND [EventTime] BETWEEN @EventTime AND @Now
	GROUP BY [ComputerName];
GO


-- Stored procedure to prevent mass mailings
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetEventsCountByName]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetEventsCountByName]
GO

CREATE PROCEDURE [dbo].[GetEventsCountByName]
	@EventName nvarchar(128),
	@EventTime datetime
WITH ENCRYPTION
AS
	DECLARE @Now datetime
	SET @Now = GETDATE()
	SELECT COUNT([Events].[ID])
	FROM [dbo].[Events]
		INNER JOIN [dbo].[EventTypes] ON [EventTypes].[ID] = [Events].[EventID]
	WHERE [EventName] = @EventName AND [EventTime] BETWEEN @EventTime AND @Now;
GO


----------------------------------------------
-- Stored procedures for Installing Vba32
----------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetStatusID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetStatusID]
GO

CREATE PROCEDURE [GetStatusID]
	@Status nvarchar(64)
WITH ENCRYPTION
AS
	
	RETURN (SELECT [ID] FROM [InstallationStatus] WHERE [Status] = @Status)
GO

--------------------------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetInstallationTaskTypeID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetInstallationTaskTypeID]
GO

CREATE PROCEDURE [GetInstallationTaskTypeID]
	@TaskType nvarchar(64)
WITH ENCRYPTION
AS
	
	RETURN (SELECT [ID] FROM [InstallationTaskType] WHERE [TaskType] = @TaskType)
GO

--------------------------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetVba32VersionID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetVba32VersionID]
GO

CREATE PROCEDURE [GetVba32VersionID]
	@Vba32Version nvarchar(64)
WITH ENCRYPTION
AS
	
	RETURN (SELECT [ID] FROM [Vba32Versions] WHERE [Vba32Version] = @Vba32Version)
GO

----------------------------------------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetInstallationTasks]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetInstallationTasks]
GO

CREATE PROCEDURE [dbo].[GetInstallationTasks]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @TasksPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] int,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[TaskType] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Vba32Version] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Status] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[InstallationDate] smalldatetime NOT NULL,
			[ExitCode] smallint,
			[Error] ntext
		)
	
		INSERT INTO @TasksPage(
			[ID], [ComputerName], [IPAddress], [TaskType], [Vba32Version], [Status], [InstallationDate], [ExitCode], [Error])
		SELECT
			t.[ID], t.[ComputerName], t.[IPAddress], tt.[TaskType], v.[Vba32Version], s.[Status], t.[InstallationDate], t.[ExitCode], t.[Error]
		FROM InstallationTasks AS t
		INNER JOIN InstallationStatus AS s ON t.[StatusID] = s.[ID]
		INNER JOIN Vba32Versions AS v ON t.[Vba32VersionID] = v.[ID]
		INNER JOIN InstallationTaskType AS tt ON t.[TaskTypeID] = tt.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [TaskType], [Vba32Version], [Status], [InstallationDate], [ExitCode], [Error]
		FROM @TasksPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO


-----------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetInstallationTasksCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetInstallationTasksCount]
GO

CREATE PROCEDURE [dbo].[GetInstallationTasksCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS	
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @TasksPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ID] int,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[TaskType] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Vba32Version] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[Status] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[InstallationDate] smalldatetime NOT NULL,
			[ExitCode] smallint
		)
	
		INSERT INTO @TasksPage(
			[ID], [ComputerName], [IPAddress], [TaskType], [Vba32Version], [Status], [InstallationDate], [ExitCode])
		SELECT
			t.[ID], t.[ComputerName], t.[IPAddress], tt.[TaskType], v.[Vba32Version], s.[Status], t.[InstallationDate], t.[ExitCode]
		FROM InstallationTasks AS t
		INNER JOIN InstallationStatus AS s ON t.[StatusID] = s.[ID]
		INNER JOIN Vba32Versions AS v ON t.[Vba32VersionID] = v.[ID]
		INNER JOIN InstallationTaskType AS tt ON t.[TaskTypeID] = tt.[ID]'

	SET @Query = @Query + N';
		SELECT COUNT(*)
		FROM @TasksPage'

	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where

	EXEC sp_executesql @Query
GO


-----------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[InsertInstallationTask]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[InsertInstallationTask]
GO

CREATE PROCEDURE [InsertInstallationTask]
	@ComputerName nvarchar(64),
	@IPAddress nvarchar(16),
	@TaskType nvarchar(64),
	@Vba32Version nvarchar(64),
	@Status nvarchar(64),
	@Date datetime,
	@Exitcode smallint
WITH ENCRYPTION
AS
	-- Retrieving TaskTypeID
	DECLARE @TaskTypeID smallint
	EXEC @TaskTypeID = dbo.GetInstallationTaskTypeID @TaskType

	-- Retrieving StatusID
	DECLARE @StatusID smallint
	EXEC @StatusID = dbo.GetStatusID @Status

	-- Retrieving Vba32VersionID
	DECLARE @Vba32VersionID smallint
	EXEC @Vba32VersionID = dbo.GetVba32VersionID @Vba32Version
	
	INSERT INTO [InstallationTasks]
	([ComputerName], [IPAddress], [TaskTypeID], [Vba32VersionID], [StatusID], [InstallationDate], [ExitCode])
	VALUES
	(@ComputerName,	@IPAddress, @TaskTypeID, @Vba32VersionID, @StatusID,	@Date, @Exitcode)

	SELECT SCOPE_IDENTITY()
GO

-----------------------------------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateInstallationTask]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateInstallationTask]
GO

CREATE PROCEDURE [UpdateInstallationTask]
	@ID int,
	@Status nvarchar(64),
	@Exitcode smallint,
	@Error ntext
WITH ENCRYPTION
AS
	-- Retrieving StatusID
	DECLARE @StatusID smallint
	EXEC @StatusID = dbo.GetStatusID @Status

	
	UPDATE [InstallationTasks]
	SET 	[StatusID] = @StatusID,
		[ExitCode] = @Exitcode,
		[Error] = @Error
	WHERE [ID] = @ID
GO
----------------------------------------------------------------------

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetListInstallationTaskTypes]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetListInstallationTaskTypes]
GO

CREATE PROCEDURE [GetListInstallationTaskTypes]
WITH ENCRYPTION
AS
	SELECT [TaskType] FROM InstallationTaskType
GO
----------------------------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetListInstallationStatus]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetListInstallationStatus]
GO

CREATE PROCEDURE [GetListInstallationStatus]
WITH ENCRYPTION
AS
	SELECT [Status] FROM InstallationStatus
GO
----------------------------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetListVba32Versions]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetListVba32Versions]
GO

CREATE PROCEDURE [GetListVba32Versions]
WITH ENCRYPTION
AS
	SELECT [Vba32Version] FROM Vba32Versions
GO
----------------------------------------------------------------------
-- Get IPAddress list for configure agent
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetIPAddressListForConfigure]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetIPAddressListForConfigure]
GO

CREATE PROCEDURE [dbo].[GetIPAddressListForConfigure]
WITH ENCRYPTION
AS
	SELECT DISTINCT(t.[IPAddress]), s.[Status], tt.[TaskType] FROM InstallationTasks AS t
	INNER JOIN InstallationTaskType AS tt ON tt.[ID] = t.[TaskTypeID]
	INNER JOIN InstallationStatus AS s ON s.[ID] = t.[StatusID]
	WHERE [IPAddress] NOT IN (SELECT [IPAddress] FROM Computers) AND tt.[TaskType] = 'Install' AND s.[Status] = 'Success'
GO
----------------------------------------------------------------------------