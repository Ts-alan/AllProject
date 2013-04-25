using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Filters.Primitive;

public partial class Controls_PrimitiveFilterRange : System.Web.UI.UserControl, IPrimitiveFilter
{
    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        cmpRange.ErrorMessage = _rangeCompareErrorMessage;
    }
    #endregion

    #region IPrimitiveFilter Members
    public string GenerateSQL()
    {
        if (!fltTemplate.IsSelected) return String.Empty;
        return PrimitiveFilterHelper.GenerateSqlForRangeValue(GetRange(), NameFieldDB, fltTemplate.UseOR,
            fltTemplate.UseNOT);
    }

    public void Clear()
    {
        tboxRangeStart.Text = String.Empty;
        tboxRangeStop.Text = String.Empty;
        fltTemplate.Clear();
    }

    public bool Validate()
    {
        if (cmpRange.Enabled)
        {
            cmpRange.Validate();
            return cmpRange.IsValid;
        }
        return true;
    }

    private Range GetRange()
    {
        int start = 0;
        if (!string.IsNullOrEmpty(tboxRangeStart.Text))
        {
            int.TryParse(tboxRangeStart.Text, out start);
        }
        int stop = int.MaxValue;
        if (!string.IsNullOrEmpty(tboxRangeStop.Text))
        {
            int.TryParse(tboxRangeStop.Text, out stop);
        }
        return new Range(start, stop);
    }

    public PrimitiveFilterState SaveState()
    {
        PrimitiveFilterState state = fltTemplate.SaveState();
        state.Content = GetRange().ToString();
        return state;
    }

    public void LoadState(PrimitiveFilterState savedState)
    {
        Range range = new Range(savedState.Content.ToString());
        tboxRangeStart.Text = (range.Start == 0)? string.Empty : range.Start.ToString();
        tboxRangeStop.Text = (range.Stop == int.MaxValue)? string.Empty :range.Stop.ToString();
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

    private String _rangeCompareErrorMessage;
    public String RangeCompareErrorMessage
    {
        get { return _rangeCompareErrorMessage; }
        set {
            _rangeCompareErrorMessage = value;
            if (cmpRange != null)
            {
                cmpRange.ErrorMessage = value;
            }
        }
    }
    #endregion
}