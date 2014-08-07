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

using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Filters;

using VirusBlokAda.CC.Settings.Common;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Collections.Generic;
using VirusBlokAda.CC.Settings.Entities;
using VirusBlokAda.CC.Common;

public partial class Notification : PageBase
{
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

        regularMailServer.ValidationExpression = RegularExpressions.IPAddress;
        regularMailFrom.ValidationExpression = RegularExpressions.Email;

        rangeGlobalEpidemyCompCount.ErrorMessage = rangeGlobalEpidemyLimit.ErrorMessage = rangeGlobalEpidemyTimeLimit.ErrorMessage =
            rangeLimit.ErrorMessage = rangeLocalHearthLimit.ErrorMessage = rangeLocalHearthTimeLimit.ErrorMessage =
            rangeTimeLimit.ErrorMessage = String.Format(Resources.Resource.ValueBetween, "0", "1000");

        InitSettingFields();
    }

    private void InitSettingFields()
    {
        NSSettingsEntity ent = null;
        try
        {
            ent = VirusBlokAda.CC.Settings.SettingsProvider.GetNSSettings();
        }
        catch{}

        InitMailFields(ent);
        InitJabberFields(ent);
        InitFlowAnalysisFields(ent);
    }

    private void InitJabberFields(NSSettingsEntity ent)
    {
        Boolean isChecked = true;         
        if (ent == null)
        {
            tboxJabberServer.Text = tboxJabberFrom.Text = tboxJabberPassword.Text = String.Empty;
            isChecked = false;
        }
        else
        {
            tboxJabberServer.Text = ent.JabberServer;
            tboxJabberFrom.Text = ent.JabberFromJID;
            tboxJabberPassword.Attributes.Add("value", ent.JabberPassword);
        }

        if (isChecked && String.IsNullOrEmpty(tboxJabberServer.Text))
            isChecked = false;

        cboxUseJabber.Checked = tboxJabberServer.Enabled = tboxJabberFrom.Enabled = tboxJabberPassword.Enabled = isChecked;
    }

    private void InitMailFields(NSSettingsEntity ent)
    {
        Boolean isChecked = true;         
        if (ent == null)
        {
            tboxMailServer.Text = tboxMailFrom.Text = tboxMailDisplayName.Text = String.Empty;
            cboxAuthorizationEnabled.Checked = false;
            tboxAuthorizationUserName.Text = tboxAuthorizationPassword.Text = String.Empty;
            isChecked = false;
        }
        else
        {
            tboxMailServer.Text = ent.MailServer;
            tboxMailFrom.Text = ent.MailFrom;
            tboxMailDisplayName.Text = ent.MailDisplayName;

            cboxAuthorizationEnabled.Checked = ent.UseMailAuthorization;
            tboxAuthorizationUserName.Text = ent.MailUsername;
            tboxAuthorizationPassword.Attributes.Add("value", ent.MailPassword);
        }

        if (isChecked && String.IsNullOrEmpty(tboxMailServer.Text))
            isChecked = false;

        cboxUseMail.Checked = tboxMailServer.Enabled = tboxMailFrom.Enabled = tboxMailDisplayName.Enabled = isChecked;
        cboxAuthorizationEnabled.Disabled = !isChecked;
        tboxAuthorizationPassword.Enabled = tboxAuthorizationUserName.Enabled = isChecked && cboxAuthorizationEnabled.Checked;
    }

    /// <summary>
    /// »нициализирует пол€, св€занные с обработкой потока уведомлений
    /// </summary>
    private void InitFlowAnalysisFields(NSSettingsEntity ent)
    {
        Boolean isChecked = true;
        if (ent == null)
        {
            isChecked = false;
            tboxGlobalEpidemyLimit.Text = tboxGlobalEpidemyTimeLimit.Text = tboxGlobalEpidemyCompCount.Text =
                tboxLocalHearthLimit.Text = tboxLocalHearthTimeLimit.Text = tboxLimit.Text = tboxTimeLimit.Text = "10";
        }
        else
        {
            tboxGlobalEpidemyTimeLimit.Text = ent.GlobalEpidemyTimeLimit.HasValue ? ent.GlobalEpidemyTimeLimit.Value.ToString() : "10";
            tboxGlobalEpidemyLimit.Text = ent.GlobalEpidemyLimit.HasValue ? ent.GlobalEpidemyLimit.Value.ToString() : "10";
            tboxGlobalEpidemyCompCount.Text = ent.GlobalEpidemyCompCount.HasValue ? ent.GlobalEpidemyCompCount.Value.ToString() : "10";
            tboxLocalHearthLimit.Text = ent.LocalHearthLimit.HasValue ? ent.LocalHearthLimit.Value.ToString() : "10";
            tboxLocalHearthTimeLimit.Text = ent.LocalHearthTimeLimit.HasValue ? ent.LocalHearthTimeLimit.Value.ToString() : "10";
            tboxLimit.Text = ent.Limit.HasValue ? ent.Limit.Value.ToString() : "10";
            tboxTimeLimit.Text = ent.TimeLimit.HasValue ? ent.TimeLimit.Value.ToString() : "10";
            cboxUseFlowAnalysis.Checked = ent.UseFlowAnalysis.HasValue ? ent.UseFlowAnalysis.Value > 0 : false;
        }

        if (isChecked)
            isChecked = cboxUseFlowAnalysis.Checked;

        tboxGlobalEpidemyLimit.Enabled = tboxGlobalEpidemyTimeLimit.Enabled = tboxGlobalEpidemyCompCount.Enabled =
                tboxLocalHearthLimit.Enabled = tboxLocalHearthTimeLimit.Enabled = tboxLimit.Enabled = tboxTimeLimit.Enabled = isChecked;
    }

    private Boolean ValidateMailFields()
    {
        if (!cboxUseMail.Checked)
        {
            tboxMailServer.Text = tboxMailFrom.Text = tboxMailDisplayName.Text = String.Empty;
            tboxAuthorizationUserName.Text = tboxAuthorizationPassword.Text = String.Empty;
            cboxAuthorizationEnabled.Checked = false;
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

    // ¬ынести эту логику в WebMethod и диалог JQuery!!!!!!!!!!!!
    protected void GridView1_RowCommand(Object source, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "SelectCommand":
                {
                    if ((String)e.CommandArgument == "EventName")
                    {
                        String eventName = (e.CommandSource as LinkButton).Attributes["EventName"];
                        lblSelectedEventName.Text = eventName;

                        Session["NotifyEvent"] = eventName;  // попробовать избавитьс€!!!
                        
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
    /// ”станавливает значение в реестре, сигнализирующее о необходимости сервису перечитать настройки.
    /// </summary>
    private void RereadSet()
    {
        Boolean retVal = true;
        try
        {
            IVba32Settings remoteObject = (IVba32Settings)Activator.GetObject(
                       typeof(IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

            NSSettingsEntity ent = new NSSettingsEntity();
            ent.ReRead = true;

            retVal = remoteObject.ChangeRegistry(ent.GenerateXML());
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
                IVba32Settings remoteObject = (IVba32Settings)Activator.GetObject(
                       typeof(IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

                NSSettingsEntity ent = new NSSettingsEntity();
                ent.ReRead = true;
                ent.MailServer = tboxMailServer.Text;
                ent.MailFrom = tboxMailFrom.Text;
                ent.MailDisplayName = tboxMailDisplayName.Text;
                ent.UseMailAuthorization = cboxAuthorizationEnabled.Checked;
                ent.MailUsername = ent.UseMailAuthorization ? tboxAuthorizationUserName.Text : "";
                ent.MailPassword = ent.UseMailAuthorization ? tboxAuthorizationPassword.Text : "";

                retVal = remoteObject.ChangeRegistry(ent.GenerateXML());
            }
            catch
            {
                throw new InvalidOperationException(Resources.Resource.Vba32SSUnavailable);
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
                IVba32Settings remoteObject = (IVba32Settings)Activator.GetObject(
                       typeof(IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

                NSSettingsEntity ent = new NSSettingsEntity();
                ent.ReRead = true;
                ent.JabberServer = tboxJabberServer.Text;
                ent.JabberFromJID = tboxJabberFrom.Text;
                ent.JabberPassword = tboxJabberPassword.Text;

                retVal = remoteObject.ChangeRegistry(ent.GenerateXML());
            }
            catch
            {
                throw new InvalidOperationException(Resources.Resource.Vba32SSUnavailable);
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
                IVba32Settings remoteObject = (IVba32Settings)Activator.GetObject(
                       typeof(IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

                NSSettingsEntity ent = new NSSettingsEntity();
                ent.ReRead = true;
                ent.UseFlowAnalysis = cboxUseFlowAnalysis.Checked ? 1 : 0;
                if (cboxUseFlowAnalysis.Checked)
                {
                    ent.LocalHearthTimeLimit = Convert.ToInt32(tboxLocalHearthTimeLimit.Text);
                    ent.LocalHearthLimit = Convert.ToInt32(tboxLocalHearthLimit.Text);
                    ent.GlobalEpidemyTimeLimit = Convert.ToInt32(tboxGlobalEpidemyTimeLimit.Text);
                    ent.GlobalEpidemyLimit = Convert.ToInt32(tboxGlobalEpidemyLimit.Text);
                    ent.Limit = Convert.ToInt32(tboxLimit.Text);
                    ent.TimeLimit = Convert.ToInt32(tboxTimeLimit.Text);
                    ent.GlobalEpidemyCompCount = Convert.ToInt32(tboxGlobalEpidemyCompCount.Text);
                }
                else
                {
                    ent.LocalHearthTimeLimit = 10;
                    ent.LocalHearthLimit = 10;
                    ent.GlobalEpidemyTimeLimit = 10;
                    ent.GlobalEpidemyLimit = 10;
                    ent.Limit = 10;
                    ent.TimeLimit = 10;
                    ent.GlobalEpidemyCompCount = 10;
                }

                retVal = remoteObject.ChangeRegistry(ent.GenerateXML());
            }
            catch
            {
                throw new InvalidOperationException(Resources.Resource.Vba32SSUnavailable);
            }

            if (!retVal)
                throw new ArgumentException("FlowSave: Vba32SS return false!");
        }
    }

    protected void lbtnSaveAll_Click(Object sender, EventArgs e)
    {
        String message = Resources.Resource.SuccessStatus;
        try
        {
            lbtnFlowSave_Click(sender, e);
            lbtnJabberSave_Click(sender, e);
            lbtnSaveRegistry_Click(sender, e);
        }
        catch (Exception ex)
        {
            message = ex.Message;
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        }

        String key = "SaveNSSettingsCallbackScript";
        String script = "alert('" + message + "');";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);
    }
}
