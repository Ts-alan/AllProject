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

using System.Globalization;
using System.ServiceProcess;
using System.Drawing;
using System.Text;

using Microsoft.Win32;

using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Filters;

using VirusBlokAda.CC.Settings.Common;
using VirusBlokAda.CC.Settings.Entities;

public partial class ControlCenter : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("Administrator"))
        {
            Response.Redirect("Default.aspx");
        }

        RegisterScript(@"js/jQuery/jquery.cookie.js");

        Page.Title = Resources.Resource.SettingsForMaintenanceService;
        if (!IsPostBack)
        {
            InitFields();
        }

        if (cboxMaintenanceEnabled.Checked)
            tboxServer.Enabled = ddlDay.Enabled = ddlEvery.Enabled = ddlTime.Enabled = true;
        else
            tboxServer.Enabled = ddlDay.Enabled = ddlEvery.Enabled = ddlTime.Enabled = false;

        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
    }

    /// <summary>
    /// Initialization fields
    /// </summary>
    protected override void InitFields()
    {
        cboxMaintenanceEnabled.Text = Resources.Resource.MaintenanceEnable;
        rangeComputersDaysToDelete.ErrorMessage = String.Format(Resources.Resource.ValueBetween, 0, 360);
        RangeTasksDaysToDelete.ErrorMessage = String.Format(Resources.Resource.ValueBetween, 0, 360);
        RangeDaysToDelete.ErrorMessage = String.Format(Resources.Resource.ValueBetween, 0, 360);
        RangeDeliveryTimeoutCheck.ErrorMessage = String.Format(Resources.Resource.ValueBetween, 60, 14400);
        RegularExpressionServer.ValidationExpression = RegularExpressions.IPAddress;

        InitSettings();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            ImageButton im = e.CommandSource as ImageButton;
            EventTypesEntity event_;
            switch ((String)e.CommandArgument)
            {
                case "Send":
                    
                    event_ = new EventTypesEntity(Convert.ToInt16(im.Attributes["eventID"]),
                            "", "", im.ImageUrl.Contains("disabled.gif"), false, false);
                    DBProviders.Event.UpdateSend(event_);

                    break;

                case "NoDelete":
                    event_ = new EventTypesEntity(Convert.ToInt16(im.Attributes["eventID"]),
                            "", "", false, im.ImageUrl.Contains("disabled.gif"), false);
                    DBProviders.Event.UpdateNoDelete(event_);
                    break;

            }

            GridView1.DataBind();
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            EventTypesEntity ent = (EventTypesEntity)e.Row.DataItem;

            Color clr = Color.FromName(ent.Color);
            Color clr2 = Color.FromArgb((Byte)~clr.R, (Byte)~clr.G, (Byte)~clr.B);

            e.Row.Cells[2].Attributes.Add("style", "color:" + "#" + clr2.R.ToString()
                + clr2.G.ToString() + clr2.B.ToString() + ";" + "background-color:" + clr.Name);

            (e.Row.Cells[0].FindControl("ibtnSend") as ImageButton).ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + (ent.Send ? "enabled.gif" : "disabled.gif");
            (e.Row.Cells[1].FindControl("ibtnNoDelete") as ImageButton).ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + (ent.NoDelete ? "enabled.gif" : "disabled.gif");
        }
    }

    #region Settings

    private void InitSettings()
    {
        PMSSettingsEntity settings = VirusBlokAda.CC.Settings.SettingsProvider.GetPMSSettings();

        //Labels
        if (settings.LastSelectDate == null)
            lblLastSelectDate.Text = Resources.Resource.NotAvailable;
        else
            lblLastSelectDate.Text = settings.LastSelectDate.ToString();

        if (settings.LastSendDate == null)
            lblLastSendDate.Text = Resources.Resource.NotAvailable;
        else
            lblLastSendDate.Text = settings.LastSendDate.ToString();

        if (settings.NextSendDate == null)
            lblNextSendDate.Text = Resources.Resource.NotAvailable;
        else
            lblNextSendDate.Text = settings.NextSendDate.ToString();

        //TextBoxes
        tboxServer.Text = settings.Server;

        if (settings.DeliveryTimeoutCheck == null)
            tboxDeliveryTimeoutCheck.Text = Resources.Resource.NotAvailable;
        else
            tboxDeliveryTimeoutCheck.Text = settings.DeliveryTimeoutCheck.ToString();


        cboxMaintenanceEnabled.Checked = tboxServer.Enabled = ddlDay.Enabled = ddlEvery.Enabled = ddlTime.Enabled = settings.MaintenanceEnabled;

        if (settings.DaysToDelete == null)
            tboxDaysToDelete.Text = Resources.Resource.NotAvailable;
        else
            tboxDaysToDelete.Text = settings.DaysToDelete.ToString();

        if (settings.TaskDaysToDelete == null)
            tboxTasksDaysToDelete.Text = Resources.Resource.NotAvailable;
        else
            tboxTasksDaysToDelete.Text = settings.TaskDaysToDelete.ToString();

        if (settings.ComputerDaysToDelete == null)
            tboxComputersDaysToDelete.Text = Resources.Resource.NotAvailable;
        else
            tboxComputersDaysToDelete.Text = settings.ComputerDaysToDelete.ToString();


        if (ddlEvery.Items.Count == 0)
        {
            ddlEvery.Items.Add(Resources.Resource.Daily);
            ddlEvery.Items.Add(Resources.Resource.Weekly);
            ddlEvery.Items.Add(Resources.Resource.Monthly);
            ddlEvery.Items.Add(Resources.Resource.Once);
        }
        if (ddlTime.Items.Count == 0)
        {
            for (Int32 i = 0; i <= 23; i++)
                ddlTime.Items.Add(i.ToString());
        }

        ddlEvery.SelectedIndex = (Int32)settings.DataSendInterval;
        InitDdlList(ddlEvery.SelectedIndex);
        if (settings.MaintenanceEnabled)
        {
            switch (ddlEvery.SelectedIndex)
            {
                case 0:
                    ddlTime.SelectedIndex = DateTime.Now.Hour;
                    break;

                case 1:
                    ddlDay.SelectedIndex = (Int32)DateTime.Now.DayOfWeek;
                    ddlTime.SelectedIndex = DateTime.Now.Hour;
                    break;

                case 2:
                    ddlDay.SelectedIndex = DateTime.Now.Day - 1;
                    ddlTime.SelectedIndex = DateTime.Now.Hour;
                    break;
                case 3:
                    if (settings.HourIntervalToSend == null)
                        ddlTime.SelectedIndex = 0;
                    else
                        ddlTime.SelectedIndex = (Int32)settings.HourIntervalToSend - 1;
                    break;
            }
        }
    }

    private Boolean ValidateFields()
    {
        if (cboxMaintenanceEnabled.Checked)
        {
            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(tboxServer.Text, out ip))
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": " +
                    Resources.Resource.IPAddress);
        }

        Validation vld = new Validation(tboxDeliveryTimeoutCheck.Text);
        if ((!vld.CheckUInt16()) || (Int16.Parse(vld.Value) < 60))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
             + Resources.Resource.MaintenanceInterval);


        vld = new Validation(tboxDaysToDelete.Text);
        if (!vld.CheckUInt16())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
             + Resources.Resource.DaysToDelete);

        vld = new Validation(tboxTasksDaysToDelete.Text);
        if (!vld.CheckUInt16())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
             + Resources.Resource.TaskDaysToDelete);

        vld = new Validation(tboxComputersDaysToDelete.Text);
        if (!vld.CheckUInt16())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
             + Resources.Resource.ComputersDaysToDelete);

        return true;
    }

    protected void lbtnSaveBoxes_Click(object sender, EventArgs e)
    {
        PMSSettingsEntity settings = null;
        String message = null;
        try
        {
            settings = GetPMSSettings();
        }
        catch (ArgumentException ex)
        {
            message = ex.Message;
        }

        if (settings != null)
        {
            message = Resources.Resource.SuccessStatus;

            Boolean retVal = false;
            try
            {
                IVba32Settings remoteObject = (IVba32Settings)Activator.GetObject(typeof(IVba32Settings), ConfigurationManager.AppSettings["Vba32SS"]);

                retVal = remoteObject.ChangeRegistry(settings.GenerateXML());

                if (!retVal)
                    message = "Reread: Vba32SS return false!";
            }
            catch
            {
                message = Resources.Resource.Vba32SSUnavailable;
            }

            InitSettings();
        }

        String key = "SaveSettingsCallbackScript";
        String script = "alert('" + message + "');";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);
    }

    private PMSSettingsEntity GetPMSSettings()
    {
        ValidateFields();

        PMSSettingsEntity ent = new PMSSettingsEntity();

        ent.DeliveryTimeoutCheck = Convert.ToInt32(tboxDeliveryTimeoutCheck.Text);

        ent.DaysToDelete = Convert.ToInt32(tboxDaysToDelete.Text);
        ent.ComputerDaysToDelete = Convert.ToInt32(tboxComputersDaysToDelete.Text);
        ent.TaskDaysToDelete = Convert.ToInt32(tboxTasksDaysToDelete.Text);

        ent.MaintenanceEnabled = cboxMaintenanceEnabled.Checked;
        if (ent.MaintenanceEnabled)
        {
            ent.Server = tboxServer.Text;
            ent.DataSendInterval = ddlEvery.SelectedIndex;

            if (ddlEvery.SelectedIndex == 3)
                ent.HourIntervalToSend = ddlTime.SelectedIndex + 1;

            //Высчитываем время следующего обновления
            DateTime now = DateTime.Now;
            DateTime dt = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            switch (ddlEvery.SelectedIndex)
            {
                case 0:
                    //ежедневно
                    if (now.Hour >= ddlTime.SelectedIndex)
                        dt = dt.AddDays(1);
                    break;

                case 1:
                    //еженедельно
                    for (Int32 i = 0; i < 7; i++)
                    {
                        if (((Int32)now.DayOfWeek) == ddlDay.SelectedIndex)
                            break;
                        dt = dt.AddDays(1);
                    }
                    break;

                case 2:
                    //ежемесячно
                    dt = new DateTime(now.Year, now.Month,
                        ddlDay.SelectedIndex + 1, ddlTime.SelectedIndex, 0, 0);
                    if (dt.Day < now.Day || (dt.Day == now.Day && dt.Hour <= now.Hour))
                        dt = dt.AddMonths(1);
                    break;

                case 3:
                    //раз в х часов
                    dt = dt.AddHours(ddlTime.SelectedIndex + 1);
                    break;
            }
            ent.NextSendDate = dt;
        }

        ent.ReRead = true;

        return ent;
    }

    protected void ddlEvery_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitDdlList(ddlEvery.SelectedIndex);
    }

    private void InitDdlList(Int32 selectedItem)
    {
        ddlDay.Items.Clear();
        switch (selectedItem)
        {
            case 0:
                ddlDay.Visible = false;
                lblIn.Visible = true;
                if (ddlTime.Items[0].Text != "0")
                {
                    ddlTime.Items.Insert(0, "0");
                }
                break;
            case 1:
                foreach (String day in System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DayNames)
                {
                    if (!String.IsNullOrEmpty(day))
                        ddlDay.Items.Add(day);
                }
                ddlDay.Visible = true;
                lblIn.Visible = true;
                if (ddlTime.Items[0].Text != "0")
                {
                    ddlTime.Items.Insert(0, "0");
                }
                break;
            case 2:
                for (Int32 i = 1; i <= 28; i++)
                    ddlDay.Items.Add(i.ToString());
                ddlDay.Visible = true;
                lblIn.Visible = true;
                if (ddlTime.Items[0].Text != "0")
                {
                    ddlTime.Items.Insert(0, "0");
                }
                break;
            case 3:
                if (ddlTime.Items[0].Text == "0")
                {
                    ddlTime.Items.RemoveAt(0);
                }
                ddlDay.Visible = false;
                lblIn.Visible = false;
                break;
        }
    }

    #endregion
}
