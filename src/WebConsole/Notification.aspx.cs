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

using System.Drawing;
using Microsoft.Win32;

using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Filters;

using Vba32.ControlCenter.SettingsService;
using System.Text.RegularExpressions;

public partial class Notification : PageBase
{
    private const String  GlobalEpidemyEvent = "vba32.cc.GlobalEpidemy";
    private const String LocalHearthEvent = "vba32.cc.LocalHearth";

    protected void Page_Init(Object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("Administrator"))
        {
            Response.Redirect("Default.aspx");
        }
        RegisterScript(@"js/jQuery/jquery.cookie.js");
        Page.Title = Resources.Resource.Vba32NSSettings;
        if (!IsPostBack)
        {
            InitFields();
        }
    }

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

        lbtnSave.Text = Resources.Resource.Save;

        cboxUseMail.Text = Resources.Resource.UseMail;
        cboxUseJabber.Text = Resources.Resource.UseJabber;
        cboxUseFlowAnalysis.Text = Resources.Resource.UseFlowAnalysis;

        lblSelectedEventName.Text = Resources.Resource.СhoiseEvent;

        lbtnSave.Text = Resources.Resource.Save;
        btnCancel.Text = Resources.Resource.Close;

        regularMailServer.ValidationExpression = RegularExpressions.IPAddress;
        regularMailFrom.ValidationExpression = RegularExpressions.Email;

        rangeGlobalEpidemyCompCount.ErrorMessage = rangeGlobalEpidemyLimit.ErrorMessage = rangeGlobalEpidemyTimeLimit.ErrorMessage =
            rangeLimit.ErrorMessage = rangeLocalHearthLimit.ErrorMessage = rangeLocalHearthTimeLimit.ErrorMessage =
            rangeTimeLimit.ErrorMessage = String.Format(Resources.Resource.ValueBetween, "0", "1000");

        RegistryKey key = GetRegisterKey();
        InitMailFields(key);
        InitJabberFields(key);
        InitFlowAnalysisFields(key);

        UpdateData();
    }

    private void InitJabberFields(RegistryKey key)
    {
        Boolean isChecked = true;
        try
        {
            tboxJabberServer.Text = (String)key.GetValue("JabberServer");
            tboxJabberFrom.Text = (String)key.GetValue("JabberFromJID");
            tboxJabberPassword.Attributes.Add("value", (String)key.GetValue("JabberPassword"));
        }
        catch
        {
            tboxJabberServer.Text = tboxJabberFrom.Text = tboxJabberPassword.Text = String.Empty;
            isChecked = false;
        }

        if (isChecked && String.IsNullOrEmpty(tboxJabberServer.Text))
            isChecked = false;

        cboxUseJabber.Checked = tboxJabberServer.Enabled = tboxJabberFrom.Enabled = tboxJabberPassword.Enabled = isChecked;
    }

    private void InitMailFields(RegistryKey key)
    {
        Boolean isChecked = true;
        try
        {
            tboxMailServer.Text = (String)key.GetValue("MailServer");
            tboxMailFrom.Text = (String)key.GetValue("MailFrom");
            tboxMailDisplayName.Text = (String)key.GetValue("MailDisplayName");
        }
        catch
        {            
            tboxMailServer.Text = tboxMailFrom.Text = tboxMailDisplayName.Text = String.Empty;
            isChecked = false;
        }

        if (isChecked && String.IsNullOrEmpty(tboxMailServer.Text))
            isChecked = false;

        cboxUseMail.Checked = tboxMailServer.Enabled = tboxMailFrom.Enabled = tboxMailDisplayName.Enabled = isChecked;
    }

    /// <summary>
    /// Инициализирует поля, связанные с обработкой потока уведомлений
    /// </summary>
    private void InitFlowAnalysisFields(RegistryKey key)
    {
        Boolean isChecked = true;
        try
        {
            Int32? tmp = (Int32?)key.GetValue("GlobalEpidemyLimit");
            tboxGlobalEpidemyLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (Int32?)key.GetValue("GlobalEpidemyTimeLimit");
            tboxGlobalEpidemyTimeLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (Int32?)key.GetValue("GlobalEpidemyCompCount");
            tboxGlobalEpidemyCompCount.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (Int32?)key.GetValue("LocalHearthLimit");
            tboxLocalHearthLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (Int32?)key.GetValue("LocalHearthTimeLimit");
            tboxLocalHearthTimeLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (Int32?)key.GetValue("Limit");
            tboxLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (Int32?)key.GetValue("TimeLimit");
            tboxTimeLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (Int32?)key.GetValue("UseFlowAnalysis");
            cboxUseFlowAnalysis.Checked = tmp.HasValue ? tmp.Value > 0 : false;

        }
        catch
        {
            isChecked = false;
            tboxGlobalEpidemyLimit.Text = tboxGlobalEpidemyTimeLimit.Text = tboxGlobalEpidemyCompCount.Text =
                tboxLocalHearthLimit.Text = tboxLocalHearthTimeLimit.Text = tboxLimit.Text = tboxTimeLimit.Text = "10";
        }

        if (isChecked)
            isChecked = cboxUseFlowAnalysis.Checked;

        tboxGlobalEpidemyLimit.Enabled = tboxGlobalEpidemyTimeLimit.Enabled = tboxGlobalEpidemyCompCount.Enabled =
                tboxLocalHearthLimit.Enabled = tboxLocalHearthTimeLimit.Enabled = tboxLimit.Enabled = tboxTimeLimit.Enabled = isChecked;
    }

    private RegistryKey GetRegisterKey()
    {
        String registryControlCenterKeyName;
        RegistryKey key;
        try
        {
            //!-OPTM Вынести такую проверку в App_Code и юзать один код
            if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
            else
                registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

            key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "Notification"); ;

        }
        catch (Exception ex)
        {
            throw new ArgumentException("Registry open 'Notification' key error: " + ex.Message);
        }

        return key;
    }

    private Boolean ValidateMailFields()
    {
        if (!cboxUseMail.Checked)
        {
            tboxMailServer.Text = tboxMailFrom.Text = tboxMailDisplayName.Text = String.Empty;
            return true;
        }

        Regex reg = new Regex(RegularExpressions.IPAddress);
        if (!reg.IsMatch(tboxMailServer.Text))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.MailServer);

        reg = new Regex(RegularExpressions.Email);
        if (!reg.IsMatch(tboxMailFrom.Text))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.MailFrom);

        return true;
    }

    private Boolean ValidateJabberFields()
    {
        if (!cboxUseJabber.Checked)
        {
            tboxJabberServer.Text = tboxJabberPassword.Text = tboxJabberFrom.Text = String.Empty;
            return true;
        }

        if (String.IsNullOrEmpty(tboxJabberServer.Text))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.JabberServer);

        if (String.IsNullOrEmpty(tboxJabberPassword.Text))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.JabberPassword);

        if (String.IsNullOrEmpty(tboxJabberFrom.Text))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.JabberFrom);

        return true;
    }

    private Boolean ValidateFlowAnalysisFields()
    {
        if (!cboxUseFlowAnalysis.Checked)
        {
            tboxGlobalEpidemyLimit.Text = tboxGlobalEpidemyTimeLimit.Text = tboxGlobalEpidemyCompCount.Text =
                tboxLocalHearthLimit.Text = tboxLocalHearthTimeLimit.Text = tboxLimit.Text = tboxTimeLimit.Text = "10";
            return true;
        }
        
        Validation vld = new Validation(tboxLocalHearthLimit.Text);

        if (!vld.CheckUInt32())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.FlowAnalysis);

        vld.Value = tboxLocalHearthTimeLimit.Text;
        if (!vld.CheckUInt32())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.FlowAnalysis);
        
        vld.Value = tboxGlobalEpidemyLimit.Text;
        if (!vld.CheckUInt32())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.FlowAnalysis);

        vld.Value = tboxGlobalEpidemyTimeLimit.Text;
        if (!vld.CheckUInt32())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.FlowAnalysis);

        vld.Value = tboxLimit.Text;
        if (!vld.CheckUInt32())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.FlowAnalysis);
        
        vld.Value = tboxTimeLimit.Text;
        if (!vld.CheckUInt32())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.FlowAnalysis);

        vld.Value = tboxGlobalEpidemyCompCount.Text;
        if (!vld.CheckUInt32())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.FlowAnalysis);
        
        return true;
    }

    private void InitializeSession()
    {
        if (Session["NotifySortExp"] == null)
            Session["NotifySortExp"] = "EventName ASC";
    }

    private void UpdateData()
    {
        InitializeSession();
        using (VlslVConnection conn = new VlslVConnection(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            EventTypesManager db = new EventTypesManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);

            String filter = "EventName like '%'";
            String sort = (String)Session["NotifySortExp"];

            Int32 count = db.Count(filter);
            Int32 pageSize = 20;
            Int32 pageCount = (Int32)Math.Ceiling((Double)count / pageSize);

            pcPaging.PageCount = pageCount;

            dlEvents.DataSource = db.List(filter, sort, pcPaging.CurrentPageIndex, pageSize);

            dlEvents.DataBind();
            conn.CloseConnection();
        }
    }

    protected void dlEvents_ItemCommand(Object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "SelectCommand":
                {
                    if ((String)e.CommandArgument == "Notify")
                    {
                        Boolean notifyState = (e.Item.FindControl("ibtnNotify") as ImageButton).ImageUrl.Contains("disabled.gif");

                        using (VlslVConnection conn = new VlslVConnection(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
                        {
                            EventTypesManager db = new EventTypesManager(conn);

                            conn.OpenConnection();
                            conn.CheckConnectionState(true);
                            
                            EventTypesEntity event_ = new EventTypesEntity(Convert.ToInt16((e.Item.FindControl("lblID") as Label).Text),
                                    "", "", false, false, notifyState);
                            db.UpdateNotify(event_);
                            conn.CloseConnection();
                        }
                        //Session["EventNameNotify"] = notifyState;
                        UpdateData();
                        break;
                    }
                    if ((String)e.CommandArgument == "EventName")
                    {
                        String eventName = (e.Item.FindControl("lbtnEventName") as LinkButton).Text;
                        lblSelectedEventName.Text = eventName;

                        Session["NotifyEvent"] = eventName;
                        notify.LoadState(eventName);

                        ModalPopupExtender.Show(); 
                    }
                    break;
                }
            case "SortCommand":
                if (Session["NotifySortExp"] == null)
                    Session["NotifySortExp"] = e.CommandArgument.ToString() + " ASC";
                else
                {
                    if (((String)Session["NotifySortExp"]).Contains("ASC"))
                        Session["NotifySortExp"] = e.CommandArgument.ToString() + " DESC";
                    else
                        Session["NotifySortExp"] = e.CommandArgument.ToString() + " ASC";
                }
                UpdateData();
                break;
        }
    }

    protected void dlEvents_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        //Item
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            EventTypesEntity et = ((EventTypesEntity)e.Item.DataItem);
            Color clr = Color.FromName(et.Color);
            Color clr2 = Color.FromArgb((Byte)~clr.R, (Byte)~clr.G, (Byte)~clr.B);

            (e.Item.FindControl("lbtnEventName") as LinkButton).Attributes.Add("style", "color:" + "#" + clr2.R.ToString()
                + clr2.G.ToString() + clr2.B.ToString() + ";" + "background-color:" + clr.Name);

            String strBool = et.Notify ? "enabled.gif" : "disabled.gif";

            (e.Item.FindControl("ibtnNotify") as ImageButton).ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + strBool;
        }

        //Header
        if (e.Item.ItemType == ListItemType.Header)
        {
            (e.Item.FindControl("lbtnNotify") as LinkButton).Text = Resources.Resource.Notify;
            (e.Item.FindControl("lbtnEventName") as LinkButton).Text = Resources.Resource.EventName;

            String[] name = ((String)Session["NotifySortExp"]).Split(' ');
            (e.Item.FindControl("lbtn" + name[0]) as LinkButton).Text += (name[1] == "ASC") ? " \u2193" : " \u2191";
        }
    }

    #region Paging
    protected void pcPaging_NextPage(Object sender, EventArgs e)
    {
        Int32 index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        UpdateData();

    }
    protected void pcPaging_PrevPage(Object sender, EventArgs e)
    {
        Int32 index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        UpdateData();
    }

    protected void pcPaging_HomePage(Object sender, EventArgs e)
    {
        pcPaging.CurrentPageIndex = 1;
        UpdateData();
    }

    protected void pcPaging_LastPage(Object sender, EventArgs e)
    {
        Int32 index = ((PagingControls.PagingControl)sender).PageCount;
        pcPaging.CurrentPageIndex = index;
        UpdateData();
    }
    #endregion

    /// <summary>
    /// Устанавливает значение в реестре, сигнализирующее о необходимости сервису перечитать настройки.
    /// </summary>
    private void RereadSet()
    {
        Boolean retVal = true;
        try
        {
            IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                       typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

            String xml = "<VbaSettings><ControlCenter><Notification>" +
                "<Reread type=" + "\"reg_dword\"" + ">1</Reread>" +
                "</Notification></ControlCenter></VbaSettings>";

            retVal = remoteObject.ChangeRegistry(xml);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Reread: " +
                ex.Message + " " + Resources.Resource.Vba32SSUnavailable);
        }

        if (!retVal)
            throw new ArgumentException("Reread: Vba32SS return false!");
    }

    protected void lbtnSave_Click(Object sender, EventArgs e)
    {
        String eventName = (String)Session["NotifyEvent"];
        if (!String.IsNullOrEmpty(eventName))
        {
            notify.SaveState(eventName, true);
            RereadSet();
        }
        else
            throw new ArgumentException(Resources.Resource.Error + ": "+
                Resources.Resource.EventName);
    }

    protected void lbtnSaveRegistry_Click(Object sender, EventArgs e)
    {
        if (ValidateMailFields())
        {
            Boolean retVal = true;
            try
            {
                IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                       typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

                System.Text.StringBuilder builder = new System.Text.StringBuilder(256);

                builder.Append("<VbaSettings><ControlCenter><Notification>");
                builder.AppendFormat("<MailServer type=" + "\"reg_sz\"" + ">{0}</MailServer>" +
                                "<MailFrom type=" + "\"reg_sz\"" + ">{1}</MailFrom>" +
                                "<MailDisplayName type=" + "\"reg_sz\"" + ">{2}</MailDisplayName>" +
                                "<Reread type=" + "\"reg_dword\"" + ">1</Reread>",
                                 tboxMailServer.Text, tboxMailFrom.Text, tboxMailDisplayName.Text);
                

                builder.Append("</Notification></ControlCenter></VbaSettings>");

                retVal = remoteObject.ChangeRegistry(builder.ToString());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("SaveSettings: " +
                    ex.Message + " " + Resources.Resource.Vba32SSUnavailable);
            }

            if (!retVal)
                throw new ArgumentException("SaveSettings: Vba32SS return false!");
        }
    }

    protected void lbtnJabberSave_Click(Object sender, EventArgs e)
    {
        if (ValidateJabberFields())
        {
            Boolean retVal = true;
            try
            {
                IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                       typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

                System.Text.StringBuilder builder = new System.Text.StringBuilder(256);

                builder.Append("<VbaSettings><ControlCenter><Notification>");
                builder.AppendFormat("<JabberServer type=" + "\"reg_sz\"" + ">{0}</JabberServer>" +
                                "<JabberFromJID type=" + "\"reg_sz\"" + ">{1}</JabberFromJID>" +
                                "<Reread type=" + "\"reg_dword\"" + ">1</Reread>",
                                tboxJabberServer.Text, tboxJabberFrom.Text);

                if (!String.IsNullOrEmpty(tboxJabberPassword.Text))
                {
                    builder.AppendFormat("<JabberPassword type=" + "\"reg_sz\"" + ">{0}</JabberPassword>",
                        tboxJabberPassword.Text);
                }

                builder.Append("</Notification></ControlCenter></VbaSettings>");

                retVal = remoteObject.ChangeRegistry(builder.ToString());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("SaveSettings: " +
                    ex.Message + " " + Resources.Resource.Vba32SSUnavailable);
            }

            if (!retVal)
                throw new ArgumentException("SaveSettings: Vba32SS return false!");
        }
    }

    protected void lbtnFlowSave_Click(Object sender, EventArgs e)
    {
        if (ValidateFlowAnalysisFields())
        {
            Boolean retVal = true;
            try
            {
                IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                       typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

                System.Text.StringBuilder builder = new System.Text.StringBuilder(256);

                builder.Append("<VbaSettings><ControlCenter><Notification>");
                builder.AppendFormat(
                                "<LocalHearthTimeLimit type=" + "\"reg_dword\"" + ">{0}</LocalHearthTimeLimit>" +
                                "<LocalHearthLimit type=" + "\"reg_dword\"" + ">{1}</LocalHearthLimit>" +
                                "<GlobalEpidemyTimeLimit type=" + "\"reg_dword\"" + ">{2}</GlobalEpidemyTimeLimit>" +
                                "<GlobalEpidemyLimit type=" + "\"reg_dword\"" + ">{3}</GlobalEpidemyLimit>" +
                                "<Limit type=" + "\"reg_dword\"" + ">{4}</Limit>" +
                                "<TimeLimit type=" + "\"reg_dword\"" + ">{5}</TimeLimit>" +
                                "<UseFlowAnalysis type=" + "\"reg_dword\"" + ">{6}</UseFlowAnalysis>" +
                                "<GlobalEpidemyCompCount type=" + "\"reg_dword\"" + ">{7}</GlobalEpidemyCompCount>" +
                                "<ReRead type=" + "\"reg_dword\"" + ">1</ReRead>",
                                cboxUseFlowAnalysis.Checked ? tboxLocalHearthTimeLimit.Text : "10",
                                cboxUseFlowAnalysis.Checked ? tboxLocalHearthLimit.Text : "10",
                                cboxUseFlowAnalysis.Checked ? tboxGlobalEpidemyTimeLimit.Text : "10",
                                cboxUseFlowAnalysis.Checked ? tboxGlobalEpidemyLimit.Text : "10",
                                cboxUseFlowAnalysis.Checked ? tboxLimit.Text : "10",
                                cboxUseFlowAnalysis.Checked ? tboxTimeLimit.Text : "10",
                                cboxUseFlowAnalysis.Checked ? "1" : "0",
                                cboxUseFlowAnalysis.Checked ? tboxGlobalEpidemyCompCount.Text : "10");


                builder.Append("</Notification></ControlCenter></VbaSettings>");

                retVal = remoteObject.ChangeRegistry(builder.ToString());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("FlowSave: " +
                    ex.Message + " " + Resources.Resource.Vba32SSUnavailable);
            }

            if (!retVal)
                throw new ArgumentException("FlowSave: Vba32SS return false!");
        }
    }

    protected void lbtnSaveAll_Click(Object sender, EventArgs e)
    {
        lbtnFlowSave_Click(sender, e);
        lbtnJabberSave_Click(sender, e);
        lbtnSaveRegistry_Click(sender, e);
    }
}
