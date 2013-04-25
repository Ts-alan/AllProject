using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using Filters.Primitive;

public partial class Controls_PrimitiveFilterText : System.Web.UI.UserControl, IPrimitiveFilter
{
    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
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
    #endregion
}
