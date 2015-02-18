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
using GreateProfile;

/// <summary>
/// Master page: 1
/// </summary>
public partial class mstrPageMain : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoginStatus1.LogoutText = ProfileCommon.CurrentUser.UserName + "[" + Resources.Resource.Logout + ']';
            lblVersion.Text = ConfigurationManager.AppSettings["Version"];

            imLogo.ImageUrl = "~/App_Themes/" + ProfileCommon.CurrentUser.Theme + "/Images/vba32ccwi.gif";
        }
    }
    protected void mnLeft_MenuItemDataBound(object sender, MenuEventArgs e)
    {
        //!-OPTM Данную вещь надо делать в sitemap, применив соответсвующий
        //декларативный подход и в webconfig 
        if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
        {
            if (
                (e.Item.Text == Resources.Resource.ControlCenter))
            {
                e.Item.Enabled = false;
                e.Item.Parent.ChildItems.Remove(e.Item);
            }
            if (
              (e.Item.Text == Resources.Resource.Tasks))
            {
                e.Item.Enabled = false;
                e.Item.Parent.ChildItems.Remove(e.Item);
            }
          /*  if (
          (e.Item.Text == Resources.Resource.GiveTask))
            {
                e.Item.Enabled = false;
                e.Item.Parent.ChildItems.Remove(e.Item);
            }*/
        }

        if (!Roles.IsUserInRole("Administrator"))
        {
            if (e.Item.Text == Resources.Resource.Administration)
            {
                e.Item.Enabled = false;
                e.Item.Parent.ChildItems.Remove(e.Item);
            }
        }

    }
    protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Session.Abandon();
    }
}
