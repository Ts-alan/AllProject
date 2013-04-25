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
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ARM2_dbcontrol.Tasks.ProactiveProtection;
using ARM2_dbcontrol.Service.Vba32NS;
using ARM2_dbcontrol.Filters;
using ARM2_dbcontrol.DataBase;


public partial class Controls_TaskProActivProtection : System.Web.UI.UserControl, ITask
{
    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

        if (Rules == null)
        {
            Rules = new List<RuleProactiveProtection>();
            InitializeRules();
        }

        UpdateTypeApp();
        rbtnKey.Text = Resources.Resource.Key;
        cbxIncludeSubkeys.Text = Resources.Resource.IncludeSubkeys;
        rbtnValue.Text = Resources.Resource.Value;
        cbxWriteReportReg.Text = Resources.Resource.WriteReport;
        cbxWriteReportFS.Text = Resources.Resource.WriteReport;
        cbxEventARMReg.Text = Resources.Resource.EventARM;
        cbxEventARMFS.Text = Resources.Resource.EventARM;
        cbxAllowRead.Text = Resources.Resource.AllowRead;
        cbxIncludeSubDir.Text = Resources.Resource.IncludeSubdirectories;

        tabPanel1.HeaderText = Resources.Resource.Applications;
        tabPanel2.HeaderText = Resources.Resource.Registry;
        tabPanel3.HeaderText = Resources.Resource.FileSystem;

        btnAddRuleReg.Text = Resources.Resource.Save;
        btnCancelRuleReg.Text = Resources.Resource.Close;
        btnAddRuleFS.Text = Resources.Resource.Save;
        btnCancelRuleFS.Text = Resources.Resource.Close;

        lbtnAddReg.Text = Resources.Resource.Add;
        lbtnAddFS.Text = Resources.Resource.Add;

        TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
        ddlProfiles.Items.Clear();
        foreach (TaskUserEntity tsk in collection)
        {
            if (tsk.Type == TaskType.ProactiveProtection)
            {
                try
                {
                    ddlProfiles.Items.Add(tsk.Name.Substring(22));
                }
                catch { }
            }
        }

        if (ddlProfiles.Items.Count == 0)
        {
            lbtnRemove.Visible = lbtnEdit.Visible = false;
        }
        else
        {
            lbtnRemove.Visible = lbtnEdit.Visible = true;
        }

