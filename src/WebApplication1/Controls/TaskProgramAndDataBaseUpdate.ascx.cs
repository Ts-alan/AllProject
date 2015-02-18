using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using System.Text;

public partial class Controls_TaskProgramAndDataBaseUpdate : System.Web.UI.UserControl, ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    #region ITask Members

    public void InitFields()
    {
    }

    public bool ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.UpdateAll;
        task.Param = BuildXml();
        return task;
    }

    private String BuildXml()
    {
        VirusBlokAda.CC.Common.Xml.XmlBuilder xml = new VirusBlokAda.CC.Common.Xml.XmlBuilder("TaskProgramAndDataBaseUpdate");
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "UpdateAll");
        xml.Generate();

        return xml.Result;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.UpdateAll)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
    }

    public String GetTask()
    {
        StringBuilder result = new StringBuilder(256);

        result.Append("<VsisCommand>");
        result.Append("<Args>");

        result.Append(@"<command><arg><key>command</key><value>update-all</value></arg><arg><key>module-id</key><value>{D4041472-FEC0-41B5-A133-8AAC758C1006}</value></arg></command>");

        result.Append(@"</Args>");
        result.Append(@"<Async>0</Async>");
        result.Append(@"</VsisCommand>");

        return result.ToString();
    }

    #endregion
}