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
using ARM2_dbcontrol.Service.Vba32NS;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Controls_TaskConfigureScheduler : System.Web.UI.UserControl, ITask
{
    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private bool _enabled = true;

    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    private static List<SchedulerRule> Rules;
    

    #region ITask Members

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

        if (Rules == null)
        {
            Rules = new List<SchedulerRule>();
        }

        lbtnAdd.Text = Resources.Resource.Add;
        btnAddRule.Text = Resources.Resource.Next;
        btnPrev.Text = Resources.Resource.Prev;
        btnCancelRule.Text = Resources.Resource.CancelButtonText;
        rbtnProcess.Text = Resources.Resource.ActionProcess;
        rbtnScan.Text = Resources.Resource.ActionScan;
        rbtnUpdate.Text = Resources.Resource.ActionUpdate;

        RadioButton1.Text = Resources.Resource.PeriodicityMinutes;
        RadioButton2.Text = Resources.Resource.PeriodicityHours;
        RadioButton3.Text = Resources.Resource.PeriodicityDays;
        RadioButton4.Text = Resources.Resource.PeriodicityWeeks;
        RadioButton5.Text = Resources.Resource.PeriodicityMonths;
        RadioButton6.Text = Resources.Resource.PeriodicityFixedDate;

        ddlScanMode.Items.Add(Resources.Resource.FastMode);
        ddlScanMode.Items.Add(Resources.Resource.SafeMode);
        ddlScanMode.Items.Add(Resources.Resource.ExcessMode);
        ddlScanMode.Items[0].Selected = true;

        cboxFilesDefault1.Text = Resources.Resource.Including;
        cboxFilesDefault2.Text = Resources.Resource.Excluding;

        rbtnFiles1.Text = Resources.Resource.All;
        rbtnFiles2.Text = Resources.Resource.DefaultFiles;
        rbtnFiles3.Text = Resources.Resource.Custom;

        cboxObject.Items.Add(Resources.Resource.ThoroughSearch);
        cboxObject.Items.Add(Resources.Resource.Ware);
        cboxObject.Items.Add(Resources.Resource.Memory);
        cboxObject.Items.Add(Resources.Resource.ScanBoot);
        cboxObject.Items.Add(Resources.Resource.ScanStartup);
        cboxObject.Items.Add(Resources.Resource.DetectInstallers);
        cboxObject.Items.Add(Resources.Resource.ScanMail);
        cboxCheckArchive.Text = Resources.Resource.ScanArchives;
        cboxMaxSizeArchive.Text = Resources.Resource.MaxArchiveSize;

        rbtnDays1.Text = Resources.Resource.RunAt;
        rbtnWeek1.Text = Resources.Resource.RunAt;
        rbtnMonth1.Text = Resources.Resource.RunAt;
        rbtnMonth2.Text = Resources.Resource.RunAnytime;
        rbtnWeek2.Text = Resources.Resource.RunAnytime;
        rbtnDays2.Text = Resources.Resource.RunAnytime;
        cboxMonth.Text = Resources.Resource.RunMissedAction;
        cboxFixedDate.Text = Resources.Resource.RunMissedAction;
        cboxWeek.Text = Resources.Resource.RunMissedAction;
        cboxDays.Text = Resources.Resource.RunMissedAction;

        cbokWeekMon.Text = Resources.Resource.Monday;
        cbokWeekTues.Text = Resources.Resource.Tuesday;
        cbokWeekWednes.Text = Resources.Resource.Wednesday;
        cboxWeekTh.Text = Resources.Resource.Thursday;
        cbokWeekFri.Text = Resources.Resource.Friday;
        cbokWeekSat.Text = Resources.Resource.Saturday;
        cbokWeekSun.Text = Resources.Resource.Sunday;

        cboxInfected.Items.Add(Resources.Resource.Cure);
        cboxInfected.Items.Add(Resources.Resource.Delete);
        cboxInfected.Items.Add(Resources.Resource.SaveToQtn);
        cboxInfected.Items.Add(Resources.Resource.DeleteArchive);
        cboxInfected.Items.Add(Resources.Resource.DeleteMail);

        cboxSuspectedToQtn.Text = Resources.Resource.SaveToQtn;
        cboxSuspectedDelete.Text = Resources.Resource.Delete;


        ddlExpertAnaliz.Items.Add(Resources.Resource.Disabled);
        ddlExpertAnaliz.Items.Add(Resources.Resource.Optimal);
        ddlExpertAnaliz.Items.Add(Resources.Resource.Maximum);
        ddlExpertAnaliz.Items.Add(Resources.Resource.Excessive);

        cboxSaveReport.Text = Resources.Resource.ReportIntoFile;
        cboxAddReport.Text = Resources.Resource.ReportAddToFile;
        cboxIncludeNameInReport.Text = Resources.Resource.CleanFilesToReport;
        cboxSaveList.Text = Resources.Resource.ListInfected;
        cboxAddList.Text = Resources.Resource.ListInfectedAdd;

        cboxAdditional1.Items.Add(Resources.Resource.EnableCache);
        cboxAdditional1.Items.Add(Resources.Resource.InterruptProgram);
        cboxAdditional1.Items[1].Selected = true;
        cboxAdditional2.Text = Resources.Resource.Bases;
        cboxAdditional3.Items.Add(Resources.Resource.Sound);
        cboxAdditional3.Items.Add(Resources.Resource.HideScaning);

        ClearFields();

        TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
        ddlProfiles.Items.Clear();
        foreach (TaskUserEntity tsk in collection)
        {
            if (tsk.Type == TaskType.ConfigureSheduler)
            {
                try
                {
                    ddlProfiles.Items.Add(tsk.Name.Substring(11));//???????????
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

        UpdateData();
    }

    public bool ValidateFields()
    {
        return true;
    }

    private bool ValidateRule()
    {                
        if (String.IsNullOrEmpty(tboxTaskName.Text)) return false;
        
        int index;
        try
        {
            index = Int32.Parse(hdnEditIndex.Value);
        }
        catch { index = -1; }

        for (int i = 0; i < Rules.Count; i++)
        {
            if (i != index)
                if (Rules[i].ActionName == tboxTaskName.Text) return false;
        }

        if (rbtnProcess.Checked)
        {
            if (String.IsNullOrEmpty(tboxPath.Text)) return false;
        }

        if (RadioButton4.Checked && !cbokWeekMon.Checked && !cbokWeekTues.Checked && !cbokWeekWednes.Checked &&
                                    !cboxWeekTh.Checked && !cbokWeekFri.Checked && !cbokWeekSat.Checked && !cbokWeekSun.Checked)
            return false;


        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureSheduler;
        task.Param = BuildXml();
        return task;
    }

    /// <summary>
    /// Метод конвертации данных в xml строку
    /// </summary>
    /// <returns>xml строка</returns>
    private string BuildXml()
    {
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("ConfigureSheduler");
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ConfigureSheduler");
        xml.AddNode("Content", Server.HtmlEncode(ObjectSerializer.ObjToXmlStr(Rules).Remove(0, 39)));
        xml.Generate();

        return xml.Result;
    }

    public string BuildTask(string xmlParam)
    {
        XmlTaskParser pars = new XmlTaskParser(xmlParam);        
        List < SchedulerRule > lstRules = ObjectSerializer.XmlStrToObj<List<SchedulerRule>>(Server.HtmlDecode(pars.GetValue("Content")));

        StringBuilder result = new StringBuilder();

        foreach (SchedulerRule rule in lstRules)
        {
            if (rule.Enabled)
            {
                result.AppendFormat(@"<VbaSchedulerAddTask><TaskName>{0}</TaskName><Action><ActionType>{1}</ActionType>", rule.ActionName, rule.ActionType.ToString());
                #region Action
                switch (rule.ActionType)
                {
                    case ActionTypeEnum.Process:
                        result.AppendFormat(@"<Path>{0}</Path><Params>{1}</Params>", rule.Path, rule.Parameters);
                        break;
                    case ActionTypeEnum.Scan:
                        //Object
                        result.AppendFormat(@"<Object><ScanMode>{0}</ScanMode><Pathes>", rule.Object_ScanMode.ToString());
                        foreach (string str in rule.Object_Pathes)
                        {
                            result.AppendFormat(@"<Path>{0}</Path>", str);
                        }
                        result.AppendFormat(@"</Pathes><ScanType>{0}</ScanType><Types>", rule.Object_ScanType.ToString());
                        switch (rule.Object_ScanType)
                        {
                            case ScanType.Custom:
                                result.AppendFormat(@"<Custom>{0}</Custom>", rule.Object_Custom);
                                break;
                            case ScanType.Default:
                                result.AppendFormat(@"<Default><Including>{0}</Including><Excluding>{1}</Excluding></Default>", rule.Object_Default_Including, rule.Object_Default_Excluding);
                                break;
                        }
                        result.Append(@"</Types>");
                        if (rule.Object_Thorough) result.Append(@"<Thorough/>");
                        if (rule.Object_Ware) result.Append(@"<Ware/>");
                        if (rule.Object_Memory) result.Append(@"<Memory/>");
                        if (rule.Object_BootSectors) result.Append(@"<BootSectors/>");
                        if (rule.Object_AtStartup) result.Append(@"<AtStartup/>");
                        if (rule.Object_Installers) result.Append(@"<Installers/>");
                        if (rule.Object_Mail) result.Append(@"<Mail/>");
                        if (rule.Object_Archives)
                        {
                            result.Append(@"<Archives>");
                            if (rule.Object_MaxSize != -1)
                            {
                                result.AppendFormat(@"<MaxSize>{0}</MaxSize>", rule.Object_MaxSize);
                            }
                            result.Append(@"</Archives>");
                        }
                        result.Append(@"</Object>");

                        //Action
                        result.Append(@"<Actions><Infected>");
                        if (rule.Actions_Infected_Cure) result.Append(@"<Cure/>");
                        if (rule.Actions_Infected_Delete) result.Append(@"<Delete/>");
                        if (rule.Actions_Infected_ToQtn) result.Append(@"<ToQtn/>");
                        if (rule.Actions_Infected_DeleteArchive) result.Append(@"<DeleteArchive/>");
                        if (rule.Actions_Infected_DeleteMail) result.Append(@"<DeleteMail/>");
                        result.Append(@"</Infected><Suspicious>");
                        if (rule.Actions_Suspicious_ToQtn) result.Append(@"<ToQtn/>");
                        if (rule.Actions_Suspicious_Delete) result.Append(@"<Delete/>");
                        result.AppendFormat(@"</Suspicious><Heuristic>{0}</Heuristic></Actions>", rule.Actions_Heuristic.ToString());

                        //Report
                        result.Append(@"<Report><Report>");
                        if (rule.Report_Report_InfoFile)
                        {
                            result.AppendFormat(@"<IntoFile/><Path>{0}</Path>", rule.Report_Report_Path);
                            if (rule.Report_Report_AddToFile) result.Append(@"<AddToFile/>");
                            if (rule.Report_Report_CleanToFile) result.Append(@"<CleanToFile/>");
                        }
                        result.Append(@"</Report><InfectedList>");
                        if (rule.Report_InfectedList_IntoFile)
                        {
                            result.AppendFormat(@"<IntoFile/><Path>{0}</Path>", rule.Report_InfectedList_Path);
                            if (rule.Report_InfectedList_AddToFile) result.Append(@"<AddToFile/>");
                        }
                        result.Append(@"</InfectedList></Report>");

                        //Additional
                        result.Append(@"<Additional>");
                        if (rule.Additional_Cache) result.Append(@"<Cache/>");
                        if (rule.Additional_Interrupt) result.Append(@"<Interrupt/>");
                        if (rule.Additional_Bases) result.AppendFormat(@"<Bases/><Path>{0}</Path>", rule.Additional_Path);
                        if (rule.Additional_Sound) result.Append(@"<Sound/>");
                        if (rule.Additional_HideWindow) result.Append(@"<HideWindow/>");
                        result.Append(@"</Additional>");
                        break;
                }
                #endregion
                result.Append(@"</Action>");

                #region Periodicity
                result.AppendFormat(@"<Periodicity><PeriodicityType>{0}</PeriodicityType>", rule.Periodicity.ToString());
                switch (rule.Periodicity)
                {
                    case PeriodicityEnum.Minutes:
                        result.AppendFormat(@"<Minutes>{0}</Minutes>", rule.RunEvery);
                        break;
                    case PeriodicityEnum.Hours:
                        result.AppendFormat(@"<Hours>{0}</Hours>", rule.RunEvery);
                        break;
                    case PeriodicityEnum.Days:
                        result.AppendFormat(@"<Days><RunEvery>{0}</RunEvery><RunAt>{1:d2}:{2:d2}:{3:d2}</RunAt>", rule.RunEvery, rule.RunAt.Hour, rule.RunAt.Minute, rule.RunAt.Second);
                        if (rule.IsRunMissedTask) result.Append(@"<RunMissedAction/>");
                        result.Append(@"</Days>");
                        break;
                    case PeriodicityEnum.Weeks:
                        result.Append(@"<Weeks>");
                        if (rule.ArrDaysRun[0]) result.Append(@"<Monday/>");
                        if (rule.ArrDaysRun[1]) result.Append(@"<Tuesday/>");
                        if (rule.ArrDaysRun[2]) result.Append(@"<Wednesday/>");
                        if (rule.ArrDaysRun[3]) result.Append(@"<Thursday/>");
                        if (rule.ArrDaysRun[4]) result.Append(@"<Friday/>");
                        if (rule.ArrDaysRun[5]) result.Append(@"<Saturday/>");
                        if (rule.ArrDaysRun[6]) result.Append(@"<Sunday/>");
                        result.AppendFormat(@"<RunAt>{0:d2}:{1:d2}:{2:d2}</RunAt>", rule.RunAt.Hour, rule.RunAt.Minute, rule.RunAt.Second);
                        if (rule.IsRunMissedTask) result.Append(@"<RunMissedAction/>");
                        result.Append(@"</Weeks>");
                        break;
                    case PeriodicityEnum.Months:
                        result.AppendFormat(@"<Months><Days>{0}</Days><RunAt>{1:d2}:{2:d2}:{3:d2}</RunAt>", rule.NumberDaysMonth, rule.RunAt.Hour, rule.RunAt.Minute, rule.RunAt.Second);
                        if (rule.IsRunMissedTask) result.Append(@"<RunMissedAction/>");
                        result.Append(@"</Months>");
                        break;
                    case PeriodicityEnum.FixedDate:
                        result.AppendFormat(@"<FixedDate><Date>{0:d2}\{1:d2}\{2:d4} {3:d2}:{4:d2}:{5:d2}</Date>", rule.FixedDate.Day, rule.FixedDate.Month, rule.FixedDate.Year, rule.RunAt.Hour, rule.RunAt.Minute, rule.RunAt.Second);
                        if (rule.IsRunMissedTask) result.Append(@"<RunMissedAction/>");
                        result.Append(@"</FixedDate>");
                        break;
                }
                result.Append(@"</Periodicity></VbaSchedulerAddTask>");
                #endregion
            }
        }

        return String.Format(@"<SendDataToMailSlot><MailSlotName>{0}</MailSlotName><MailSlotData><![CDATA[{1}]]></MailSlotData></SendDataToMailSlot>", @"\\.\mailslot\Vba32\Scheduler\Events", result.ToString());
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.ConfigureSheduler)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser pars = new XmlTaskParser(task.Param);
        Rules = ObjectSerializer.XmlStrToObj<List<SchedulerRule>>(Server.HtmlDecode(pars.GetValue("Content")));        
        UpdateData();
    }

    #endregion

    private void UpdateData()
    {
        DataTable dt = new DataTable();
        CreateColumnsRegistry(dt);
        
        foreach (SchedulerRule rule in Rules)
        {
            AddNewItem(dt, rule);
        }
        
        dlTasks.DataSource = new DataView(dt);
        dlTasks.DataBind();
    }

    private void CreateColumnsRegistry(DataTable dt)
    {
        dt.Columns.Add(new DataColumn("ActionName", typeof(String)));
        dt.Columns.Add(new DataColumn("ActionType", typeof(String)));
        dt.Columns.Add(new DataColumn("PeriodicityType", typeof(String)));        
        dt.Columns.Add(new DataColumn("EnableDisable", typeof(CheckBox)));
    }

    private void AddNewItem(DataTable dt, SchedulerRule rule)
    {
        DataRow dr = dt.NewRow();
        dr[0] = rule.ActionName;
        dr[1] = rule.ActionType.ToString();
        dr[2] = rule.Periodicity.ToString();
        CheckBox cbox = new CheckBox();
        dr[3] = cbox;
        dt.Rows.Add(dr);
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        if (!ValidateRule()) return;
        int index;
        try
        {
            index = Int32.Parse(hdnEditIndex.Value);
        }
        catch { return; }
        if (index == -1)
        {
            Rules.Add(GetRule());            
        }
        else
            Rules[index] = GetRule();

        ClearFields();
        btnAddRule.Text = Resources.Resource.Next;
        UpdateData();
    }

    protected void btnCancelRule_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderRule.Hide();

        ClearFields();
    }

    private void ClearFields()
    {
        hdnStep.Value = "0";
        hdnEditIndex.Value = "-1";        

        //Main
        rbtnScan.Checked = rbtnUpdate.Checked = false;
        rbtnProcess.Checked = true;
        tboxTaskName.Text = "";
        //Process
        tboxPath.Text = tboxParameters.Text = "";

        //Scan
        ddlScanMode.SelectedIndex = 0;
        ddlScanPath.Items.Clear();

        cboxMaxSizeArchive.InputAttributes.Add("disabled", "true");
        cboxCheckArchive.Checked = false;
        tboxMaxSizeArchive.Enabled = false;

        cboxFilesDefault1.InputAttributes.Add("disabled", "true");
        cboxFilesDefault2.InputAttributes.Add("disabled", "true");
        tboxFilesDefault1.Enabled = tboxFilesDefault2.Enabled = false;
        tboxFilesDefault2.Text = tboxFilesDefault1.Text = "";

        tboxFilesCustom.Text = "";
        foreach (ListItem item in cboxObject.Items)
        {
            item.Selected = false;
        }

        rbtnFiles3.Checked = false;
        rbtnFiles2.Checked = false;
        rbtnFiles1.Checked = true;

        ddlExpertAnaliz.SelectedIndex = 0;
        foreach (ListItem item in cboxInfected.Items)
        {
            item.Selected = false;
        }

        cboxSuspectedToQtn.Checked = false;
        cboxSuspectedDelete.Checked = false;
        cboxSuspectedToQtn.InputAttributes.Add("disabled", "true");
        cboxSuspectedDelete.InputAttributes.Add("disabled", "true");


        
        cboxSaveReport.Checked = true;
        tboxSaveReport.Enabled = true;
        tboxSaveReport.Text = "";
        cboxAddReport.Checked = cboxIncludeNameInReport.Checked = false;
        cboxAddReport.Enabled = cboxIncludeNameInReport.Enabled = true;

        cboxSaveList.Checked = true;
        tboxSaveList.Enabled = cboxAddList.Enabled = true;
        tboxSaveList.Text = "";
        cboxAddList.Checked = false;

        cboxAdditional2.Checked = false;
        tboxAdditional2.Text = "";
        tboxAdditional2.Enabled = false;

        //Periodicity
        RadioButton2.Checked = RadioButton3.Checked = RadioButton4.Checked = RadioButton5.Checked = RadioButton6.Checked = false;
        RadioButton1.Checked = true;

        tboxMinutes.Text = "30";

        tboxHours.Text = "1";

        tboxDays.Text = "1";
        rbtnDays2.Checked = false;
        rbtnDays1.Checked = true;
        tboxDaysTime.Enabled = true;
        tboxDaysTime.Text = String.Format("{0:d2}:{1:d2}:{2:d2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        cboxDays.Checked = true;

        cboxWeekTh.Checked = cbokWeekFri.Checked = cbokWeekMon.Checked = cbokWeekSat.Checked = cbokWeekSun.Checked = cbokWeekTues.Checked = cbokWeekWednes.Checked = true;
        rbtnWeek2.Checked = false;
        rbtnWeek1.Checked = true;
        tboxWeekTime.Enabled = true;
        tboxWeekTime.Text = String.Format("{0:d2}:{1:d2}:{2:d2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        cboxWeek.Checked = true;

        tboxMonths.Text = "1";
        rbtnMonth2.Checked = false;
        rbtnMonth1.Checked = true;
        tboxMonthTime.Enabled = true;
        tboxMonthTime.Text = String.Format("{0:d2}:{1:d2}:{2:d2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        cboxMonth.Checked = true;

        tboxDate.Text = String.Format("{0:d2}/{1:d2}/{2:d4}", DateTime.Today.Month, DateTime.Today.Day, DateTime.Today.Year);
        tboxFixedDateTime.Text = String.Format("{0:d2}:{1:d2}:{2:d2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        cboxFixedDate.Checked = true;
        
    }

    protected void CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = ((CheckBox)sender);
        DataListItem item = ((DataListItem)chk.NamingContainer);

        SchedulerRule rule = Rules[item.ItemIndex];
        rule.Enabled = chk.Checked;
        Rules[item.ItemIndex] = rule;

        ClearFields();
    }

    private SchedulerRule GetRule()
    {
        SchedulerRule rule = new SchedulerRule();

        rule.ActionName = tboxTaskName.Text;
        if (rbtnProcess.Checked) rule.ActionType = ActionTypeEnum.Process;
        else if (rbtnScan.Checked) rule.ActionType = ActionTypeEnum.Scan;
        else rule.ActionType = ActionTypeEnum.Update;

        rule.Enabled = true;

        #region Action Type
        switch (rule.ActionType)
        {
            case ActionTypeEnum.Process:
                rule.Path = tboxPath.Text;
                rule.Parameters = tboxParameters.Text;
                break;
            case ActionTypeEnum.Scan:
                //Object
                rule.Object_ScanMode = (ScanMode)ddlScanMode.SelectedIndex;

                rule.Object_Pathes = new System.Collections.Generic.List<string>();

                foreach (string str in hdnPath.Value.Split(';'))
                {
                    if (str != String.Empty && str != null)
                        rule.Object_Pathes.Add(str);
                }

                rule.Object_Thorough = cboxObject.Items[0].Selected;
                rule.Object_Ware = cboxObject.Items[1].Selected;
                rule.Object_Memory = cboxObject.Items[2].Selected;
                rule.Object_BootSectors = cboxObject.Items[3].Selected;
                rule.Object_AtStartup = cboxObject.Items[4].Selected;
                rule.Object_Installers = cboxObject.Items[5].Selected;
                rule.Object_Mail = cboxObject.Items[6].Selected;
                rule.Object_Archives = cboxCheckArchive.Checked;
                if (cboxMaxSizeArchive.Checked)
                {
                    try
                    {
                        rule.Object_MaxSize = Int32.Parse(tboxMaxSizeArchive.Text);
                    }
                    catch { rule.Object_MaxSize = 0; }
                }
                else { rule.Object_MaxSize = -1; }

                if (rbtnFiles1.Checked) rule.Object_ScanType = ScanType.All;
                else if (rbtnFiles2.Checked) rule.Object_ScanType = ScanType.Default;
                else if (rbtnFiles3.Checked) rule.Object_ScanType = ScanType.Custom;
                switch (rule.Object_ScanType)
                {
                    case ScanType.Custom:
                        rule.Object_Custom = tboxFilesCustom.Text;
                        break;
                    case ScanType.Default:
                        if (cboxFilesDefault1.Checked) rule.Object_Default_Excluding = tboxFilesDefault1.Text;
                        else rule.Object_Default_Excluding = String.Empty;
                        if (cboxFilesDefault2.Checked) rule.Object_Default_Including = tboxFilesDefault2.Text;
                        else rule.Object_Default_Including = String.Empty;
                        break;
                }

                //Action
                rule.Actions_Heuristic = (Heuristic)ddlExpertAnaliz.SelectedIndex;
                rule.Actions_Infected_Cure = cboxInfected.Items[0].Selected;
                rule.Actions_Infected_Delete = cboxInfected.Items[1].Selected;
                rule.Actions_Infected_ToQtn = cboxInfected.Items[2].Selected;
                rule.Actions_Infected_DeleteArchive = cboxInfected.Items[3].Selected;
                rule.Actions_Infected_DeleteMail = cboxInfected.Items[4].Selected;
                rule.Actions_Suspicious_ToQtn = cboxSuspectedToQtn.Checked;
                rule.Actions_Suspicious_Delete = cboxSuspectedDelete.Checked;

                //Report
                rule.Report_Report_InfoFile = cboxSaveReport.Checked;
                rule.Report_Report_Path = tboxSaveReport.Text;
                rule.Report_Report_AddToFile = cboxAddReport.Checked;
                rule.Report_Report_CleanToFile = cboxIncludeNameInReport.Checked;
                rule.Report_InfectedList_IntoFile = cboxSaveList.Checked;
                rule.Report_InfectedList_Path = tboxSaveList.Text;
                rule.Report_InfectedList_AddToFile = cboxAddList.Checked;

                //Additional
                rule.Additional_Cache = cboxAdditional1.Items[0].Selected;
                rule.Additional_Interrupt = cboxAdditional1.Items[1].Selected;
                rule.Additional_Bases = cboxAdditional2.Checked;
                rule.Additional_Path = tboxAdditional2.Text;
                rule.Additional_Sound = cboxAdditional3.Items[0].Selected;
                rule.Additional_HideWindow = cboxAdditional3.Items[1].Selected;


                break;
        }
        #endregion

        #region Periodicity
        if (RadioButton1.Checked) rule.Periodicity = PeriodicityEnum.Minutes;
        else if (RadioButton2.Checked) rule.Periodicity = PeriodicityEnum.Hours;
        else if (RadioButton3.Checked) rule.Periodicity = PeriodicityEnum.Days;
        else if (RadioButton4.Checked) rule.Periodicity = PeriodicityEnum.Weeks;
        else if (RadioButton5.Checked) rule.Periodicity = PeriodicityEnum.Months;
        else if (RadioButton6.Checked) rule.Periodicity = PeriodicityEnum.FixedDate;

        switch (rule.Periodicity)
        {
            case PeriodicityEnum.Minutes:

                try
                {
                    rule.RunEvery = Int32.Parse(tboxMinutes.Text);
                }
                catch { rule.RunEvery = 30; }

                break;
            case PeriodicityEnum.Hours:

                try
                {
                    rule.RunEvery = Int32.Parse(tboxHours.Text);
                }
                catch { rule.RunEvery = 1; }

                break;
            case PeriodicityEnum.Days:

                try
                {
                    rule.RunEvery = Int32.Parse(tboxDays.Text);
                }
                catch { rule.RunEvery = 1; }
                if (rbtnDays1.Checked)
                {
                    rule.IsRunAt = true;
                    try
                    {
                        rule.RunAt = DateTime.Parse(tboxDaysTime.Text);
                    }
                    catch { rule.RunAt = DateTime.Now; }
                }
                else rule.IsRunAt = false;
                rule.IsRunMissedTask = cboxDays.Checked;

                break;
            case PeriodicityEnum.Weeks:

                if (rbtnWeek1.Checked)
                {
                    rule.IsRunAt = true;
                    try
                    {
                        rule.RunAt = DateTime.Parse(tboxWeekTime.Text);
                    }
                    catch { rule.RunAt = DateTime.Now; }
                }
                else rule.IsRunAt = false;
                rule.IsRunMissedTask = cboxWeek.Checked;
                bool[] arr = new bool[] { cbokWeekMon.Checked, cbokWeekTues.Checked, cbokWeekWednes.Checked, cboxWeekTh.Checked, cbokWeekFri.Checked, cbokWeekSat.Checked, cbokWeekSun.Checked };
                rule.ArrDaysRun = arr;

                break;
            case PeriodicityEnum.Months:

                if (rbtnMonth1.Checked)
                {
                    rule.IsRunAt = true;
                    try
                    {
                        rule.RunAt = DateTime.Parse(tboxMonthTime.Text);
                    }
                    catch { rule.RunAt = DateTime.Now; }
                }
                else rule.IsRunAt = false;
                rule.IsRunMissedTask = cboxMonth.Checked;
                rule.NumberDaysMonth = tboxMonths.Text;

                break;
            case PeriodicityEnum.FixedDate:

                try
                {
                    rule.RunAt = DateTime.Parse(tboxFixedDateTime.Text);
                }
                catch { rule.RunAt = DateTime.Now; }

                rule.IsRunMissedTask = cboxFixedDate.Checked;
                try
                {
                    rule.FixedDate = DateTime.Parse(tboxDate.Text);
                }
                catch { rule.FixedDate = DateTime.Now; }

                break;
        }
        #endregion

        return rule;
    }

    private void SetRule(SchedulerRule rule)
    {
        tboxTaskName.Text = rule.ActionName;
        switch (rule.ActionType)
        {
            case ActionTypeEnum.Process:
                rbtnScan.Checked = false;
                rbtnUpdate.Checked = false;
                rbtnProcess.Checked = true;
                break;
            case ActionTypeEnum.Scan:
                rbtnUpdate.Checked = false;
                rbtnProcess.Checked = false;
                rbtnScan.Checked = true;
                break;
            case ActionTypeEnum.Update:
                rbtnProcess.Checked = false;
                rbtnScan.Checked = false;
                rbtnUpdate.Checked = true;
                break;
        }

        #region Action Type
        switch (rule.ActionType)
        {
            case ActionTypeEnum.Process:
                tboxPath.Text = rule.Path;
                tboxParameters.Text = rule.Parameters;
                break;
            case ActionTypeEnum.Scan:
                //Object
                ddlScanMode.SelectedIndex = (int)rule.Object_ScanMode;

                hdnPath.Value = "";
                ddlScanPath.Items.Clear();
                foreach (string str in rule.Object_Pathes)
                {
                    hdnPath.Value += str + ";";
                    ddlScanPath.Items.Add(str);
                }

                cboxObject.Items[0].Selected = rule.Object_Thorough;
                cboxObject.Items[1].Selected = rule.Object_Ware;
                cboxObject.Items[2].Selected = rule.Object_Memory;
                cboxObject.Items[3].Selected = rule.Object_BootSectors;
                cboxObject.Items[4].Selected = rule.Object_AtStartup;
                cboxObject.Items[5].Selected = rule.Object_Installers;
                cboxObject.Items[6].Selected = rule.Object_Mail;

                cboxCheckArchive.Checked = rule.Object_Archives;
                cboxMaxSizeArchive.Checked = !(rule.Object_MaxSize == -1);
                tboxMaxSizeArchive.Text = rule.Object_MaxSize == -1 ? String.Empty : String.Format("{0}", rule.Object_MaxSize);
                if (cboxCheckArchive.Checked) cboxMaxSizeArchive.Enabled = true; //cboxMaxSizeArchive.InputAttributes.Add("disabled", "false");
                else
                {
                    cboxMaxSizeArchive.InputAttributes.Add("disabled", "true");
                    tboxMaxSizeArchive.Enabled = false;
                }
                if (cboxMaxSizeArchive.Checked) tboxMaxSizeArchive.Enabled = true;

                switch (rule.Object_ScanType)
                {
                    case ScanType.All:
                        rbtnFiles2.Checked = false;
                        rbtnFiles3.Checked = false;
                        rbtnFiles1.Checked = true;
                        break;
                    case ScanType.Default:
                        rbtnFiles1.Checked = false;
                        rbtnFiles3.Checked = false;
                        rbtnFiles2.Checked = true;
                        cboxFilesDefault1.Checked = !String.IsNullOrEmpty(rule.Object_Default_Excluding);
                        tboxFilesDefault1.Text = rule.Object_Default_Excluding;
                        cboxFilesDefault2.Checked = !String.IsNullOrEmpty(rule.Object_Default_Including);
                        tboxFilesDefault2.Text = rule.Object_Default_Including;
                        break;
                    case ScanType.Custom:
                        rbtnFiles2.Checked = false;
                        rbtnFiles1.Checked = false;
                        rbtnFiles3.Checked = true;
                        tboxFilesCustom.Text = rule.Object_Custom;
                        break;
                }

                if (rule.Object_ScanType == ScanType.Default)
                {
                    cboxFilesDefault1.Enabled = true;
                    cboxFilesDefault2.Enabled = true;
                    if (cboxFilesDefault1.Checked) tboxFilesDefault1.Enabled = true;
                    else tboxFilesDefault1.Enabled = false;
                    if (cboxFilesDefault2.Checked) tboxFilesDefault2.Enabled = true;
                    else tboxFilesDefault2.Enabled = false;
                }
                else
                {
                    cboxFilesDefault1.InputAttributes.Add("disabled", "true");
                    cboxFilesDefault2.InputAttributes.Add("disabled", "true");
                    tboxFilesDefault1.Enabled = false;
                    tboxFilesDefault2.Enabled = false;
                }
               
                //Action
                ddlExpertAnaliz.SelectedIndex = (int)rule.Actions_Heuristic;
                cboxInfected.Items[0].Selected = rule.Actions_Infected_Cure;
                cboxInfected.Items[1].Selected = rule.Actions_Infected_Delete;
                cboxInfected.Items[2].Selected = rule.Actions_Infected_ToQtn;
                cboxInfected.Items[3].Selected = rule.Actions_Infected_DeleteArchive;
                cboxInfected.Items[4].Selected = rule.Actions_Infected_DeleteMail;

                
                if (ddlExpertAnaliz.SelectedIndex == 0)
                {
                    cboxSuspectedToQtn.InputAttributes.Add("disabled", "true");
                    cboxSuspectedDelete.InputAttributes.Add("disabled", "true");
                }
                else 
                {
                    cboxSuspectedToQtn.InputAttributes.Add("disabled", "false");
                    cboxSuspectedDelete.InputAttributes.Add("disabled", "false");
                }

                cboxSuspectedToQtn.Checked = rule.Actions_Suspicious_ToQtn;
                cboxSuspectedDelete.Checked = rule.Actions_Suspicious_Delete;


                //Report
                cboxSaveReport.Checked = rule.Report_Report_InfoFile;
                tboxSaveReport.Text = rule.Report_Report_Path;
                cboxAddReport.Checked = rule.Report_Report_AddToFile;
                cboxIncludeNameInReport.Checked = rule.Report_Report_CleanToFile;
                cboxSaveList.Checked = rule.Report_InfectedList_IntoFile;
                tboxSaveList.Text = rule.Report_InfectedList_Path;
                cboxAddList.Checked = rule.Report_InfectedList_AddToFile;

                //Additional
                cboxAdditional1.Items[0].Selected = rule.Additional_Cache;
                cboxAdditional1.Items[1].Selected = rule.Additional_Interrupt;
                cboxAdditional2.Checked = rule.Additional_Bases;
                tboxAdditional2.Text = rule.Additional_Path;
                cboxAdditional3.Items[0].Selected = rule.Additional_Sound;
                cboxAdditional3.Items[1].Selected = rule.Additional_HideWindow;

                break;
        }
        #endregion

        #region Periodicity

        RadioButton1.Checked = false;
        RadioButton2.Checked = false;
        RadioButton3.Checked = false;
        RadioButton4.Checked = false;
        RadioButton5.Checked = false;
        RadioButton6.Checked = false;
        switch (rule.Periodicity)
        {
            case PeriodicityEnum.Minutes:
                RadioButton1.Checked = true;
                break;
            case PeriodicityEnum.Hours:
                RadioButton2.Checked = true;
                break;
            case PeriodicityEnum.Days:
                RadioButton3.Checked = true;
                break;
            case PeriodicityEnum.Weeks:
                RadioButton4.Checked = true;
                break;
            case PeriodicityEnum.Months:
                RadioButton5.Checked = true;
                break;
            case PeriodicityEnum.FixedDate:
                RadioButton6.Checked = true;
                break;
        }

        switch (rule.Periodicity)
        {
            case PeriodicityEnum.Minutes:

                tboxMinutes.Text = String.Format("{0}", rule.RunEvery);
                break;
            case PeriodicityEnum.Hours:

                tboxHours.Text = String.Format("{0}", rule.RunEvery);
                break;
            case PeriodicityEnum.Days:

                tboxDays.Text = String.Format("{0}", rule.RunEvery);
                if (rule.IsRunAt)
                {
                    rbtnDays2.Checked = false;
                    rbtnDays1.Checked = true;
                    tboxDaysTime.Text = String.Format("{0}:{1}:{2}", rule.RunAt.Hour, rule.RunAt.Minute, rule.RunAt.Second);
                }
                else
                {
                    rbtnDays1.Checked = false;
                    rbtnDays2.Checked = true;
                }
                cboxDays.Checked = rule.IsRunMissedTask;

                break;
            case PeriodicityEnum.Weeks:

                cbokWeekMon.Checked = rule.ArrDaysRun[0];
                cbokWeekTues.Checked = rule.ArrDaysRun[1];
                cbokWeekWednes.Checked = rule.ArrDaysRun[2];
                cboxWeekTh.Checked = rule.ArrDaysRun[3];
                cbokWeekFri.Checked = rule.ArrDaysRun[4];
                cbokWeekSat.Checked = rule.ArrDaysRun[5];
                cbokWeekSun.Checked = rule.ArrDaysRun[6];

                if (rule.IsRunAt)
                {
                    rbtnWeek2.Checked = false;
                    rbtnWeek1.Checked = true;
                    tboxWeekTime.Text = String.Format("{0}:{1}:{2}", rule.RunAt.Hour, rule.RunAt.Minute, rule.RunAt.Second);
                }
                else
                {
                    rbtnWeek1.Checked = false;
                    rbtnWeek2.Checked = true;
                }
                cboxWeek.Checked = rule.IsRunMissedTask;

                break;
            case PeriodicityEnum.Months:

                if (rule.IsRunAt)
                {
                    rbtnMonth2.Checked = false;
                    rbtnMonth1.Checked = true;
                    tboxMonthTime.Text = String.Format("{0}:{1}:{2}", rule.RunAt.Hour, rule.RunAt.Minute, rule.RunAt.Second);
                }
                else
                {
                    rbtnMonth1.Checked = false;
                    rbtnMonth2.Checked = true;
                }
                cboxMonth.Checked = rule.IsRunMissedTask;
                tboxMonths.Text = rule.NumberDaysMonth;
                break;
            case PeriodicityEnum.FixedDate:

                tboxFixedDateTime.Text = String.Format("{0}:{1}:{2}", rule.RunAt.Hour, rule.RunAt.Minute, rule.RunAt.Second);
                cboxFixedDate.Checked = rule.IsRunMissedTask;
                tboxDate.Text = String.Format("{0}/{1}/{2}", rule.FixedDate.Month, rule.FixedDate.Day, rule.FixedDate.Year);
                break;
        }
        #endregion

        
    }

    protected void dlTasks_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DeleteCommand":
                Rules.RemoveAt(e.Item.ItemIndex);
                UpdateData();
                break;
            case "EditCommand":
                SetRule(Rules[e.Item.ItemIndex]);
                ModalPopupExtenderRule.Show();
                hdnEditIndex.Value = String.Format("{0}", e.Item.ItemIndex);
                break;
        }
    }

    protected void dlTasks_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header)
        {
            (e.Item.FindControl("cbox8") as CheckBox).Checked = Rules[e.Item.ItemIndex].Enabled;
            (e.Item.FindControl("lbtnDelete") as LinkButton).Attributes.Add("onclick", "return confirm('" + Resources.Resource.AreYouSureTask + "');");
        }
    }

    protected void lbtnCreateProfile_Click(object sender, EventArgs e)
    {
        dlTasks.DataBind();

        Rules = new List<SchedulerRule>();
        TaskUserEntity task = GetCurrentState();
        task.Name = String.Empty;

        string editing = "&Mode=Edit";
        string type = task.Type.ToString();
        Session["CurrentUserTask"] = task;
        Response.Redirect("TaskCreate.aspx?Type=" + type + editing);
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
        EditClick(sender, e, collection.Get(String.Format("Scheduler: {0}", ddlProfiles.SelectedValue)));
    }

    protected void lbtnRemove_Click(object sender, EventArgs e)
    {
        TaskUserCollection collection =
            new TaskUserCollection(Profile.TasksList);

        collection.Delete(String.Format("Scheduler: {0}", ddlProfiles.SelectedValue));
        ddlProfiles.Items.RemoveAt(ddlProfiles.SelectedIndex);
        if (ddlProfiles.Items.Count == 0)
        {
            lbtnRemove.Visible = lbtnEdit.Visible = false;
        }
        collection = collection.Deserialize();

        Profile.TasksList = collection.Serialize();
        Session["TaskUser"] = collection;
    }

    public void EditClick(object sender, EventArgs e, TaskUserEntity task)
    {
        string editing = "&Mode=Edit";
        string type = task.Type.ToString();
        Session["CurrentUserTask"] = task;
        Response.Redirect("TaskCreate.aspx?Type=" + type + editing);
    }

    public void ShowProfiles()
    {
        tblProfileMngmt.Visible = true;
        tblProfileOptions.Visible = false;        
    }

    public void ShowProfileDetails(TaskUserEntity task)
    {
        dlTasks.DataBind();
        LoadState(task);

        tblProfileMngmt.Visible = false;
        tblProfileOptions.Visible = true;
    }
}
