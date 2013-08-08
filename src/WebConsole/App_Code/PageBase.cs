using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Threading;

//using VirusBlokAda.Vba32CC.Policies;

/// <summary>
/// Summary description for PageBase
/// </summary>
public abstract class PageBase : PackViewState.Page
{
	public PageBase()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    protected void RegisterScript(string src)
    {
        HtmlGenericControl include = new HtmlGenericControl("script");
        include.Attributes.Add("type", "text/javascript");
        include.Attributes.Add("src", src);
        this.Page.Header.Controls.Add(include);
    }
    protected void RegisterBlockScript(string scriptText)
    {
        HtmlGenericControl include = new HtmlGenericControl("script");
        include.Attributes.Add("type", "text/javascript");
        include.Attributes.Add("runat", "server");
        include.InnerHtml = scriptText;        
        this.Page.Header.Controls.Add(include);
    }
    protected void RegisterLink(string src)
    {
        HtmlGenericControl include = new HtmlGenericControl("link");
        include.Attributes.Add("type", "text/css");
        include.Attributes.Add("src", src);
        this.Page.Header.Controls.Add(include);
    }
    
    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.MasterPageFile = (string) HttpContext.Current.Profile.GetPropertyValue("MasterPage");
        Page.Theme = (string)HttpContext.Current.Profile.GetPropertyValue("Theme");
    }
    protected void Page_Init(object sender, EventArgs e)
    {
         RegisterLink("~/App_Themes/" + (string)HttpContext.Current.Profile.GetPropertyValue("Theme") + @"/ui/jquery-ui-1.10.3.custom.min.css");
        RegisterScript(@"js/jQuery/jquery-1.10.2.min.js");
        RegisterScript(@"js/jQuery/jquery-ui-1.10.3.custom.min.js"); 
    }
   
    protected override void InitializeCulture()
    {
        CultureInfo culture = new CultureInfo((string)HttpContext.Current.Profile.GetPropertyValue("Culture"));
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        base.InitializeCulture();
    }


    protected abstract void InitFields();

}
