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

using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Filters;

using Vba32.ControlCenter.SettingsService;

public partial class ControlCenter : PageBase
{

    private const string GlobalEpidemyEvent = "vba32.cc.GlobalEpidemy";
    private const string LocalHearthEvent = "vba32.cc.LocalHearth";

    /*protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.MasterPageFile = Profile.MasterPage;
        Page.Theme = Profile.Theme;
    }

    protected override void InitializeCulture()
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo(Profile.Culture);
        base.InitializeCulture();
    }*/


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Roles.IsUserInRole("Administrator"))
        {
            Response.Redirect("Default.aspx");
        }

        RegisterScript(@"js/jQuery/jquery-1.3.2.js");
        RegisterScript(@"js/jQuery/ui.core.js");
        RegisterScript(@"js/jQuery/ui.tabs.js");
        RegisterScript(@"js/jQuery/jquery.cookie.js");

        RegisterLink("~/App_Themes/" + Profile.Theme + @"\ui.all.css");

        Page.Title = Resources.Resource.SettingsForMaintenanceService;
        if (!IsPostBack)
        {
            InitializeSession();
            InitFields();
            UpdateData();
        }

        if (cboxMaintenanceEnabled.Checked)
        {
            tboxServer.Enabled = ddlDay.Enabled = ddlEvery.Enabled = ddlTime.Enabled = true;
        }
        else
        {
            tboxServer.Enabled = ddlDay.Enabled = ddlEvery.Enabled = ddlTime.Enabled = false;
        }
        //MembershipUser user = Membership.GetUser("aidan");
        //user.ChangePassword(user.ResetPassword(), "1234qwer%");
    }

     /// <summary>
    /// Initialization fields
    /// </summary>
    protected override void InitFields()
    {

        pcPaging.CurrentPageIndex = 1;
        pcPaging.PageCount = 1;
        pcPaging.PageText = Resources.Resource.Page;
        pcPaging.OfText = Resources.Resource.Of;
        pcPaging.NextText = Resources.Resource.Next;
        pcPaging.PrevText = Resources.Resource.Prev;

        pcPaging.HomeText = Resources.Resource.HomePaging;
        pcPaging.LastText = Resources.Resource.LastPaging;

        pcPagingTop.CurrentPageIndex = 1;
        pcPagingTop.PageCount = 1;
        pcPagingTop.PageText = Resources.Resource.Page;
        pcPagingTop.OfText = Resources.Resource.Of;
        pcPagingTop.NextText = Resources.Resource.Next;
        pcPagingTop.PrevText = Resources.Resource.Prev;

        pcPagingTop.HomeText = Resources.Resource.HomePaging;
        pcPagingTop.LastText = Resources.Resource.LastPaging;

        cboxMaintenanceEnabled.Text = Resources.Resource.MaintenanceEnable;

        string strValue = String.Empty;
        DateTime dt = new DateTime();

        lbtnSaveBoxes.Text = Resources.Resource.Save;

        string registryControlCenterKeyName;
        if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
            registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
        else
            registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";


        RegistryKey key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName+ "PeriodicalMaintenance"); ;
        
        try
        {
            //Labels
            IFormatProvider culture = new CultureInfo("ru-RU");

            dt = Convert.ToDateTime(key.GetValue("LastSelectDate"), culture);
            if ((dt == null) || (dt == DateTime.MinValue))
                lblLastSelectDate.Text = Resources.Resource.NotAvailable;
            else
                lblLastSelectDate.Text = dt.ToString();

            dt = Convert.ToDateTime(key.GetValue("LastSendDate"), culture);
            if ((dt == null) || (dt == DateTime.MinValue))
                lblLastSendDate.Text = Resources.Resource.NotAvailable;
            else
                lblLastSendDate.Text = dt.ToString();

            dt = Convert.ToDateTime(key.GetValue("NextSendDate"), culture);
            if ((dt == null) || (dt == DateTime.MinValue))
                lblNextSendDate.Text = Resources.Resource.NotAvailable;
            else
                lblNextSendDate.Text = dt.ToString();

            //TextBoxes
            tboxServer.Text = (string)key.GetValue("Server");

            //object tmp = key.GetValue("Port");
            //if (tmp == null)
            //    tboxPort.Text = Resources.Resource.NotAvailable;
            //else
            //    tboxPort.Text = tmp.ToString();

            //tmp = key.GetValue("MaxFileLength");
            //if (tmp == null)
            //    tboxMaxFileLength.Text = Resources.Resource.NotAvailable;
            //else
            //    tboxMaxFileLength.Text = Convert.ToString((Convert.ToInt32(tmp) / 1024));

            object tmp = key.GetValue("DeliveryTimeoutCheck");
            if (tmp == null)
                tboxDeliveryTimeoutCheck.Text = Resources.Resource.NotAvailable;
            else
                tboxDeliveryTimeoutCheck.Text = tmp.ToString();


            tmp = key.GetValue("MaintenanceEnabled");
            if (tmp == null)
                tmp = true;

            cboxMaintenanceEnabled.Checked = Convert.ToBoolean(tmp);
            if (cboxMaintenanceEnabled.Checked)
            {
                tboxServer.Enabled = ddlDay.Enabled = ddlEvery.Enabled = ddlTime.Enabled = true;
            }
            else
            {
                tboxServer.Enabled = ddlDay.Enabled = ddlEvery.Enabled = ddlTime.Enabled = false;
            }

            tmp = key.GetValue("DaysToDelete");
            if (tmp == null)
                tboxDaysToDelete.Text = Resources.Resource.NotAvailable;
            else
                tboxDaysToDelete.Text = tmp.ToString();

            tmp = key.GetValue("TaskDaysToDelete");
            if (tmp == null)
                tboxTasksDaysToDelete.Text = Resources.Resource.NotAvailable;
            else
                tboxTasksDaysToDelete.Text = tmp.ToString();


            if (ddlEvery.Items.Count == 0)
            {

                ddlEvery.Items.Add(Resources.Resource.Daily);
                ddlEvery.Items.Add(Resources.Resource.Weekly);
                ddlEvery.Items.Add(Resources.Resource.Monthly);
                ddlEvery.Items.Add(Resources.Resource.Once);
            }
            if (ddlTime.Items.Count == 0)
            {
                for (int i = 0; i <= 23; i++)
                    ddlTime.Items.Add(i.ToString());                
            }

            ReadDdlSettings(key);

            key.Close();
        }
        catch
        {

        }

    }

    private void InitializeSession()
    {
        if (Session["ControlCenterSortExp"] == null)
            Session["ControlCenterSortExp"] = "EventName ASC";
    }

    private void UpdateData()
    {
        InitializeSession();
        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            EventTypesManager db = new EventTypesManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);

            string filter = "EventName like '%'";
            string sort = (string)Session["ControlCenterSortExp"];

           

            int count = db.Count(filter);
            int pageSize = 20;
            int pageCount = (int)Math.Ceiling((double)count / pageSize);

            pcPaging.PageCount = pageCount;
            pcPagingTop.PageCount = pageCount;

            dlEvents.DataSource = db.List(filter, sort, pcPaging.CurrentPageIndex, pageSize);

            dlEvents.DataBind();
            conn.CloseConnection();
        }
    }



    private void InitDdlList(int selectedItem)
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
                foreach (string day in System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DayNames)
                if (day != String.Empty)
                    ddlDay.Items.Add(day);
                ddlDay.Visible = true;
                lblIn.Visible = true;
                if (ddlTime.Items[0].Text != "0")
                {
                    ddlTime.Items.Insert(0, "0");
                }
                break;
            case 2:
                for (int i = 1; i <= 28; i++)
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

    private bool ValidateFields()
    {
        if (cboxMaintenanceEnabled.Checked)
        {
            Validation vld = new Validation(tboxDeliveryTimeoutCheck.Text);
            if ((!vld.CheckUInt16()) || (Int16.Parse(vld.Value) < 60))
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                 + Resources.Resource.MaintenanceInterval);


            vld = new Validation(tboxDaysToDelete.Text);
            if (!vld.CheckUInt16() || (Convert.ToUInt16(tboxDaysToDelete.Text)==0))
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                 + Resources.Resource.DaysToDelete);

            vld = new Validation(tboxTasksDaysToDelete.Text);
            if (!vld.CheckUInt16() || (Convert.ToUInt16(tboxTasksDaysToDelete.Text) == 0))
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                 + Resources.Resource.TaskDaysToDelete);
        }

        //vld.Value = tboxPort.Text;
        //if (!vld.CheckUInt32())
        //    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //     + Resources.Resource.Port);

        //vld.Value = tboxMaxFileLength.Text;
        //if (!vld.CheckUInt32())
        //    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //     + Resources.Resource.TemporaryFileSize);

        return true;
    }


    protected void dlEvents_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "SelectCommand")
        {
            switch ((string)e.CommandArgument)
            {

                case "Send":
                    using (VlslVConnection conn = new VlslVConnection(
                        ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
                    {
                        EventTypesManager db = new EventTypesManager(conn);

                        conn.OpenConnection();
                        conn.CheckConnectionState(true);

                        EventTypesEntity event_ = new EventTypesEntity(Convert.ToInt16((e.Item.FindControl("lblID") as Label).Text),
                                "", "", (e.Item.FindControl("ibtnSend") as ImageButton).ImageUrl.Contains("disabled.gif"), false,false);
                        db.UpdateSend(event_);
                        conn.CloseConnection();
                    }

                    break;

                case "NoDelete":
                    using (VlslVConnection conn = new VlslVConnection(
                        ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
                    {
                        EventTypesManager db = new EventTypesManager(conn);

                        conn.OpenConnection();
                        conn.CheckConnectionState(true);

                        EventTypesEntity event_ = new EventTypesEntity(Convert.ToInt16((e.Item.FindControl("lblID") as Label).Text),
                                "", "", false, (e.Item.FindControl("ibtnNoDelete") as ImageButton).ImageUrl.Contains("disabled.gif"),false);
                        db.UpdateNoDelete(event_);
                        conn.CloseConnection();
                    }
                    break;

            }
        }
        if (e.CommandName == "SortCommand")
        {
            if (Session["ControlCenterSortExp"] == null)
                Session["ControlCenterSortExp"] = e.CommandArgument.ToString() + " ASC";
            else
            {
                if (((string)Session["ControlCenterSortExp"]).Contains("ASC"))
                    Session["ControlCenterSortExp"] = e.CommandArgument.ToString() + " DESC";
                else
                    Session["ControlCenterSortExp"] = e.CommandArgument.ToString() + " ASC";
            }
        }
        UpdateData();
    }

   

    protected void dlEvents_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //Item
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            (e.Item.FindControl("lblName") as Label).Text = ((EventTypesEntity)e.Item.DataItem).EventName;
            
            Color clr = Color.FromName(((EventTypesEntity)e.Item.DataItem).Color);
            Color clr2 = Color.FromArgb((byte)~clr.R, (byte)~clr.G, (byte)~clr.B);

            (e.Item.FindControl("lblName") as Label).Attributes.Add("style", "color:" + "#" + clr2.R.ToString() 
                + clr2.G.ToString() + clr2.B.ToString() + ";" + "background-color:" + clr.Name);

            //if ((((EventTypesEntity)e.Item.DataItem).EventName != GlobalEpidemyEvent)
            //   && (((EventTypesEntity)e.Item.DataItem).EventName != LocalHearthEvent))
            {
                string strBool = "";
                if (((EventTypesEntity)e.Item.DataItem).Send)
                    strBool = "enabled.gif";
                else
                    strBool = "disabled.gif";

                (e.Item.FindControl("ibtnSend") as ImageButton).ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + strBool;

                if (((EventTypesEntity)e.Item.DataItem).NoDelete)
                    strBool = "enabled.gif";
                else
                    strBool = "disabled.gif";

                (e.Item.FindControl("ibtnNoDelete") as ImageButton).ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + strBool;
            }
            //else
            //{
            //    (e.Item.FindControl("ibtnSend") as ImageButton).Visible = false;
            //    (e.Item.FindControl("ibtnNoDelete") as ImageButton).Visible = false;
            //}
        }
        //Header
        if(e.Item.ItemType == ListItemType.Header)
        {
            (e.Item.FindControl("lbtnSend") as LinkButton).Text = Resources.Resource.Send;
                (e.Item.FindControl("lbtnNoDelete") as LinkButton).Text = Resources.Resource.NoDelete;
                (e.Item.FindControl("lbtnEventName") as LinkButton).Text = Resources.Resource.EventName;

                string currentSorting = (string)Session["ControlCenterSortExp"];
                string[] name = currentSorting.Split(' ');
                if (name[1] == "ASC")
                {
                    (e.Item.FindControl("lbtn" + name[0]) as LinkButton).Text += " \u2193";
                }
                else
                {
                    (e.Item.FindControl("lbtn" + name[0]) as LinkButton).Text += " \u2191";
                }
        }
    }

    protected void lbtnSaveBoxes_Click(object sender, EventArgs e)
    {
        if (ValidateFields())
        {

            //Высчитываем время следующего обновления
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                        DateTime.Now.Day, ddlTime.SelectedIndex, 0, 0);
            switch (ddlEvery.SelectedIndex)
            {
                case 0:
                    //ежедневно
                    break;

                case 1:
                    //еженедельно
                    for (int i = 0; i < 7; i++)
                    {
                        if (((int)dt.DayOfWeek) == ddlDay.SelectedIndex)
                            break;
                        dt = dt.AddDays(1);
                    }
                    break;

                case 2:
                    //ежемесячно
                    dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                        ddlDay.SelectedIndex + 1, ddlTime.SelectedIndex, 0, 0);
                    break;

                case 3:
                    //раз в х часов
                    dt = DateTime.Now;
                    dt.AddHours(ddlTime.SelectedIndex + 1);
                    break;
            }
           
            bool retVal = false;
            try
            {
                IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                       typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

                //Конечно, взаимодействовать между собой приложениям .NET из одного программного продукта
                //посредством использания реестра чудовищно... ТЗ есть ТЗ
                IFormatProvider culture = new CultureInfo("ru-RU");
               
                StringBuilder xml = new StringBuilder(1024);
                xml.AppendFormat("<VbaSettings><ControlCenter><PeriodicalMaintenance>" +
                                "<DeliveryTimeoutCheck type=" + "\"reg_dword\"" + ">{0}</DeliveryTimeoutCheck>" +
                                "<DataSendInterval type=" + "\"reg_dword\"" + ">{1}</DataSendInterval>" +
                                "<DaysToDelete type=" + "\"reg_dword\"" + ">{2}</DaysToDelete>" +
                                "<TaskDaysToDelete type=" + "\"reg_dword\"" + ">{5}</TaskDaysToDelete>" +
                                "<NextSendDate type=" + "\"reg_sz\"" + ">{3}</NextSendDate>" +
                                "<MaintenanceEnabled type=" + "\"reg_dword\"" + ">{4}</MaintenanceEnabled>" +
                                "<Reread type=" + "\"reg_dword\"" + ">1</Reread>",    
                                tboxDeliveryTimeoutCheck.Text,
                                ddlEvery.SelectedIndex, tboxDaysToDelete.Text, dt.ToString(culture),
                                (cboxMaintenanceEnabled.Checked ? "1" : "0"),tboxTasksDaysToDelete.Text);

                if (cboxMaintenanceEnabled.Checked)
                {
                    System.Net.IPAddress ip;
                    if (!System.Net.IPAddress.TryParse(tboxServer.Text, out ip))
                        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": " +
                            Resources.Resource.IPAddress);

                    xml.AppendFormat("<Server type=" + "\"reg_sz\"" + ">{0}</Server>", tboxServer.Text);
                }
                if (ddlEvery.SelectedIndex == 3)
                    xml.AppendFormat("<HourIntervalToSend type=" + "\"reg_dword\"" + ">{0}</HourIntervalToSend>", ddlTime.SelectedIndex + 1);
                xml.Append("</PeriodicalMaintenance></ControlCenter></VbaSettings>");

                retVal = remoteObject.ChangeRegistry(xml.ToString());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("SaveSettings: " +
                    ex.Message + " " + Resources.Resource.Vba32SSUnavailable);
            }

            if (!retVal)
                throw new ArgumentException("Reread: Vba32SS return false!");

            //Для обновления страницы после применения настроек
            //(Необходимо перечитать с реестра)
            InitFields();

        }
    }
    protected void ddlEvery_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitDdlList(ddlEvery.SelectedIndex);
    }

    private void ReadDdlSettings(RegistryKey key)
    {
        object tmp = key.GetValue("DataSendInterval");
        if (tmp == null)
            throw new Exception("Cannot read DataSendInterval from registry");

        DateTime dt = Convert.ToDateTime(key.GetValue("NextSendDate"));
        if ((dt == null) || (dt == DateTime.MinValue))
            throw new Exception("Cannot read NextSendDate from registry");

        ddlEvery.SelectedIndex = (int)tmp;
        InitDdlList(ddlEvery.SelectedIndex);
        switch (ddlEvery.SelectedIndex)
        {
            case 0:
                ddlTime.SelectedIndex = dt.Hour;
                break;

            case 1:
                ddlDay.SelectedIndex = (int)dt.DayOfWeek;
                ddlTime.SelectedIndex = dt.Hour;
                break;

            case 2:
                ddlDay.SelectedIndex = dt.Day-1;
                ddlTime.SelectedIndex = dt.Hour;
                break;
            case 3:
                object tmpHour = key.GetValue("HourIntervalToSend");
                if (tmpHour == null)
                    ddlTime.SelectedIndex = 0;
                else
                {
                    ddlTime.SelectedIndex = (int)tmpHour - 1;
                }
                break;
        }

        
    }

    protected void pcPaging_NextPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;
        UpdateData();

    }
    protected void pcPaging_PrevPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;
        UpdateData();
    }

    protected void pcPaging_HomePage(object sender, EventArgs e)
    {
        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        UpdateData();
    }

    protected void pcPaging_LastPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).PageCount;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;

        UpdateData();
    }

}
