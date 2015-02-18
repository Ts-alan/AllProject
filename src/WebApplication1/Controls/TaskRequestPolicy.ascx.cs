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
using System.Text;

public partial class Controls_TaskRequestPolicy : System.Web.UI.UserControl, ITask
{
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

    public bool ValidateFields()
    {
        return true;
    }


    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.RequestPolicy;

        task.Param = @"<RequestPolicy></RequestPolicy>";        
        
        return task;
    }

    public string BuildParam(string taskParam)
    {
        StringBuilder param = new StringBuilder();

        param.Append("<Task>");
        param.Append(taskParam);
        param.AppendFormat("<Vba32CCUser>{0}</Vba32CCUser>", Anchor.GetStringForTaskGivedUser());
        param.Append("<Type>RequestPolicy</Type>");
        param.Append("</Task>");

        return param.ToString();
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.RequestPolicy)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
    }
}
