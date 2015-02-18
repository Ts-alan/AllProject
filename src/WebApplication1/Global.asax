<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        try
        {
            //Создаем роли, если их не существует
            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");
            if (!Roles.RoleExists("Operator"))
                Roles.CreateRole("Operator");
            if (!Roles.RoleExists("Viewer"))
                Roles.CreateRole("Viewer");

            //создаем пользователя с именем admin и паролем по умолчанию
            string admin = "admin";
            //Membership.DeleteUser(admin);

            if (Membership.GetUser(admin) == null)
            {
                MembershipUser msAdmin = Membership.CreateUser(admin, "1234qwer!", "admin@admin.com");

                if (Roles.GetRolesForUser(admin).Length != 0)
                    Roles.RemoveUserFromRoles(admin, Roles.GetRolesForUser(admin));
                Roles.AddUserToRole(admin, "Administrator");
            }
            
        }
        catch
        {
            //Application["SqlExceptOccured"] = new Exception("vbaMembership");
        }

        try
        {
            EncryptConnString();
        }
        catch { }
    }

    private void EncryptConnString()
    {        
        Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/Vba32CCWebConsole"); //  "/WebConsole" for local
        ConfigurationSection section = config.GetSection("connectionStrings");
        if (!section.SectionInformation.IsProtected)
        {
            section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
            config.Save();
        }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    { 
        //Session["ErrorException"] = Server.GetLastError().InnerException;
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

        //[HttpException]: Session state has created a session id, but cannot
        //				   save it because the response was already flushed by
        string sessionId = Session.SessionID;
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
