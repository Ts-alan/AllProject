/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SET NOCOUNT ON

PRINT N'Inserting constant data...';
GO


PRINT N'[dbo].[TaskStates]...';
GO
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Delivery');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Execution');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Completed successfully');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Execution error');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Stopped');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Delivery timeout');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Execution timeout');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'In queue');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Sended');
INSERT INTO [dbo].[TaskStates]([TaskState]) VALUES(N'Error');
GO
PRINT N'[dbo].[TaskStates]: successfully.';
GO


PRINT N'[dbo].[ComponentSettings]...';
GO
INSERT INTO [dbo].[ComponentSettings]([Name], [Settings]) VALUES(N'(unknown)', N'(unknown)');
GO
PRINT N'[dbo].[ComponentSettings]: successfully.';
GO


PRINT N'[dbo].[ComponentStates]...';
GO
INSERT INTO [dbo].[ComponentStates]([ComponentState]) VALUES(N'On');
INSERT INTO [dbo].[ComponentStates]([ComponentState]) VALUES(N'Off');
INSERT INTO [dbo].[ComponentStates]([ComponentState]) VALUES(N'Not installed');
GO
PRINT N'[dbo].[ComponentStates]: successfully.';
GO


PRINT N'[dbo].[EventTypes]...';
GO
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.cc.LocalHearth', 1, 1, 1);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.cc.GlobalEpidemy', 1, 1, 1);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.virus.found', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.virus.suspict', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.virus.cured', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.deleted', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.renamed', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.moved', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.quarantined', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.virus.found_cured', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.deferredcured', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.skipped', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.error', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.deleted.deferred', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.device.inserted', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.device.mounted', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.device.blocked', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.device.unknown.blocked', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.device.unmounted', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.started', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.finished', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.terminated', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.virbase.loaded', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.skipped', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.pathscan.started', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.pathscan.finished', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.process.terminated', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.scanner.loaded', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.scanner.unloaded', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.scanner.paused', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.scanner.resumed', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.scanner.started', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.scanner.stopped', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.scanner.finished', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.monitor.loaded', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.monitor.unloaded', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.monitor.activated', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.monitor.deactivated', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.object.locked', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.loader.loaded', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.loader.unloaded', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.module.corrupted', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.update.success', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.update.success.reboot', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.update.error', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.program.update.terminated', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.mail.virus.found', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.mail.virus.suspict', 0, 0, 0);
GO
PRINT N'[dbo].[ComponentStates]: successfully.';
GO


PRINT N'[dbo].[DeviceTypes]...';
GO
INSERT INTO [dbo].[DeviceTypes]([TypeName]) VALUES(N'USB');
GO
PRINT N'[dbo].[DeviceTypes]: successfully.';
GO


PRINT N'[dbo].[DevicePolicyStates]...';
GO
INSERT INTO [dbo].[DevicePolicyStates]([StateName]) VALUES(N'Undefined');
INSERT INTO [dbo].[DevicePolicyStates]([StateName]) VALUES(N'Enabled');
INSERT INTO [dbo].[DevicePolicyStates]([StateName]) VALUES(N'Disabled');
GO
PRINT N'[dbo].[DevicePolicyStates]: successfully.';
GO


PRINT N'[dbo].[Vba32Versions]...';
GO
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 WinNT Workstation');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 WinNT Server');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 for Windows Vista');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 for Windows Server 2008');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 Remote Console Scanner');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 Remote Control Agent');
GO
PRINT N'[dbo].[Vba32Versions]: successfully.';
GO


PRINT N'[dbo].[InstallationStatus]...';
GO
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Initializing');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Connecting');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Copying');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Processing');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Success');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Fail');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Error');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Parsing');
INSERT INTO [dbo].[InstallationStatus]([Status]) VALUES(N'Installed');
GO
PRINT N'[dbo].[InstallationStatus]: successfully.';
GO


PRINT N'[dbo].[ControlDeviceType]...';
GO
INSERT INTO [dbo].[ControlDeviceType] ([ControlName]) VALUES ('Unknown');
INSERT INTO [dbo].[ControlDeviceType] ([ControlName]) VALUES ('Loader');
INSERT INTO [dbo].[ControlDeviceType] ([ControlName]) VALUES ('Vsis');
INSERT INTO [dbo].[ControlDeviceType] ([ControlName]) VALUES ('RCS');
GO
PRINT N'[dbo].[ControlDeviceType]: successfully.';
GO


PRINT N'Inserting update info...';
GO
INSERT [dbo].[UpdateLog] ([BuildId], [DeployDatetime]) VALUES ('$(BuildId)', GETDATE());
GO