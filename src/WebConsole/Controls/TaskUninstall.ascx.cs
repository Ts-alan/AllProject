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
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;
using VirusBlokAda.RemoteOperations.Wmi;
using VirusBlokAda.RemoteOperations.Common;

public partial class Controls_TaskUninstall : System.Web.UI.UserControl
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        rbtnlProviders.Items.Add(Resources.Resource.WMI);
        rbtnlProviders.Items.Add(Resources.Resource.RemoteService);
        rbtnlProviders.SelectedIndex = 0;
    }

    public string Domain
    {
        get 
        {
            return tboxDomain.Text;
        }
    }
    public string Login
    {
        get 
        {
            return tboxLogin.Text;
        }
    }

    public string Password
    {
        get
        {
            return tboxPassword.Text;
        }
    }

    public bool DoRestart
    {
        get 
        {
            return cbRebootAfterInstall.Checked;
        }
    }

    public RemoteMethodsEnum Provider
    {
        get 
        {
            if (rbtnlProviders.SelectedValue== Resources.Resource.RemoteService)
            {
                return RemoteMethodsEnum.RemoteService;
            }
            return RemoteMethodsEnum.Wmi;
        }
    }

    public TimeSpan PollingTime
    {
        get 
        {
            return new TimeSpan(0, 0, 10);

            //if (String.IsNullOrEmpty(tbPollingTime.Text))
            //{
            //    return new TimeSpan(0, 0, 10);
            //}
            //else
            //{
            //    return new TimeSpan(0, 0, int.Parse(tbPollingTime.Text));
            //} 
        }
    }

    public TimeSpan Timeout
    {
        get
        {
            return new TimeSpan(0, 10, 0);
        }
    }

    public bool ValidateFields()
    {
        if (String.IsNullOrEmpty(tboxLogin.Text))
        {
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.Login);
        }
        if (String.IsNullOrEmpty(tboxPassword.Text))
        {
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.PasswordLabelText);
        }
        if (String.IsNullOrEmpty(tboxDomain.Text))
        {
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.DomainName);
        }
        return true;
    }

}
