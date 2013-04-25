using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;

public partial class UserManager : PageBase
{
    #region page life cycle
    protected new void Page_PreInit(object sender, EventArgs e)
    {
        Page.MasterPageFile = Profile.MasterPage;
        Page.Theme = Profile.Theme;
        base.Page_PreInit(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Roles.IsUserInRole("Administrator"))
        {
            Response.Redirect("Default.aspx");
        }
        Controls_PagerUserControl.AddGridViewExtendedAttributes(gridViewUsers, objectDataSourceUsers);
        Page.Title = Resources.Resource.PageUserManaging;
        if (!IsPostBack)
        {
            InitFields();
        }
        SetRegularExpressions();
    }

    protected override void InitFields()
    {
        btnCancelCreateUser.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/close.gif";
        btnCancelEditUser.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/close.gif";
    }
    protected void SetRegularExpressions()
    {
        regexPassword.ValidationExpression =
        "^.*(?=.{7,})(?=.*[\\W]).*$";
        regexEmail.ValidationExpression =
            "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
        regexEmailEdit.ValidationExpression =
            "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
    }
    #endregion

    #region users grid
    private static string defaultAdmin = "admin";
    protected void gridViewUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                break;
            case DataControlRowType.DataRow:
                UserEntity user = e.Row.DataItem as UserEntity;
                Literal lRole = e.Row.FindControl("lRole") as Literal;
                string role = user.Role;
                if (role == "Administrator")
                    role = Resources.Resource.Administrator;
                else if (role == "Operator")
                    role = Resources.Resource.Operator;
                else if (role == "Viewer")
                    role = Resources.Resource.Viewer;
                else
                    role = Resources.Resource.NotDefined;
                lRole.Text = role;
                
                ImageButton imgDelete = e.Row.FindControl("imgDelete") as ImageButton;
                if (user.Login == User.Identity.Name || user.Login == defaultAdmin)
                {
                    imgDelete.Visible = false;
                    imgDelete.Enabled = false;
                }
                else
                {
                    imgDelete.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/user_delete.png";
                    imgDelete.OnClientClick = "return confirm('" + Resources.Resource.AreYouSureUser + "');";
                }

                ImageButton imgEdit = e.Row.FindControl("imgEdit") as ImageButton;
                if (user.Login == defaultAdmin && User.Identity.Name != defaultAdmin)
                {
                    imgEdit.Visible = false;
                    imgEdit.Enabled = false;
                }
                else
                {
                    imgEdit.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/user_edit.png";                    
                    imgEdit.OnClientClick = String.Format("openEditPopup({0}, {1})", e.Row.RowIndex,
                        (user.Login == User.Identity.Name) ? "true" : "false");                
                }
                break;
            default:
                break;
        }
    }

    protected void gridViewUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteX") {
            string message = String.Empty;
            string user = e.CommandArgument as string;
            if (UsersDataContainer.Delete(user, out message))
            {
                gridViewUsers.DataBind();
                string key = "DeleteUserCallbackScript";
                string script = "alert('" + message + "');";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);
            }

        }
        
    }

    protected void btnHiddenRefresh_Init(object sender, EventArgs e)
    {
        btnHiddenRefresh.Width = Unit.Pixel(1);
        btnHiddenRefresh.Height = Unit.Pixel(1);
        btnHiddenRefresh.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");
        btnHiddenRefresh.Style.Add(HtmlTextWriterStyle.Padding, "0");
        btnHiddenRefresh.Style.Add(HtmlTextWriterStyle.Margin, "0");
        btnHiddenRefresh.Style.Add(HtmlTextWriterStyle.Overflow, "hidden");
        btnHiddenRefresh.Style.Add(HtmlTextWriterStyle.BackgroundColor, "transparent");
    }

    protected void btnHiddenRefresh_Click(object sender, EventArgs e)
    {
        gridViewUsers.DataBind();
    }

    #endregion

    #region create user
    [WebMethod]
    public static UserManagerResponse CreateUser(Dictionary<string, object> createUserRequest)
    {
        //Thread.Sleep(10000);
        string username = createUserRequest["username"] as string;
        string password = createUserRequest["password"] as string;
        string email = createUserRequest["email"] as string;
        string firstname = createUserRequest["firstname"] as string;
        string lastname = createUserRequest["lastname"] as string;
        string role = createUserRequest["role"] as string;
        string message = String.Empty;
        bool success = UsersDataContainer.Create(username, password, email, firstname, lastname, role, out message);
        return new UserManagerResponse(success, message);
    }

    public struct UserManagerResponse
    {
        public bool Success;
        public string Message;
        public UserManagerResponse(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
    }

    protected void imgProcessing_Init(object sender, EventArgs e)
    {
        (sender as Image).ImageUrl =
            "~/App_Themes/" + Profile.Theme + "/Images/loading-transparent.gif";
    }
    #endregion

    #region edit user
    [WebMethod]
    public static UserManagerResponse UpdateUser(Dictionary<string, object> createUserRequest)
    {
        //Thread.Sleep(10000);
        string username = createUserRequest["username"] as string;
        string email = createUserRequest["email"] as string;
        string firstname = createUserRequest["firstname"] as string;
        string lastname = createUserRequest["lastname"] as string;
        string role = createUserRequest["role"] as string;
        string message = String.Empty;
        bool success = UsersDataContainer.Update(username, email, firstname, lastname, role, out message);
        return new UserManagerResponse(success, message);
    }
    #endregion
}