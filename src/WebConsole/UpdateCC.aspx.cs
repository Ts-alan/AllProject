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
using VirusBlokAda.Vba32CC.Service.VSIS;

public partial class UpdateCC : PageBase
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
        Page.Title = Resources.Resource.PageUpdateTitle;

        if (!Page.IsPostBack)
            InitFields();

        CheckUpdateProcess();
    }

    private void CheckUpdateProcess()
    {
        lblLastSuccessUpdate.Text = Resources.Resource.LastSuccessUpdate + ": " + (VSISWrapper.LastUpdate != DateTime.MinValue ? VSISWrapper.LastUpdate.ToString() : "-");
        divLastUpdate.Visible = true;

        if (VSISWrapper.IsUpdateAlive)
        {
            lbtnCancelUpdate.Enabled = true;
            lbtnUpdate.Enabled = false;
            lblLastUpdate.Text = Resources.Resource.UpdateCC + "..."
                + (!String.IsNullOrEmpty(VSISWrapper.UpdateStopReason) ?
                String.Format("({0}: {1})", Resources.Resource.ErrorMessageError, VSISWrapper.UpdateStopReason) : "");
        }
        else
        {
            lbtnCancelUpdate.Enabled = false;
            lbtnUpdate.Enabled = true;
            if (!String.IsNullOrEmpty(VSISWrapper.UpdateStopReason))
            {
                lblLastUpdate.Text = Resources.Resource.LastUpdate + ": " + String.Format("({0}: {1})", Resources.Resource.ErrorMessageError, VSISWrapper.UpdateStopReason);
            }
            else
            {
                divLastUpdate.Visible = false;
            }
        }
    }

    protected override void InitFields()
    {
        lblUpdateSource.Text = Resources.Resource.UpdatePath;

        cboxProxyEnabled.Text = Resources.Resource.UseProxy;
        lblProxyAddress.Text = Resources.Resource.Server;
        lblProxyPort.Text = Resources.Resource.Port;

        cboxAuthorizationEnabled.Text = Resources.Resource.UseAuthorization;
        lblAuthorizationUserName.Text = Resources.Resource.User;
        lblAuthorizationPassword.Text = Resources.Resource.PasswordLabelText;

        cboxProxyAuthorizationEnabled.Text = Resources.Resource.UseAuthorization;
        lblProxyAuthorizationUserName.Text = Resources.Resource.User;
        lblProxyAuthorizationPassword.Text = Resources.Resource.PasswordLabelText;

        lbtnSave.Text = Resources.Resource.Save;

        VSISWrapper.Initialize();
        InitState();
    }

    private Boolean ValidateFields(out String error)
    {
        if (String.IsNullOrEmpty(tboxUpdateSource.Text))
        {
            error = Resources.Resource.ErrorInvalidValue + ": " + Resources.Resource.UpdatePath;
            return false;
        }

        if (cboxAuthorizationEnabled.Checked)
        {
            if (String.IsNullOrEmpty(tboxAuthorizationUserName.Text))
            {
                error = Resources.Resource.ErrorInvalidValue + ": " + Resources.Resource.User;
                return false;
            }
        }

        if (cboxProxyEnabled.Checked)
        {
            if (String.IsNullOrEmpty(tboxProxyAddress.Text))
            {
                error = Resources.Resource.ErrorInvalidValue + ": " + Resources.Resource.Server;
                return false;
            }

            Validation vld = new Validation(tboxProxyPort.Text);
            if (!vld.CheckUInt32())
            {
                error = Resources.Resource.ErrorInvalidValue + ": " + Resources.Resource.Port;
                return false;
            }

            if (cboxProxyAuthorizationEnabled.Checked)
            {
                if (String.IsNullOrEmpty(tboxProxyAuthorizationUserName.Text))
                {
                    error = Resources.Resource.ErrorInvalidValue + ": " + Resources.Resource.Proxy + " - " + Resources.Resource.User;
                    return false;
                }
            }
        }

        error = String.Empty;
        return true;
    }

    private void InitState()
    {
        UpdateProperties up = VSISWrapper.GetUpdateParameters();

        tboxUpdateSource.Text = up.UpdatePathes.Length > 0 ? up.UpdatePathes[0] : String.Empty;

        if (!String.IsNullOrEmpty(up.AuthorityName))
        {
            cboxAuthorizationEnabled.Checked = true;
            tboxAuthorizationUserName.Text = up.AuthorityName;
            tboxAuthorizationPassword.Attributes.Add("value", up.AuthorityPassword);
        }
        else
        {
            tboxAuthorizationPassword.Enabled = tboxAuthorizationUserName.Enabled = false;
        }

        if (!String.IsNullOrEmpty(up.ProxyAddress))
        {
            cboxProxyEnabled.Checked = true;
            tboxProxyAddress.Text = up.ProxyAddress;
            tboxProxyPort.Text = up.ProxyPort.ToString();
            if (!String.IsNullOrEmpty(up.ProxyAuthorityName))
            {
                cboxProxyAuthorizationEnabled.Checked = true;                
                tboxProxyAuthorizationUserName.Text = up.ProxyAuthorityName;
                tboxProxyAuthorizationPassword.Attributes.Add("value", up.ProxyAuthorityPassword);
            }
            else
            {
                tboxProxyAuthorizationUserName.Enabled = tboxProxyAuthorizationPassword.Enabled = false;
            }
        }
        else
        {
            tboxProxyAddress.Enabled = tboxProxyPort.Enabled = tboxProxyAuthorizationUserName.Enabled = tboxProxyAuthorizationPassword.Enabled = false;
            cboxProxyAuthorizationEnabled.InputAttributes.Add("disabled", "true");
        }
        
    }

    protected void lbtnSave_Click(Object sender, EventArgs e)
    {
        String error;
        if (!ValidateFields(out error))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ValidationFalseScript", String.Format("alert('{0}');", error), true);
            return;
        }
                
        UpdateProperties up = new UpdateProperties();
        
        up.UpdatePathes = new String[] { tboxUpdateSource.Text };
        
        if (cboxAuthorizationEnabled.Checked)
        {
            up.AuthorityName = tboxAuthorizationUserName.Text;
            up.AuthorityPassword = tboxAuthorizationPassword.Text;
        }

        if (cboxProxyEnabled.Checked)
        {
            up.ProxyAddress = tboxProxyAddress.Text;
            up.ProxyPort = Convert.ToUInt32(tboxProxyPort.Text);
            if (cboxProxyAuthorizationEnabled.Checked)
            {
                up.ProxyAuthorityName = tboxProxyAuthorizationUserName.Text;
                up.ProxyAuthorityPassword = tboxProxyAuthorizationPassword.Text;
            }
        }

        VSISWrapper.SetUpdateParameters(up);
        InitState();
    }

    protected void lbtnUpdate_Click(Object sender, EventArgs e)
    {
        VSISWrapper.Update();
        lbtnCancelUpdate.Enabled = true;
        lbtnUpdate.Enabled = false;
    }

    protected void lbtnCancelUpdate_Click(Object sender, EventArgs e)
    {
        VSISWrapper.UpdateAbort();
        lbtnCancelUpdate.Enabled = false;
        lbtnUpdate.Enabled = true;
    }

    protected void timer1_tick(Object sender, EventArgs e)
    {
    }
}
