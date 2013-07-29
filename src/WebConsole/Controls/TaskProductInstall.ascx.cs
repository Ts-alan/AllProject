using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using System.Text;
using VirusBlokAda.RemoteOperations.MsiInfo;

public partial class Controls_TaskProductInstall : System.Web.UI.UserControl, ITask
{
    private Boolean _hideHeader = false;
    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private Boolean _hideBound = false;
    public Boolean HideBound
    {
        get { return _hideBound; }
        set
        {
            _hideBound = value;
            if (_hideBound)
            {
                tblInstall.Attributes.Add("class", "");
            }
            else
            {
                tblInstall.Attributes.Add("class", "ListContrastTable");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            InitFields();
    }

    #region ITask Members

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

        List<String> list = new List<String>();
        list.Add(Resources.Resource.Antivirus);
        list.Add(Resources.Resource.RemoteConsoleScanner);

        ddlProduct.DataSource = list;
        ddlProduct.DataBind();
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();        
        task.Type = TaskType.Install;
        task.Param = BuildXml();

        return task;
    }
    
    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.Install)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser parser = new XmlTaskParser(task.Param);
        Int32 value;
        try
        {
            value = Int32.Parse(parser.GetXmlTagContent("ServerFile"));
        }
        catch
        {
            throw new Exception("Selected product for installation is wrong.");
        }

        ddlProduct.SelectedIndex = value;
        tboxArguments.Text = parser.GetXmlTagContent("AdditionalArgs");
    }

    #endregion

    private String BuildXml()
    {
        StringBuilder content = new StringBuilder();
        content.Append("<InstallProduct>");
        content.AppendFormat(@"<ServerFile>{0}</ServerFile>", ddlProduct.SelectedIndex);
        content.AppendFormat(@"<AdditionalArgs>{0}</AdditionalArgs>", tboxArguments.Text);
        content.Append(@"</InstallProduct>");

        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("Install");
        xml.Top = String.Empty;
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ProductInstall");        
        xml.AddNode("Content", content.ToString());
        xml.Generate();

        return xml.Result;
    }

    public String BuildTask(String osVersion)
    {
        StringBuilder result = new StringBuilder();
        String version = ddlProduct.SelectedIndex == 0 ? Vba32MsiStorage.GetVba32VersionByOSVersion(osVersion) : Vba32VersionInfo.Vba32RemoteConsoleScanner;

        result.Append("<InstallProduct>");
        result.AppendFormat(@"<ServerFile>{0}</ServerFile>", GetPath(version));        
        result.AppendFormat(@"<AdditionalArgs>{0}</AdditionalArgs>", tboxArguments.Text);
        result.AppendFormat(@"<Hash>{0}</Hash>", GetHash(version));
        result.Append(@"</InstallProduct>");

        return result.ToString();
    }
    
    private String GetPath(String version)
    {
        return Request.ApplicationPath + @"/Installs/" + Vba32MsiStorage.GetPathMSI(version);
    }

    private String GetHash(String version)
    {
        return Vba32MsiStorage.GetHash(version);
    }
}