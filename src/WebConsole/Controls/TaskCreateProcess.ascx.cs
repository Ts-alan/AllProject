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

/// <summary>
/// Задача создания процесса
/// </summary>
public partial class Controls_TaskCreateProcess : System.Web.UI.UserControl, ITask
{
    private string tag_CommandLine = "CommandLine";
    private string tag_CommandSpec = "ComSpec";

    private string comSpecOption = "%COMSPEC% /c ";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.CreateProcess;

        ARM2_dbcontrol.Generation.XmlBuilder xml = 
            new ARM2_dbcontrol.Generation.XmlBuilder("task");
        
        string str = String.Empty;
       
        str = tboxCreateProcess.Text;

        str = str.Replace(" ","&#160;");
        str = str.Replace("&", "&amp;");
        if (cboxCommand.Checked)
            str = comSpecOption + str;

        xml.AddNode(tag_CommandLine,str);
        xml.AddNode(tag_CommandSpec, cboxCommand.Checked?"1":"0");
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "CreateProcess");
        xml.Generate();

        task.Param = xml.Result;

        return task;
    }

    public bool ValidateFields()
    {
        Validation vld = new Validation(tboxCreateProcess.Text);
        if((!vld.CheckStringToTask())||(tboxCreateProcess.Text==""))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.CreateProcess);

        return true;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.CreateProcess)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
       
        
        XmlTaskParser pars = new XmlTaskParser(task.Param);

        cboxCommand.Checked = (pars.GetValue(tag_CommandSpec) == "1") ? true : false;
       
        string str = pars.GetValue(tag_CommandLine);
        if (cboxCommand.Checked)
            str = str.Replace(comSpecOption, "");

        //Убираем кавычки
        //str = str.Replace("\"", "");
        str = str.Replace("&#160;", " ");
        str = str.Replace("&amp;", "&");
        tboxCreateProcess.Text = str;
        
    }

    #region Property

    public string TagCommandLine
    {
        get { return this.tag_CommandLine; }
    }

    public string TagCommandSpec
    {
        get { return this.tag_CommandSpec; }
    }

    #endregion

}
