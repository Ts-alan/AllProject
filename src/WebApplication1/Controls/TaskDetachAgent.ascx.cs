using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using System.IO;
using System.Text;

public partial class Controls_TaskDetachAgent : System.Web.UI.UserControl, ITask
{
    private Boolean _hideHeader = false;
    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    #region ITask Members

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.DetachAgent;

        VirusBlokAda.CC.Common.Xml.XmlBuilder xml =
             new VirusBlokAda.CC.Common.Xml.XmlBuilder("task");
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "DetachAgent");
        xml.Generate();
        task.Param = xml.Result;

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.DetachAgent)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
    }

    #endregion

    /// <summary>
    /// Get XML for agent
    /// </summary>
    /// <returns></returns>
    public String BuildTask()
    {
       return @"<ForgetServer />";
    }
}