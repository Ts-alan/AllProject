using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
//!- ОСТОРОЖНО! ГОВНОКОД!
namespace DateControl
{
	/// <summary>
	/// Неплохо бы отрефакторить или полностью переписать этот говноконтрол
	/// </summary>
	public class DateCustomControl : System.Web.UI.WebControls.WebControl,INamingContainer
	{
		#region variables

		private Label lblFrom = new Label();
		private Label lblTo = new Label();

		private DropDownList ddlFromDay = new DropDownList();
		private DropDownList ddlFromMonth = new DropDownList();
        private DropDownList ddlFromYear = new DropDownList();
        private DropDownList ddlFromHour = new DropDownList();
        private DropDownList ddlFromMinute = new DropDownList();
       // private DropDownList ddlFromSecond = new DropDownList();

		private DropDownList ddlToDay = new DropDownList();
		private DropDownList ddlToMonth = new DropDownList();
		private DropDownList ddlToYear = new DropDownList();
        private DropDownList ddlToHour = new DropDownList();
        private DropDownList ddlToMinute = new DropDownList();
       // private DropDownList ddlToSecond = new DropDownList();

        private DropDownList ddlInterval = new DropDownList();
        private CheckBox cboxInterval = new CheckBox();

        private DropDownList ddlIntervalMode = new DropDownList();


		private int fday_index=0;
		private int fmonth_index=0;
		private int fyear_index=0;
        private int fhour_index = 0;
        private int fminute_index = 0;
        //private int fsecond_index = 0;

		private int tday_index=0;
		private int tmonth_index=0;
		private int tyear_index=0;
        private int thour_index = 0;
        private int tminute_index = 0;
        //private int tsecond_index = 0;
        private int _intervalIndex = 0;

        public int IntervalIndex
        {
            get { return ddlInterval.SelectedIndex; }
            set {
                _intervalIndex = value;
                ddlInterval.SelectedIndex = value; 
            }
        }

        //private bool _isIntervalUse = false;

        public bool IsIntervalUse
        {
            get { return cboxInterval.Checked; }
            set { cboxInterval.Checked = value; }
        }

		private object fday;
		private object fmonth;
		private object fyear;
        private object fhour;
        private object fminute;
        //private object fsecond;

		private object tday;
		private object tmonth;
		private object tyear;
        private object thour;
        private object tminute;
        //private object tsecond;

        private object interval;

