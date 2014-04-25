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

public partial class Controls_TaskRestoreFileFromQtn : System.Web.UI.UserControl, ITask
{

    private string tagPath = "Path";

    private string comSpecOption = "\"%VBA32%Vba32Qtn.exe\" RF=";


    protected void Page_Load(object sender, EventArgs e)
    {

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


    public string GetCommandLine
    {
        get { return String.Format("{0}\"{1}\"", comSpecOption, tboxCreateProcess.Text); }
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.RestoreFileFromQtn;

        VirusBlokAda.CC.Common.Xml.XmlBuilder xml = new VirusBlokAda.CC.Common.Xml.XmlBuilder("task");

        string str = tboxCreateProcess.Text;



        xml.AddNode(tagPath, str);
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "RestoreFileFromQtn");
        xml.Generate();

        task.Param = xml.Result;

        return task;
    }

    public bool ValidateFields()
    {
        Validation vld = new Validation(tboxCreateProcess.Text);
        if ((!vld.CheckStringToTask()) || (tboxCreateProcess.Text == ""))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.CreateProcess);
        
        return true;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.RestoreFileFromQtn)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);


        VirusBlokAda.CC.Common.Xml.XmlTaskParser pars = new VirusBlokAda.CC.Common.Xml.XmlTaskParser(task.Param);

        string str = pars.GetValue(tagPath);
        //str = str.Replace(comSpecOption, "");
        tboxCreateProcess.Text = str;
    }
}
