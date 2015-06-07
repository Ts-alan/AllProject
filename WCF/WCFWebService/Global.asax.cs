using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WCFWebService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //EFContext.ContextHelper.Open();
#if DEBUG
            
#else
            //var proc = new System.Diagnostics.Process();
            //proc.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["NovellConnection"];
            //proc.Start();//  as string);
            NovellUtils.NetWare.ConnectToNovell();
#endif
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}