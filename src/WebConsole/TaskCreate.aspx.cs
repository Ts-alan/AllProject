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

using ARM2_dbcontrol.Filters;
using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using VirusBlokAda.CC.DataBase;
using VirusBlokAda.CC.Common.Xml;

/// <summary>
/// Task create
/// </summary>
public partial class TaskCreate : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
        {
            //throw new Exception(Resources.Resource.ErrorAccessDenied);
            Response.Redirect("Default.aspx");
        }
        Page.Title = Resources.Resource.PageTaskCreateTitle;

        if (!Page.IsPostBack)
        {
            InitFields();

            if ((Request.QueryString["Mode"] != null) && (Request.QueryString["Mode"] == "Edit"))
            {
                if (Session["CurrentUserTask"] == null)
                {
                    lblMessage.Text = Resources.Resource.ErrorSelectTaskToEdit;
                    mpPicture.Attributes["class"] = "ModalPopupPictureError";
                    CorrectPositionModalPopup();
                    ModalPopupExtender.Show();
                    return;
                }

                TaskUserEntity task = (TaskUserEntity)Session["CurrentUserTask"];

                tskCreateProcess.Visible = false;
                tskSendFile.Visible = false;
                tskSystemInfo.Visible = false;
                tskListProcesses.Visible = false;
                tskCancelTask.Visible = false;
                tskConfigureLoader.Visible = false;
                tskConfigureMonitor.Visible = false;
                tskConfigureScanner.Visible = false;
                tskComponentState.Visible = false;
                tskConfigurePassword.Visible = false;
                tskConfigureQuarantine.Visible = false;
                tskRestoreFileFromQtn.Visible = false;
                tskProactiveProtection.Visible = false;
                tskFirewall.Visible = false;
                tskChangeDeviceProtect.Visible = false;
                tskRequestPolicy.Visible = false;
                tskConfigureScheduler.Visible = false;
                tskRunScanner.Visible = false;
                tskConfigureIntegrityCheck.Visible = false;

                LoadStateTask(task);


                if (task.Type == TaskType.Firewall && task.Name != String.Empty)
                {
                    tbSaveAs.Text = task.Name.Substring(10);
                }
                else
                    if (task.Type == TaskType.ConfigureSheduler && task.Name != String.Empty)
                    {
                        tbSaveAs.Text = task.Name.Substring(11);
                    }
                    else
                        tbSaveAs.Text = task.Name;
            }

            if (Request.QueryString["ID"] != null)
            {
                Int64 id;
                try
                {
                    id = Convert.ToInt64(Request.QueryString["ID"]);
                }
                catch
                {
                    lblMessage.Text = "ID must be Int64 value.";
                    mpPicture.Attributes["class"] = "ModalPopupPictureError";
                    CorrectPositionModalPopup();
                    ModalPopupExtender.Show();
                    return;
                }

                TaskEntity task = new TaskEntity();
                
                try
                {
                    task = DBProviders.Task.Get(id);
                }
                catch
                {
                    Response.Redirect("ErrorSql.aspx");
                    return;
                }


                TaskUserEntity taskUserType = new TaskUserEntity();

                taskUserType.Param = task.TaskParams;
                if (task.TaskParams.Contains("<monitor>"))
                {
                    taskUserType.Param = "<Task>" + task.TaskParams + "</Task>";
                }

                if (task.TaskParams.Contains("RequestPolicy"))
                {
                    taskUserType.Type = TaskType.RequestPolicy;
                }

                XmlTaskParser pars = new XmlTaskParser(taskUserType.Param);

                switch (pars.GetValue("Type"))
                {
                    case "CreateProcess":
                        taskUserType.Type = TaskType.CreateProcess;
                        break;
                    case "SendFile":
                        taskUserType.Type = TaskType.SendFile;
                        break;
                    case "SystemInfo":
                        taskUserType.Type = TaskType.SystemInfo;
                        break;
                    case "ListProcesses":
                        taskUserType.Type = TaskType.ListProcesses;
                        break;
                    case "ComponentState":
                        taskUserType.Type = TaskType.ComponentState;
                        break;
                    case "ConfigureLoader":
                        taskUserType.Type = TaskType.ConfigureLoader;
                        break;
                    case "ConfigureMonitor":
                        taskUserType.Type = TaskType.ConfigureMonitor;
                        break;
                    case "ConfigureScanner":
                        taskUserType.Type = TaskType.ConfigureScanner;
                        break;
                    case "RunScanner":
                        taskUserType.Type = TaskType.RunScanner;
                        break;
                    case "ConfigurePassword":
                        taskUserType.Type = TaskType.ConfigurePassword;
                        break;
                    case "ConfigureQuarantine":
                        taskUserType.Type = TaskType.ConfigureQuarantine;
                        break;
                    case "RestoreFileFromQtn":
                        taskUserType.Type = TaskType.RestoreFileFromQtn;
                        break;
                    case "ProactiveProtection":
                        taskUserType.Type = TaskType.ProactiveProtection;
                        break;
                    case "Firewall":
                        taskUserType.Type = TaskType.Firewall;
                        break;
                    case "ConfigureSheduler":
                        taskUserType.Type = TaskType.ConfigureSheduler;
                        break;
                    case "ChangeDeviceProtect":
                        taskUserType.Type = TaskType.ChangeDeviceProtect;
                        break;
                    case "IntegrityCheck":
                        taskUserType.Type = TaskType.ConfigureIntegrityCheck;
                        break;
                }

                Session["CurrentUserTask"] = taskUserType;

                string mode = "";
                //if ((Request.QueryString["Mode"] != null) && (Request.QueryString["Mode"] == "Edit"))
                mode = "&Mode=Edit";
                Response.Redirect("TaskCreate.aspx?Type=" + taskUserType.Type + mode);

            }
        }
    }

    private void LoadStateTask(TaskUserEntity task)
    {
        switch (task.Type)
        {
            case TaskType.CreateProcess:

                tskCreateProcess.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskCreateProcess.Visible = true;
                break;

            case TaskType.SendFile:

                tskSendFile.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskSendFile.Visible = true; ;
                break;

            case TaskType.SystemInfo:

                tskSystemInfo.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskSystemInfo.Visible = true;

                tbSaveAs.Visible = false;
                lbtnSaveAs.Visible = false;
                lblTaskName.Visible = false;
                break;

            case TaskType.ListProcesses:

                tskListProcesses.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskListProcesses.Visible = true;

                tbSaveAs.Visible = false;
                lbtnSaveAs.Visible = false;
                lblTaskName.Visible = false;

                break;

            case TaskType.ComponentState:

                tskComponentState.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskComponentState.Visible = true;

                tbSaveAs.Visible = false;
                lbtnSaveAs.Visible = false;
                lblTaskName.Visible = false;
                break;

            case TaskType.ConfigureLoader:

                tskConfigureLoader.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskConfigureLoader.Visible = true;
                break;

            case TaskType.ConfigureMonitor:

                tskConfigureMonitor.InitFields();
                tskConfigureMonitor.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskConfigureMonitor.Visible = true;
                break;

            case TaskType.ConfigureScanner:

                tskConfigureScanner.InitFields();
                tskConfigureScanner.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskConfigureScanner.Visible = true;
                break;

            case TaskType.RunScanner:

                tskRunScanner.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskRunScanner.Visible = true;
                break;

            case TaskType.ConfigurePassword:

                tskConfigurePassword.InitFields();
                tskConfigurePassword.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskConfigurePassword.Visible = true;

                tbSaveAs.Visible = false;
                lbtnSaveAs.Visible = false;
                lblTaskName.Visible = false;

                break;

            case TaskType.ConfigureQuarantine:

                //tskConfigureQuarantine.InitFields();
                tskConfigureQuarantine.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskConfigureQuarantine.Visible = true;
                break;
            case TaskType.RestoreFileFromQtn:

                //tskConfigureQuarantine.InitFields();
                tskRestoreFileFromQtn.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskRestoreFileFromQtn.Visible = true;
                break;
            case TaskType.ProactiveProtection:
                tskProactiveProtection.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskProactiveProtection.Visible = true;

                break;
            case TaskType.Firewall:
                tskFirewall.InitFields();
                tskFirewall.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskFirewall.Visible = true;

                break;
            case TaskType.ConfigureSheduler:

                tskConfigureScheduler.ShowProfileDetails(task);
                lblTaskName.Text = Resources.Resource.ProfileName;
                tskConfigureScheduler.Visible = true;
                break;
            case TaskType.ChangeDeviceProtect:

                tskChangeDeviceProtect.InitFields();
                tskChangeDeviceProtect.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskChangeDeviceProtect.Visible = true;

                tbSaveAs.Visible = false;
                lbtnSaveAs.Visible = false;
                lblTaskName.Visible = false;
                break;
            case TaskType.RequestPolicy:

                tskRequestPolicy.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskRequestPolicy.Visible = true;

                tbSaveAs.Visible = false;
                lbtnSaveAs.Visible = false;
                lblTaskName.Visible = false;

                break;
            case TaskType.ConfigureIntegrityCheck:
                tskConfigureIntegrityCheck.InitFields();
                tskConfigureIntegrityCheck.LoadState(task);
                lblTaskName.Text = Resources.Resource.TaskName;
                tskConfigureIntegrityCheck.Visible = true;

                break;
        }
    }

    /// <summary>
    /// Init fields
    /// </summary>
    protected override void InitFields()
    {
        lbtnSaveAs.Text = Resources.Resource.Save;        
        btnClose.Text = Resources.Resource.Close;
        InitializeSession();
    }
    public void InitializeSession()
    {
        if (Session["TaskUser"] == null)
            Session["TaskUser"] = new TaskUserCollection(Profile.TasksList);
    }

    /// <summary>
    /// Save as
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnSaveAs_Click(object sender, EventArgs e)
    {
        InitializeSession();
        string type = Request.QueryString["Type"];
        TaskUserEntity task = new TaskUserEntity();
        try
        {
            switch (type)
            {
                case "CreateProcess":
                    task.Type = TaskType.CreateProcess;
                    task = tskCreateProcess.GetCurrentState();
                    tskCreateProcess.ValidateFields();
                    break;
                case "SendFile":
                    task.Type = TaskType.SendFile;
                    task = tskSendFile.GetCurrentState();
                    tskSendFile.ValidateFields();
                    break;
                case "SystemInfo":
                    task.Type = TaskType.SystemInfo;

                    break;
                case "ListProcesses":
                    task.Type = TaskType.ListProcesses;

                    break;
                case "ComponentState":
                    task.Type = TaskType.ComponentState;

                    break;

                case "ConfigureLoader":
                    task.Type = TaskType.ConfigureLoader;
                    task = tskConfigureLoader.GetCurrentState();
                    tskConfigureLoader.ValidateFields();
                    break;

                case "ConfigureMonitor":
                    task.Type = TaskType.ConfigureMonitor;
                    task = tskConfigureMonitor.GetCurrentState();
                    tskConfigureMonitor.ValidateFields();
                    break;

                case "ConfigureScanner":
                    task.Type = TaskType.ConfigureScanner;
                    task = tskConfigureScanner.GetCurrentState();
                    tskConfigureScanner.ValidateFields();
                    break;

                case "RunScanner":
                    task.Type = TaskType.RunScanner;
                    task = tskRunScanner.GetCurrentState();
                    tskRunScanner.ValidateFields();
                    break;

                case "ConfigurePassword":
                    task.Type = TaskType.ConfigurePassword;
                    task = tskConfigurePassword.GetCurrentState();
                    tskConfigurePassword.ValidateFields();
                    break;
                case "ConfigureQuarantine":
                    task.Type = TaskType.ConfigureQuarantine;
                    task = tskConfigureQuarantine.GetCurrentState();
                    tskConfigureQuarantine.ValidateFields();
                    break;

                case "RestoreFileFromQtn":
                    task.Type = TaskType.RestoreFileFromQtn;
                    task = tskRestoreFileFromQtn.GetCurrentState();
                    tskRestoreFileFromQtn.ValidateFields();
                    break;

                case "ProactiveProtection":
                    task.Type = TaskType.ProactiveProtection;
                    task = tskProactiveProtection.GetCurrentState();
                    tskProactiveProtection.ValidateFields();
                    break;
                case "Firewall":
                    task.Type = TaskType.Firewall;
                    task = tskFirewall.GetCurrentState();
                    tskFirewall.ValidateFields();
                    break;
                case "ConfigureSheduler":
                    task.Type = TaskType.ConfigureSheduler;
                    task = tskConfigureScheduler.GetCurrentState();
                    tskConfigureScheduler.ValidateFields();
                    break;
                case "ConfigureIntegrityCheck":
                    task.Type = TaskType.ConfigureIntegrityCheck;
                    task = tskConfigureIntegrityCheck.GetCurrentState();
                    tskConfigureIntegrityCheck.ValidateFields();
                    break;

                default:
                    break;
            }


            if (task.Type == TaskType.Firewall)
            {
                task.Name = String.Format("Firewall: {0}", tbSaveAs.Text);
                if (tbSaveAs.Text == String.Empty)
                    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": " + Resources.Resource.TaskName);
            }
            else
                if (task.Type == TaskType.ConfigureSheduler)
                {
                    task.Name = String.Format("Scheduler: {0}", tbSaveAs.Text);
                    if (tbSaveAs.Text == String.Empty)
                        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": " + Resources.Resource.TaskName);
                }
                else
                {
                    task.Name = tbSaveAs.Text;

                    //Проверка имени..

                    Validation vld = new Validation(task.Name);
                    if (!vld.CheckStringFilterName())
                        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                            + Resources.Resource.TaskName);
                }
            //Проверим, не является ли это имя именем базовой задачи
            ResourceControl resctrl = new ResourceControl();

            if ((resctrl.IsExist("Process", task.Name)) ||
                (resctrl.IsExist("SendFile", task.Name)) ||
                (resctrl.IsExist("MenuSystemInfo", task.Name)) ||
                (resctrl.IsExist("TaskNameListProcesses", task.Name)) ||
                (resctrl.IsExist("TaskNameComponentState", task.Name)) ||
                (resctrl.IsExist("CongLdrConfigureLoader", task.Name)) ||
                (resctrl.IsExist("CongLdrConfigureMonitor", task.Name)) ||
                (resctrl.IsExist("TaskNameConfigureScanner", task.Name)) ||
                (resctrl.IsExist("TaskNameRunScanner", task.Name)) ||
                (resctrl.IsExist("TaskNameConfigureQuarantine", task.Name)) ||
                (resctrl.IsExist("TaskSeparate", task.Name)) ||
                (resctrl.IsExist("TaskNameVba32LoaderEnable", task.Name)) ||
                (resctrl.IsExist("TaskNameVba32LoaderDisable", task.Name)) ||
                (resctrl.IsExist("TaskNameVba32MonitorEnable", task.Name)) ||
                (resctrl.IsExist("TaskNameVba32MonitorDisable", task.Name)) ||
                (resctrl.IsExist("TaskNameVba32ProgramAndDataBaseUpdate", task.Name)) ||
                (resctrl.IsExist("TaskNameRestoreFileFromQtn", task.Name)) ||
                (resctrl.IsExist("TaskNameConfigurePassword", task.Name)) ||
                (resctrl.IsExist("TaskConfigureIntegrityCheck", task.Name))
                )
            {
                throw new Exception(Resources.Resource.ErrorCreateTaskWithBaseName);
            }

            //Добавим в коллекцию
            TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
            if (collection.Get(task.Name).Name != String.Empty)
            {
                if ((Request.QueryString["Mode"] != null) && (Request.QueryString["Mode"] == "Edit"))
                {

                    collection.Delete(task.Name);
                    collection = collection.Deserialize();
                    collection.Add(task);

                }
                else
                {
                    throw new InvalidOperationException(Resources.Resource.ErrorTaskExistInCollection);
                }
            }
            else
            {
                //!-OPTM нафига здесь это?
                DBProviders.Task.GetTaskTypeID(task.Name, true);

                collection.Add(task);
            }

            Profile.TasksList = collection.Serialize();
            Session["TaskUser"] = collection;
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            CorrectPositionModalPopup();
            ModalPopupExtender.Show();
            return;
        }

        Session["CurrentUserTask"] = task;
        LoadStateTask(task);

        lblMessage.Text = Resources.Resource.TaskSaved;
        if (task.Type == TaskType.ProactiveProtection || task.Type == TaskType.ConfigureSheduler)
        {
            lblMessage.Text = Resources.Resource.ProfileSaved;
        }
        mpPicture.Attributes["class"] = "ModalPopupPictureSuccess";
        CorrectPositionModalPopup();
        ModalPopupExtender.Show();

    }

    private void CorrectPositionModalPopup()
    {
        try
        {
            ModalPopupExtender.X = int.Parse(hdnWidth.Value) / 2 - 200;
            ModalPopupExtender.Y = int.Parse(hdnHeight.Value) / 2 - 60;
        }
        catch
        {
            ModalPopupExtender.X = ModalPopupExtender.Y = 200;
        }
    }
   
}
