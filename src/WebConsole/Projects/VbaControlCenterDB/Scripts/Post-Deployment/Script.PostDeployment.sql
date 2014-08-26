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
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_SERVICE_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_SERVICE_STOP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_UPDATE_PRODUCTS_CORRUPTED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_STOP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_APPLIED_RULES_OK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_APPLIED_RULES_FAILED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_AUDIT_WRITE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_AUDIT_READ', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_AUDIT_DELETE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_AUDIT_EXECUTE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_AUDIT_OPEN_WRITE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VPP_AUDIT_OPEN_READ', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_QTN_ADD', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_QTN_DELETE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_QTN_RESTORE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VAS_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VAS_STOP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VAS_INFECTED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VAS_SUSPICIOUS', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VAS_ACTION_BLOCK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VAS_ACTION_CURE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VAS_ACTION_DELETE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VAS_ACTION_NONE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_STOP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_APPLIED_SETTINGS_OK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_APPLIED_SETTINGS_FAILED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_CHECK_RESULT_SUSPECTED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_CHECK_RESULT_INFECTED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_ACTION_BLOCK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_ACTION_CURE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_ACTION_DELETE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VMT_ACTION_NONE', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VND_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VND_STOP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VND_APPLIED_RULES_OK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VND_APPLIED_RULES_FAILED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VND_AUIDIT_TCP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VND_AUIDIT_UDP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VND_AUIDIT_OTHER', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_REGISTRY_CHANGED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_DEVICES_STATE_SAVE_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_FILES_STATE_SAVE_FINISHED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_REGISTRY_STATE_CHECK_FINISHED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_REGISTRY_STATE_SAVE_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_DEVICE_CAN_NOT_READ', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_REGISTRY_STATE_CHECK_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_DEVICES_STATE_SAVE_FINISHED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_FILES_STATE_CHECK_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_FILES_STATE_CHECK_FINISHED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_FILES_CAN_NOT_OPEN', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_DEVICES_STATE_CHECK_FINISHED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_REGISTRY_CAN_NOT_OPEN_KEY', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_DEVICES_STATE_CHECK_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_FILES_STATE_SAVE_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_FILE_CHANGED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_DEVICE_CHANGED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VIC_REGISTRY_STATE_SAVE_FINISHED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VFC_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VFC_STOP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VFC_APPLIED_SETTINGS_OK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VFC_APPLIED_SETTINGS_FAILED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VFC_REMOVED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_START', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_STOP', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_APPLIED_USB_DEVICES_SETTINGS_OK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_APPLIED_USB_CLASSES_SETTINGS_OK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_APPLIED_CLASSES_RULES_SETTINGS_OK', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_APPLIED_USB_DEVICES_SETTINGS_FAILED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_APPLIED_USB_CLASSES_SETTINGS_FAILED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_APPLIED_CLASSES_RULES_SETTINGS_FAILED', 0, 0, 0);
INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_VDD_AUDIT_USB', 0, 0, 0);
GO
PRINT N'[dbo].[ComponentStates]: successfully.';
GO


PRINT N'[dbo].[DeviceTypes]...';
GO
INSERT INTO [dbo].[DeviceTypes]([TypeName]) VALUES(N'USB');
INSERT INTO [dbo].[DeviceTypes]([TypeName]) VALUES(N'NET');
GO
PRINT N'[dbo].[DeviceTypes]: successfully.';
GO


PRINT N'[dbo].[Vba32Versions]...';
GO
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 Remote Console Scanner');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 Remote Control Agent');
INSERT INTO [dbo].[Vba32Versions]([Vba32Version]) VALUES(N'Vba32 Antivirus');
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

PRINT N'[dbo].[DeviceClassMode]...';
GO
INSERT INTO [dbo].[DeviceClassMode] ([ModeName]) VALUES('Undefined');
INSERT INTO [dbo].[DeviceClassMode] ([ModeName]) VALUES('Disabled');
INSERT INTO [dbo].[DeviceClassMode] ([ModeName]) VALUES('Enabled');
INSERT INTO [dbo].[DeviceClassMode] ([ModeName]) VALUES('BlockWrite');
GO
PRINT N'[dbo].[DeviceClassMode]: successfully.';
GO

PRINT N'[dbo].[DeviceClass]...';
GO
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{36FC9E60-C465-11CF-8056-444553540000}', 'USB');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E965-E325-11CE-BFC1-08002BE10318}', 'CDROM');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E969-E325-11CE-BFC1-08002BE10318}', 'fdc');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E96A-E325-11CE-BFC1-08002BE10318}', 'hdc');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E96D-E325-11CE-BFC1-08002BE10318}', 'Modem');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E970-E325-11CE-BFC1-08002BE10318}', 'MTD');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E972-E325-11CE-BFC1-08002BE10318}', 'Net');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E977-E325-11CE-BFC1-08002BE10318}', 'PCMCIA');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E978-E325-11CE-BFC1-08002BE10318}', 'Ports');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{4D36E980-E325-11CE-BFC1-08002BE10318}', 'FloppyDisk');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{6BDD1FC1-810F-11D0-BEC7-08002BE2092F}', '1394');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{6BDD1FC5-810F-11D0-BEC7-08002BE2092F}', 'Infrared');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName]) VALUES('{E0CBF06C-CD8B-4647-BB8A-263B43F0F974}', 'Bluetooth');
GO
PRINT N'[dbo].[DeviceClass]: successfully.';
GO

PRINT N'[dbo].[DeviceClass] USB classes...';
GO
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('1', 'Audio', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('2', 'CDC Control', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('3', 'HID', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('5', 'Physical', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('6', 'Image', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('7', 'Printer', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('8', 'Mass Storage', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('9', 'Hub', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('10', 'CDC-Data', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('11', 'Smart Card', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('13', 'Content Security', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('14', 'Video', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('15', 'Personal Healthcare', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('16', 'Audio/Video Devices', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('220', 'Diagnostic Device', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('224', 'Wireless Controller', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('239', 'Miscellaneous', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('254', 'Application Specific', 'USB class');
INSERT INTO [dbo].[DeviceClass] ([UID], [ClassName], [Class]) VALUES('255', 'Vendor Specific', 'USB class');
GO
PRINT N'[dbo].[DeviceClass]: USB classes successfully.';
GO

PRINT N'[dbo].[UpdateStates]...';
GO
INSERT [dbo].[UpdateStates] ([State]) VALUES ('Processing');
INSERT [dbo].[UpdateStates] ([State]) VALUES ('Success');
INSERT [dbo].[UpdateStates] ([State]) VALUES ('Fail');
GO
PRINT N'[dbo].[UpdateStates]: successfully.';
GO