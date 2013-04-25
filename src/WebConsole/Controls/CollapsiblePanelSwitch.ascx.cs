using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Controls_CollapsiblePanelSwitch : System.Web.UI.UserControl
{
    #region Public Methods
    public void Initialize(HtmlGenericControl collapsiblePanel)
    {
        initialized = true;
        if (collapsiblePanel == null)
        {
            throw new ArgumentNullException("collapsiblePanel", "CollapsiblePanelSwitch does not support null argument");
        }
        this.collapsiblePanel = collapsiblePanel;
        
    }
    #endregion

    #region Fields
    private bool initialized = false;
    private HtmlGenericControl collapsiblePanel;
    #endregion

    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void RegisterScripts()
    {
        Page.ClientScript.RegisterClientScriptInclude("CollapsiblePanelSwitch", @"js/CollapsiblePanelSwitch.js");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!initialized)
        {
            throw new HttpException(String.Format("CollapsiblePanelSwitch control {0} was never initialized ", ClientID));
        }
        SetPanelVisible();
        RegisterScripts();
        SetButtonOnclick();
    }    
    #endregion

    #region Helper Methods
    private void SetPanelVisible()
    {
        bool visible = false;
        Boolean.TryParse(hfSwitch.Value, out visible);
        if (visible)
        {
            collapsiblePanel.Style["visibility"] = "visible";
            collapsiblePanel.Style["height"] = "auto";
            imbtnSwitch.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/arrow_up.gif";
        }
        else
        {
            collapsiblePanel.Style["visibility"] = "hidden";
            collapsiblePanel.Style["height"] = "0px";
            collapsiblePanel.Style["overflow"] = "hidden";
            imbtnSwitch.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/arrow_down.gif";
        }
    }

    private void SetButtonOnclick()
    {
        imbtnSwitch.Attributes.Add("onclick", String.Format("CollapsiblePanel.Toggle('{0}', '{1}', '{2}', '{3}'); return false;",
            collapsiblePanel.ClientID, imbtnSwitch.ClientID, hfSwitch.ClientID, Profile.Theme));
    }
    #endregion
}