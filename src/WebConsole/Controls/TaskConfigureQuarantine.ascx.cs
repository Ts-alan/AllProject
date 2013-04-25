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

using System.Text;

using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;

/// <summary>
/// Задача настройки карантина
/// </summary>
public partial class Controls_TaskConfigureQuarantine : System.Web.UI.UserControl, ITask
{
    private string passPrefix = "HER!%&$";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
        ChangeEnabledControl();
    }

    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private void ChangeEnabledControl()
    {
        Tabs.Enabled = _enabled;
    }

    private bool _enabled = true;

    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

        
        tabPanel1.HeaderText = Resources.Resource.CongQtnGeneral;
        tabPanel2.HeaderText = Resources.Resource.CongQtnMaintenance;

        //lbtnGeneral.Text = Resources.Resource.CongQtnGeneral;
        //lbtnMaintenance.Text = Resources.Resource.CongQtnMaintenance;

        lblCongLdrUserName.Text = Resources.Resource.CongLdrUserName;
        lblCongLdrPassword.Text = Resources.Resource.CongLdrPassword;
        lblCongLdrAddress.Text = Resources.Resource.CongLdrAddress;
        lblCongLdrPort.Text = Resources.Resource.CongLdrPort;

        if (tboxRemote.Text == "")
            tboxRemote.Text = "http://www.anti-virus.by/cgi-bin/vbar.cgi";
        
    }

    public bool ValidateFields()
    {
        Validation vld = new Validation("");
        if (cboxUseProxyServer.Checked)
        {
            vld.Value = tboxAddress.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrAddress);

            vld.Value = tboxPort.Text;
            if (!vld.CheckSize())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrPort);
       
            /*vld.Value = tboxUserName.Text;
            if (!vld.CheckStringValue())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrUserName);*/
        }

        if (cboxMaintenancePeriod.Checked)
        {
            vld.Value = tboxServicePeriod.Text;
            if (!vld.CheckPositiveInteger())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongQtnMaintenancePeriod);

            if (cboxMaximumQuarantineSize.Checked)
            {
               vld.Value = tboxMaxSize.Text;
               if (!vld.CheckPositiveInteger())
                   throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                       + Resources.Resource.CongQtnMaximumQuarantineSize);
            }

            if (cboxMaximumStorageTime.Checked)
            {
                vld.Value =  tboxMaximumStorageTime.Text;
                if (!vld.CheckPositiveInteger())
                    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.CongQtnMaximumStorageTime);
            }
        }

        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.ConfigureQuarantine;

        task.Param = BuildXml();

        return task;
    }

    /// <summary>
    /// Конструирует строку xml-файла
    /// </summary>
    /// <returns></returns>
    private string BuildXml()
    {
        ARM2_dbcontrol.Generation.XmlBuilder xml = 
            new ARM2_dbcontrol.Generation.XmlBuilder("qtn");

        //Удаленное хранилище
        if(tboxRemote.Text!=String.Empty)
            xml.AddNode("WebServer", "reg_sz:" + tboxRemote.Text);

        //Прокси-сервер и авторизация
        xml.AddNode("UseProxy", cboxUseProxyServer.Checked ? "reg_dword:1" : "reg_dword:0");
        if (cboxUseProxyServer.Checked)
        {
            xml.AddNode("UserName", "reg_sz:" + tboxUserName.Text);


            //Шифруем пароль
            if (!tboxPassword.Text.Contains(passPrefix))
            {
                byte[] bytes = Encoding.Unicode.GetBytes(tboxPassword.Text);

                byte xorValue = 0xAA;
                byte delta = 0x1;

                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] ^= xorValue;
                    delta = Convert.ToByte(delta % 3 + 1);
                    xorValue += delta;
                }

                xml.AddNode("Password", "reg_binary:" + Anchor.ConvertToDumpString(bytes));
            }
            else
            {
                string str = tboxPassword.Text.Replace(passPrefix,"");
                xml.AddNode("Password", str);
            }

            xml.AddNode("Proxy", "reg_sz:" + tboxAddress.Text);
            xml.AddNode("ProxyPort","reg_dword:"+ tboxPort.Text);
        }

        //вкладка "Обслуживание"
        xml.AddNode("TimeOutEx", cboxMaintenancePeriod.Checked ? "reg_dword:1" : "reg_dword:0");
        if (cboxMaintenancePeriod.Checked)
        {
            xml.AddNode("TimeOut", "reg_dword:" + tboxServicePeriod.Text);

            xml.AddNode("MaxSizeEx", cboxMaximumQuarantineSize.Checked ? "reg_dword:1" : "reg_dword:0");
            if (cboxMaximumQuarantineSize.Checked)
                xml.AddNode("MaxSize", "reg_dword:" + tboxMaxSize.Text);

            xml.AddNode("MaxTimeEx", cboxMaximumStorageTime.Checked ? "reg_dword:1" : "reg_dword:0");
            if (cboxMaximumQuarantineSize.Checked)
                xml.AddNode("MaxTime", "reg_dword:" + tboxMaximumStorageTime.Text);

            xml.AddNode("AutoSend", cboxAutomaticallySendSuspiciousObject.Checked ? "reg_dword:1" : "reg_dword:0");
            xml.AddNode("INARACTIVE_MAINT", cboxInteractive.Checked ? "reg_dword:1" : "reg_dword:0");
        }
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ConfigureQuarantine");

        xml.Generate();

        return xml.Result;

    }

    /// <summary>
    /// Загружаем состояние карантина
    /// </summary>
    /// <param name="task">сохраненное состояние</param>
    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureQuarantine)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser pars = new XmlTaskParser(task.Param);

        string str = pars.GetValue("WebServer", "reg_sz:");
        if (str != String.Empty)
            tboxRemote.Text = str;

        cboxUseProxyServer.Checked = pars.GetValue("UseProxy", "reg_dword:") == "1" ? true : false;
        if (cboxUseProxyServer.Checked)
        {
            tboxUserName.Text = pars.GetValue("UserName", "reg_sz:");

            tboxPassword.Text = pars.GetValue("Password", "reg_binary:");
            tboxPassword.Attributes.Add("Value", passPrefix + tboxPassword.Text);

            tboxAddress.Text = pars.GetValue("Proxy", "reg_sz:");
            tboxPort.Text = pars.GetValue("ProxyPort", "reg_dword:");
        }

        //вкладка обслуживание
        cboxMaintenancePeriod.Checked = pars.GetValue("TimeOutEx", "reg_dword:") == "1" ? true : false;
        if (cboxMaintenancePeriod.Checked)
        {
            cboxMaximumQuarantineSize.InputAttributes.Remove("disabled");
            tboxServicePeriod.Enabled = true;
            tboxMaxSize.Enabled = true;
            cboxMaximumStorageTime.InputAttributes.Remove("disabled");
            tboxMaximumStorageTime.Enabled = true;
            cboxAutomaticallySendSuspiciousObject.InputAttributes.Remove("disabled");
            cboxInteractive.InputAttributes.Remove("disabled");

            tboxServicePeriod.Text = pars.GetValue("TimeOut", "reg_dword:");
            cboxMaximumQuarantineSize.Checked = pars.GetValue("MaxSizeEx", "reg_dword:") == "1" ? true : false;
            if (cboxMaximumQuarantineSize.Checked)
                tboxMaxSize.Text = pars.GetValue("MaxSize", "reg_dword:");

            cboxMaximumStorageTime.Checked = pars.GetValue("MaxTimeEx", "reg_dword:") == "1" ? true : false;
            if (cboxMaximumStorageTime.Checked)
                tboxMaximumStorageTime.Text = pars.GetValue("MaxTime", "reg_dword:");

            cboxAutomaticallySendSuspiciousObject.Checked = pars.GetValue("AutoSend", "reg_dword:") == "1" ? true : false;
            cboxInteractive.Checked = pars.GetValue("INARACTIVE_MAINT", "reg_dword:") == "1" ? true : false;

        }
        else
        {
            cboxMaximumQuarantineSize.InputAttributes.Add("disabled", "true");
            tboxServicePeriod.Enabled = false;
            tboxMaxSize.Enabled = false;
            cboxMaximumStorageTime.InputAttributes.Add("disabled", "true");
            tboxMaximumStorageTime.Enabled = false;
            cboxAutomaticallySendSuspiciousObject.InputAttributes.Add("disabled", "true");
            cboxInteractive.InputAttributes.Add("disabled", "true");
        }

    }

    /*protected void lbtnCommon_Click(object sender, EventArgs e)
    {
        tblGeneral.Visible = true;
        tblMaintenance.Visible = false;
    }

    protected void lbtnService_Click(object sender, EventArgs e)
    {
        tblGeneral.Visible = false;
        tblMaintenance.Visible = true;
    }*/
}
