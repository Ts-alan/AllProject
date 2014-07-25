using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32ControlCenterUpdate.DBPatches
{
    internal sealed class DBPatch_1_0_0_7 : DBPatchBase
    {
        public DBPatch_1_0_0_7()
            : base("1.0.0.7")
        {
        }

        protected override String GetUpdateScript()
        {
            StringBuilder builder = new StringBuilder(1024);
            
            builder.Append(@"DELETE FROM [dbo].[EventTypes];");
            builder.Append(@"GO");
            builder.Append(Environment.NewLine);
            builder.Append(@"INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.cc.LocalHearth', 1, 1, 1);
                INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'vba32.cc.GlobalEpidemy', 1, 1, 1);
                INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_SERVICE_START', 0, 0, 0);
                INSERT INTO [dbo].[EventTypes]([EventName], [Send], [NoDelete], [Notify]) VALUES (N'JE_SERVICE_STOP', 0, 0, 0);
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
                GO");
            return builder.ToString();
        }
    }
}
