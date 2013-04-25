----------------------
-- Database creation
USE master
GO

-- Checking whether the database already exists
IF EXISTS (SELECT *	FROM master.dbo.sysdatabases
					WHERE [NAME] = N'vbaControlCenterDB')
    DROP DATABASE vbaControlCenterDB
GO

CREATE DATABASE vbaControlCenterDB
ON PRIMARY(
	NAME = VbaCCDB,
	FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL\Data\VbaControlCenter.mdf',
	SIZE = 10MB,
	MAXSIZE = 1000MB,
	FILEGROWTH = 10MB
)
LOG ON(
	NAME = VbaCCLog,
	FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL\Data\VbaControlCenter.ldf',
	SIZE = 5MB,
	MAXSIZE = 500MB,
	FILEGROWTH = 5MB
)
GO

ALTER DATABASE vbaControlCenterDB SET RECOVERY SIMPLE
GO

ALTER DATABASE vbaControlCenterDB SET AUTO_SHRINK ON
GO

USE vbaControlCenterDB
GO

-- Database created and made active


-------------------------------------
-------------------------------------
CREATE TABLE [dbo].[OSTypes] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[OSName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_OSTypes]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_OSTypes_OSName]
		UNIQUE NONCLUSTERED ([OSName])
)

-----------------------------------------
CREATE TABLE [dbo].[Computers] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[ControlCenter] bit NOT NULL,
	[DomainName] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL,
	[UserLogin] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL,
	[OSTypeID] smallint NULL,
	[RAM] smallint NULL,
	[CPUClock] smallint NULL,
	[RecentActive] smalldatetime NOT NULL,
	[LatestUpdate] smalldatetime NULL,
	[Vba32Version] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL,
	[LatestInfected] smalldatetime NULL,
	[LatestMalware] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL,
	[Vba32Integrity] bit NULL,
	[Vba32KeyValid] bit NULL,
	[Description] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL,
	CONSTRAINT [PK_Computers]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_Computers_OSTypes]
		FOREIGN KEY (OSTypeID) REFERENCES OSTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
)

-----------------------------------------
-- Event-related tables
CREATE TABLE [dbo].[EventTypes] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[EventName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Color] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL,
	[Send] bit NOT NULL CONSTRAINT DF_EventTypes_Send DEFAULT(0),
	[NoDelete] bit NOT NULL CONSTRAINT DF_EventTypes_NoDelete DEFAULT(0),
	[Notify] bit NOT NULL CONSTRAINT DF_EventTypes_Notify DEFAULT(0),
	CONSTRAINT [PK_EventTypes]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_EventTypes_EventName]
		UNIQUE NONCLUSTERED ([EventName])
)
GO

-----------------------------------------
CREATE TABLE [dbo].[ComponentTypes] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[ComponentName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_ComponentTypes]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_ComponentTypes_ComponentName]
		UNIQUE NONCLUSTERED ([ComponentName])
 )

GO

----------------------------------------
CREATE TABLE [dbo].[Events] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] smallint NOT NULL,
	[EventID] smallint NOT NULL,
	[EventTime] datetime NOT NULL,
	[ComponentID] smallint NOT NULL,
	[Object] nvarchar(260) COLLATE Cyrillic_General_CI_AS NULL,
	[Comment] nvarchar(256) COLLATE Cyrillic_General_CI_AS NULL,
	CONSTRAINT [PK_Events]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_Events_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Events_EventTypes]
		FOREIGN KEY (EventID) REFERENCES EventTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Events_Components]
		FOREIGN KEY (ComponentID) REFERENCES ComponentTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
)
GO

-----------------------------------------
-- Task-related tables
CREATE TABLE [dbo].[TaskTypes] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[TaskName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_TaskTypes]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_TaskTypes_TaskName]
		UNIQUE NONCLUSTERED ([TaskName])
)

GO

-----------------------------------------
CREATE TABLE [dbo].[TaskStates] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[TaskState] nvarchar(32) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_TaskStates]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_TaskStates_TaskState]
		UNIQUE NONCLUSTERED ([TaskState])
)

GO

