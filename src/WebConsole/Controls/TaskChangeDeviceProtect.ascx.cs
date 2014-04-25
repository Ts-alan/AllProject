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

public partial class Controls_TaskChangeDeviceProtect : System.Web.UI.UserControl, ITask
{
    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private void ChangeEnabledControl()
    {
        lblMode.Enabled = _enabled;
        ddlMode.Enabled = _enabled;
    }

    private bool _enabled = true;

    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    private bool _hideBound = false;
    public bool HideBound
    {
        get { return _hideBound; }
        set {
            _hideBound = value;
            if (_hideBound) tblProtect.Attributes.Add("class", "");
            else tblProtect.Attributes.Add("class", "ListContrastTable");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && ddlMode.Items.Count == 0)
        {
            InitFields();
        }

        ChangeEnabledControl();
    }

    public void InitFields()
    {
        ddlMode.Items.Clear();
        ddlMode.Items.Add(Resources.Resource.Deactivate);
        ddlMode.Items.Add(Resources.Resource.Activate);
        ddlMode.Items.Add(Resources.Resource.LogEvents);        
                
        if (HideHeader) HeaderName.Visible = false;
    }

    public bool ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Param = BuildXml();
        task.Type = TaskType.ChangeDeviceProtect;

        return task;
    }

    private string BuildXml()
    {
        VirusBlokAda.CC.Common.Xml.XmlBuilder xml = new VirusBlokAda.CC.Common.Xml.XmlBuilder("protect");
        xml.Top = String.Empty;
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ChangeDeviceProtect");
        xml.AddNode("Content", BuildTask());
        xml.Generate();

        return xml.Result;
    }

    public string BuildTask()
    {
        StringBuilder result = new StringBuilder(256);

        result.Append("<SetRegistrySettings>");
        result.AppendFormat(@"<Common><RegisterPath>{0}</RegisterPath><IsDeleteOld>0</IsDeleteOld></Common>",
                    @"HKLM\SOFTWARE\Vba32\Loader\Devices");
        result.AppendFormat(@"<Settings><DEVICE_PROTECT>reg_dword:{0}</DEVICE_PROTECT></Settings></SetRegistrySettings>", ddlMode.SelectedIndex);

        return result.ToString();
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ChangeDeviceProtect)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        VirusBlokAda.CC.Common.Xml.XmlTaskParser parser = new VirusBlokAda.CC.Common.Xml.XmlTaskParser(task.Param);

        string check = parser.GetXmlTagContent("DEVICE_PROTECT");
        check = check.Replace("reg_dword:", "");
        int mode = 0;
        try
        {
            mode = Int32.Parse(check);
        }
        catch
        {
            mode = 0;
        }

        ddlMode.Items[mode].Selected = true;
    }
    
}
