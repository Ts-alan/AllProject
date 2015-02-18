using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.CC.Filters.Primitive;

public partial class Controls_PrimitiveFilterIpAddress : System.Web.UI.UserControl, IPrimitiveFilter
{
    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();
        FixIpAddressDialogOnAsyncPostBack();
    }


    private void FixIpAddressDialogOnAsyncPostBack()
    {
        //asp.net ajax unregistered all events on async postback
        //make sure that computers dialog\tree are recreated in this case
        ScriptManager sm = ScriptManager.GetCurrent(Page);
        if (sm == null) return;
        if (sm.IsInAsyncPostBack)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "PrimitiveFilterIpAddressFix",
                "IpAddressDialog.setAsyncPostBack()", true);
        }
    }

    private void RegisterScripts()
    {
        Page.ClientScript.RegisterClientScriptInclude("PageRequestManagerHelper", @"js/PageRequestManagerHelper.js"); 
    }

    #endregion

    #region IPrimitiveFilter Members
    public string GenerateSQL()
    {
        if (!fltTemplate.IsSelected) return String.Empty;
        return PrimitiveFilterHelper.GenerateSqlForIpValue(tboxFilter.Text, NameFieldDB, fltTemplate.UseOR,
            fltTemplate.UseNOT);
    }

    public void Clear()
    {
        tboxFilter.Text = String.Empty;
        fltTemplate.Clear();
    }

    public bool Validate()
    {
        if (regexFilter.Enabled)
        {
            regexFilter.Validate();
            return regexFilter.IsValid;
        }
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

    #endregion
}