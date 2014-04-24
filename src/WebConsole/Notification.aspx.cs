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

using VirusBlokAda.Vba32CC.DataBase;
using ARM2_dbcontrol.Filters;

using Vba32.ControlCenter.SettingsService;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Collections.Generic;

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

        ResourceRegister(LoadResourcesList());
        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
    }

    private String[] LoadResourcesList()
    {
        List<String> list = new List<String>();
        list.Add("Disable");
        list.Add("Enable");

        return list.ToArray();
    }

    protected override void InitFields()
    {
        cboxUseMail.Text = Resources.Resource.UseMail;
        cboxUseJabber.Text = Resources.Resource.UseJabber;
        cboxUseFlowAnalysis.Text = Resources.Resource.UseFlowAnalysis;

        //lblSelectedEventName.Text = Resources.Resource.СhoiseEvent;
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


    // Вынести эту логику в WebMethod и диалог JQuery!!!!!!!!!!!!
    protected void GridView1_RowCommand(Object source, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "SelectCommand":
                {
                    if ((String)e.CommandArgument == "EventName")
                    {
                        String eventName = (e.CommandSource as LinkButton).Text;
                        lblSelectedEventName.Text = eventName;

                        Session["NotifyEvent"] = eventName;  // попробовать избавиться!!!
                        
                        notify.LoadState(eventName);

                        ModalPopupExtender.Show();
                    }
                    break;
                }
        }
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            EventTypesEntity _event = (EventTypesEntity)e.Row.DataItem;
            Color clr = Color.FromName(_event.Color);
            Color clr2 = Color.FromArgb((Byte)~clr.R, (Byte)~clr.G, (Byte)~clr.B);

            (e.Row.FindControl("lbtnEventName") as LinkButton).Attributes.Add("style", "color:" + "#" + clr2.R.ToString()
                + clr2.G.ToString() + clr2.B.ToString() + ";" + "background-color:" + clr.Name);

            String strBool = _event.Notify ? "enabled.gif" : "disabled.gif";

            (e.Row.FindControl("ibtnNotify") as HtmlImage).Src = "App_Themes/" + Profile.Theme + "/Images/" + strBool;
            (e.Row.FindControl("ibtnNotify") as HtmlImage).Attributes.Add("title", _event.Notify ? Resources.Resource.Disable : Resources.Resource.Enable);
            (e.Row.FindControl("ibtnNotify") as HtmlImage).Attributes.Add("state", _event.Notify ? "Enabled" : "Disabled");
        }
    }

    [WebMethod()]
    public static void UpdateNotify(Int16 id, Boolean isEnabled)
    {
        EventTypesEntity event_ = new EventTypesEntity(id, "", "", false, false, !isEnabled);
        if (!EventsDataContainer.UpdateNotify(event_))
            throw new Exception("Update notify is not success.");
    }

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
