using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Security;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitFields();
        }
        if (Request.UserAgent.IndexOf("AppleWebKit") > 0)
        {
            Request.Browser.Adapters.Clear();
        }

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        LogoutButton.ServerClick += new EventHandler(tmp);
    }

    private void InitFields()
    {
        
        lblVersion.Text = ConfigurationManager.AppSettings["Version"];
        
    }

    protected void tmp(object sender, EventArgs e)
    {
        Session.Abandon();
        FormsAuthentication.SignOut();
        FormsAuthentication.RedirectToLoginPage();
    }
}
