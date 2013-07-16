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

public partial class Notification : PageBase
{
    //private string eventName = String.Empty;

    private const string  GlobalEpidemyEvent = "vba32.cc.GlobalEpidemy";
    private const string LocalHearthEvent = "vba32.cc.LocalHearth";

    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("Administrator"))
        {
            //throw new Exception(Resources.Resource.ErrorAccessDenied);
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

        //pcPagingTop.CurrentPageIndex = 1;
        //pcPagingTop.PageCount = 1;
        //pcPagingTop.PageText = Resources.Resource.Page;
        //pcPagingTop.OfText = Resources.Resource.Of;
        //pcPagingTop.NextText = Resources.Resource.Next;
        //pcPagingTop.PrevText = Resources.Resource.Prev;

        //pcPagingTop.HomeText = Resources.Resource.HomePaging;
        //pcPagingTop.LastText = Resources.Resource.LastPaging;


        lblMailServer.Text = Resources.Resource.MailServer;
        lblMailFrom.Text = Resources.Resource.MailFrom;
        lblDisplayName.Text = Resources.Resource.MailDisplayName;

        lblJabberSever.Text = Resources.Resource.JabberServer;
        lblJabberFrom.Text = Resources.Resource.JabberFrom;
        lblJabberPassword.Text = Resources.Resource.JabberPassword;

        lbtnSave.Text = Resources.Resource.Save;
        //lbtnSaveRegistry.Text = Resources.Resource.Save;
        //lbtnJabberSave.Text = Resources.Resource.Save;

        cboxUseFlowAnalysis.Text = Resources.Resource.UseFlowAnalysis;
        //lbtnFlowSave.Text = Resources.Resource.Save;

        lbtnSaveAll.Text = Resources.Resource.Save;


        lblSelectedEventName.Text = Resources.Resource.�hoiseEvent;

        lbtnSave.Text = Resources.Resource.Save;
        btnCancel.Text = Resources.Resource.Close;
        
        string registryControlCenterKeyName;
        RegistryKey key;
        try
        {
            //!-OPTM ������� ����� �������� � App_Code � ����� ���� ���
            if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
            else
                registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

            key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "Notification"); ;

        }
        catch(Exception ex)
        {
            throw new ArgumentException("Registry open 'Notification' key error: "+ex.Message);
        }

        try
        {
            //TextBoxes
            tboxMailServer.Text = (string)key.GetValue("MailServer");
            tboxMailFrom.Text = (string)key.GetValue("MailFrom");
            tboxMailDisplayName.Text = (string)key.GetValue("MailDisplayName");

            tboxJabberServer.Text = (string)key.GetValue("JabberServer");
            tboxJabberFrom.Text = (string)key.GetValue("JabberFromJID");

            //tboxJabberPassword.Text = (string)key.GetValue("JabberPassword");

        }
        catch
        {
        }
        finally
        {
            //key.Close();
        }

        InitFlowAnalysisFields();

        UpdateData();

    }

    /// <summary>
    /// �������������� ����, ��������� � ���������� ������ �����������
    /// </summary>
    private void InitFlowAnalysisFields()
    {
        string registryControlCenterKeyName;
        RegistryKey key;
        try
        {
            //!-OPTM ������� ����� �������� � App_Code � ����� ���� ���
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

        try
        {
            //TextBoxes
            int? tmp = (int?) key.GetValue("GlobalEpidemyLimit");
            tboxGlobalEpidemyLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (int?)key.GetValue("GlobalEpidemyTimeLimit");
            tboxGlobalEpidemyTimeLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (int?)key.GetValue("GlobalEpidemyCompCount");
            tboxGlobalEpidemyCompCount.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (int?)key.GetValue("LocalHearthLimit");
            tboxLocalHearthLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (int?)key.GetValue("LocalHearthTimeLimit");
            tboxLocalHearthTimeLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (int?)key.GetValue("Limit");
            tboxLimit.Text = tmp.HasValue ? tmp.Value.ToString() : "10";

            tmp = (int?)key.GetValue("TimeLimit");
            tboxTimeLimit.Text = tmp.HasValue ? tmp.Value.ToString():"10";

            tmp = (int?)key.GetValue("UseFlowAnalysis");
            cboxUseFlowAnalysis.Checked = tmp.HasValue ? tmp.Value > 0 : false;

            bool isEnabled;
            if (cboxUseFlowAnalysis.Checked) isEnabled = true;
            else isEnabled = false;
            tboxGlobalEpidemyLimit.Enabled = tboxGlobalEpidemyTimeLimit.Enabled = tboxGlobalEpidemyCompCount.Enabled =
                tboxLocalHearthLimit.Enabled = tboxLocalHearthTimeLimit.Enabled = tboxLimit.Enabled = tboxTimeLimit.Enabled = isEnabled;
            
        }
        catch
        {
        }
        finally
        {
            //key.Close();
        }
    }


    private bool ValidateMailFields()
    {
        Validation vld = new Validation(tboxMailServer.Text);

        if (!vld.CheckIP())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.MailServer);

        try
        {
            System.Net.Mail.MailAddress ml =
                new System.Net.Mail.MailAddress(tboxMailFrom.Text);
        }
        catch
        {
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.MailFrom);
        }

        return true;
    }

    private bool ValidateFlowAnalysisFields()
    {
        if (!cboxUseFlowAnalysis.Checked) return true;
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
        
        
        
        return true; //�� � ����� �����
    }

    private void InitializeSession()
    {
        if (Session["NotifySortExp"] == null)
            Session["NotifySortExp"] = "EventName ASC";
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
            string sort = (string)Session["NotifySortExp"];

            int count = db.Count(filter);
            int pageSize = 20;
            int pageCount = (int)Math.Ceiling((double)count / pageSize);

            pcPaging.PageCount = pageCount;
            //pcPagingTop.PageCount = pageCount;

            dlEvents.DataSource = db.List(filter, sort, pcPaging.CurrentPageIndex, pageSize);

            dlEvents.DataBind();
            conn.CloseConnection();
        }
    }

    protected void dlEvents_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "SelectCommand":
                {
                    if ((string)e.CommandArgument == "Notify")
                    {
                        bool notifyState = (e.Item.FindControl("ibtnNotify") as ImageButton).ImageUrl.Contains("disabled.gif");

                        using (VlslVConnection conn = new VlslVConnection(
                        ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
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
                    if ((string)e.CommandArgument == "EventName")
                    {
                        string eventName = (e.Item.FindControl("lbtnEventName") as LinkButton).Text;
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
                    if (((string)Session["NotifySortExp"]).Contains("ASC"))
                        Session["NotifySortExp"] = e.CommandArgument.ToString() + " DESC";
                    else
                        Session["NotifySortExp"] = e.CommandArgument.ToString() + " ASC";
                }
                UpdateData();
                break;
        }
    }

    protected void dlEvents_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //Item
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            EventTypesEntity et = ((EventTypesEntity)e.Item.DataItem);
            Color clr = Color.FromName(et.Color);
            Color clr2 = Color.FromArgb((byte)~clr.R, (byte)~clr.G, (byte)~clr.B);

            (e.Item.FindControl("lbtnEventName") as LinkButton).Attributes.Add("style", "color:" + "#" + clr2.R.ToString()
                + clr2.G.ToString() + clr2.B.ToString() + ";" + "background-color:" + clr.Name);

            //if ((et.EventName != GlobalEpidemyEvent) 
            //    && (et.EventName != LocalHearthEvent))
            {
                string strBool = "";
                if (et.Notify)
                    strBool = "enabled.gif";
                else
                    strBool = "disabled.gif";

                (e.Item.FindControl("ibtnNotify") as ImageButton).ImageUrl =
                    Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + strBool;
            }
            //else
            //{
            //    (e.Item.FindControl("ibtnNotify") as ImageButton).Visible = false;
            //}
        }

        //Header
        if (e.Item.ItemType == ListItemType.Header)
        {
            (e.Item.FindControl("lbtnNotify") as LinkButton).Text = Resources.Resource.Notify;
            (e.Item.FindControl("lbtnEventName") as LinkButton).Text = Resources.Resource.EventName;

            string currentSorting = (string)Session["NotifySortExp"];
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

    #region Paging
    protected void pcPaging_NextPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        //pcPagingTop.CurrentPageIndex = index;
        UpdateData();

    }
    protected void pcPaging_PrevPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        //pcPagingTop.CurrentPageIndex = index;
        UpdateData();
    }

    protected void pcPaging_HomePage(object sender, EventArgs e)
    {
        pcPaging.CurrentPageIndex = 1;
        //pcPagingTop.CurrentPageIndex = 1;

        UpdateData();
    }

    protected void pcPaging_LastPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).PageCount;
        pcPaging.CurrentPageIndex = index;
        //pcPagingTop.CurrentPageIndex = index;

        UpdateData();
    }
    #endregion

    /// <summary>
    /// ������������� �������� � �������, ��������������� � ������������� ������� ���������� ���������.
    /// </summary>
    private void RereadSet()
    {
        bool retVal = true;
        try
        {
            IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                       typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

            string xml = "<VbaSettings><ControlCenter><Notification>" +
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

    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        string eventName = (string)Session["NotifyEvent"];
        if (!String.IsNullOrEmpty(eventName))
        {
            bool notifyState = true;
            //if (Session["EventNameNotify"] != null)
            //    notifyState = (bool)Session["EventNameNotify"];            
            notify.SaveState(eventName, notifyState);
            RereadSet();
        }
        else
            throw new ArgumentException(Resources.Resource.Error + ": "+
                Resources.Resource.EventName);
    }

    protected void lbtnSaveRegistry_Click(object sender, EventArgs e)
    {
        if (ValidateMailFields())
        {
            bool retVal = true;
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
    protected void lbtnJabberSave_Click(object sender, EventArgs e)
    {
        bool retVal = true;
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
                string password = tboxJabberPassword.Text;

                /*//�������� ������ �� ������� ��������
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] != 0x01;
                password = System.Text.Encoding.UTF8.GetString(bytes);
                //*/
                builder.AppendFormat("<JabberPassword type=" + "\"reg_sz\"" + ">{0}</JabberPassword>",
                    password);
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
    protected void lbtnFlowSave_Click(object sender, EventArgs e)
    {
        ValidateFlowAnalysisFields();
        bool retVal = true;
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
    protected void lbtnSaveAll_Click(object sender, EventArgs e)
    {
        lbtnFlowSave_Click(sender, e);
        lbtnJabberSave_Click(sender, e);
        lbtnSaveRegistry_Click(sender, e);
        InitFlowAnalysisFields();
    }
}
