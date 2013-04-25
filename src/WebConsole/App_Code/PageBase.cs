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

    protected override void InitializeCulture()
    {
        CultureInfo culture = new CultureInfo((string)HttpContext.Current.Profile.GetPropertyValue("Culture"));
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        base.InitializeCulture();
    }


    protected abstract void InitFields();

}