        lbtnRemove.Attributes.Add("onclick", "return confirm('" + Resources.Resource.AreYouSureProfile + "');");
    }

    public bool ValidateFields()
    {
        for (int i = 3; i < Rules.Count; i++)
        {
            if (Rules[i].Applications.Count != 1)
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue);
        }

        if (Rules[0].Applications.Count > 0 ||
            (Rules[1].Applications.Count > 0 && (Rules[1].RegistryRules.Count > 0 || Rules[1].FileSystemRules.Count > 0)) ||
            (Rules[2].Applications.Count > 0 && (Rules[2].RegistryRules.Count > 0 || Rules[2].FileSystemRules.Count > 0))) return true;
        else
            for (int i = 3; i < Rules.Count; i++)
            {
                if (Rules[i].RegistryRules.Count > 0 || Rules[i].FileSystemRules.Count > 0) return true;                
            }

        throw new ArgumentException(Resources.Resource.ErrorInvalidValue);
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ProactiveProtection;
        task.Param = BuildXml();
        return task;
    }
    /// <summary>
    /// Метод конвертации данных в xml строку
    /// </summary>
    /// <returns>xml строка</returns>
    private string BuildXml()
    {
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("ProactiveProtection");
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ProactiveProtection");
        xml.AddNode("Content", Server.HtmlEncode(ObjectSerializer.ObjToXmlStr(Rules).Remove(0, 39)));
        xml.Generate();

        return xml.Result;
    }

    /// <summary>
    /// Метод конвертации из xml в необходимый config файлу формат
    /// </summary>
    /// <param name="xmlParam">Сериализованные в xml данные</param>
    /// <returns>Содержимое config файла</returns>
    private string BuildConfig(string xmlParam)
    {
        List<RuleProactiveProtection> lstRules = ObjectSerializer.XmlStrToObj<List<RuleProactiveProtection>>(xmlParam);

        string result = "[Config]\n";
        result += "log_mode=0\n";
        result += "save_log_days=0\n";
        result += "[TrustedApps]\n";
        foreach (string item in lstRules[0].Applications)
        {
            result += String.Format("\"{0}\"\n", item);
        }
        result += "[NeutralApps]\n";
        foreach (string item in lstRules[1].Applications)
        {
            result += String.Format("\"{0}\"\n", item);
        }
        result += "[NonTrustedApps]\n";
        foreach (string item in lstRules[2].Applications)
        {
            result += String.Format("\"{0}\"\n", item);
        }
        //Добавляем правила для нейтральных приложений
        result += "[NeutralAppsRegPolicy]\n";
        foreach (RegistryRule rule in lstRules[1].RegistryRules)
        {
            if (rule.eventARM != String.Empty) result += string.Format("{0}: ", rule.eventARM);
            result += string.Format("\"{0}\" TYPE {1} RECURSE_SUBKEYS {2}", FormatRegistryPath(rule.Path), rule.TypeNote == "Key" ? "KEY" : "VALUE", rule.Subkeys ? "YES" : "NO");
            if (!rule.Log) result += " NOLOG";
            result += '\n';
        }
        result += "[NeutralAppsFilePolicy]\n";
        foreach (FileSystemRule rule in lstRules[1].FileSystemRules)
        {
            if (rule.eventARM != String.Empty) result += string.Format("{0}: ", rule.eventARM);
            result += string.Format("\"{0}\" ALLOW_READ {1} RECURSE_SUBDIRS {2}", rule.Path, rule.AlloRead ? "YES" : "NO", rule.Subdirs ? "YES" : "NO");
            if (!rule.Log) result += " NOLOG";
            result += '\n';
        }
        //Добавляем правила для недоверенных приложений
        result += "[NonTrustedAppsRegPolicy]\n";
        foreach (RegistryRule rule in lstRules[2].RegistryRules)
        {
            if (rule.eventARM != String.Empty) result += string.Format("{0}: ", rule.eventARM);
            result += string.Format("\"{0}\" TYPE {1} RECURSE_SUBKEYS {2}", FormatRegistryPath(rule.Path), rule.TypeNote == "Key" ? "KEY" : "VALUE", rule.Subkeys ? "YES" : "NO");
            if (!rule.Log) result += " NOLOG";
            result += '\n';
        }
        result += "[NonTrustedAppsFilePolicy]\n";
        foreach (FileSystemRule rule in lstRules[2].FileSystemRules)
        {
            if (rule.eventARM != String.Empty) result += string.Format("{0}: ", rule.eventARM);
            result += string.Format("\"{0}\" ALLOW_READ {1} RECURSE_SUBDIRS {2}", rule.Path, rule.AlloRead ? "YES" : "NO", rule.Subdirs ? "YES" : "NO");
            if (!rule.Log) result += " NOLOG";
            result += '\n';
        }
        //частные политики
        result += "[PrivatePolicy]\n";
        for (int i = 3; i < lstRules.Count; i++)
        {
            result += String.Format("\"{0}\" =RP_{1:D4} FP_{1:D4}", lstRules[i].Applications[0], i - 3) + '\n';
        }

        for (int i = 3; i < lstRules.Count; i++)
        {
            result += String.Format("[RP_{0:D4}]\n", i - 3);
            foreach (RegistryRule rule in lstRules[i].RegistryRules)
            {
                if (rule.eventARM != String.Empty) result += string.Format("{0}: ", rule.eventARM);
                result += string.Format("\"{0}\" TYPE {1} RECURSE_SUBKEYS {2}", FormatRegistryPath(rule.Path), rule.TypeNote == "Key" ? "KEY" : "VALUE", rule.Subkeys ? "YES" : "NO");
                if (!rule.Log) result += " NOLOG";
                result += '\n';
            }
            result += String.Format("[FP_{0:D4}]\n", i - 3);
            foreach (FileSystemRule rule in lstRules[i].FileSystemRules)
            {
                if (rule.eventARM != String.Empty) result += string.Format("{0}: ", rule.eventARM);
                result += string.Format("\"{0}\" ALLOW_READ {1} RECURSE_SUBDIRS {2}", rule.Path, rule.AlloRead ? "YES" : "NO", rule.Subdirs ? "YES" : "NO");
                if (!rule.Log) result += " NOLOG";
                result += '\n';
            }
        }

        return result;
    }

    /// <summary>
    /// Метод преобразования ветки реестра
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string FormatRegistryPath(string path)
    {
        if(path.Contains("HKEY_CLASSES_ROOT\\"))
        {
            path = path.Replace("HKEY_CLASSES_ROOT", @"\REGISTRY\CLASSES_ROOT");
        }
        else
            if (path.Contains("HKEY_CURRENT_USER\\"))
            {
                path = path.Replace("HKEY_CURRENT_USER", @"\REGISTRY\CURRENT_USER");
            }
            else
                if (path.Contains("HKEY_LOCAL_MACHINE\\"))
                {
                    path = path.Replace("HKEY_LOCAL_MACHINE", @"\REGISTRY\MACHINE");
                }
                else
                    if (path.Contains("HKEY_USERS\\"))
                    {
                        path = path.Replace("HKEY_USERS", @"\REGISTRY\USER");
                    }
                    else
                        if (path.Contains("HKEY_CURRENT_CONFIG\\"))
                        {
                            path = path.Replace("HKEY_CURRENT_CONFIG", @"\REGISTRY\CURRENT_CONFIG");
                        }
                        else
                            if (path.Contains("HKCU\\"))
                            {
                                path = path.Replace("HKCU", @"\REGISTRY\CURRENT_USER");
                            }
                            else
                                if (path.Contains("HKLM\\"))
                                {
                                    path = path.Replace("HKLM", @"\REGISTRY\MACHINE");
                                }
                                else
                                    if (path.Contains("HKCR\\"))
                                    {
                                        path = path.Replace("HKCR", @"\REGISTRY\CLASSES_ROOT");
                                    }
        return path;
    }
    /// <summary>
    /// Метод генерации задачи
    /// </summary>
    /// <param name="fullPath">полный путь config файла</param>
    /// <param name="xmlParam">xml настроек</param>
    /// <returns></returns>
    public string BuildTask(string fullPath, string xmlParam)
    {
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("CreateFile");
        xml.Top = String.Empty;

        XmlTaskParser pars = new XmlTaskParser(xmlParam);

        xml.AddNode("FullPath", fullPath);
        xml.AddNode("Content", BuildConfig(Server.HtmlDecode(pars.GetValue("Content"))));
        xml.AddNode("IsNeedCreate", "1");
        xml.Generate();

        return xml.Result;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.ProactiveProtection)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);        

        XmlTaskParser pars = new XmlTaskParser(task.Param);
        Rules = ObjectSerializer.XmlStrToObj<List<RuleProactiveProtection>>(Server.HtmlDecode(pars.GetValue("Content")));
        UpdateTypeApp();
        UpdateData();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitFields();
        }
    }

    private void UpdateData()
    {
        ClearField();
        foreach (string item in Rules[ddlTypeApp.SelectedIndex].Applications)
        {
            lbxApplications.Items.Add(item);
        }
        DataTable dt = new DataTable();
        CreateColumnsRegistry(dt);
        foreach (RegistryRule rule in Rules[ddlTypeApp.SelectedIndex].RegistryRules)
        {
            AddNewItem(dt, rule);
        }
        dlRegistryRules.DataSource = new DataView(dt);
        dlRegistryRules.DataBind();        

        dt = new DataTable();
        CreateColumnsFileSystem(dt);
        foreach (FileSystemRule rule in Rules[ddlTypeApp.SelectedIndex].FileSystemRules)
        {
            AddNewItem(dt, rule);
        }
        dlFileSystemRules.DataSource = new DataView(dt);
        dlFileSystemRules.DataBind();
                            
        Tabs.ActiveTabIndex = int.Parse(ActiveTab.Value);
    }

    private void UpdateTypeApp()
    {
        InitializeTypeApp();
        for (int i = 3; i < Rules.Count; i++)
        {
            ddlTypeApp.Items.Add(new ListItem(String.Format("{0} {1}", Resources.Resource.PrivateRule, i - 2), String.Format("{0}", i)));
        }
    }

    protected void ddlTypeApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        ActiveTab.Value = "0";
        switch (ddlTypeApp.SelectedIndex)
        {
            case 0:
                //для доверенных приложений скрываем панели для правил реестра и файловой системы
                tabPanel2.Enabled = tabPanel3.Enabled = false;
                break;
            default:
                tabPanel2.Enabled = tabPanel3.Enabled = true;
                break;
        }
        //проверяем на частное правило
        if (ddlTypeApp.SelectedIndex < 3) lbtnDeleteTypeApp.Visible = false;
        else lbtnDeleteTypeApp.Visible = true;

        UpdateData();
    }

    public void ShowProfiles()
    {
        tblProfileMngmt.Visible = true;
        tblProfileOptions.Visible = false;
        tblProfile.Visible = false;
    }

    public void ShowProfileDetails(TaskUserEntity task)
    {
        ClearRule();
        LoadState(task);

        tblProfileMngmt.Visible = false;
        ddlTypeApp.SelectedIndex = 0;
        tblProfileOptions.Visible = true;
        tblProfile.Visible = true;
    }

    private void ClearRule()
    {
        InitializeRules();
        ClearField();
        InitializeTypeApp();
    }

    private void InitializeRules()
    {
        Rules.Clear();
        Rules.Add(new RuleProactiveProtection(RuleTypeEnum.Trusted));
        Rules.Add(new RuleProactiveProtection(RuleTypeEnum.Neutral));
        Rules.Add(new RuleProactiveProtection(RuleTypeEnum.Distrusted));
    }

    private void ClearField()
    {
        lbxApplications.Items.Clear();
        dlRegistryRules.DataBind();
        dlFileSystemRules.DataBind();
    }

    private void InitializeTypeApp()
    {
        ddlTypeApp.Items.Clear();
        ddlTypeApp.Items.Add(new ListItem(Resources.Resource.TrustedApplications, "0", true));
        ddlTypeApp.Items.Add(new ListItem(Resources.Resource.NeutralApplications, "1"));
        ddlTypeApp.Items.Add(new ListItem(Resources.Resource.DistrustedApplications, "2"));
    }

    #region Create/Edit profile

    #region TypeApp

    protected void lbtnCreateTypeApp_Click(object sender, EventArgs e)
    {
        Rules.Add(new RuleProactiveProtection(RuleTypeEnum.Private));
        UpdateTypeApp();
        UpdateData();
        lbtnDeleteTypeApp.Visible = true;
        ddlTypeApp.SelectedIndex = Rules.Count - 1;
        ddlTypeApp_SelectedIndexChanged(sender, e);
    }

    protected void lbtnDeleteTypeApp_Click(object sender, EventArgs e)
    {
        Rules.RemoveAt(ddlTypeApp.SelectedIndex);
        ddlTypeApp.Items.RemoveAt(ddlTypeApp.SelectedIndex);
        ddlTypeApp.SelectedIndex = 0;
        ddlTypeApp_SelectedIndexChanged(sender, e);
    }

    #endregion

    #region Application

    protected void lbtnAddApp_Click(object sender, EventArgs e)
    {        
            //для частного правила можно добавить только 1 приложение
            if (ddlTypeApp.SelectedIndex > 2 && Rules[ddlTypeApp.SelectedIndex].Applications.Count > 0) return;
            if (!ValidateApplication(fuApplicationAuthorized.Text)) return;

            Rules[ddlTypeApp.SelectedIndex].Applications.Add(fuApplicationAuthorized.Text);
            fuApplicationAuthorized.Text = String.Empty;
            UpdateData();     
    }

    protected void lbtnRemoveApp_Click(object sender, EventArgs e)
    {
        if (lbxApplications.SelectedIndex > -1)
        {
            Rules[ddlTypeApp.SelectedIndex].Applications.RemoveAt(lbxApplications.SelectedIndex);
            UpdateData();
        }
    }

    private bool ValidateApplication(string application)
    {
        if (application == String.Empty) return false;
        for (int i = 0; i < Rules.Count; i++)
        {
            foreach (string item in Rules[i].Applications)
            {
                if (application == item) return false;
            }
        }
        return true;        
    }

    #endregion


    #region Registry

    protected void lbtnAddReg_Click(object sender, EventArgs e)
    {        
        if (!ValidateRegistryRule(txtRegistry.Text)) return;

        if (!isEditingRegistry)
        {
            Rules[ddlTypeApp.SelectedIndex].RegistryRules.Add(GetNewRegistryRule());
        }
        else
        {
            Rules[ddlTypeApp.SelectedIndex].RegistryRules[indexItem] = GetNewRegistryRule();
            isEditingRegistry = false;
        }
        UpdateData();
        txtRegistry.Text = txtEventARMReg.Text = "";
        cbxEventARMReg.Checked = cbxIncludeSubkeys.Checked = rbtnValue.Checked = cbxWriteReportReg.Checked = txtEventARMReg.Enabled = false;
        rbtnKey.Checked = cbxIncludeSubkeys.Enabled = true;
    }

    protected void btnCancelRuleReg_Click(object sender, EventArgs e)
    {
        isEditingRegistry = false;
        txtRegistry.Text = txtEventARMReg.Text = "";
        cbxEventARMReg.Checked = cbxIncludeSubkeys.Checked = rbtnValue.Checked = cbxWriteReportReg.Checked = txtEventARMReg.Enabled = false;
        rbtnKey.Checked = cbxIncludeSubkeys.Enabled = true;        
    }

    protected void dlRegistryRules_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DeleteCommand":
                Rules[ddlTypeApp.SelectedIndex].RegistryRules.RemoveAt(e.Item.ItemIndex);
                UpdateData();
                break;
            case "EditCommand":
                isEditingRegistry = true;
                indexItem = e.Item.ItemIndex;
                
                txtRegistry.Text = Rules[ddlTypeApp.SelectedIndex].RegistryRules[indexItem].Path;
                rbtnKey.Checked = Rules[ddlTypeApp.SelectedIndex].RegistryRules[indexItem].TypeNote == "Key" ? true : false;
                rbtnValue.Checked = !rbtnKey.Checked;
                cbxIncludeSubkeys.Checked = Rules[ddlTypeApp.SelectedIndex].RegistryRules[indexItem].Subkeys;                
