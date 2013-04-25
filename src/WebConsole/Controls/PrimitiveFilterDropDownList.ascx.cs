using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Filters.Primitive;
using System.Collections;
using System.ComponentModel;

public partial class Controls_PrimitiveFilterDropDownList : System.Web.UI.UserControl, IPrimitiveFilter
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
        return PrimitiveFilterHelper.GenerateSqlForTextValue(MultipleSelectionDropDownList1.SelectedValue, NameFieldDB, fltTemplate.UseOR, fltTemplate.UseNOT);
    }

    public void Clear()
    {
        MultipleSelectionDropDownList1.SelectedValue = String.Empty;
        fltTemplate.Clear();
    }

    public Boolean Validate()
    {
        return true;
    }

    public PrimitiveFilterState SaveState()
    {
        PrimitiveFilterState state = fltTemplate.SaveState();
        state.Content = MultipleSelectionDropDownList1.SelectedValue;
        return state;
    }

    public void LoadState(PrimitiveFilterState savedState)
    {
        MultipleSelectionDropDownList1.SelectedValue = (String)savedState.Content;
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

    private Boolean _isLocalized = true;

    public Boolean IsLocalized
    {
        get { return _isLocalized; }
        set { _isLocalized = value; }
    }

    public virtual object DataSource
    {
       get
       {
           return MultipleSelectionDropDownList1.DataSource;
       }
       set
       {
           System.Collections.ICollection list = value as System.Collections.ICollection;
           if (list != null && _isLocalized)
           {
               ArrayList array = new ArrayList();
               foreach (Object item in list)
               {
                   array.Add(new Option(DatabaseNameLocalization.GetNameForCurrentCulture(item.ToString()), item.ToString()));
               }
               MultipleSelectionDropDownList1.DataSource = array;
           }
           else
           {
               MultipleSelectionDropDownList1.DataSource = value;
           }
       }
    }

    public override void DataBind()
    {
        MultipleSelectionDropDownList1.DataBind();
    }
    #endregion

    #region Helper Class
    public class Option
    {
        private string _text;
        private string _value;

        public Option(string text, string value)
        {
            _text = text;
            _value = value;
        }

        public string Text
        {
            get
            {
                return _text;
            }
        }

        public string Value
        {

            get
            {
                return _value;
            }
        }
    }


    #endregion
}