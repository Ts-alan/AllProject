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

using ARM2_dbcontrol.Filters;
using ARM2_dbcontrol.Tasks;

/// <summary>
/// Registration new user
/// </summary>
public partial class Registration : System.Web.UI.Page
{
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
        Page.Title = Resources.Resource.PageRegistrationTitle;

        if (!Roles.IsUserInRole("Administrator"))
        {
            //throw new Exception(Resources.Resource.ErrorAccessDenied);
            Response.Redirect("Default.aspx");
        }

        if (!IsPostBack)
        {
            InitFields();
        }       
    }

    /// <summary>
    /// Initialization fields
    /// </summary>
    private void InitFields()
    {
        //
        Control ctrl = (CreateUserWizard1.WizardSteps[0].Controls[0].Controls[0].Controls[0].Controls[0]);
        (ctrl.FindControl("ConfirmPasswordLabel") as Label).Text = Resources.Resource.ConfirmPasswordLabelText;
        (ctrl.FindControl("PasswordLabel") as Label).Text = Resources.Resource.PasswordLabelText;
        (ctrl.FindControl("EmailLabel") as Label).Text = Resources.Resource.Email;
        (ctrl.FindControl("PasswordCompare") as Label).Text = Resources.Resource.ConfirmPasswordCompareErrorMessage;
        (ctrl.FindControl("PasswordRequired") as Label).Text = Resources.Resource.InvalidPasswordErrorMessage;
        (ctrl.FindControl("UserNameLabel") as Label).Text = Resources.Resource.UserLogin;

        (ctrl.FindControl("lblFirstName") as Label).Text = Resources.Resource.FirstName;
        (ctrl.FindControl("lblLastName") as Label).Text = Resources.Resource.LastName;

        CreateUserWizard1.CreateUserButtonText = Resources.Resource.CreateUserButtonText;

        (ctrl.FindControl("rbRoles") as RadioButtonList).Items.Add(Resources.Resource.Viewer);
        (ctrl.FindControl("rbRoles") as RadioButtonList).Items.Add(Resources.Resource.Operator);
        (ctrl.FindControl("rbRoles") as RadioButtonList).Items.Add(Resources.Resource.Administrator);
        (ctrl.FindControl("rbRoles") as RadioButtonList).SelectedIndex = 0;
        //.
        
        
    }

    protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
    {
        Validation vld = new Validation(CreateUserWizard1.UserName);
        if (!vld.CheckUserLogin())
            throw new ArgumentException(Resources.Resource.ErrorInvalidUserName);

        vld.Value = ((CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("tboxFirstName") as TextBox)).Text;
        if (!vld.CheckStringEnRuValue())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue +": "+Resources.Resource.FirstName);

        vld.Value = ((CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("tboxLastName") as TextBox).Text);

        if (!vld.CheckStringEnRuValue())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": " + Resources.Resource.LastName);

        vld.Value = CreateUserWizard1.Email;
        if(!vld.CheckEmail())
            throw new ArgumentException(Resources.Resource.ErrorInvalidEmail);

    }
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        string userName = CreateUserWizard1.UserName;
        
        if (userName == null)
            throw new InvalidOperationException(Resources.Resource.ErrorCreateUser);
        if (Roles.GetRolesForUser(userName).Length != 0)
            Roles.RemoveUserFromRoles(userName, Roles.GetRolesForUser(userName));
        Control ctrl = (CreateUserWizard1.WizardSteps[0].Controls[0].Controls[0].Controls[0].Controls[0]);

        string strRole = "Viewer";
        if ((ctrl.FindControl("rbRoles") as RadioButtonList).SelectedValue == Resources.Resource.Administrator)
            strRole = "Administrator";
        if ((ctrl.FindControl("rbRoles") as RadioButtonList).SelectedValue == Resources.Resource.Operator)
            strRole = "Operator";
        Roles.AddUserToRole(userName, strRole);

        ProfileCommon prof = Profile.GetProfile(userName);
        prof.FirstName = ((CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("tboxFirstName") as TextBox)).Text;
        prof.LastName = ((CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("tboxLastName") as TextBox)).Text;
        prof.Save();
    }
}
