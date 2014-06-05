using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.IO;

using ARM2_dbcontrol.Filters;

/// <summary>
/// Settings main
/// </summary>
public partial class SettingsMain : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(ddlLanguage.Items.Count ==0)
            GetLanguageList();

        if (ddlMasterPage.Items.Count == 0)
        {
            ddlMasterPage.DataSource = GetMasterPages();
            ddlMasterPage.DataBind();
        }

        if (ddlTheme.Items.Count == 0)
        {
            ddlTheme.DataSource = GetThemes();
            ddlTheme.DataBind();
        }

        Page.Title = Resources.Resource.PageSettingsMainTitle;
        if (!IsPostBack)
        {
            InitFields();     
        }
    }

    /// <summary>
    /// Initialization fields
    /// </summary>
    protected override void InitFields()
    {
        chpwdPassword.ContinueButtonText = Resources.Resource.ContinueButtonText;
        chpwdPassword.NewPasswordRegularExpressionErrorMessage = Resources.Resource.NewPasswordRegularExpressionErrorMessage;
        chpwdPassword.PasswordRequiredErrorMessage = Resources.Resource.PasswordRequiredErrorMessage;

        (chpwdPassword.Controls[0].FindControl("CurrentPasswordRequired") as Label).Text = Resources.Resource.ChangePasswordFailureText;
        (chpwdPassword.Controls[0].FindControl("NewPasswordRequired") as Label).Text = Resources.Resource.NewPasswordRequiredErrorMessage;
        (chpwdPassword.Controls[0].FindControl("ConfirmNewPasswordRequired") as Label).Text = Resources.Resource.ConfirmPasswordRequiredErrorMessage;
        (chpwdPassword.Controls[0].FindControl("NewPasswordCompare") as Label).Text = Resources.Resource.ConfirmPasswordCompareErrorMessage;

        tbFirstName.Text = Profile.FirstName == String.Empty ? "admin" : Profile.FirstName;
        tbLastName.Text = Profile.LastName == String.Empty ? "admin" : Profile.LastName;

        lblLogin.Text = Profile.UserName;
        String[] master = Page.MasterPageFile.Split('.');

        ddlMasterPage.SelectedValue = master[0];
        ddlTheme.SelectedValue = Page.Theme;
    }

    /// <summary>
    /// Save click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Validation vld = new Validation(tbFirstName.Text);
        if (!vld.CheckStringValue())
            throw new ArgumentException(Resources.Resource.ErrorInvalidFirstName);
        Profile.FirstName = tbFirstName.Text;

        vld.Value = tbLastName.Text;

        if (!vld.CheckStringValue())
            throw new ArgumentException(Resources.Resource.ErrorInvalidLastName);

        Profile.LastName = tbLastName.Text;
        Profile.MasterPage = ddlMasterPage.SelectedValue+".master";
        Profile.Theme = ddlTheme.SelectedValue;

       
        switch (ddlLanguage.SelectedValue)
        {
            case "Русский":
                Profile.Culture = "ru-RU";
                break;

            case "English":
            default:
                Profile.Culture = "en-US";
                break;
        }
       
       Response.Redirect("Default.aspx"); 
    }

    /// <summary>
    /// Themes list
    /// </summary>
    /// <returns></returns>
    public String[] GetThemes()
    {
        // получаем список путей к подпапкам папки App_Themes 
        String[] folders = Directory.GetDirectories(
            System.Web.HttpContext.Current.Server.MapPath("~/App_Themes"));
        // отрезаем от найденных полных путей только названия самих папок 
        for (Int32 i = 0; i < folders.Length; i++)
        {
            folders[i] = new DirectoryInfo(folders[i]).Name;
        }
        return folders;
    }

    /// <summary>
    /// Master page list
    /// </summary>
    /// <returns></returns>
    public List<String> GetMasterPages()
    {     
        List<String> list = new List<String>();
        DirectoryInfo dir = new DirectoryInfo(
            System.Web.HttpContext.Current.Server.MapPath("~"));

        foreach (FileInfo file in dir.GetFiles())
        {         
            if (file.Extension == ".master")
            {
                String[] names = file.Name.Split('.');
                //list.Add(file.Name);
                list.Add(names[0]);
            }
        }

        return list;
    }

    /// <summary>
    /// Language list
    /// </summary>
    /// <returns></returns>
    public Hashtable GetLanguageList()
    {
        if (ddlLanguage.Items.Count == 0)
        {
            ddlLanguage.Items.Add("English");
            ddlLanguage.Items.Add("Русский");
        }

        if (Profile.Culture == "ru-RU")
            ddlLanguage.SelectedValue = "Русский";

        if (Profile.Culture == "en-US")
            ddlLanguage.SelectedValue = "en-US";

        return null;
    }
}