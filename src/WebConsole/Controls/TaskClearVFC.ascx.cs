using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using System.Text;

public partial class Controls_TaskClearVFC : System.Web.UI.UserControl, ITask
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
        task.Type = TaskType.ClearVFC;
        task.Param = BuildXml();
        return task;
    }

    private String BuildXml()
    {
        VirusBlokAda.CC.Common.Xml.XmlBuilder xml = new VirusBlokAda.CC.Common.Xml.XmlBuilder("task");

        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ClearVFC");

        xml.Generate();

        return xml.Result;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ClearVFC)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
    }

    public String GetTask()
    {
        StringBuilder result = new StringBuilder(256);

        result.Append("<VsisCommand>");
        result.Append("<Args>");

        result.Append(@"<command><arg><key>module-id</key><value>{76DC546B-D814-4E18-AF4B-C7D17BC0AB90}</value></arg>");
        result.Append(@"<arg><key>command</key><value>cleanup</value></arg>");
        result.Append(@"</command>");

        result.Append(@"</Args>");
        result.Append(@"<Async>0</Async>");
        result.Append(@"</VsisCommand>");

        return result.ToString();
    }

    #endregion
}