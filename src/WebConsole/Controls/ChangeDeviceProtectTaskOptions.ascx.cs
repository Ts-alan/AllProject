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
using VirusBlokAda.CC.Tasks.Common;
using VirusBlokAda.CC.Tasks.Entities;

public partial class Controls_ChangeDeviceProtectTaskOptions : System.Web.UI.UserControl, ITaskOptions, ITaskOptionsHelper<ChangeDeviceProtectTaskEntity>
{
    #region Page LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    #endregion

    #region Control LifeCycle
    protected override void OnInit(EventArgs e)
    {
        Visible = false;
        base.OnInit(e);
    }

    #endregion

    #region ITaskOptions
    public void LoadTaskEntity(TaskEntity entity)
    {
        LoadTaskEntity(ConvertTaskEntity(entity));
    }

    public TaskEntity SaveTaskEntity(TaskEntity oldEntity, out bool changed)
    {
        return SaveTaskEntity(ConvertTaskEntity(oldEntity), out changed);
    }

    public string DivOptionsClientID
    {
        get
        {
            return tskChangeDeviceProtect.ClientID;
        }
    }

    public Type TaskType
    {
        get { return typeof(ChangeDeviceProtectTaskEntity); }
    }
    #endregion

    #region ITaskOptionsHelper
    public void LoadTaskEntity(ChangeDeviceProtectTaskEntity entity)
    {
        ddlMode.SelectedIndex = entity.Index;
    }

    public ChangeDeviceProtectTaskEntity SaveTaskEntity(ChangeDeviceProtectTaskEntity oldEntity, out bool changed)
    {
        ChangeDeviceProtectTaskEntity entity = new ChangeDeviceProtectTaskEntity();
        entity.Index = ddlMode.SelectedIndex;
        changed = !oldEntity.Equals(entity);
        return entity;
    }

    public ChangeDeviceProtectTaskEntity ConvertTaskEntity(TaskEntity entity)
    {
        ChangeDeviceProtectTaskEntity processEntity = entity as ChangeDeviceProtectTaskEntity;
        if ((entity as ChangeDeviceProtectTaskEntity) == null)
        {
            processEntity = new ChangeDeviceProtectTaskEntity();
        }
        return processEntity;
    }
    #endregion











    //private bool _hideHeader = false;

    //public bool HideHeader
    //{
    //    get { return _hideHeader; }
    //    set { _hideHeader = value; }
    //}

    //private void ChangeEnabledControl()
    //{
    //    lblMode.Enabled = _enabled;
    //    ddlMode.Enabled = _enabled;
    //}

    //private bool _enabled = true;

    //public bool Enabled
    //{
    //    get { return _enabled; }
    //    set { _enabled = value; }
    //}

    //private bool _hideBound = false;
    //public bool HideBound
    //{
    //    get { return _hideBound; }
    //    set {
    //        _hideBound = value;
    //        if (_hideBound) tblProtect.Attributes.Add("class", "");
    //        else tblProtect.Attributes.Add("class", "ListContrastTable");
    //    }
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!IsPostBack && ddlMode.Items.Count == 0)
    //    {
    //        InitFields();
    //    }

    //    ChangeEnabledControl();
    //}

    //public void InitFields()
    //{
    //    ddlMode.Items.Clear();
    //    ddlMode.Items.Add(Resources.Resource.Deactivate);
    //    ddlMode.Items.Add(Resources.Resource.�ctivate);
    //    ddlMode.Items.Add(Resources.Resource.LogEvents);        
                
    //    if (HideHeader) HeaderName.Visible = false;
    //}

    //public bool ValidateFields()
    //{
    //    return true;
    //}

    //public TaskUserEntity GetCurrentState()
    //{
    //    TaskUserEntity task = new TaskUserEntity();
    //    task.Param = BuildXml();
    //    task.Type = TaskType.ChangeDeviceProtect;

    //    return task;
    //}

    //private string BuildXml()
    //{
    //    ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("protect");
    //    xml.Top = String.Empty;
    //    xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
    //    xml.AddNode("Type", "ChangeDeviceProtect");
    //    xml.AddNode("Content", BuildTask());
    //    xml.Generate();

    //    return xml.Result;
    //}

    //public string BuildTask()
    //{
    //    StringBuilder result = new StringBuilder(256);

    //    result.Append("<SetRegistrySettings>");
    //    result.AppendFormat(@"<Common><RegisterPath>{0}</RegisterPath><IsDeleteOld>0</IsDeleteOld></Common>",
    //                @"HKLM\SOFTWARE\Vba32\Loader\Devices");
    //    result.AppendFormat(@"<Settings><DEVICE_PROTECT>reg_dword:{0}</DEVICE_PROTECT></Settings></SetRegistrySettings>", ddlMode.SelectedIndex);

    //    return result.ToString();
    //}

    //public void LoadState(TaskUserEntity task)
    //{
    //    if (task.Type != TaskType.ChangeDeviceProtect)
    //        throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

    //    XmlTaskParser parser = new XmlTaskParser(task.Param);

    //    string check = parser.GetXmlTagContent("DEVICE_PROTECT");
    //    check = check.Replace("reg_dword:", "");
    //    int mode = 0;
    //    try
    //    {
    //        mode = Int32.Parse(check);
    //    }
    //    catch
    //    {
    //        mode = 0;
    //    }

    //    ddlMode.Items[mode].Selected = true;
    //}
    
}
