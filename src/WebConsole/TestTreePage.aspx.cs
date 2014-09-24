using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestTreePage : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScript(@"js/jstree.js");
        RegisterLink("~/App_Themes/" + (String)HttpContext.Current.Profile.GetPropertyValue("Theme") + @"/jsTree/style.css");
        
    }

    protected override void InitFields()
    {
        
    }
}