using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_SaveAsDialog : System.Web.UI.UserControl
{
    #region Page LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();
    }

    private void RegisterScripts()
    {
        //register needed scripts
        Page.ClientScript.RegisterClientScriptInclude("Ext-Base", @"js/Groups/ext-1.1.1/adapter/ext/ext-base.js");
        Page.ClientScript.RegisterClientScriptInclude("Ext-All", @"js/Groups/ext-1.1.1/ext-all.js");
        Page.ClientScript.RegisterClientScriptInclude("PageRequestManagerHelper", @"js/PageRequestManagerHelper.js");
    }
    #endregion

    #region Control LifeCycle
    #region PreRender
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        FixSaveAsDialogOnAsyncPostBack();
        SetJavascriptObjectParameters();
    }

    private void SetJavascriptObjectParameters()
    {
        //store in javascript SaveAsDialog object values if they are set
        string script = String.Empty;
        if (!String.IsNullOrEmpty(restrictedNames))
        {
            script += String.Format("{0}.setRestrictedNames({1}); ", JavascriptObject, restrictedNames);
        }
        if (!String.IsNullOrEmpty(usedNames))
        {
            script += String.Format("{0}.setUsedNames({1}); ", JavascriptObject, usedNames);
        }
        if (!String.IsNullOrEmpty(callbackFunction))
        {
            script += String.Format("{0}.setSaveCallback({1}); ", JavascriptObject, callbackFunction);
        }
        if (!String.IsNullOrEmpty(script))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(),
                String.Format("{0}_SetJavascriptObjectParameters", JavascriptObject),
                script, true);
        }
    }

    private void FixSaveAsDialogOnAsyncPostBack()
    {
        //asp.net ajax unregistered all events on async postback
        //make sure that save as dialog is recreated in this case
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm == null) return;
        if (sm.IsInAsyncPostBack)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(),
                String.Format("{0}_Fix", JavascriptObject),
                String.Format("{0}.setAsyncPostBack();", JavascriptObject), true);
        }
    }
    #endregion
    #endregion

    #region Properties
    #region SaveAsDialog JavascriptObject
    public string JavascriptObject
    {
        get { return String.Format("SaveAsDialog_{0}", ClientID); }
    }

    private string callbackFunction;
    public string CallbackFunction
    {
        get { return callbackFunction; }
        set { callbackFunction = value; }
    }

    public string JavascriptObjectShow
    {
        get { return String.Format("SaveAsDialog_{0}.show(); ", ClientID); }
    }
    #endregion

    #region Arrays
    private string restrictedNames;
    public string RestrictedNames
    {
        get { return restrictedNames; }
        set { restrictedNames = value; }
    }

    private string usedNames;
    public string UsedNames
    {
        get { return usedNames; }
        set { usedNames = value; }
    }
    #endregion

    #region Messages
    private string nameEmptyErrorMessage;
    public string NameEmptyErrorMessage
    {
        get { return nameEmptyErrorMessage; }
        set { nameEmptyErrorMessage = value; }
    }

    private string nameRestrictedErrorMessage;
    public string NameRestrictedErrorMessage
    {
        get { return nameRestrictedErrorMessage; }
        set { nameRestrictedErrorMessage = value; }
    }

    private string nameExistsConfirmRewriteMessage;
    public string NameExistsConfirmRewriteMessage
    {
        get { return nameExistsConfirmRewriteMessage; }
        set { nameExistsConfirmRewriteMessage = value; }
    }
    #endregion

    #region SaveAsName
    public string SaveAsName
    {
        get { return tboxSaveAsName.Text; }
    }
    #endregion
    #endregion
}