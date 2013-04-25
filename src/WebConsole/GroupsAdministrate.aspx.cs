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

public partial class GroupsAdministrate : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("Administrator"))
        {
            Response.Redirect("Default.aspx");
        }

        if (!Page.IsPostBack)
        {
            InitFields();
        }

        Page.Title = Resources.Resource.GroupManagment;
    }

    protected override void InitFields()
    {
        RegisterLink("~/App_Themes/" + Profile.Theme + @"/Groups/Groups.css");
        RegisterLink("~/App_Themes/" + Profile.Theme + @"/Groups/css/ext-all.css");       
    }
}
