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
using VirusBlokAda.CC.Filters.Primitive;
using System.Globalization;
using System.Diagnostics;

public partial class Controls_PrimitiveFilterDateTime : System.Web.UI.UserControl, IPrimitiveFilter
{
    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private void RegisterScripts()
    {
        Page.ClientScript.RegisterClientScriptInclude("PrimitiveFilterDateTime", @"js/PrimitiveFilterDateTime.js");
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        InitCalendarCulture();

        valTimeFrom.InvalidValueMessage = Resources.Resource.TimeIncorrect + " " + Resources.Resource.CorrectFormat + " " + Resources.Resource.TimeFormat;
        valTimeTo.InvalidValueMessage = Resources.Resource.TimeIncorrect + " " + Resources.Resource.CorrectFormat + " " + Resources.Resource.TimeFormat;
        valDeclinationValue.ErrorMessage = String.Format(Resources.Resource.ValueBetween, 1, 100);
        valDateFrom.InvalidValueMessage = Resources.Resource.DateIncorrect + " " + Resources.Resource.CorrectFormat + " " + Resources.Resource.DateFormat;
        valDateTo.InvalidValueMessage = Resources.Resource.DateIncorrect + " " + Resources.Resource.CorrectFormat + " " + Resources.Resource.DateFormat;
        cmpDate.ErrorMessage = Resources.Resource.RangeError;

        ddlDeclinationDirection.Items.Add(Resources.Resource.More);
        ddlDeclinationDirection.Items.Add(Resources.Resource.Less);

        ddlIntervalType.Items.Add(new ListItem(Resources.Resource.Minutes, "0"));
        ddlIntervalType.Items.Add(new ListItem(Resources.Resource.Hours, "1"));
        ddlIntervalType.Items.Add(new ListItem(Resources.Resource.Days2, "2"));
        ddlIntervalType.Items.Add(new ListItem(Resources.Resource.Weeks2, "3"));
        ddlIntervalType.Items.Add(new ListItem(Resources.Resource.Months2, "4"));
        ddlIntervalType.Items.Add(new ListItem(Resources.Resource.Years, "5"));

        InitDefaultValues();
        SetRadioOnclick();
    }

    private void SetRadioOnclick()
    {
        rbtnDateInterval.Attributes.Add("onclick", String.Format("RadioDateTime.dateIntervalActive('{0}', '{1}');", 
            panDateInterval.ClientID, panDateDeclination.ClientID));
        rbtnDateDeclination.Attributes.Add("onclick", String.Format("RadioDateTime.dateDeclinationActive('{0}', '{1}');",
            panDateInterval.ClientID, panDateDeclination.ClientID));
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        TogglePanels();
        RegisterScripts();
    }

    private void TogglePanels()
    {
        bool isRange = rbtnDateInterval.Checked;

        PanelEnable(panDateInterval, isRange);
        PanelEnable(panDateDeclination, !isRange);
    }

    private void PanelEnable(Panel panel, bool enable)
    {
        foreach (Control next in panel.Controls)
        {
            ControlEnable(next, enable);
        }
    }

    private void ControlEnable(Control control, bool enable)
    {
        //add radioDisabled attributes

        //validators
        BaseValidator validator = control as BaseValidator;
        if (validator != null)
        {
            validator.Enabled = enable;
            if (enable)
            {
                validator.Attributes.Remove("radioDisabled");
            }
            else
            {
                validator.Attributes.Add("radioDisabled", "");
            }
        }
        //controls
        WebControl webcontrol = control as WebControl;
        if (webcontrol != null)
        {
            if (enable)
            {
                webcontrol.Attributes.Remove("disabled");
                webcontrol.Attributes.Remove("radioDisabled");
            }
            else
            {
                webcontrol.Attributes.Add("disabled", "disabled");
                webcontrol.Attributes.Add("radioDisabled", "");
            }
        }

        //for html controls
        HtmlControl htmlcontrol = control as HtmlControl;
        if (htmlcontrol != null)
        {
            if (enable)
            {
                htmlcontrol.Attributes.Remove("disabled");
                htmlcontrol.Attributes.Remove("radioDisabled");
            }
            else
            {
                htmlcontrol.Attributes.Add("disabled", "disabled");
                htmlcontrol.Attributes.Add("radioDisabled", "");
            }
        }
    }

    private void InitCalendarCulture()
    {
        //Set the culture settings
        CultureInfo oCulture = CultureInfo.CurrentCulture;
        meDateFrom.CultureName = oCulture.Name;
        meDateTo.CultureName = oCulture.Name;
        CalendarExtender1.Format = oCulture.DateTimeFormat.ShortDatePattern;
        CalendarExtender2.Format = oCulture.DateTimeFormat.ShortDatePattern;
        //CalendarMaskExtender expects something like "99/99/9999"
        string Mask = CalendarExtender1.Format.Replace(oCulture.DateTimeFormat.DateSeparator, "/");
        if (Mask.IndexOf("d") != Mask.LastIndexOf("d"))
        {
            Mask = Mask.Replace("d", "9");
        }
        else
        {
            Mask = Mask.Replace("d", "99");
        }
        if (Mask.IndexOf("M") != Mask.LastIndexOf("M"))
        {
            Mask = Mask.Replace("M", "9");
        }
        else
        {
            Mask = Mask.Replace("M", "99");
        }
        Mask = Mask.Replace("y", "9");
        meDateFrom.Mask = Mask;
        meDateTo.Mask = Mask;
    }

