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

using System.Text;

using Microsoft.Win32;

using Vba32.ControlCenter.SettingsService;
using ARM2_dbcontrol.Filters;

public partial class UpdateCC : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.MasterPageFile = Profile.MasterPage;
        Page.Theme = Profile.Theme;
    }

    protected override void InitializeCulture()
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo(Profile.Culture);
        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("Administrator"))
        {
            //throw new Exception(Resources.Resource.ErrorAccessDenied);
            Response.Redirect("Default.aspx");
        }
        RegisterScript(@"js/jQuery/jquery.cookie.js");

        //RegisterLink("~/App_Themes/" + Profile.Theme + @"\ui.all.css");


        Page.Title = Resources.Resource.PageUpdateTitle;
        if (!Page.IsPostBack)
            InitFields();
    }

    protected override void InitFields()
    {
        cboxPeriodicalUpdateEnabled.Text = Resources.Resource.EnableAutoUpdate;
        lblPeriodicalUpdate.Text = Resources.Resource.PeriodicalUpdateInterval;
        lblUpdateSource.Text = Resources.Resource.UpdatePath;

        cboxProxyEnabled.Text = Resources.Resource.UseProxy;
        lblProxyAddress.Text = Resources.Resource.Server;
        lblProxyPort.Text = Resources.Resource.Port;

        cboxAuthorizationEnabled.Text = Resources.Resource.UseAuthorization;
        lblAuthorizationUserName.Text = Resources.Resource.User;
        lblAuthorizationPassword.Text = Resources.Resource.PasswordLabelText;

        cboxAuthorizationNTLMEnabled.Text = Resources.Resource.AuthorizationNTLMEnabled;

        cboxImpersonationAccountEnabled.Text = Resources.Resource.Impersonation;
        lblImpersonationAccountUsername.Text = Resources.Resource.User;
        lblImpersonationAccountPassword.Text = Resources.Resource.PasswordLabelText;

        lbtnSave.Text = Resources.Resource.Save;

        InitState();
    }

    private bool ValidateFields()
    {

        Validation vld = new Validation(tboxProxyPort.Text);

        if (cboxProxyEnabled.Checked)
        {
            if(String.IsNullOrEmpty(tboxProxyAddress.Text))
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                 + Resources.Resource.Server);

            if (!vld.CheckUInt32())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                 + Resources.Resource.Port);
        }

        return true;
    }

    private void InitState()
    {
        RegistryKey key;
        object tmp;
        string registryControlCenterKeyName;
        try
        {
            if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
            else
                registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

            key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName+"Update"); ;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Registry open 'ControlCenter' key error: " + ex.Message);
        }

        try
        {
            
            tmp = key.GetValue("PeriodicalUpdateEnabled");
            if (tmp != null)
                cboxPeriodicalUpdateEnabled.Checked = Convert.ToInt32(tmp) == 1;

            tmp = key.GetValue("PeriodicalUpdatePeriod");
            if (tmp == null)
                tboxPeriodicalUpdated.Text = Resources.Resource.NotAvailable;
            else
                tboxPeriodicalUpdated.Text = tmp.ToString();
        }
        catch
        {
        }

        if (key != null)
            key.Close();


        try
        {
            key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "Update\\Source00"); ;

            tboxUpdateSource.Text = (string)key.GetValue("UPDATESOURCE");
            if (cboxPeriodicalUpdateEnabled.Checked)
            { tboxUpdateSource.Enabled = tboxPeriodicalUpdated.Enabled = true; }
            else { tboxUpdateSource.Enabled = tboxPeriodicalUpdated.Enabled = false; }
            //Обновление с прокси-сервера
            tmp = key.GetValue("ProxyEnabled");
            if (tmp != null)
                cboxProxyEnabled.Checked = Convert.ToInt32(tmp) == 1;
            if (cboxProxyEnabled.Checked)
            {
                tboxProxyAddress.Text = (string)key.GetValue("PROXYADDRESS");

                tmp = key.GetValue("PROXYPORT");
                if (tmp == null)
                    tboxProxyPort.Text = Resources.Resource.NotAvailable;
                else
                    tboxProxyPort.Text = tmp.ToString();
                tboxProxyAddress.Enabled = tboxProxyPort.Enabled = true;
            }
            else { tboxProxyAddress.Enabled = tboxProxyPort.Enabled = false; }

            //авторизация
            tmp = key.GetValue("AuthorizationEnabled");
            if (tmp != null)
                cboxAuthorizationEnabled.Checked = Convert.ToInt32(tmp) == 1;
            if (cboxAuthorizationEnabled.Checked)
            {
                tboxAuthorizationUserName.Text = (string)key.GetValue("AUTHORIZATIONUSERNAME");

                tboxAuthorizationPassword.Enabled = tboxAuthorizationUserName.Enabled = true;
                trAuth.Disabled = false;
            }
            else
            {
                tboxAuthorizationPassword.Enabled = tboxAuthorizationUserName.Enabled = false;
                trAuth.Disabled = true;
            }

            //NTLM
            tmp = key.GetValue("AUTHORIZATIONNTLMENABLED");
            if (tmp != null)
                cboxAuthorizationNTLMEnabled.Checked = Convert.ToInt32(tmp) == 1;

            //
            tmp = key.GetValue("ImpersonationAccountEnabled");
            if (tmp != null)
                cboxImpersonationAccountEnabled.Checked = Convert.ToInt32(tmp) == 1;

            if (cboxImpersonationAccountEnabled.Checked)
            {
                tboxImpersonationAccountUsername.Text =
                    (string)key.GetValue("IMPERSONATIONACCOUNTUSERNAME");
                tboxImpersonationAccountPassword.Enabled = tboxImpersonationAccountUsername.Enabled = true;
            }
            else { tboxImpersonationAccountPassword.Enabled = tboxImpersonationAccountUsername.Enabled = false; }
        }
        catch
        {
        }
    }


    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        ValidateFields();

        bool retVal = false;
        try
        {
            StringBuilder builder = new StringBuilder(256);
            builder.Append("<VbaSettings>");
            builder.Append("<ControlCenter>");

            builder.Append("<Update>");
            builder.AppendFormat("<PeriodicalUpdateEnabled type=" + "\"reg_dword\">" +
                 "{0}</PeriodicalUpdateEnabled><PeriodicalUpdatePeriod type=" + "\"reg_dword\">" +
                "{1}</PeriodicalUpdatePeriod>", cboxPeriodicalUpdateEnabled.Checked ? "1" : "0",
                tboxPeriodicalUpdated.Text);

            if (cboxPeriodicalUpdateEnabled.Checked)
            {
                builder.Append("<Source00>");
                builder.AppendFormat("<UPDATESOURCE type="+ "\"reg_sz\">" +
                    "{0}</UPDATESOURCE><ProxyEnabled type="+ "\"reg_dword\">" +
                    "{1}</ProxyEnabled>",tboxUpdateSource.Text.TrimEnd(' ').TrimStart(' '),cboxProxyEnabled.Checked?"1":"0");
                
                if (cboxProxyEnabled.Checked)
                {
                    builder.AppendFormat("<ProxyAddress type="+ "\"reg_sz\">" +"{0}</ProxyAddress>",
                        tboxProxyAddress.Text);
                    builder.AppendFormat("<ProxyPort type=" + "\"reg_dword\">" + "{0}</ProxyPort>",
                        tboxProxyPort.Text);
                }

                builder.AppendFormat("<AuthorizationEnabled type=" + "\"reg_dword\">" + "{0}</AuthorizationEnabled>",
                       cboxAuthorizationEnabled.Checked ? "1" : "0");
                if (cboxAuthorizationEnabled.Checked)
                {
                    builder.AppendFormat("<AuthorizationUsername type=" + "\"reg_sz\">" + "{0}</AuthorizationUsername>",
                        tboxAuthorizationUserName.Text);
                    //!- преобразовать в то, что нужно
                    byte[] bytes = Encoding.Default.GetBytes(tboxAuthorizationPassword.Text);
                    builder.AppendFormat("<AuthorizationPassword type=" + "\"reg_binary\">" + "{0}</AuthorizationPassword>",
                        Anchor.ConvertToDumpString(bytes));
                }

                builder.AppendFormat("<AuthorizationNTLMEnabled type=" + "\"reg_dword\">" + "{0}</AuthorizationNTLMEnabled>",
                      cboxAuthorizationNTLMEnabled.Checked ? "1" : "0");

                builder.AppendFormat("<ImpersonationAccountEnabled type=" + "\"reg_dword\">" + "{0}</ImpersonationAccountEnabled>",
                     cboxImpersonationAccountEnabled.Checked ? "1" : "0");

                if (cboxImpersonationAccountEnabled.Checked)
                {
                    builder.AppendFormat("<ImpersonationAccountUsername type=" + "\"reg_sz\">" + "{0}</ImpersonationAccountUsername>",
                     tboxImpersonationAccountUsername.Text);
                    //!- преобразовать в то, что нужно
                    byte[] bytes = Encoding.Default.GetBytes(tboxImpersonationAccountPassword.Text);
                    builder.AppendFormat("<ImpersonationAccountPassword type=" + "\"reg_binary\">" + "{0}</ImpersonationAccountPassword>",
                        Anchor.ConvertToDumpString(bytes));
                }

                builder.Append("</Source00>");
            }

            builder.Append("</Update>");

            builder.Append("</ControlCenter>");
            builder.Append("</VbaSettings>");


            IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                      typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                      ConfigurationManager.AppSettings["Vba32SS"]);

            retVal = remoteObject.ChangeRegistry(builder.ToString());

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("SaveSettings: " +
                    ex.Message + " " + Resources.Resource.Vba32SSUnavailable);
        }

        if (!retVal)
            throw new ArgumentException("Reread: Vba32SS return false!");
        InitState();

    }
}
