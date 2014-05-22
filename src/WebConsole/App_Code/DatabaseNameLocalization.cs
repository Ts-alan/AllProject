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