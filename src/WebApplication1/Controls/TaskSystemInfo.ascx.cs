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

using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;

/// <summary>
/// Задача получения системной информации с клиента
/// </summary>
public partial class Controls_TaskSystemInfo : System.Web.UI.UserControl, ITask
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

        task.Type = TaskType.SystemInfo;

        VirusBlokAda.CC.Common.Xml.XmlBuilder xml =
             new VirusBlokAda.CC.Common.Xml.XmlBuilder("task");
        xml.AddNode( "Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "SystemInfo");
        
        xml.Generate();

        task.Param = xml.Result;

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.SystemInfo)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
    }

}