-----------------------------------------
CREATE TABLE [dbo].[Tasks] (
	[ID] bigint IDENTITY(1, 1) NOT NULL,
	[TaskID] smallint NOT NULL,
	[ComputerID] smallint NOT NULL,
	[StateID] smallint NOT NULL,
	[DateIssued] datetime NOT NULL,
	[DateComplete] datetime NULL,
	[DateUpdated] datetime NOT NULL,
	[TaskParams] ntext COLLATE Cyrillic_General_CI_AS NULL,
	[TaskUser] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_Tasks]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_Tasks_TaskTypes]
		FOREIGN KEY (TaskID) REFERENCES TaskTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Tasks_TaskStates]
		FOREIGN KEY (StateID) REFERENCES TaskStates([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Tasks_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
)

GO

-----------------------------------------
-- Component-related tables
CREATE TABLE [dbo].[ComponentStates] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[ComponentState] nvarchar(32) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_ComponentStates]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_ComponentStates_ComponentState]
		UNIQUE NONCLUSTERED ([ComponentState])
)

GO

-----------------------------------------
CREATE TABLE [dbo].[ComponentSettings] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[Name] nvarchar(32) COLLATE Cyrillic_General_CI_AS NULL,
	[Settings] ntext COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_ComponentSettings]
		PRIMARY KEY NONCLUSTERED ([ID])
)

GO

-----------------------------------------
CREATE TABLE [dbo].[Components] (
	[ComputerID] smallint NOT NULL,
	[ComponentID] smallint NOT NULL,
	[StateID] smallint NOT NULL,
	[Version] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL,
	[SettingsID] smallint NOT NULL CONSTRAINT [DF_Components_SettingsID] DEFAULT(1),
	CONSTRAINT [PK_Components]
		PRIMARY KEY NONCLUSTERED ([ComputerID], [ComponentID]),
	CONSTRAINT [FK_Components_ComponentTypes]
		FOREIGN KEY (ComponentID) REFERENCES ComponentTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Components_ComponentStates]
		FOREIGN KEY (StateID) REFERENCES ComponentStates([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Components_ComponentSettings]
		FOREIGN KEY (SettingsID) REFERENCES ComponentSettings([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Components_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
)

GO

-----------------------------------------
-- Process-related tables
CREATE TABLE [dbo].[Processes] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] smallint NOT NULL,
	[ProcessName] nvarchar(260) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[MemorySize] int NULL,
	[LastDate] datetime NOT NULL,
	CONSTRAINT [PK_Processes]
		PRIMARY KEY NONCLUSTERED ([ID]),	
	CONSTRAINT [FK_Processes_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
)

GO

--Policy tables
CREATE TABLE [dbo].[PolicyTypes] (
	[ID] [smallint] IDENTITY(1, 1) NOT NULL ,
	[TypeName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Params] ntext COLLATE Cyrillic_General_CI_AS NULL,
	[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS,
	
	CONSTRAINT [PK_PolicyTypes]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_PolicyTypes_TypeName]
		UNIQUE NONCLUSTERED ([TypeName])
) 
GO

-- policies to computer
CREATE TABLE [dbo].[Policies] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] [smallint] NOT NULL ,
	[PolicyID] [smallint] NOT NULL,
	
	CONSTRAINT [PK_Policies]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_Policies_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Policies_PolicyTypes]
		FOREIGN KEY (PolicyID) REFERENCES PolicyTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
) 
GO

--Device tables

--Type of devices. usb 
CREATE TABLE [dbo].[DeviceTypes] (
	[ID] [smallint] IDENTITY(1, 1) NOT NULL ,
	[TypeName] nvarchar(256) COLLATE Cyrillic_General_CI_AS NOT NULL,
	
	CONSTRAINT [PK_DeviceTypes]
		PRIMARY KEY NONCLUSTERED ([ID]),
		
	CONSTRAINT [U_DeviceTypes_TypeName]
		UNIQUE NONCLUSTERED ([TypeName])
) 
GO


-- Actions for devices: enable, disable
CREATE TABLE [dbo].[DevicePolicyStates] (
	[ID] [smallint] IDENTITY(1, 1) NOT NULL ,
	[StateName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	
	CONSTRAINT [PK_DevicePolicyStates]
		PRIMARY KEY NONCLUSTERED ([ID]),
		
	CONSTRAINT [U_DevicePolicyStates_StateName]
		UNIQUE NONCLUSTERED ([StateName])
) 
GO

-- unique device
CREATE TABLE [dbo].[Devices] (
	[ID] [smallint] IDENTITY(1, 1) NOT NULL,
	[SerialNo] nvarchar(256) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[DeviceTypeID] [smallint] NOT NULL,
	[Comment] nvarchar(128) COLLATE Cyrillic_General_CI_AS,
	
	CONSTRAINT [PK_Devices]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_Devices_DeviceTypes]
		FOREIGN KEY (DeviceTypeID) REFERENCES DeviceTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
) 
GO

