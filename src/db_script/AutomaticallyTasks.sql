IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AutomaticallyTasks]')
					   AND OBJECTPROPERTY(id, N'IsTable') = 1)
DROP TABLE [dbo].[AutomaticallyTasks]
GO

CREATE TABLE [dbo].[AutomaticallyTasks] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[EventID] smallint NOT NULL,
	[TaskID] smallint NOT NULL,	
	[Params] ntext COLLATE Cyrillic_General_CI_AS NULL,
	[IsAllowed] bit NOT NULL,
	CONSTRAINT [PK_AutomaticallyTasks]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_AutomaticallyTasks_EventID]
		UNIQUE NONCLUSTERED ([EventID])
)
GO

-- Get task by EventType
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetTaskByEventType]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetTaskByEventType]
GO

CREATE PROCEDURE [GetTaskByEventType]
	@EventName nvarchar(128)
WITH ENCRYPTION
AS
BEGIN	
	SELECT a.[ID], a.[EventID], a.[TaskID], a.[Params], a.[IsAllowed]
	FROM [AutomaticallyTasks] AS a
	INNER JOIN [EventTypes] AS e ON e.[ID] = a.[EventID]
	WHERE e.[EventName] = @EventName
END
GO

-- Get computer names and IP-addresses
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputerNamesAndIP]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputerNamesAndIP]
GO

CREATE PROCEDURE [GetComputerNamesAndIP]
WITH ENCRYPTION
AS
BEGIN	
	SELECT [ComputerName], [IPAddress] FROM [Computers]
END
GO


-- Get task state
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[IsRunningTask]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[IsRunningTask]
GO

CREATE PROCEDURE [IsRunningTask]
	@ComputerName nvarchar(64),
	@TaskID smallint
WITH ENCRYPTION
AS
BEGIN	
	SELECT ts.[TaskState], DATEDIFF(minute, t.[DateIssued], GetDate())
	FROM Tasks AS t
	INNER JOIN Computers AS c ON c.[ID] = t.[ComputerID]
	INNER JOIN TaskStates AS ts ON ts.[ID] = t.[StateID]
	WHERE c.[ComputerName] = @ComputerName 
	AND t.[TaskID] = @TaskID 
	AND t.[DateIssued] = (SELECT MAX(Tasks.[DateIssued]) 
							FROM Tasks 
							INNER JOIN Computers ON Computers.[ID] = Tasks.[ComputerID]
							INNER JOIN TaskStates ON TaskStates.[ID] = Tasks.[StateID]
							WHERE Computers.[ComputerName] = @ComputerName AND Tasks.[TaskID] = @TaskID )	
END
GO

------Insert automattically tasks-------------------------------------------------------------------------
DECLARE @EventID smallint
SET @EventID = (SELECT [ID] FROM EventTypes WHERE [EventName] = 'vba32.virus.found')

IF @EventID IS NULL
BEGIN
	INSERT INTO EventTypes ([EventName], [Color], [Send], [NoDelete], [Notify])
	VALUES ('vba32.virus.found', NULL, 0, 0, 0)
	SET @EventID = @@IDENTITY
END


DECLARE @TaskID smallint
SET @TaskID = (SELECT [ID] FROM TaskTypes WHERE [TaskName] = 'RunScanner')

IF @TaskID IS NULL
BEGIN
	INSERT INTO TaskTypes ([TaskName])
	VALUES ('RunScanner')
	SET @TaskID = @@IDENTITY
END

INSERT INTO AutomaticallyTasks ([EventID], [TaskID], [Params], [IsAllowed])
VALUES (@EventID, @TaskID, '<?xml version="1.0" encoding="utf-8"?> <task> <IsCheckArchives>0</IsCheckArchives> <IsCheckMacros>0</IsCheckMacros> <IsCheckMail>0</IsCheckMail> <IsCheckMemory>0</IsCheckMemory> <IsCleanFiles>0</IsCleanFiles> <IsCheckCure>0</IsCheckCure> <IsCheckCureBoot>0</IsCheckCureBoot> <IsDeleteArchives>0</IsDeleteArchives> <IsDeleteMail>0</IsDeleteMail> <IsDetectAdware>0</IsDetectAdware> <IsEnableCach>0</IsEnableCach> <IsExclude>0</IsExclude> <IsSaveInfectedToQuarantine>0</IsSaveInfectedToQuarantine> <IsSaveInfectedToReport>0</IsSaveInfectedToReport> <IsSaveSusToQuarantine>0</IsSaveSusToQuarantine> <IsScanBootSectors>0</IsScanBootSectors> <IsSFX>0</IsSFX> <IsScanStartup>0</IsScanStartup> <IsSet>0</IsSet> <IsKeep>0</IsKeep> <IsAdd>0</IsAdd> <IsAddArch>0</IsAddArch> <IsAddInf>0</IsAddInf> <IsUpdateBase>0</IsUpdateBase> <CheckObjects>*:</CheckObjects> <PathToScanner>%VBA32%</PathToScanner> <Mode>0</Mode> <HeuristicAnalysis>2</HeuristicAnalysis> <IsShowScanProgress>0</IsShowScanProgress> <Remove></Remove> <IfCureChecked>1</IfCureChecked> <Vba32CCUser>admin (admin admin) 192.168.234.226</Vba32CCUser> <Type>RunScanner</Type> </task>', 1)

----------------------------------------------------------------------------------------
