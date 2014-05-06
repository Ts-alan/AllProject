using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Configuration;
using Microsoft.Win32;
using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Filters;
using Vba32.ControlCenter.Service;

/// <summary>
/// Defaults page
/// </summary>
public partial class _Default : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFields();
        }
    }

    protected override void InitFields()
    {
        if (Session["LoginVisit"] != null)
            lblLastVist.Text =
                ((DateTime)Session["LoginVisit"]).ToString();
        else
        {
            try
            {
                MembershipUser user = Membership.GetUser(Profile.UserName == String.Empty ? "Admin" : Profile.UserName);
                lblLastVist.Text = user.LastLoginDate.ToString();
            }
            catch{}
        }

        Page.Title = Resources.Resource.PageDefaultTitle;

        try
        {

            lblARM2DataBaseDataSource.Text = ((new Regex(@"data source=[a-zA-Z_0-9-().\\//]+")).Match(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString)).Value.Substring(12);
            //tboxARM2DataBaseDataSource.Text = ((new Regex(@"data source=[a-zA-Z_0-9-()]+")).Match(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString)).Value.Substring(12);

            lblARM2DataBaseUserID.Text = ((new Regex(@"user id=\w+")).Match(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString)).Value.Substring(8);
            lblARM2DataBaseInitialCatalog.Text = ((new Regex(@"initial catalog=\w+")).Match(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString)).Value.Substring(16);

            lblMembershipDataSource.Text = ((new Regex(@"data source=[a-zA-Z_0-9-().\\//]+")).Match(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString)).Value.Substring(12);
            lblMembershipUserID.Text = ((new Regex(@"user id=\w+")).Match(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString)).Value.Substring(8);
            lblMembershipInitialCatalog.Text = ((new Regex(@"initial catalog=\w+")).Match(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString)).Value.Substring(16);

        }
        catch
        {
        }

        DataBaseEntity dbEntity;
        DataBaseEntity dbEntityLog;

        try
        {
            dbEntity = DBProviders.DataBase.Get("vbaCCDB");
        }
        catch
        {
            dbEntity = new DataBaseEntity();
        }

        try
        {
            dbEntityLog = DBProviders.DataBase.Get("vbaCCLog");
        }
        catch
        {   
            dbEntityLog = new DataBaseEntity();
        }

        int size = 0;
        int maxsize = 1;
        double percent = 0.0;
        try
        {
            size = Int32.Parse(dbEntity.Size.Substring(0, dbEntity.Size.IndexOf(' ')));
            maxsize = Int32.Parse(dbEntity.MaxSize.Substring(0, dbEntity.MaxSize.IndexOf(' ')));
            percent = (double)size / (double)maxsize;
        }
        catch { }

        lblARM2DBName.Text = dbEntity.Name;
        lblARM2DBPath.Text = dbEntity.Path;
        lblARM2DBSize.Text = String.Format("{0} / {1}", dbEntity.Size, dbEntity.MaxSize);
        if (percent >= 0.9)
        {
            rowDBSize.Attributes.Add("class", "stateKeyBad");
        }
        else
        {
            rowDBSize.Attributes.Add("class", "");
        }

        try
        {
            size = Int32.Parse(dbEntityLog.Size.Substring(0, dbEntityLog.Size.IndexOf(' ')));
            maxsize = Int32.Parse(dbEntityLog.MaxSize.Substring(0, dbEntityLog.MaxSize.IndexOf(' ')));
            percent = (double)size / (double)maxsize;
        }
        catch { }
        lblARM2LogName.Text = dbEntityLog.Name;
        lblARM2LogPath.Text = dbEntityLog.Path;
        lblARM2LogSize.Text = String.Format("{0} / {1}", dbEntityLog.Size, dbEntityLog.MaxSize);
        if (percent >= 0.9)
        {
            rowDBLogSize.Attributes.Add("class", "stateKeyBad");
        }
        else
        {
            rowDBLogSize.Attributes.Add("class", "");
        }

        GetKeyInfo();

        FillServicesTable();


    }
        
    /// <summary>
    /// Получаем информацию о лицензионном ключе
    /// </summary>
    private void GetKeyInfo()
    {
        string registryControlCenterKeyName;
        if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
            registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
        else
            registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

        try
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName + "Signature");

            byte[] authInfo = (byte[])key.GetValue("AuthInfo");
            AuthInfo auth = new AuthInfo(authInfo);

            //получаем кол-во зарегистрированных рабочих станций
            int count;

            CompFilterEntity filter = new CompFilterEntity();
            filter.ComputerName = "*";
            filter.GenerateSQLWhereStatement();

            count = DBProviders.Computer.Count(filter.GetSQLWhereStatement);

            switch (auth.KeyState)
            {
                case VbaKeyState.Success:
                    lblKeyState.Text = (string)HttpContext.GetGlobalResourceObject("Resource", auth.KeyState.ToString());
                    //rowKeyState.Attributes.Add("class", "stateKeySuccess");
                    break;
                default:
                    lblSuccess.Text = (string)HttpContext.GetGlobalResourceObject("Resource", auth.KeyState.ToString());
                    rowSuccess.Visible = true;
                    rowLicenseNumber.Visible = rowCustomerName.Visible = rowComputerLimit.Visible = rowKeyState.Visible = rowExpirationDate.Visible = false;
                    break;
            }

            lblCustomerName.Text = auth.CustomerName;
            lblComputerLimit.Text = auth.ComputerLimit.ToString();
            if (count > auth.ComputerLimit)
            {
                rowComputerLimit.Attributes.Add("class", "stateKeyBad");
            }

            lblLicenseNumber.Text = string.Format("{0:D9}", auth.LicenseNumber);

            lblExpirationDate.Text = auth.ExpirationDate.ToString();
            if (TimeSpan.Compare(auth.ExpirationDate.Subtract(DateTime.Now), new TimeSpan(3, 0, 0, 0)) > 0)
            {
                //rowExpirationDate.Attributes.Add("class", "stateKeySuccess");
            }
            else
            {
                rowExpirationDate.Attributes.Add("class", "stateKeyPrecarious");
            }
        }
        catch
        {
            rowSuccess.Visible = true;
            rowLicenseNumber.Visible = rowCustomerName.Visible = rowComputerLimit.Visible = rowKeyState.Visible = rowExpirationDate.Visible = false;
        }
    }

    /// <summary>
    /// Заполняет таблицу информацией о сервисах
    /// </summary>
    private void FillServicesTable()
    {
        List<string> servNames = new List<string>();
        servNames.Add("Vba32SS");
        servNames.Add("Vba32NS");
        servNames.Add("Vba32PMS");
        servNames.Add("VbaControlCenter");
        servNames.Add("VbaTaskAssignment");
        servNames.Add("VbaUpdateService");

        Dictionary<string, string>.Enumerator enumerator =
            Vba32ServiceControllerInfo.GetServicesInfo(servNames).GetEnumerator();

        while (enumerator.MoveNext())
        {
            TableRow r = new TableRow();
            TableCell c1 = new TableCell();
            c1.Controls.Add(new LiteralControl(enumerator.Current.Key));
            c1.Style.Add("padding-left", "5px");
            r.Cells.Add(c1);

            TableCell c2 = new TableCell();

            string runningState =
                System.ServiceProcess.ServiceControllerStatus.Running.ToString();
            string stoppedStatus =
                System.ServiceProcess.ServiceControllerStatus.Stopped.ToString();

            string strBool = "disabled.gif";
            if (enumerator.Current.Value == runningState)
            {
                strBool = "enabled.gif";
            }

            Image img = new Image();
            img.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + strBool;

            c2.Controls.Add(img);
            c2.Style.Add("width", "50%");
            c2.Style.Add("align", "center");
            r.Cells.Add(c2);

            tblService.Rows.Add(r);
        }

    }


    //!- Web-приложение не имеет права изменять свои config-файлы таким образом

    //public bool ChangeArmDataBase(string fileName, string attrName, string oldName,
    //        string newName)
    //{
    /* XmlDocument doc = new XmlDocument();
     doc.Load(fileName);
     XmlNode root = doc.SelectSingleNode("connectionStrings");

     foreach (XmlNode node in root.ChildNodes)
     {
         if (node.Attributes["name"].Value == "ARM2DataBase")
         {
             oldName = "data source=" + oldName + ';';
             newName = "data source=" + newName + ';';

             node.Attributes["connectionString"].Value =
               node.Attributes["connectionString"].Value.Replace(oldName, newName);
             doc.Save(fileName);
         }
     }*/

    //oldName = "data source=" + oldName + ';';
    //newName = "data source=" + newName + ';';

    //string connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString.Replace(oldName, newName);

    //string connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString.Replace(oldName, newName);
    //ConfigurationManager.ConnectionStrings.Remove("ARM2DataBase");
    //ConnectionStringSettings css =
    //    new ConnectionStringSettings("ARM2DataBase", connStr);
    //ConfigurationManager.ConnectionStrings.Add(css);

    //ConfigurationManager.ConnectionStrings["ARM2DataBase"]. ConnectionString =
    //  ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString.Replace(oldName, newName);

    //    return true;
    //}

    //protected void lbtnARM2DataBaseDataSourceSave_Click(object sender, EventArgs e)
    //{
    //    //!-Неплохо бы проверить, что за херню пользователь ввел..
    //    ChangeArmDataBase(Server.MapPath("ConnectionStrings.config"),
    //        "ARM2DataBase",
    //        ((new Regex(@"data source=[a-zA-Z_0-9-()]+")).Match(
    //        ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString)
    //        ).Value.Substring(12),
    //        tboxARM2DataBaseDataSource.Text);
    //}
}
