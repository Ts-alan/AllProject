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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    private Boolean _hideHeader = false;

    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }


    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;


        lblCongLdrUserName.Text = Resources.Resource.CongLdrUserName;
        lblCongLdrPassword.Text = Resources.Resource.CongLdrPassword;
        lblCongLdrAddress.Text = Resources.Resource.CongLdrAddress;
        lblCongLdrPort.Text = Resources.Resource.CongLdrPort;
    }

    public Boolean ValidateFields()
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
    private String BuildXml()
    {
        ARM2_dbcontrol.Tasks.ConfigureQuarantine.TaskConfigureQuarantine task = new ARM2_dbcontrol.Tasks.ConfigureQuarantine.TaskConfigureQuarantine();

        //Удаленное хранилище        
        task.StoragePath = tboxRemote.Text;

        //Прокси-сервер и авторизация
        task.UseProxy = cboxUseProxyServer.Checked ? 1 : 0;
        if (cboxUseProxyServer.Checked)
        {
            task.UserName = tboxUserName.Text;
            task.Password = tboxPassword.Text;

            task.Proxy = tboxAddress.Text;
            task.ProxyPort = Int32.Parse(tboxPort.Text);
        }

        //вкладка "Обслуживание"
        task.TimeOutEx = cboxMaintenancePeriod.Checked ? 1 : 0;
        if (cboxMaintenancePeriod.Checked)
        {
            task.TimeOut = Int32.Parse(tboxServicePeriod.Text);

            task.MaxSizeEx = cboxMaximumQuarantineSize.Checked ? 1 : 0;
            if (cboxMaximumQuarantineSize.Checked)
                task.MaxSize = Int32.Parse(tboxMaxSize.Text);

            task.MaxTimeEx = cboxMaximumStorageTime.Checked ? 1 : 0;
            if (cboxMaximumStorageTime.Checked)
                task.MaxTime = Int32.Parse(tboxMaximumStorageTime.Text);

            task.AutoSend = cboxAutomaticallySendSuspiciousObject.Checked ? 1 : 0;
            task.INARACTIVE_MAINT = cboxInteractive.Checked ? 1 : 0;
        }
        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task.SaveToXml();
    }

    /// <summary>
    /// Загружаем состояние карантина
    /// </summary>
    /// <param name="task">сохраненное состояние</param>
    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureQuarantine)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        ARM2_dbcontrol.Tasks.ConfigureQuarantine.TaskConfigureQuarantine tsk = new ARM2_dbcontrol.Tasks.ConfigureQuarantine.TaskConfigureQuarantine();
        tsk.LoadFromXml(task.Param);

        tboxRemote.Text = tsk.StoragePath;

        cboxUseProxyServer.Checked = tsk.UseProxy == 1;
        if (cboxUseProxyServer.Checked)
        {
            tboxUserName.Text = tsk.UserName;

            tboxPassword.Text = tsk.Password;
            tboxPassword.Attributes.Add("Value", tboxPassword.Text);

            tboxAddress.Text = tsk.Proxy;
            tboxPort.Text = tsk.ProxyPort.ToString();
        }

        //вкладка обслуживание
        cboxMaintenancePeriod.Checked = tsk.TimeOutEx == 1;
        if (cboxMaintenancePeriod.Checked)
        {
            cboxMaximumQuarantineSize.InputAttributes.Remove("disabled");
            tboxServicePeriod.Enabled = true;
            tboxMaxSize.Enabled = true;
            cboxMaximumStorageTime.InputAttributes.Remove("disabled");
            tboxMaximumStorageTime.Enabled = true;
            cboxAutomaticallySendSuspiciousObject.InputAttributes.Remove("disabled");
            cboxInteractive.InputAttributes.Remove("disabled");

            tboxServicePeriod.Text = tsk.TimeOut.ToString();
            cboxMaximumQuarantineSize.Checked = tsk.MaxSizeEx == 1;
            if (cboxMaximumQuarantineSize.Checked)
                tboxMaxSize.Text = tsk.MaxSize.ToString();

            cboxMaximumStorageTime.Checked = tsk.MaxTimeEx == 1;
            if (cboxMaximumStorageTime.Checked)
                tboxMaximumStorageTime.Text = tsk.MaxTime.ToString();

            cboxAutomaticallySendSuspiciousObject.Checked = tsk.AutoSend == 1;
            cboxInteractive.Checked = tsk.INARACTIVE_MAINT == 1;
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
}