-- device policy for computer
CREATE TABLE [dbo].[DevicesPolicies] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] [smallint] NOT NULL,
	[DeviceID] [smallint] NOT NULL,
	[DevicePolicyStateID] [smallint] NOT NULL,
	[LatestInsert] smalldatetime NULL,
	
	CONSTRAINT [PK_DevicesPolicies]
		PRIMARY KEY NONCLUSTERED ([ID]),
		
	CONSTRAINT [FK_DevicesPolicies_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
			
	CONSTRAINT [FK_DevicesPolicies_Devices]
		FOREIGN KEY (DeviceID) REFERENCES Devices([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
			
			CONSTRAINT [FK_DevicesPolicies_DevicePolicyStates]
		FOREIGN KEY (DevicePolicyStateID) REFERENCES DevicePolicyStates([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
) 
GO


-----------------------------------------
-- tasks of installation vba32

CREATE TABLE [dbo].[InstallationStatus] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[Status] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_InstallationStatus]
		PRIMARY KEY NONCLUSTERED ([ID])
)

-----------------------------------------
CREATE TABLE [dbo].[Vba32Versions] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[Vba32Version] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_Vba32Versions]
		PRIMARY KEY NONCLUSTERED ([ID])
)

-----------------------------------------
CREATE TABLE [dbo].[InstallationTaskType] (
	[ID] smallint IDENTITY(1, 1) NOT NULL,
	[TaskType] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	CONSTRAINT [PK_InstallationTaskType]
		PRIMARY KEY NONCLUSTERED ([ID])
)

-----------------------------------------
CREATE TABLE [dbo].[InstallationTasks] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[TaskTypeID] smallint NOT NULL,
	[Vba32VersionID] smallint NOT NULL,
	[StatusID] smallint NOT NULL,
	[InstallationDate] smalldatetime NOT NULL,
	[ExitCode] smallint NULL,
	[Error] ntext COLLATE Cyrillic_General_CI_AS NULL,
	CONSTRAINT [PK_InstallationTasks]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_InstallationTasks_InstallationStatus]
		FOREIGN KEY (StatusID) REFERENCES InstallationStatus([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,


	CONSTRAINT [FK_InstallationTasks_Vba32Versions]
		FOREIGN KEY (Vba32VersionID) REFERENCES Vba32Versions([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,

	CONSTRAINT [FK_InstallationTasks_InstallationTaskType]
		FOREIGN KEY (TaskTypeID) REFERENCES InstallationTaskType([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
)

-----------------------------------------
-- Inserting constant data
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Delivery');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Execution');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Completed successfully');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Execution error');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Stopped');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Delivery timeout');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Execution timeout');

INSERT INTO [dbo].[ComponentSettings]([Name], [Settings]) VALUES(N'(unknown)', N'(unknown)');

INSERT INTO [dbo].[ComponentStates]([ComponentState]) VALUES(N'On');
INSERT INTO [dbo].[ComponentStates]([ComponentState]) VALUES(N'Off');
INSERT INTO [dbo].[ComponentStates]([ComponentState]) VALUES(N'Not installed');

INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.cc.LocalHearth', 1, 1, 1);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.cc.GlobalEpidemy', 1, 1, 1);


INSERT INTO [dbo].[DeviceTypes]([TypeName]) VALUES(N'USB');

INSERT INTO [dbo].[DevicePolicyStates]([StateName]) VALUES(N'Undefined');
INSERT INTO [dbo].[DevicePolicyStates]([StateName]) VALUES(N'Enabled');
INSERT INTO [dbo].[DevicePolicyStates]([StateName]) VALUES(N'Disabled');

INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 WinNT Workstation');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 WinNT Server');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 for Windows Vista');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 for Windows Server 2008');

INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Initializing');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Connecting');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Copying');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Processing');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Success');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Fail');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Error');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Parsing');


INSERT INTO [dbo].[InstallationTaskType]([TaskType]) VALUES(N'Install');
INSERT INTO [dbo].[InstallationTaskType]([TaskType]) VALUES(N'Uninstall');