//!--           посмотреть, как сделать cbxIncludeSubkeys disable                
                cbxWriteReportReg.Checked = Rules[ddlTypeApp.SelectedIndex].RegistryRules[indexItem].Log;
                cbxEventARMReg.Checked = Rules[ddlTypeApp.SelectedIndex].RegistryRules[indexItem].eventARM == String.Empty ? false : true;
                if (cbxEventARMReg.Checked)
                    txtEventARMReg.Enabled = true;
                else txtEventARMReg.Enabled = false;
                txtEventARMReg.Text = Rules[ddlTypeApp.SelectedIndex].RegistryRules[indexItem].eventARM;

                ModalPopupExtenderRule.Show();
                break;            
        }
    }

    private bool ValidateRegistryRule(string pathRegistry)
    {
        if (pathRegistry == String.Empty) return false;

        for (int i = 0; i < Rules[ddlTypeApp.SelectedIndex].RegistryRules.Count; i++)
        {
            if ((!isEditingRegistry && Rules[ddlTypeApp.SelectedIndex].RegistryRules[i].Path == pathRegistry) ||
                (isEditingRegistry && indexItem != i && Rules[ddlTypeApp.SelectedIndex].RegistryRules[i].Path == pathRegistry))
                return false;
        }

        return true;
    }

    private RegistryRule GetNewRegistryRule()
    {
        RegistryRule rule = new RegistryRule();

        rule.eventARM = cbxEventARMReg.Checked ? txtEventARMReg.Text : String.Empty;
        rule.Path = txtRegistry.Text;
        rule.TypeNote = rbtnKey.Checked ? "Key" : "Value";
        rule.Subkeys = cbxIncludeSubkeys.Checked;
        rule.Log = cbxWriteReportReg.Checked;

        return rule;
    }

    private void AddNewItem(DataTable dt, RegistryRule rule)
    {
        DataRow dr = dt.NewRow();
        dr[0] = rule.eventARM;
        dr[1] = rule.Path;
        dr[2] = rule.TypeNote == "Key" ? Resources.Resource.Key : Resources.Resource.Value;
        dr[3] = rule.Subkeys == true ? Resources.Resource.Yes : Resources.Resource.No;
        dr[4] = rule.Log == true ? Resources.Resource.Yes : Resources.Resource.No;
        dt.Rows.Add(dr);
    }

    private void CreateColumnsRegistry(DataTable dt)
    {
        dt.Columns.Add(new DataColumn("EventARM", typeof(String)));
        dt.Columns.Add(new DataColumn("Path", typeof(String)));
        dt.Columns.Add(new DataColumn("Type", typeof(String)));
        dt.Columns.Add(new DataColumn("Subkeys", typeof(String)));
        dt.Columns.Add(new DataColumn("Log", typeof(String)));
    }

    #endregion

    #region FileSystem

    protected void lbtnAddFS_Click(object sender, EventArgs e)
    {
        if (!ValidateFSRule(txtNameDir.Text)) return;
        if (!isEditingFileSystem)
        {
            Rules[ddlTypeApp.SelectedIndex].FileSystemRules.Add(GetNewFileSystemRule());
        }
        else
        {
            Rules[ddlTypeApp.SelectedIndex].FileSystemRules[indexItem] = GetNewFileSystemRule();
            isEditingFileSystem = false;
        }
        UpdateData();
        txtNameDir.Text = txtEventARMFS.Text = "";
        cbxEventARMFS.Checked = cbxIncludeSubDir.Checked = cbxWriteReportFS.Checked = txtEventARMFS.Enabled = cbxAllowRead.Checked = false;
        cbxIncludeSubDir.Enabled = true;        
    }

    protected void btnCancelRuleFS_Click(object sender, EventArgs e)
    {
        isEditingFileSystem = false;
        txtNameDir.Text = txtEventARMFS.Text = "";
        cbxEventARMFS.Checked = cbxIncludeSubDir.Checked = cbxWriteReportFS.Checked = txtEventARMFS.Enabled = cbxAllowRead.Checked = false;
        cbxIncludeSubDir.Enabled = true;         
    }

    protected void dlFileSystemRules_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DeleteCommand":
                Rules[ddlTypeApp.SelectedIndex].FileSystemRules.RemoveAt(e.Item.ItemIndex);
                UpdateData();
                break;
            case "EditCommand":
                isEditingFileSystem = true;
                indexItem = e.Item.ItemIndex;

                txtNameDir.Text = Rules[ddlTypeApp.SelectedIndex].FileSystemRules[indexItem].Path;
                cbxAllowRead.Checked = Rules[ddlTypeApp.SelectedIndex].FileSystemRules[indexItem].AlloRead;
                cbxWriteReportFS.Checked = Rules[ddlTypeApp.SelectedIndex].FileSystemRules[indexItem].Log;
                cbxIncludeSubDir.Checked = Rules[ddlTypeApp.SelectedIndex].FileSystemRules[indexItem].Subdirs;
                cbxEventARMFS.Checked = Rules[ddlTypeApp.SelectedIndex].FileSystemRules[indexItem].eventARM == String.Empty ? false : true;
                txtEventARMFS.Text = Rules[ddlTypeApp.SelectedIndex].FileSystemRules[indexItem].eventARM;
                if (cbxEventARMFS.Checked)
                    txtEventARMFS.Enabled = true;
                else txtEventARMFS.Enabled = false;

                ModalPopupExtenderRule1.Show();
                break;
        }
    }

    private bool ValidateFSRule(string path)
    {
        if (path == String.Empty) return false;

        for (int i = 0; i < Rules[ddlTypeApp.SelectedIndex].FileSystemRules.Count; i++)
        {
            if ((!isEditingFileSystem && Rules[ddlTypeApp.SelectedIndex].FileSystemRules[i].Path == path) ||
                (isEditingFileSystem && indexItem != i && Rules[ddlTypeApp.SelectedIndex].FileSystemRules[i].Path == path))
                return false;
        }

        return true;
    }

    private FileSystemRule GetNewFileSystemRule()
    {
        FileSystemRule rule = new FileSystemRule();

        rule.eventARM = cbxEventARMFS.Checked ? txtEventARMFS.Text : String.Empty;
        rule.Path = txtNameDir.Text;
        rule.AlloRead = cbxAllowRead.Checked;
        rule.Subdirs = cbxIncludeSubDir.Checked;
        rule.Log = cbxWriteReportFS.Checked;

        return rule;
    }

    private void AddNewItem(DataTable dt, FileSystemRule rule)
    {
        DataRow dr = dt.NewRow();
        dr[0] = rule.eventARM;
        dr[1] = rule.Path;
        dr[2] = rule.AlloRead == true ? Resources.Resource.Yes : Resources.Resource.No;
        dr[3] = rule.Subdirs == true ? Resources.Resource.Yes : Resources.Resource.No;
        dr[4] = rule.Log == true ? Resources.Resource.Yes : Resources.Resource.No;
        dt.Rows.Add(dr);
    }

    private void CreateColumnsFileSystem(DataTable dt)
    {
        dt.Columns.Add(new DataColumn("EventARM", typeof(String)));
        dt.Columns.Add(new DataColumn("Path", typeof(String)));
        dt.Columns.Add(new DataColumn("AllowRead", typeof(String)));
        dt.Columns.Add(new DataColumn("Subdirs", typeof(String)));
        dt.Columns.Add(new DataColumn("Log", typeof(String)));
    }

    #endregion

    #endregion

    #region Profiles

    protected void lbtnRemove_Click(object sender, EventArgs e)
    {
        TaskUserCollection collection =
            new TaskUserCollection(Profile.TasksList);

        collection.Delete(String.Format("Proactive Protection: {0}", ddlProfiles.SelectedValue));
        ddlProfiles.Items.RemoveAt(ddlProfiles.SelectedIndex);
        if (ddlProfiles.Items.Count == 0)
        {
            lbtnRemove.Visible = lbtnEdit.Visible = false;
        }
        collection = collection.Deserialize();

        Profile.TasksList = collection.Serialize();
        Session["TaskUser"] = collection;
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
        EditClick(sender, e, collection.Get(String.Format("Proactive Protection: {0}", ddlProfiles.SelectedValue)));
    }

    public void EditClick(object sender, EventArgs e, TaskUserEntity task)
    {
        string editing = "&Mode=Edit";
        string type = task.Type.ToString();
        Session["CurrentUserTask"] = task;
        Response.Redirect("TaskCreate.aspx?Type=" + type + editing);
    }

    protected void lbtnCreateProfile_Click(object sender, EventArgs e)
    {
        ClearRule();
        TaskUserEntity task = GetCurrentState();
        task.Name = String.Empty;

        string editing = "&Mode=Edit";
        string type = task.Type.ToString();
        Session["CurrentUserTask"] = task;
        Response.Redirect("TaskCreate.aspx?Type=" + type + editing);
    }

    #endregion

    private static List<RuleProactiveProtection> Rules;
    private static bool isEditingRegistry = false;
    private static bool isEditingFileSystem = false;
    private static int indexItem;
}