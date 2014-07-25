using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for DatabaseNameLocalization
/// </summary>
public static class DatabaseNameLocalization
{
    private static Dictionary<String, String> _dict;
	static DatabaseNameLocalization()
	{
        _dict = new Dictionary<String, String>();

        _dict.Add("On", "ComponentOn");
        _dict.Add("Off", "ComponentOff");
        _dict.Add("Not installed", "ComponentNotInstalled");

        _dict.Add("Install", "Install");
        _dict.Add("Uninstall", "Uninstall");

        _dict.Add("Initializing", "Initializing");
        _dict.Add("Connecting", "Connecting");
        _dict.Add("Copying", "Copying");
        _dict.Add("Processing", "Processing");
        _dict.Add("Success", "SuccessStatus");
        _dict.Add("Fail", "Fail");
        _dict.Add("Error", "Error");
        _dict.Add("Parsing", "Parsing");
        _dict.Add("Installed", "Installed");

        _dict.Add("Delivery", "TaskStateDelivery");
        _dict.Add("Execution", "TaskStateExecution");
        _dict.Add("Completed successfully", "TaskStateCompletedSuccessfully");
        _dict.Add("Execution error", "TaskStateExecutionError");
        _dict.Add("Stopped", "TaskStateStopped");
        _dict.Add("Delivery timeout", "TaskStateDeliveryTimeout");
        _dict.Add("Execution timeout", "TaskStateExecutionTimeout");
        _dict.Add("In queue", "TaskStateInQueue");
        _dict.Add("Sended", "TaskStateSended");

        _dict.Add("BlockWrite", "BlockWrite");
        _dict.Add("Enabled", "Enabled");
        _dict.Add("Disabled", "Disabled");

        _dict.Add("{2E406790-5472-4E0C-9EBF-88D081AA09AC}", "VAS");
        _dict.Add("{87005109-1276-483A-B0A9-F3119AFA4E5B}", "VDD");
        _dict.Add("{76DC546B-D814-4E18-AF4B-C7D17BC0AB90}", "VFC");
        _dict.Add("{99266E33-09E2-461E-97B2-2CBCF405A368}", "VGI");
        _dict.Add("{B4A234EB-3BAF-4822-9262-2467B035856C}", "VIC");
        _dict.Add("{A3F5FCA0-46DC-4328-8568-5FDF961E87E6}", "VMT");
        _dict.Add("{AAEBBB59-4CD4-478B-A7C6-15F4DAF74078}", "VND");
        _dict.Add("{7B7D499C-541E-4971-BFD5-286A78CAE649}", "VPP");
        _dict.Add("{67E7B7BE-BE06-4A0D-A812-7E1A0142C0F6}", "VQTN");

	    //Events
        _dict.Add("JE_VPP_START", "JE_VPP_START");
        _dict.Add("JE_VPP_STOP", "JE_VPP_STOP");
        _dict.Add("JE_VPP_APPLIED_RULES_OK", "JE_VPP_APPLIED_RULES_OK");
        _dict.Add("JE_VPP_APPLIED_RULES_FAILED", "JE_VPP_APPLIED_RULES_FAILED");
        _dict.Add("JE_VPP_AUDIT_WRITE", "JE_VPP_AUDIT_WRITE");
        _dict.Add("JE_VPP_AUDIT_READ", "JE_VPP_AUDIT_READ");
        _dict.Add("JE_VPP_AUDIT_DELETE", "JE_VPP_AUDIT_DELETE");
        _dict.Add("JE_VPP_AUDIT_EXECUTE", "JE_VPP_AUDIT_EXECUTE");
        _dict.Add("JE_VPP_AUDIT_OPEN_WRITE", "JE_VPP_AUDIT_OPEN_WRITE");
        _dict.Add("JE_VPP_AUDIT_OPEN_READ", "JE_VPP_AUDIT_OPEN_READ");
        _dict.Add("JE_VPP_PRINTER_GRANTED", "JE_VPP_PRINTER_GRANTED");
        _dict.Add("JE_VPP_PRINTER_DENIED", "JE_VPP_PRINTER_DENIED");
        
        _dict.Add("JE_QTN_ADD", "JE_QTN_ADD");
        _dict.Add("JE_QTN_DELETE", "JE_QTN_DELETE");
        _dict.Add("JE_QTN_RESTORE", "JE_QTN_RESTORE");

        _dict.Add("JE_VND_START", "JE_VND_START");
        _dict.Add("JE_VND_STOP", "JE_VND_STOP");
        _dict.Add("JE_VND_APPLIED_RULES_OK", "JE_VND_APPLIED_RULES_OK");
        _dict.Add("JE_VND_APPLIED_RULES_FAILED", "JE_VND_APPLIED_RULES_FAILED");
        _dict.Add("JE_VND_AUIDIT_TCP", "JE_VND_AUIDIT_TCP");
        _dict.Add("JE_VND_AUIDIT_UDP", "JE_VND_AUIDIT_UDP");
        _dict.Add("JE_VND_AUIDIT_OTHER", "JE_VND_AUIDIT_OTHER");

        _dict.Add("JE_VDD_START", "JE_VDD_START");
        _dict.Add("JE_VDD_STOP", "JE_VDD_STOP");
        _dict.Add("JE_VDD_APPLIED_USB_DEVICES_SETTINGS_OK", "JE_VDD_APPLIED_USB_DEVICES_SETTINGS_OK");
        _dict.Add("JE_VDD_APPLIED_USB_CLASSES_SETTINGS_OK", "JE_VDD_APPLIED_USB_CLASSES_SETTINGS_OK");
        _dict.Add("JE_VDD_APPLIED_CLASSES_RULES_SETTINGS_OK", "JE_VDD_APPLIED_CLASSES_RULES_SETTINGS_OK");
        _dict.Add("JE_VDD_APPLIED_USB_DEVICES_SETTINGS_FAILED", "JE_VDD_APPLIED_USB_DEVICES_SETTINGS_FAILED");
        _dict.Add("JE_VDD_APPLIED_USB_CLASSES_SETTINGS_FAILED", "JE_VDD_APPLIED_USB_CLASSES_SETTINGS_FAILED");
        _dict.Add("JE_VDD_APPLIED_CLASSES_RULES_SETTINGS_FAILED", "JE_VDD_APPLIED_CLASSES_RULES_SETTINGS_FAILED");
        _dict.Add("JE_VDD_AUDIT_USB", "JE_VDD_AUDIT_USB");

        _dict.Add("JE_VAS_START", "JE_VAS_START");
        _dict.Add("JE_VAS_STOP", "JE_VAS_STOP");
        _dict.Add("JE_VAS_INFECTED", "JE_VAS_INFECTED");
        _dict.Add("JE_VAS_SUSPICIOUS", "JE_VAS_SUSPICIOUS");
        _dict.Add("JE_VAS_ACTION_BLOCK", "JE_VAS_ACTION_BLOCK");
        _dict.Add("JE_VAS_ACTION_CURE", "JE_VAS_ACTION_CURE");
        _dict.Add("JE_VAS_ACTION_DELETE", "JE_VAS_ACTION_DELETE");
        _dict.Add("JE_VAS_ACTION_NONE", "JE_VAS_ACTION_NONE");

        _dict.Add("JE_VMT_START", "JE_VMT_START");
        _dict.Add("JE_VMT_STOP", "JE_VMT_STOP");
        _dict.Add("JE_VMT_APPLIED_SETTINGS_OK", "JE_VMT_APPLIED_SETTINGS_OK");
        _dict.Add("JE_VMT_APPLIED_SETTINGS_FAILED", "JE_VMT_APPLIED_SETTINGS_FAILED");
        _dict.Add("JE_VMT_CHECK_RESULT_SUSPECTED", "JE_VMT_CHECK_RESULT_SUSPECTED");
        _dict.Add("JE_VMT_CHECK_RESULT_INFECTED", "JE_VMT_CHECK_RESULT_INFECTED");
        _dict.Add("JE_VMT_ACTION_BLOCK", "JE_VMT_ACTION_BLOCK");
        _dict.Add("JE_VMT_ACTION_CURE", "JE_VMT_ACTION_CURE");
        _dict.Add("JE_VMT_ACTION_DELETE", "JE_VMT_ACTION_DELETE");
        _dict.Add("JE_VMT_ACTION_NONE", "JE_VMT_ACTION_NONE");

        _dict.Add("JE_VIC_REGISTRY_CHANGED", "JE_VIC_REGISTRY_CHANGED");
        _dict.Add("JE_VIC_DEVICES_STATE_SAVE_START", "JE_VIC_DEVICES_STATE_SAVE_START");
        _dict.Add("JE_VIC_FILES_STATE_SAVE_FINISHED", "JE_VIC_FILES_STATE_SAVE_FINISHED");
        _dict.Add("JE_VIC_REGISTRY_STATE_CHECK_FINISHED", "JE_VIC_REGISTRY_STATE_CHECK_FINISHED");
        _dict.Add("JE_VIC_REGISTRY_STATE_SAVE_START", "JE_VIC_REGISTRY_STATE_SAVE_START");
        _dict.Add("JE_VIC_DEVICE_CAN_NOT_READ", "JE_VIC_DEVICE_CAN_NOT_READ");
        _dict.Add("JE_VIC_REGISTRY_STATE_CHECK_START", "JE_VIC_REGISTRY_STATE_CHECK_START");
        _dict.Add("JE_VIC_DEVICES_STATE_SAVE_FINISHED", "JE_VIC_DEVICES_STATE_SAVE_FINISHED");
        _dict.Add("JE_VIC_FILES_STATE_CHECK_START", "JE_VIC_FILES_STATE_CHECK_START");
        _dict.Add("JE_VIC_FILES_STATE_CHECK_FINISHED", "JE_VIC_FILES_STATE_CHECK_FINISHED");
        _dict.Add("JE_VIC_FILES_CAN_NOT_OPEN", "JE_VIC_FILES_CAN_NOT_OPEN");
        _dict.Add("JE_VIC_DEVICES_STATE_CHECK_FINISHED", "JE_VIC_DEVICES_STATE_CHECK_FINISHED");
        _dict.Add("JE_VIC_REGISTRY_CAN_NOT_OPEN_KEY", "JE_VIC_REGISTRY_CAN_NOT_OPEN_KEY");
        _dict.Add("JE_VIC_DEVICES_STATE_CHECK_START", "JE_VIC_DEVICES_STATE_CHECK_START");
        _dict.Add("JE_VIC_FILES_STATE_SAVE_START", "JE_VIC_FILES_STATE_SAVE_START");
        _dict.Add("JE_VIC_FILE_CHANGED", "JE_VIC_FILE_CHANGED");
        _dict.Add("JE_VIC_DEVICE_CHANGED", "JE_VIC_DEVICE_CHANGED");
        _dict.Add("JE_VIC_REGISTRY_STATE_SAVE_FINISHED", "JE_VIC_REGISTRY_STATE_SAVE_FINISHED");

        _dict.Add("JE_VFC_START", "JE_VFC_START");
        _dict.Add("JE_VFC_STOP", "JE_VFC_STOP");
        _dict.Add("JE_VFC_APPLIED_SETTINGS_OK", "JE_VFC_APPLIED_SETTINGS_OK");
        _dict.Add("JE_VFC_APPLIED_SETTINGS_FAILED", "JE_VFC_APPLIED_SETTINGS_FAILED");
        _dict.Add("JE_VFC_REMOVED", "JE_VFC_REMOVED");

        _dict.Add("JE_SERVICE_START", "JE_SERVICE_START");
        _dict.Add("JE_SERVICE_STOP", "JE_SERVICE_STOP");
        _dict.Add("JE_SERVICE_CORRUPTED", "JE_SERVICE_CORRUPTED");
	}

    public static String GetNameForCurrentCulture(String name)
    {
        if (_dict.ContainsKey(name))
        {
            String nameLocalization = ResourceControl.GetStringForCurrentCulture(_dict[name]);
            return String.IsNullOrEmpty(nameLocalization) ? name : nameLocalization;
        }
        else
            return name;
    }
}