using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Filters.Primitive;
using System.Collections;
using System.ComponentModel;
using System.Configuration;

public partial class Controls_PrimitiveFilterSingleDropDownListForStatistics : System.Web.UI.UserControl, IPrimitiveFilter
{
    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    #endregion

    #region IPrimitiveFilter Members
    public String GenerateSQL()
    {
        if (!fltTemplate.IsSelected) return String.Empty;
        return PrimitiveFilterHelper.GenerateSqlForTextValue(SingleSelectionDropDownList1.SelectedValue.Substring(2), NameFieldDB, fltTemplate.UseOR, fltTemplate.UseNOT);
    }

    public void Clear()
    {
        SingleSelectionDropDownList1.SelectedValue = String.Empty;
        fltTemplate.Clear();
    }

    public Boolean Validate()
    {
        return true;
    }

    public PrimitiveFilterState SaveState()
    {
        PrimitiveFilterState state = fltTemplate.SaveState();
        state.Content = SingleSelectionDropDownList1.SelectedValue;
        return state;
    }

    public void LoadState(PrimitiveFilterState savedState)
    {
        SingleSelectionDropDownList1.SelectedValue = (String)savedState.Content;
        fltTemplate.LoadState(savedState);
    }

    public String GetID()
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

    public String GroupBy
    {
        get
        {
            if (SingleSelectionDropDownList1.SelectedIndex == 2)
                return "[Comment]";

            return "[ComputerName]";
        }
    }



    #endregion

   
}