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

public partial class Controls_TaskAgentSettings : System.Web.UI.UserControl, ITask
{
    private Boolean _hideHeader = false;
    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private void ChangeEnabledControl()
    {
        lblPoollingTimeInterval.Enabled = _enabled;
        txtPoollingTimeInterval.Enabled = _enabled;
        lblTime.Enabled = _enabled;
    }

    private Boolean _enabled = true;
    public Boolean Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    private Boolean _hideBound = false;
    public Boolean HideBound
    {
        get { return _hideBound; }
        set
        {
            _hideBound = value;
            if (_hideBound) tblAgentSettings.Attributes.Add("class", "");
            else tblAgentSettings.Attributes.Add("class", "ListContrastTable");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFields();
        }

        ChangeEnabledControl();
    }

    #region ITask Members

    public void InitFields()
    {
        txtPoollingTimeInterval.Text = "1";
        if (HideHeader) HeaderName.Visible = false;
    }

    public Boolean ValidateFields()
    {
        Int32 val = 0;
        return Int32.TryParse(txtPoollingTimeInterval.Text, out val);
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Param = BuildXml();
        task.Type = TaskType.AgentSettings;

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.AgentSettings)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser parser = new XmlTaskParser(task.Param);

        String val = parser.GetXmlTagContent("StateSendInterval");
        val = val.Replace("reg_dword:", "");
        Int32 interval = 0;
        try
        {
            interval = Int32.Parse(val) / 60000;
        }
        catch
        {
            interval = 1;
        }
                
        txtPoollingTimeInterval.Text = interval.ToString();
    }

    #endregion

    private String BuildXml()
    {
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("agentSettings");
        xml.Top = String.Empty;
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "AgentSettings");
        xml.AddNode("Content", BuildTask());
        xml.Generate();

        return xml.Result;
    }

    public String BuildTask()
    {
        if (!ValidateFields())
            return String.Empty;

        StringBuilder result = new StringBuilder(256);
        result.Append("<SetRegistrySettings>");
        result.AppendFormat(@"<Common><RegisterPath>{0}</RegisterPath><IsDeleteOld>0</IsDeleteOld></Common>",
                    @"HKLM\SOFTWARE\Vba32\ControlAgent");
        result.AppendFormat(@"<Settings><StateSendInterval>reg_dword:{0}</StateSendInterval></Settings></SetRegistrySettings>", Convert.ToInt32(txtPoollingTimeInterval.Text) * 60000);

        return result.ToString();
    }
}
