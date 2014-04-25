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

        string culture;
            
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

    void SiteSpecificUserLoggingMethod(string UserName)
    {

        // Insert code to record the current date and time
        // when this user was authenticated at the site.

        //LazyLoad
        /*ProfileCommon profile = Profile.GetProfile(UserName);

        //Starup init: read user settings
        SettingsEntity settings = new SettingsEntity();
        try
        {
            settings = settings.Deserialize(profile.Settings);
        }
        catch
        {
            settings = new SettingsEntity();
        }

        //!-- учесть ситуацию если фильтров вообще нет в базе.. Что тогда будет?
        CompFilterCollection compCollection;
        EventFilterCollection eventCollection;
        TaskFilterCollection taskCollection;
        CmptFilterCollection cmptCollection;
        ProcFilterCollection procCollection;
        TaskUserCollection taskUserCollection;
        //   try
        //   {
        compCollection = new CompFilterCollection(profile.CompFilters);
        //   }
        //  catch
        //  {
        //      compCollection = new CompFilterCollection();
        //  }
        //   try
        //   {
        eventCollection = new EventFilterCollection(profile.EventFilters);
        //  }
        //  catch
        //  {
        //     eventCollection = new EventFilterCollection();
        //  }
        taskCollection = new TaskFilterCollection(profile.TaskFilters);
        taskUserCollection = new TaskUserCollection(profile.TasksList);

        cmptCollection = new CmptFilterCollection(profile.ComponentFilters);
        procCollection = new ProcFilterCollection(profile.ProcessFilters);

        //Context.Session
        Session["LoginVisit"] = profile.LastVisit;

        profile.LastVisit = DateTime.Now;      

        profile.Save();    

        Session["Settings"] = settings;
        Session["CompFilters"] = compCollection;
        Session["EventFilters"] = eventCollection;
        Session["TaskFilters"] = taskCollection;
        Session["ComponentFilters"] = cmptCollection;
        Session["ProcessFilters"] = procCollection;
        Session["TaskUser"] = taskUserCollection;*/
    }


    protected void lgLogin_LoggedIn(object sender, EventArgs e)
    {
        SiteSpecificUserLoggingMethod(lgLogin.UserName);
        
    }
   
    private void CheckConnection()
    {
        lblError.Visible = false;
        try
        {
            using (VlslVConnection conn = new VlslVConnection(
            ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                conn.OpenConnection();
                conn.CloseConnection();
            }
        }
        catch (SqlException ex)
        {
            lgLogin.Enabled = false;
            lblError.Visible = true;

            string errorText = "Cannot connect to database: ";
            switch (Request.QueryString["lang"])
            {
                case "ru":
                    errorText = "Ошибка при попытке подключиться к бд: ";
                    break;
            }


            lblError.Text = errorText + ex.Message;
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
