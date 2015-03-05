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
using System.Text;
using GreateProfile;

//using VirusBlokAda.Vba32CC.Policies;

/// <summary>
/// Summary description for PageBase
/// </summary>
public abstract class PageBase : PackViewState.Page
{
	public PageBase()
	{
	}

    #region Registering

    protected void RegisterScript(String src)
    {
        HtmlGenericControl include = new HtmlGenericControl("script");
        include.Attributes.Add("type", "text/javascript");
        include.Attributes.Add("src", src);
        this.Page.Header.Controls.Add(include);
    }

    protected void RegisterBlockScript(String scriptText)
    {
        HtmlGenericControl include = new HtmlGenericControl("script");
        include.Attributes.Add("type", "text/javascript");
        include.InnerHtml = scriptText;        
        this.Page.Header.Controls.Add(include);
    }

    protected void RegisterLink(String src)
    {
        HtmlGenericControl include = new HtmlGenericControl("link");
        include.Attributes.Add("type", "text/css");
        include.Attributes.Add("src", src);
        this.Page.Header.Controls.Add(include);
    }

    /// <summary>
    /// Register resources for using in JavaScript
    /// </summary>
    /// <param name="resources"></param>
    protected void ResourceRegister(String[] resources)
    {
        StringBuilder script = new StringBuilder("var Resource={");

        for (Int32 index = 0; index < resources.Length; index++)
        {
            script.AppendFormat("{0}:'{1}'", resources[index], ResourceControl.GetStringForCurrentCulture(resources[index]));
            if (index != resources.Length - 1)
                script.Append(", ");
        }        
        script.Append("}");
        RegisterBlockScript(script.ToString());
    }

    #endregion

    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.MasterPageFile = ProfileCommon.CurrentUser.MasterPage;//(String) HttpContext.Current.Profile.GetPropertyValue("MasterPage")!=""?(String) HttpContext.Current.Profile.GetPropertyValue("MasterPage"):"mstrPageMain.master";
        Page.Theme = ProfileCommon.CurrentUser.Theme; //(String)HttpContext.Current.Profile.GetPropertyValue("Theme");
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        RegisterLink("~/App_Themes/" + (String)HttpContext.Current.Profile.GetPropertyValue("Theme") + @"/ui/jquery-ui-1.10.3.custom.min.css");
        RegisterScript(@"js/jQuery/jquery-1.10.2.min.js");
        RegisterScript(@"js/jQuery/jquery-ui-1.10.3.custom.min.js");
    }
   
    protected override void InitializeCulture()
    {
        CultureInfo culture = new CultureInfo((String)HttpContext.Current.Profile.GetPropertyValue("Culture"));
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        base.InitializeCulture();
    }


    protected abstract void InitFields();

}