    private void InitDefaultValues()
    {
        tboxTimeFrom.Text = tboxTimeTo.Text = "00:00";
        CalendarExtender1.SelectedDate = DateTime.Now.AddDays(1.0 - (Double)DateTime.Now.Day);
        CalendarExtender2.SelectedDate = DateTime.Now;
        tboxDeclinationValue.Text = "1";
        if (ddlIntervalType.Items.Count != 0) ddlIntervalType.SelectedIndex = 0;
        if (ddlDeclinationDirection.Items.Count != 0) ddlDeclinationDirection.SelectedIndex = 0;
    }

    #endregion

    #region IPrimitiveFilter Members
    public string GenerateSQL()
    {
        if (!fltTemplate.IsSelected) return String.Empty;
        RangeDateTime r = GetDateTimeRange();
        return PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(r.GetStart(), r.GetStop(), NameFieldDB, fltTemplate.UseOR,
            fltTemplate.UseNOT);
    }

    public void Clear()
    {
        InitDefaultValues();

        ddlDeclinationDirection.SelectedIndex = 0;
        ddlIntervalType.SelectedIndex = 0;

        rbtnDateDeclination.Checked = false;
        rbtnDateInterval.Checked = true;

        fltTemplate.Clear();
    }

    public bool Validate()
    {
        if (valTimeFrom.Enabled)
        {
            valTimeFrom.Validate();
            if (!valTimeFrom.IsValid) return false;
        }
        if (valDateFrom.Enabled)
        {
            valDateFrom.Validate();
            if (!valDateFrom.IsValid) return false;
        }

        if (valTimeTo.Enabled)
        {
            valTimeTo.Validate();
            if (!valTimeTo.IsValid) return false;
        }
        if (valDateTo.Enabled)
        {
            valDateTo.Validate();
            if (!valDateTo.IsValid) return false;
        }

        if (valDeclinationValue.Enabled)
        {
            valDeclinationValue.Validate();
            if (!valDeclinationValue.IsValid) return false;
        }

        return true;
    }

    public PrimitiveFilterState SaveState()
    {
        PrimitiveFilterState state = fltTemplate.SaveState();
        state.Content = GetDateTimeRange().ToString();
        return state;
    }

    public void LoadState(PrimitiveFilterState savedState)
    {
        InitDefaultValues();
        RangeDateTime range = new RangeDateTime(savedState.Content.ToString());
        if (range.IsRange)
        {
            rbtnDateDeclination.Checked = false;
            rbtnDateInterval.Checked = true;

            CalendarExtender1.SelectedDate = range.Start.Date;
            CalendarExtender2.SelectedDate = range.Stop.Date;

            tboxTimeFrom.Text = String.Format("{0:HH}:{0:mm}", range.Start);
            tboxTimeTo.Text = String.Format("{0:HH}:{0:mm}", range.Stop);
        }
        else
        {
            rbtnDateInterval.Checked = false;
            rbtnDateDeclination.Checked = true;

            ddlDeclinationDirection.SelectedIndex = range.IsGreater ? 0 : 1;
            tboxDeclinationValue.Text = range.IntervalValue.ToString();
            ddlIntervalType.SelectedValue = ((Int32)range.DateTimeIntervalValue).ToString();
        }

        fltTemplate.LoadState(savedState);
    }

    public String GetID()
    {
        return fltTemplate.GetID();
    }

    private RangeDateTime GetDateTimeRange()
    {
        CheckValuesOnEmpty();
        if (rbtnDateInterval.Checked)
        {
            DateTime start = DateTime.MinValue;
            if (!String.IsNullOrEmpty(tboxDateFrom.Text) && !String.IsNullOrEmpty(tboxTimeFrom.Text))
            {
                DateTime.TryParse(tboxDateFrom.Text + " " + tboxTimeFrom.Text, out start);
            }
            CalendarExtender1.SelectedDate = start.Date;
            DateTime stop = DateTime.MaxValue;
            if (!String.IsNullOrEmpty(tboxDateTo.Text) && !String.IsNullOrEmpty(tboxTimeTo.Text))
            {
                DateTime.TryParse(tboxDateTo.Text + " " + tboxTimeTo.Text, out stop);
            }
            CalendarExtender2.SelectedDate = stop.Date;
            return new RangeDateTime(start, stop);
        }
        else
        {
            Int32 intervalValue = 0;
            if (!String.IsNullOrEmpty(tboxDeclinationValue.Text))
            {
                Int32.TryParse(tboxDeclinationValue.Text, out intervalValue);
            }
            Int32 intervalType = 0;
            Int32.TryParse(ddlIntervalType.SelectedValue, out intervalType);

            return new RangeDateTime(ddlDeclinationDirection.SelectedIndex == 0, intervalValue, (DateTimeInterval)intervalType);
        }
    }

    private void CheckValuesOnEmpty()
    {
        if (String.IsNullOrEmpty(tboxTimeFrom.Text)) tboxTimeFrom.Text = "00:00";
        if (String.IsNullOrEmpty(tboxTimeTo.Text)) tboxTimeTo.Text = "00:00";
        if (String.IsNullOrEmpty(tboxDateFrom.Text))
        {
            CalendarExtender1.SelectedDate = DateTime.Now.AddDays(1.0 - (Double)DateTime.Now.Day);
            tboxDateFrom.Text = String.Format("{0:dd}.{0:MM}.{0:yyyy}", CalendarExtender1.SelectedDate);
        }
        if (String.IsNullOrEmpty(tboxDateTo.Text))
        {
            CalendarExtender2.SelectedDate = DateTime.Now;
            tboxDateTo.Text = String.Format("{0:dd}.{0:MM}.{0:yyyy}", CalendarExtender2.SelectedDate);
        }
        if (String.IsNullOrEmpty(tboxDeclinationValue.Text)) tboxDeclinationValue.Text = "1";
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