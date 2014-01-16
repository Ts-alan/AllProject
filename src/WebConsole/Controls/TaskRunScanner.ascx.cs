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
using ARM2_dbcontrol.Tasks;


/// <summary>
/// «адача запуска консольного сканера
/// </summary>
public partial class Controls_TaskRunScanner : System.Web.UI.UserControl, ITask
{
    private Boolean _hideHeader = false;
    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private Boolean _enabled = true;
    public Boolean Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region ITask Members

    public void InitFields()
    {
        HeaderName.Visible = !HideHeader;
        
        lboxPathes.Items.Clear();
        tboxPath.Text = String.Empty;

        SetEnabled();
    }

    private void SetEnabled()
    {
        tboxPath.Enabled = _enabled;
        lboxPathes.Enabled = _enabled;
        cboxCheckMemory.Enabled = _enabled;
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.RunScanner)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        ARM2_dbcontrol.Tasks.RunScanner.TaskRunScanner tsk = new ARM2_dbcontrol.Tasks.RunScanner.TaskRunScanner();
        tsk.LoadFromXml(task.Param);
        
        cboxCheckMemory.Checked = tsk.IsCheckMemory;
        
        lboxPathes.Items.Clear();
        foreach (String item in tsk.PathScan)
        {
            lboxPathes.Items.Add(item);
        }
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.RunScanner;
        task.Param = GetTaskEntity().SaveToXml();
        return task;
    }

    public String GetTaskForVSIS()
    {
        return GetTaskEntity().GetTaskForVSIS();
    }

    private ARM2_dbcontrol.Tasks.RunScanner.TaskRunScanner GetTaskEntity()
    {
        ARM2_dbcontrol.Tasks.RunScanner.TaskRunScanner task = new ARM2_dbcontrol.Tasks.RunScanner.TaskRunScanner();

        foreach (ListItem item in lboxPathes.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.PathScan.Add(item.Value);
        }
        task.IsCheckMemory = cboxCheckMemory.Checked;
        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task;
    }

    #endregion

    protected void lbtnAddPath_Click(Object sender, EventArgs e)
    {        
        if (!String.IsNullOrEmpty(tboxPath.Text))
        {
            lboxPathes.Items.Add(tboxPath.Text);
            tboxPath.Text = String.Empty;
        }
    }

    protected void lbtnDeletePath_Click(Object sender, EventArgs e)
    {        
        if (lboxPathes.SelectedIndex > -1 && lboxPathes.SelectedIndex < lboxPathes.Items.Count)
            lboxPathes.Items.RemoveAt(lboxPathes.SelectedIndex);
    }
}
