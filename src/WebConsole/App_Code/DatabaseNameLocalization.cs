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