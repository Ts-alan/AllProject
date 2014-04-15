using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class Controls_TaskChangeDeviceProtectEx : System.Web.UI.UserControl, ITask
{
    private Boolean _hideHeader = false;

    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private void ChangeEnabledControl()
    {
        lblMode.Enabled = _enabled;
        ddlMode.Enabled = _enabled;

        cboxUseDailyProtect.Enabled = _enabled;
        Boolean enbl = false;
        if (_enabled && cboxUseDailyProtect.Checked)
            enbl = true;

        ddlMonday.Enabled = enbl;
        ddlTuesday.Enabled = enbl;
        ddlWednesday.Enabled = enbl;
        ddlThursday.Enabled = enbl;
        ddlFriday.Enabled = enbl;
        ddlSaturday.Enabled = enbl;
        ddlSunday.Enabled = enbl;
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
            if (_hideBound)
            {
                tblProtect.Attributes.Add("class", "");
                tblDailyProtect.Attributes.Add("class", "");                
            }
            else
            {
                tblProtect.Attributes.Add("class", "ListContrastTable");
                tblDailyProtect.Attributes.Add("class", "ListContrastTable");
            }
        }
    }

    protected void Page_Load(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFields();
        }

        ChangeEnabledControl();
    }

    #region Initialization
    
    private void FillDDList()
    {
        if (ddlMonday.Items.Count != 0) return;
        List<String> source = GetDDLData();
        ddlMonday.DataSource = source;
        ddlMonday.DataBind();
        ddlThursday.DataSource = source;
        ddlThursday.DataBind();
        ddlWednesday.DataSource = source;
        ddlWednesday.DataBind();
        ddlTuesday.DataSource = source;
        ddlTuesday.DataBind();
        ddlFriday.DataSource = source;
        ddlFriday.DataBind();
        ddlSaturday.DataSource = source;
        ddlSaturday.DataBind();
        ddlSunday.DataSource = source;
        ddlSunday.DataBind();

        ddlMode.DataSource = source;
        ddlMode.DataBind();
    }

    private List<String> GetDDLData()
    {
        List<String> list = new List<String>();
        list.Add(Resources.Resource.Deactivate);
        list.Add(Resources.Resource.Activate);
        list.Add(Resources.Resource.LogEvents);

        return list;
    }

    private void CreateScripts()
    {
        StringBuilder script = new StringBuilder();
        String name = String.Format("SetEnabled{0}", this.ClientID);
        script.AppendFormat("function {0}(obj)", name);
        script.Append("{");        

        script.AppendFormat("$get('{0}').disabled = !obj.checked;", ddlMonday.ClientID);
        script.AppendFormat("$get('{0}').disabled = !obj.checked;", ddlTuesday.ClientID);
        script.AppendFormat("$get('{0}').disabled = !obj.checked;", ddlWednesday.ClientID);
        script.AppendFormat("$get('{0}').disabled = !obj.checked;", ddlThursday.ClientID);
        script.AppendFormat("$get('{0}').disabled = !obj.checked;", ddlFriday.ClientID);
        script.AppendFormat("$get('{0}').disabled = !obj.checked;", ddlSaturday.ClientID);
        script.AppendFormat("$get('{0}').disabled = !obj.checked;", ddlSunday.ClientID);

        script.Append("}");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), null, script.ToString(), true);

        cboxUseDailyProtect.Attributes.Add("onclick", String.Format("{0}(this);", name));
    }    

    public void InitFields()
    {
        FillDDList();
        CreateScripts();
        if (HideHeader) HeaderName.Visible = false;
        cboxUseDailyProtect.Text = Resources.Resource.UseDailyProtect;
    }    
    #endregion

    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Param = BuildXml();
        task.Type = TaskType.DailyDeviceProtect;

        return task;
    }

    private String BuildXml()
    {
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("DailyDeviceProtect");
        xml.Top = String.Empty;
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "DailyDeviceProtect");
        xml.AddNode("Content", BuildTask());
        xml.Generate();

        return xml.Result;
    }

    public String BuildTask()
    {
        StringBuilder result = new StringBuilder(256);
        
        result.Append("<SetRegistrySettings>");
        result.AppendFormat(@"<Common><RegisterPath>{0}</RegisterPath><IsDeleteOld>0</IsDeleteOld></Common>",
                    @"HKLM\SOFTWARE\Vba32\Loader\Devices");
        result.Append(@"<Settings>");

        result.AppendFormat(@"<DEVICE_PROTECT>reg_dword:{0}</DEVICE_PROTECT>", ddlMode.SelectedIndex);

        result.AppendFormat(@"<USE_DAILY_PROTECT>reg_dword:{0}</USE_DAILY_PROTECT>", cboxUseDailyProtect.Checked ? 1 : 0);
        if (cboxUseDailyProtect.Checked)
        {
            result.AppendFormat(@"<{1}>reg_dword:{0}</{1}>", ddlMonday.SelectedIndex, "MONDAY");
            result.AppendFormat(@"<{1}>reg_dword:{0}</{1}>", ddlTuesday.SelectedIndex, "TUESDAY");
            result.AppendFormat(@"<{1}>reg_dword:{0}</{1}>", ddlWednesday.SelectedIndex, "WEDNESDAY");
            result.AppendFormat(@"<{1}>reg_dword:{0}</{1}>", ddlThursday.SelectedIndex, "THURSDAY");
            result.AppendFormat(@"<{1}>reg_dword:{0}</{1}>", ddlFriday.SelectedIndex, "FRIDAY");
            result.AppendFormat(@"<{1}>reg_dword:{0}</{1}>", ddlSaturday.SelectedIndex, "SATURDAY");
            result.AppendFormat(@"<{1}>reg_dword:{0}</{1}>", ddlSunday.SelectedIndex, "SUNDAY");
        }

        result.Append(@"</Settings></SetRegistrySettings>");

        return result.ToString();
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.DailyDeviceProtect)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser parser = new XmlTaskParser(task.Param);

        String check = parser.GetXmlTagContent("DEVICE_PROTECT");
        check = check.Replace("reg_dword:", "");
        Int32 mode = 0;
        try
        {
            mode = Int32.Parse(check);
        }
        catch
        {
            mode = 0;
        }
        UnselectItems(ddlMode);
        ddlMode.Items[mode].Selected = true;

        check = parser.GetXmlTagContent("USE_DAILY_PROTECT");
        check = check.Replace("reg_dword:", "");
        mode = 0;
        try
        {
            mode = Int32.Parse(check);
        }
        catch
        {
            mode = 0;
        }

        cboxUseDailyProtect.Checked = (mode == 1);

        if (cboxUseDailyProtect.Checked)
        {
            UnselectItems(ddlMonday);
            UnselectItems(ddlTuesday);
            UnselectItems(ddlWednesday);
            UnselectItems(ddlThursday);
            UnselectItems(ddlFriday);
            UnselectItems(ddlSaturday);
            UnselectItems(ddlSunday);

            check = parser.GetXmlTagContent("MONDAY");
            check = check.Replace("reg_dword:", "");
            try { mode = Int32.Parse(check); }
            catch { mode = 0; }
            ddlMonday.Items[mode].Selected = true;

            check = parser.GetXmlTagContent("TUESDAY");
            check = check.Replace("reg_dword:", "");
            try { mode = Int32.Parse(check); }
            catch { mode = 0; }
            ddlTuesday.Items[mode].Selected = true;

            check = parser.GetXmlTagContent("WEDNESDAY");
            check = check.Replace("reg_dword:", "");
            try { mode = Int32.Parse(check); }
            catch { mode = 0; }
            ddlWednesday.Items[mode].Selected = true;

            check = parser.GetXmlTagContent("THURSDAY");
            check = check.Replace("reg_dword:", "");
            try { mode = Int32.Parse(check); }
            catch { mode = 0; }
            ddlThursday.Items[mode].Selected = true;

            check = parser.GetXmlTagContent("FRIDAY");
            check = check.Replace("reg_dword:", "");
            try { mode = Int32.Parse(check); }
            catch { mode = 0; }
            ddlFriday.Items[mode].Selected = true;

            check = parser.GetXmlTagContent("SATURDAY");
            check = check.Replace("reg_dword:", "");
            try { mode = Int32.Parse(check); }
            catch { mode = 0; }
            ddlSaturday.Items[mode].Selected = true;

            check = parser.GetXmlTagContent("SUNDAY");
            check = check.Replace("reg_dword:", "");
            try { mode = Int32.Parse(check); }
            catch { mode = 0; }
            ddlSunday.Items[mode].Selected = true;
        }
    }

    private void UnselectItems(DropDownList ddl)
    {
        foreach (ListItem item in ddl.Items)
        {
            item.Selected = false;
        }
    }
}