using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using System.Text;
using System.IO;

public partial class Controls_TaskConfigureAgent : System.Web.UI.UserControl, ITask
{
    private TaskConfigureAgent _task = new TaskConfigureAgent();
    public String ConfigFile
    {
        get { return _task.ConfigFile; }
        set { _task.ConfigFile = value; }
    }

    private Boolean _hideHeader = false;
    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }
    
    #region ITask Members

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        lbtnUpload.Text = Resources.Resource.Upload;
        lblDetails.Text = Resources.Resource.SelectFile;
    }

    public Boolean ValidateFields()
    {
        return !String.IsNullOrEmpty(ConfigFile);
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.ConfigureAgent;
        task.Param = BuildXml();

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureAgent)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        TaskConfigureAgent tsk = new TaskConfigureAgent();
        tsk.LoadFromXml(task.Param);

        tboxConfigPath.Text = tsk.ConfigFile;
    }

    #endregion

    /// <summary>
    /// Get XML for DB storage
    /// </summary>
    /// <returns></returns>
    private String BuildXml()
    {
        TaskConfigureAgent task = new TaskConfigureAgent();

        task.ConfigFile = tboxConfigPath.Text;
        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task.SaveToXml();
    }

    public String BuildTask()
    {
        _task.ConfigFile = tboxConfigPath.Text;
        return _task.GetTask();
    }
    
    /// <summary>
    /// Загрузка файла на сервер
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnUpload_Click(Object sender, EventArgs e)
    {
        if (fuClient.HasFile == false)
        {
            // No file uploaded!
            lblDetails.Text = Resources.Resource.NoSelectFile;
        }
        else
        {
            // Display the uploaded file's details
            lblDetails.Text = Resources.Resource.FileUploaded;
            // Save the file

            String fileName = Guid.NewGuid().ToString();

            String filePath = Server.MapPath("~/Downloads/" + fileName);
            fuClient.SaveAs(filePath);
                        
            tboxConfigPath.Text = filePath;
        }
    }   
}