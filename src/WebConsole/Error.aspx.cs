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

using ARM2_dbcontrol.Generation;

/// <summary>
/// Страница для отображения сообщений об ошибках
/// </summary>
public partial class Error : System.Web.UI.Page
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
        try
        {
            Page.Title = Resources.Resource.Error;
            lblClear.Text = "<br/>" + Resources.Resource.ErrorClearSession;
            lbtnClear.Text = Resources.Resource.ErrorHere;

            Exception exc = (Exception)Session["ErrorException"];
            if (exc != null)
            {
                string errorMsg = exc.Message;
                string pageErrorOccured = Context.Request.UrlReferrer.ToString();

                Session["ErrorException"] = null;
              
                //!-OPTM: StringBilder будет лучше тут
                lblMessage.Text = Resources.Resource.ErrorRunTime+"<br/><br/>";
                lblMessage.Text = String.Format("{0} " + Resources.Resource.ErrorTryAgain +
                    " <a href='{1}'>"+Resources.Resource.ErrorHere+"</a>.<br/><br/>",
                    lblMessage.Text, pageErrorOccured);

                lblMessage.Text = lblMessage.Text +
                Resources.Resource.ErrorMessageError + ": '" + errorMsg+"'";

                //this code is obsolete
               // if( (ConfigurationManager.AppSettings["ErrorLog"]!=null)||(ConfigurationManager.AppSettings["ErrorLog"] != String.Empty))
                //{
                //    Logger log = new Logger(System.AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["ErrorLog"],
                //                 true, System.Text.Encoding.Unicode, Profile.UserName);
                //    log.Write("PAGE: "+pageErrorOccured +". ERROR MESSAGE: "+ errorMsg);
                //}
            }
            else
                lblMessage.Text = Resources.Resource.Error;

           
        }
        catch
        {
        }

    }
    protected void lbtnClear_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Application.Clear();
        Response.Redirect("Default.aspx");
    }
}