        public object Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                DrawControls();
            }
        }

		private string toText;
		private string fromText;


        private bool _renderInterval = false;

        public bool RenderInterval
        {
            get { return _renderInterval; }
            set { _renderInterval = value; }
        }

        private object _intervalMode;

        public object IntervalMode
        {
            get { return _intervalMode; }
            set
            {
                _intervalMode = value;
                DrawControls();
            }
        }

        private int _intervalModeIndex = 0;

        public int IntervalModeIndex
        {
            get 
            { 
                return ddlIntervalMode.SelectedIndex; 
            }
            set 
            {
                ddlIntervalMode.SelectedIndex = value;
                _intervalModeIndex = value;
            }
        }


		#endregion

		/// <summary>
		/// Create controls in Paging control
		/// </summary>
		protected override void CreateChildControls()
		{
			//Add controls

            Label lbl1 = new Label();
            lbl1.Text = "<table border=0><tr><td>";
            Controls.Add(lbl1);

            Controls.Add(lblFrom);
    
            Label lbl2 = new Label();
            if(!RenderInterval)
                lbl2.Text = "</td><td>";
            else
                lbl2.Text = "</td><td colspan=2>";    
            Controls.Add(lbl2);

            Controls.Add(ddlFromYear);
            Controls.Add(ddlFromMonth);
			Controls.Add(ddlFromDay);

            Label lblFromSpace = new Label();
            lblFromSpace.Text = "&nbsp;&nbsp;";
            Controls.Add(lblFromSpace);

            Controls.Add(ddlFromHour);
            Controls.Add(ddlFromMinute);
            //Controls.Add(ddlFromSecond);

            Label lbl3 = new Label();
            lbl3.Text = "</td></tr><tr><td>";
            Controls.Add(lbl3);

            Controls.Add(lblTo);

            Label lbl4 = new Label();

            if (!RenderInterval)
                lbl4.Text = "</td><td>";
            else
                lbl4.Text = "</td><td colspan=2>";    

            Controls.Add(lbl4);

            Controls.Add(ddlToYear);
            Controls.Add(ddlToMonth);
			Controls.Add(ddlToDay);

            Label lblToSpace = new Label();
            lblToSpace.Text = "&nbsp;&nbsp;";
            Controls.Add(lblToSpace);

            Controls.Add(ddlToHour);
            Controls.Add(ddlToMinute);
            //Controls.Add(ddlToSecond);

            if (RenderInterval)
            {
                Controls.Add(new LiteralControl("</td></tr><tr><td>"));
                cboxInterval.Text = "";
                Controls.Add(cboxInterval);
                Controls.Add(new LiteralControl("</td><td>"));
                ddlIntervalMode.Style.Add("width","100%");
                Controls.Add(ddlIntervalMode);
                Controls.Add(new LiteralControl("</td><td>"));
                ddlInterval.Style.Add("width", "100%");
                Controls.Add(ddlInterval);
            }

            Label lbl5 = new Label();
            lbl5.Text = "</td></tr></table>";
            Controls.Add(lbl5);
			
			DrawControls();
		}

		public void CheckDateTime()
		{
            //DrawControls();
            if(DateTime.Compare(this.GetDateFrom(),
                this.GetDateTo()) > 0) throw new ArgumentException("Invalid value: DateTime");

		}

        public string ddlSkinID
        {
            set
            {
                EnsureChildControls();
                ddlFromDay.SkinID = value;
                ddlFromMonth.SkinID = value;
                ddlFromYear.SkinID = value;
                ddlFromHour.SkinID = value;
                ddlFromMinute.SkinID = value;
                //ddlFromSecond.SkinID = value;

                ddlToDay.SkinID = value;
                ddlToMonth.SkinID = value;
                ddlToYear.SkinID = value;
                ddlToHour.SkinID = value;
                ddlToMinute.SkinID = value;

                ddlInterval.SkinID = value;
                ddlIntervalMode.SkinID = value;
                //ddlToSecond.SkinID = value;
            }
        }

		public DateTime GetDateFrom()
		{
			return new DateTime(Convert.ToInt32(ddlFromYear.SelectedValue),
				Convert.ToInt32(ddlFromMonth.SelectedIndex)+1,Convert.ToInt32(ddlFromDay.SelectedValue),
                Convert.ToInt32(ddlFromHour.SelectedValue), Convert.ToInt32(ddlFromMinute.SelectedValue),0 /*Convert.ToInt32(ddlFromSecond.SelectedValue)*/);
		}

		public DateTime GetDateTo()
		{
			return new DateTime(Convert.ToInt32(ddlToYear.SelectedValue),
				Convert.ToInt32(ddlToMonth.SelectedIndex)+1,Convert.ToInt32(ddlToDay.SelectedValue),
                Convert.ToInt32(ddlToHour.SelectedValue), Convert.ToInt32(ddlToMinute.SelectedValue), 0/*Convert.ToInt32(ddlToSecond.SelectedValue)*/);
		}

		public void SetDateFrom(DateTime dateFrom, string firstYear)
		{

			int startYear = Convert.ToInt32(firstYear);

			fday_index = dateFrom.Day - 1;
			fmonth_index = dateFrom.Month - 1;
			fyear_index = dateFrom.Year - startYear;

            fhour_index = dateFrom.Hour;
            fminute_index = dateFrom.Minute;
            //fsecond_index = dateFrom.Second;
            
            DrawControls();
		}

        public void SetDateInterval(int index)
        {
            ddlInterval.SelectedIndex = index;
        }

        public void SetDateIntervalMode(int index)
        {
            ddlIntervalMode.SelectedIndex = index;
        }

		public void SetDateTo(DateTime dateTo, string firstYear)
		{

			int startYear = Convert.ToInt32(firstYear);

			tday_index = dateTo.Day - 1;
			tmonth_index = dateTo.Month - 1;
			tyear_index = dateTo.Year - startYear;

            thour_index = dateTo.Hour;
            tminute_index = dateTo.Minute;
            //tsecond_index = dateTo.Second;

            DrawControls();

		}

		private void DrawControls()
		{
         
			// data source
			ddlFromDay.DataSource = fday;
			ddlFromDay.SelectedIndex = fday_index;
			ddlFromDay.DataBind();

			ddlFromMonth.DataSource = fmonth;
			ddlFromMonth.SelectedIndex = fmonth_index;
			ddlFromMonth.DataBind();

			ddlFromYear.DataSource = fyear;
			ddlFromYear.SelectedIndex = fyear_index;
			ddlFromYear.DataBind();

            ddlFromHour.DataSource = fhour;
            ddlFromHour.SelectedIndex = fhour_index;
            ddlFromHour.DataBind();

            ddlFromMinute.DataSource = fminute;
            ddlFromMinute.SelectedIndex = fminute_index;
            ddlFromMinute.DataBind();

            //ddlFromSecond.DataSource = fsecond;
            //ddlFromSecond.SelectedIndex = fsecond_index;
            //ddlFromSecond.DataBind();

			ddlToDay.DataSource = tday;
			ddlToDay.SelectedIndex = tday_index;
			ddlToDay.DataBind();

			ddlToMonth.DataSource = tmonth;
			ddlToMonth.SelectedIndex = tmonth_index;
			ddlToMonth.DataBind();

			ddlToYear.DataSource = tyear;
			ddlToYear.SelectedIndex = tyear_index;
			ddlToYear.DataBind();

            ddlToHour.DataSource = thour;
            ddlToHour.SelectedIndex = thour_index;
            ddlToHour.DataBind();

            ddlToMinute.DataSource = tminute;
            ddlToMinute.SelectedIndex = tminute_index;
            ddlToMinute.DataBind();

            //ddlToSecond.DataSource = tsecond;
            //ddlToSecond.SelectedIndex = tsecond_index;
            //ddlToSecond.DataBind();

            ddlInterval.DataSource = interval;
            ddlInterval.SelectedIndex = _intervalIndex;
            ddlInterval.DataBind();

            ddlIntervalMode.DataSource = _intervalMode;
            ddlIntervalMode.SelectedIndex = _intervalModeIndex;
            ddlIntervalMode.DataBind();

            //cboxInterval.Checked = _isIntervalUse;

			lblFrom.Text = fromText;
			lblTo.Text = toText;

		}

		#region property
		
		//set datasource to drop down list objects
		public object FromDayObject
		{
            get { return fday; }
			set {fday = value;}
		}

		public object FromMonthObject
		{
            get { return fmonth; }
			set {fmonth = value;}
		}

		public object FromYearObject
		{
            get { return fyear; }
			set {fyear = value;}
		}

        public object FromHourObject
        {
            get { return fhour; }
            set { fhour = value; }
        }

        public object FromMinuteObject
        {
            get { return fminute; }
            set { fminute = value; }
        }

        //public object FromSecondObject
        //{
        //    get { return fsecond; }
        //    set { fsecond = value; }
        //}

		public object ToDayObject
		{
            get { return tday; }
			set {tday= value;}
		}

		public object ToMonthObject
		{
            get { return tmonth; }
			set {tmonth = value;}
		}

		public object ToYearObject
		{
            get { return tyear; }
			set {tyear = value;}
		}


        public object ToHourObject
        {
            get { return thour; }
            set { thour = value; }
        }

        public object ToMinuteObject
        {
            get { return tminute; }
            set { tminute = value; }
        }

        //public object ToSecondObject
        //{
        //    get { return tsecond; }
        //    set { tsecond = value; }
        //}

		//Set text to label
		public string SetFromText
		{
            set { fromText = value + "&nbsp;"; }
		}

		public string SetToText
		{
            set { toText =  value + "&nbsp;"; }
		}

		//index property's

		public int FromDayIndex
		{
			set {fday_index = value;}
		}

		public int FromMonthIndex
		{
			set {fmonth_index = value;}
		}

		public int FromYearIndex
		{
			set {fyear_index = value;}
		}

        public int FromHourIndex
        {
            set { fhour_index = value; }
        }

        public int FromMinuteIndex
        {
            set { fminute_index = value; }
        }

        //public int FromSecondIndex
        //{
        //    set { fsecond_index = value; }
        //}

		public int ToDayIndex
		{
			set {tday_index = value;}
		}

		public int ToMonthIndex
		{
			set {tmonth_index = value;}
		}

		public int ToYearIndex
		{
			set {tyear_index = value;}
		}

        public int ToHourIndex
        {
            set { thour_index = value; }
        }

        public int ToMinuteIndex
        {
            set { tminute_index = value; }
        }

        //public int ToSecondIndex
        //{
        //    set { tsecond_index = value; }
        //}

		#endregion

		#region ViewState
		protected override object SaveViewState()
		{
            //EnsureChildControls();
			object baseState = base.SaveViewState();
			object[] allStates = new object[32];
			allStates[0] = baseState;
			
			allStates[1] = fday_index;
			allStates[2] = fmonth_index;
			allStates[3] = fyear_index;
			allStates[4] = tday_index;
			allStates[5] = tmonth_index;
			allStates[6] = tyear_index;

			allStates[7] = fday;
			allStates[8] = fmonth;
			allStates[9] = fyear;
			allStates[10] = tday;
			allStates[11] = tmonth;
			allStates[12] = tyear;
			allStates[13] = fromText;
			allStates[14] = toText;

            allStates[15] = fhour_index;
            allStates[16] = fminute_index;
            //allStates[17] = fsecond_index;
            allStates[18] = thour_index;
            allStates[19] = tminute_index;
            //allStates[20] = tsecond_index;
            allStates[21] = fhour;
            allStates[22] = fminute;
            //allStates[23] = fsecond;
            allStates[24] = thour;
            allStates[25] = tminute;
            //allStates[26] = tsecond;
            allStates[27] = interval;
            allStates[28] = _intervalIndex;
            allStates[29] = cboxInterval.Checked;
            allStates[30] = _intervalMode;
            allStates[31] = _intervalModeIndex;
			return allStates;
		}

		protected override void LoadViewState(object savedState)
		{
            if (savedState != null)
            {
                object[] myState = (object[])savedState;
                if (myState[0] != null)
                    base.LoadViewState(myState[0]);
                if (myState[1] != null)
                    fday_index = (int)myState[1];
                if (myState[2] != null)
                    fmonth_index = (int)myState[2];
                if (myState[3] != null)
                    fyear_index = (int)myState[3];
                if (myState[4] != null)
                    tday_index = (int)myState[4];
                if (myState[5] != null)
                    tmonth_index = (int)myState[5];
                if (myState[6] != null)
                    tyear_index = (int)myState[6];
                if (myState[7] != null)
                    fday = myState[7];
                if (myState[8] != null)
                    fmonth = myState[8];
                if (myState[9] != null)
                    fyear = myState[9];
                if (myState[10] != null)
                    tday = myState[10];
                if (myState[11] != null)
                    tmonth = myState[11];
                if (myState[12] != null)
                    tyear = myState[12];
                if (myState[13] != null)
                    fromText = (string)myState[13];
                if (myState[14] != null)
                    toText = (string)myState[14];

                if (myState[15] != null)
                    fhour_index = (int)myState[15];
                if (myState[16] != null)
                    fminute_index = (int)myState[16];
                //if (myState[17] != null)
                //    fsecond_index = (int)myState[17];
                if (myState[18] != null)
                    thour_index = (int)myState[18];
                if (myState[19] != null)
                    tminute_index = (int)myState[19];
                //if (myState[20] != null)
                //    tsecond_index = (int)myState[20];
                if (myState[21] != null)
                    fhour = myState[21];
                if (myState[22] != null)
                    fminute = myState[22];
                //if (myState[23] != null)
                //     fsecond= myState[23];
                if (myState[24] != null)
                    thour = myState[24];
                if (myState[25] != null)
                    tminute = myState[25];
                //if (myState[26] != null)
                //    tsecond = myState[26];

                if (myState[27] != null)
                    interval = myState[27];
                if (myState[28] != null)
                    _intervalIndex = (int) myState[28];
                if (myState[29] != null)
                    cboxInterval.Checked = (bool)myState[29];

                if (myState[30] != null)
                    _intervalMode = myState[30];
                if (myState[31] != null)
                   _intervalModeIndex = (int)myState[31];
            }
            DrawControls();
		}
		#endregion

        /// <summary>
        /// Clear state
        /// </summary>
        public void Clear()
        {
            try
            {
                ddlFromDay.SelectedIndex = DateTime.Now.Day-1;
                ddlFromHour.SelectedIndex = DateTime.Now.Hour;
                ddlFromMinute.SelectedIndex = DateTime.Now.Minute;
                if (DateTime.Now.Month != 1)
                {
                    ddlFromMonth.SelectedIndex = DateTime.Now.Month - 2;
                    ddlFromYear.SelectedIndex = 1;
                }
                else
                {
                    ddlFromMonth.SelectedIndex = 11;
                    ddlFromYear.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("DateControl.Clear():: From : "+ex);
            }

            try
            {
                ddlToDay.SelectedIndex = DateTime.Now.Day - 1;
                ddlToHour.SelectedIndex = DateTime.Now.Hour;
                ddlToMinute.SelectedIndex = DateTime.Now.Minute;
                ddlToMonth.SelectedIndex = DateTime.Now.Month - 1;
                ddlToYear.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("DateControl.Clear():: To : " + ex);
            }

            try
            {
                ddlInterval.SelectedIndex = 0;
                ddlIntervalMode.SelectedIndex = 0;
                cboxInterval.Checked = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("DateControl.Clear():: Interval : " + ex);
            }
        }
		
		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			//check, exist controls and create...
			EnsureChildControls();
			base.Render(output);
		}
	}

}
