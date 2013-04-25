using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Globalization;

using ARM2_dbcontrol.Filters;

public partial class Controls_CompFiltersDate : System.Web.UI.UserControl
{
    /*protected void Page_PreInit(object sender, EventArgs e)
    {
        InitFields();
    }*/

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFields();

        }

    }
    /// <summary>
    /// Init date control: set list of date, text..
    /// </summary>
    private void InitFields()
    {
       if (dccRecentActive.FromMonthObject != null) return;

       List<string> monthList = new List<string>();
        foreach (string month in CultureInfo.CurrentCulture.DateTimeFormat.MonthNames)
            if (month != String.Empty)
                monthList.Add(month);
      
        dccRecentActive.FromMonthObject = monthList;
        dccRecentActive.ToMonthObject = monthList;

        dccLatestInfected.FromMonthObject = monthList;
        dccLatestInfected.ToMonthObject = monthList;

        dccLatestUpdate.FromMonthObject = monthList;
        dccLatestUpdate.ToMonthObject = monthList;

        //set day list
        List<string> dayList = new List<string>();
        for (int i = 1; i <= 31; i++)
            dayList.Add(i.ToString());

        dccRecentActive.FromDayObject = dayList;
        dccRecentActive.ToDayObject = dayList;

        dccLatestInfected.FromDayObject = dayList;
        dccLatestInfected.ToDayObject = dayList;

        dccLatestUpdate.FromDayObject = dayList;
        dccLatestUpdate.ToDayObject = dayList;

        //set year list
        List<string> yearList = new List<string>();
        for (int i = 1; i > -2; i-- )
            yearList.Add(Convert.ToString(DateTime.Now.Year - i));

        dccRecentActive.FromYearObject = yearList;
        dccRecentActive.ToYearObject = yearList;

        dccLatestInfected.FromYearObject = yearList;
        dccLatestInfected.ToYearObject = yearList;

        dccLatestUpdate.FromYearObject = yearList;
        dccLatestUpdate.ToYearObject = yearList;


        List<string> hourList = new List<string>();
        for (int i = 0; i < 24; i++)
            hourList.Add(i.ToString());

        dccRecentActive.FromHourObject = hourList;
        dccRecentActive.ToHourObject = hourList;

        dccLatestInfected.FromHourObject = hourList;
        dccLatestInfected.ToHourObject = hourList;

        dccLatestUpdate.FromHourObject = hourList;
        dccLatestUpdate.ToHourObject = hourList;

        List<string> minuteList = new List<string>();
        for (int i = 0; i < 60; i++)
            minuteList.Add(i.ToString());

        dccRecentActive.FromMinuteObject = minuteList;
        dccRecentActive.ToMinuteObject = minuteList;

        //dccRecentActive.FromSecondObject = minuteList;
        //dccRecentActive.ToSecondObject = minuteList;

        dccLatestInfected.FromMinuteObject = minuteList;
        dccLatestInfected.ToMinuteObject = minuteList;

        //dccLatestInfected.FromSecondObject = minuteList;
        //dccLatestInfected.ToSecondObject = minuteList;

        dccLatestUpdate.FromMinuteObject = minuteList;
        dccLatestUpdate.ToMinuteObject = minuteList;

        //dccLatestUpdate.FromSecondObject = minuteList;
        //dccLatestUpdate.ToSecondObject = minuteList;

        //Set text
        dccRecentActive.SetFromText = Resources.Resource.From;
        dccRecentActive.SetToText = Resources.Resource.To;

        dccLatestInfected.SetFromText = Resources.Resource.From;
        dccLatestInfected.SetToText = Resources.Resource.To;

        dccLatestUpdate.SetFromText = Resources.Resource.From;
        dccLatestUpdate.SetToText = Resources.Resource.To;

        //!-OPTM AddMonth(-1)
        DateTime dt = DateTime.Now.AddMonths(-1);

        string lyear = ((int)(DateTime.Now.Year - 1)).ToString();
        dccLatestInfected.SetDateFrom(dt, lyear);
        dccLatestInfected.SetDateTo(DateTime.Now, lyear);

        dccLatestUpdate.SetDateFrom(dt, lyear);
        dccLatestUpdate.SetDateTo(DateTime.Now, lyear);

        dccRecentActive.SetDateFrom(dt, lyear);
        dccRecentActive.SetDateTo(DateTime.Now, lyear);


        List<string> list = Anchor.GetDateIntervals();

        dccLatestInfected.Interval = list;
        dccLatestUpdate.Interval = list;
        dccRecentActive.Interval = list;

        List<string> intervalTypes = new List<string>();
        intervalTypes.Add(Resources.Resource.HighDate);
        intervalTypes.Add(Resources.Resource.LowDate);

        dccLatestInfected.IntervalMode = intervalTypes;
        dccLatestUpdate.IntervalMode = intervalTypes;
        dccRecentActive.IntervalMode = intervalTypes;
        
        //cbox label:
        cboxLatestInfected.Text = Resources.Resource.LatestInfected;
        cboxLatestUpdate.Text = Resources.Resource.LatestUpdate;
        cboxRecentActive.Text = Resources.Resource.RecentActive;


        List<string> terms = new List<string>();
        terms.Add("AND");
        terms.Add("OR");
        terms.Add("NOT");

        try
        {
            ddlTermRecentActive.DataSource = terms;
            ddlTermRecentActive.DataBind();
        }
        catch
        {
            ddlTermRecentActive.SelectedIndex = -1;
            ddlTermRecentActive.SelectedValue = null;
            ddlTermRecentActive.DataSource = terms;
            ddlTermRecentActive.DataBind();
        }

        try
        {
            ddlTermLatestInfected.DataSource = terms;
            ddlTermLatestInfected.DataBind();
        }
        catch
        {
            ddlTermLatestInfected.SelectedIndex = -1;
            ddlTermLatestInfected.SelectedValue = null;
            ddlTermLatestInfected.DataSource = terms;
            ddlTermLatestInfected.DataBind();
        }

        try
        {
            ddlTermLatestUpdate.DataSource = terms;
            ddlTermLatestUpdate.DataBind();

        }
        catch
        {
            ddlTermLatestUpdate.SelectedIndex = -1;
            ddlTermLatestUpdate.SelectedValue = null;
            ddlTermLatestUpdate.DataSource = terms;
            ddlTermLatestUpdate.DataBind();

        }

    }

    public void LoadFilter(CompFilterEntity filter)
    {
        InitFields();
        string startYear = Convert.ToString(DateTime.Now.Year - 1);

        if ((filter.RecentActiveFrom != DateTime.MinValue) ||
            (filter.RecentActiveTo != DateTime.MinValue))
        {

            cboxRecentActive.Checked = true;
            dccRecentActive.SetDateFrom(filter.RecentActiveFrom, startYear);
            dccRecentActive.SetDateTo(filter.RecentActiveTo, startYear);

            dccRecentActive.CheckDateTime();

            ddlTermRecentActive.SelectedValue = filter.TermRecentActive;
        }

        if (filter.RecentActiveIntervalIndex != Int32.MinValue)
        {
            cboxRecentActive.Checked = true;
            dccRecentActive.IsIntervalUse = true;
            dccRecentActive.SetDateInterval(filter.RecentActiveIntervalIndex);
            dccRecentActive.SetDateIntervalMode(filter.RecentActiveIntervalModeIndex);
        }

        if ((filter.LatestInfectedFrom != DateTime.MinValue) ||
            (filter.LatestInfectedTo != DateTime.MinValue))
        {

            cboxLatestInfected.Checked = true;
            dccLatestInfected.SetDateFrom(filter.LatestInfectedFrom, startYear);
            dccLatestInfected.SetDateTo(filter.LatestInfectedTo, startYear);

            dccLatestInfected.CheckDateTime();

            ddlTermLatestInfected.SelectedValue = filter.TermLatestInfected;
        }

        if (filter.LatestInfectedIntervalIndex != Int32.MinValue)
        {
            cboxLatestInfected.Checked = true;
            dccLatestInfected.IsIntervalUse = true;
            dccLatestInfected.SetDateInterval(filter.LatestInfectedIntervalIndex);
            dccLatestInfected.SetDateIntervalMode(filter.LatestInfectedIntervalModeIndex);
        }

        if ((filter.LatestUpdateFrom != DateTime.MinValue) ||
            (filter.LatestUpdateTo != DateTime.MinValue))
        {
            cboxLatestUpdate.Checked = true;
            dccLatestUpdate.SetDateFrom(filter.LatestUpdateFrom, startYear);
            dccLatestUpdate.SetDateTo(filter.LatestUpdateTo, startYear);

            dccLatestUpdate.CheckDateTime();

            ddlTermLatestUpdate.SelectedValue = filter.TermLatestInfected;

        }

        if (filter.LatestUpdateIntervalIndex != Int32.MinValue)
        {
            cboxLatestUpdate.Checked = true;
            dccLatestUpdate.IsIntervalUse = true;
            dccLatestUpdate.SetDateInterval(filter.LatestUpdateIntervalIndex);
            dccLatestUpdate.SetDateIntervalMode(filter.LatestUpdateIntervalModeIndex);
        }

    }

    public void LoadFilter(string filterName)
    {
        InitFields();
        CompFilterCollection collection = (CompFilterCollection)Session["CompFilters"];
        CompFilterEntity filter = collection.Get(filterName);

        this.LoadFilter(filter);

  
    }

    public void GetCurrentStateFilter(ref CompFilterEntity fltr)
    {
            if (cboxLatestInfected.Checked)
            {
                if (dccLatestInfected.IsIntervalUse)
                {
                    fltr.LatestInfectedIntervalIndex = dccLatestInfected.IntervalIndex;
                    fltr.LatestInfectedIntervalModeIndex = dccLatestInfected.IntervalModeIndex;
                }
                else
                {

                    fltr.LatestInfectedFrom = dccLatestInfected.GetDateFrom();
                    fltr.LatestInfectedTo = dccLatestInfected.GetDateTo();

                    fltr.TermLatestInfected = ddlTermLatestInfected.SelectedValue;

                    dccLatestInfected.CheckDateTime();
                }
            }
            if (cboxLatestUpdate.Checked)
            {
                if (dccLatestUpdate.IsIntervalUse)
                {
                    fltr.LatestUpdateIntervalIndex = dccLatestUpdate.IntervalIndex;
                    fltr.LatestUpdateIntervalModeIndex = dccLatestUpdate.IntervalModeIndex;
                }
                else
                {

                    fltr.LatestUpdateFrom = dccLatestUpdate.GetDateFrom();
                    fltr.LatestUpdateTo = dccLatestUpdate.GetDateTo();

                    fltr.TermLatestUpdate = ddlTermLatestUpdate.SelectedValue;

                    dccLatestUpdate.CheckDateTime();

                }
            }
            if (cboxRecentActive.Checked)
            {

                if (dccRecentActive.IsIntervalUse)
                {
                    fltr.RecentActiveIntervalIndex = dccRecentActive.IntervalIndex;
                    fltr.RecentActiveIntervalModeIndex = dccRecentActive.IntervalModeIndex;
                }
                else
                {

                    fltr.RecentActiveFrom = dccRecentActive.GetDateFrom();
                    fltr.RecentActiveTo = dccRecentActive.GetDateTo();

                    fltr.TermRecentActive = ddlTermRecentActive.SelectedValue;

                    dccRecentActive.CheckDateTime();
                }
            }
       // return fltr;
    }

    public void Clear()
    {
        cboxLatestInfected.Checked = false;
        cboxLatestUpdate.Checked = false;
        cboxRecentActive.Checked = false;

        dccLatestInfected.Clear();
        dccRecentActive.Clear();
        dccLatestUpdate.Clear();


    }




}
