using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using VirusBlokAda.CC.Filters.Primitive;
using VirusBlokAda.CC.Filters.Common;

public partial class Controls_PrimitiveFilterComputers : System.Web.UI.UserControl, IPrimitiveFilter
{
    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();        
        FixComputersDialogOnAsyncPostBack();              
    }



    private void FixComputersDialogOnAsyncPostBack()
    {
        //asp.net ajax unregistered all events on async postback
        //make sure that computers dialog\tree are recreated in this case
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm == null) return;
        if (sm.IsInAsyncPostBack)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PrimitiveFilterComputersFix",
                "ComputersDialog.setAsyncPostBack()", true);
        }            
    }

    private void RegisterScripts()
    {
        Page.ClientScript.RegisterClientScriptInclude("Ext-All", @"js/Groups/ext-4.1.1/ext-all.js");
        Page.ClientScript.RegisterClientScriptInclude("PageRequestManagerHelper", @"js/PageRequestManagerHelper.js"); 
    }

    #endregion
    #region IPrimitiveFilter Members


    public string GenerateSQL()
    {
        if (!fltTemplate.IsSelected) return String.Empty;
        return PrimitiveFilterHelper.GenerateSqlForTextValue(tboxFilter.Text, NameFieldDB, fltTemplate.UseOR,
            fltTemplate.UseNOT);
    }

    public void Clear()
    {
        tboxFilter.Text = String.Empty;
        fltTemplate.Clear();
    }

    public bool Validate()
    {
        return true;
    }

    public PrimitiveFilterState SaveState()
    {
        PrimitiveFilterState state = fltTemplate.SaveState();
        state.Content = tboxFilter.Text;

        return state;
    }

    public void LoadState(PrimitiveFilterState savedState)
    {
        tboxFilter.Text = savedState.Content.ToString();
        fltTemplate.LoadState(savedState);
    }

    public string GetID()
    {
        return fltTemplate.GetID();
    }

    #endregion

    #region Properties
    public String TextFilter
    {
        get { return fltTemplate.TextFilter; }
        set { fltTemplate.TextFilter = value; }
    }
    public String NameFieldDB
    {
        get { return fltTemplate.NameFieldDB; }
        set { fltTemplate.NameFieldDB = value; }
    }
    public PrimitiveFilterComputerTypes Mode
    {
        get { return this.mode; }
        set { this.mode = value; }
    }
    protected String HandlerUrl
    {
        get
        {
            switch (this.mode)
            {
                case PrimitiveFilterComputerTypes.Computers:
                    return Request.ApplicationPath + "/Handlers/CheckedComputerTreeHandler.ashx";
                case PrimitiveFilterComputerTypes.InstallComputers:
                    return Request.ApplicationPath + "/Handlers/CheckedInstallComputerTreeHandler.ashx";

                default:
                    return Request.ApplicationPath + "/Handlers/CheckedComputerTreeHandler.ashx";
            }
        }
    }
    #endregion
    #region Fields
    private PrimitiveFilterComputerTypes mode = PrimitiveFilterComputerTypes.Computers;
    #endregion
}