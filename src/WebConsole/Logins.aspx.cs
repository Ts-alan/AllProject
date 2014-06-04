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

using System.Data.SqlClient;

using ARM2_dbcontrol.Filters;
using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Tasks;


public partial class Logins : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        String culture;
            
        switch (Request.QueryString["lang"] )
        {
            case "ru":
                culture = "ru-RU";
                break;

            case "en":
            default:
                culture = "en-US";
                break;
        }

        System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo(culture);
        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Resources.Resource.PageLoginTitle;
        if (!IsPostBack)
        {
            InitFields();
        }
        CheckConnection();
    }

    private void InitFields()
    {
        lgLogin.UserNameLabelText = Resources.Resource.Login + "&nbsp;&nbsp;";
        lgLogin.PasswordLabelText = Resources.Resource.PasswordLabelText + "&nbsp;&nbsp;";
        lgLogin.TitleText = Resources.Resource.Authentification;
        lgLogin.LoginButtonText = Resources.Resource.LoginButton;
        lgLogin.FailureText = Resources.Resource.LoginFailureText;
        
        CheckAdmin(); 
    }

    private void CheckConnection()
    {
        lblError.Visible = false;
        Exception error;
        if (!DataBaseProvider.CheckConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString, out error))
        {
            lgLogin.Enabled = false;
            lblError.Visible = true;

            String errorText = "Cannot connect to database: ";
            switch (Request.QueryString["lang"])
            {
                case "ru":
                    errorText = "Ошибка при попытке подключиться к бд: ";
                    break;
            }

            lblError.Text = errorText + error.Message;
            return;
        }
    }

    private void CheckAdmin()
    {
        try
        {
            MembershipUser admin = Membership.GetUser("admin");
            if (admin.IsLockedOut)
            {
                admin.UnlockUser();
            }
        }
        catch(Exception ex)
        {
            lgLogin.Enabled = false;
            lblError.Visible = true;
            lblError.Text = "Ошибка при попытке подключиться к бд: " + ex.Message;
            return;
        }
    }
}
