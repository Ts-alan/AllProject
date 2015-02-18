using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Control that shows text Loading in upper part of the screen during asynchronous postbacks
/// </summary>
public partial class Controls_AsyncLoadingStateControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //can be used only on pages with registered ScriptManager
        if (ScriptManager.GetCurrent(Page) == null)
        {
            throw new InvalidOperationException(String.Format(
                @"The control with ID '{0}' requires a ScriptManager on the page. The ScriptManager must appear
                    before any controls that need it.", ID));
        }
    }
}