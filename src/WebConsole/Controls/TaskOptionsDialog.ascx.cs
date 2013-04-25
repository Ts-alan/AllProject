using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Control Task Options Dialog that is used to show options of CustomizableTask control
/// </summary>
public partial class Controls_TaskOptionsDialog : System.Web.UI.UserControl
{
    #region Page LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();
    }

    private void RegisterScripts()
    {
        //register extjs
        Page.ClientScript.RegisterClientScriptInclude("Ext-Base", @"js/Groups/ext-1.1.1/adapter/ext/ext-base.js");
        Page.ClientScript.RegisterClientScriptInclude("Ext-All", @"js/Groups/ext-1.1.1/ext-all.js");
    }
    #endregion
}