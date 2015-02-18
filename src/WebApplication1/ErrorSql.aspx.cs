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

public partial class ErrorSql : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {

        Page.MasterPageFile = ProfileCommon.CurrentUser.MasterPage;
        Page.Theme = ProfileCommon.CurrentUser.Theme;
    }

    protected override void InitializeCulture()
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo(ProfileCommon.CurrentUser.Culture);
        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Resources.Resource.Error;
    }
}
