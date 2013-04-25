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
using System.Collections.Generic;

/// <summary>
/// Задача настройки Монитора
/// </summary>
public partial class Controls_TaskConfigureMonitor : System.Web.UI.UserControl,ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
            InitFields();

        ChangeEnabledControl();
    }

    private string defaultFilters = "COM.EXE.DLL.DRV.SYS.OV?.VXD.SCR.CPL.OCX.BPL.AX.PIF.DO?.XL?.HLP.RTF.WI?.WZ?.MSI.MSC.HT*.VB*.JS.JSE.ASP*.CGI.PHP*.?HTML.BAT.CMD.EML.NWS.MSG.XML.MSO.WPS.PPT.PUB.JPG.JPEG.ANI.INF.SWF.PDF";

    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private void ChangeEnabledControl()
    {
        Tabs.Enabled = _enabled;        
    }
    
    private bool _enabled = true;

    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

        //if (ddlInfected.Items.Count == 0)
        LoadSource();
        tabPanel1.HeaderText = Resources.Resource.CongMonitorObjects;
        tabPanel2.HeaderText = Resources.Resource.BackgroundScanning;
        tabPanel3.HeaderText = Resources.Resource.Actions;
        tabPanel4.HeaderText = Resources.Resource.CongMonitorReport;
        /*lbtnObjects.Text = Resources.Resource.CongMonitorObjects;
        lbtnBackgroundScanning.Text = Resources.Resource.BackgroundScanning;
        lbtnActions.Text = Resources.Resource.Actions;
        lbtnReport.Text = Resources.Resource.CongMonitorReport;*/
        

        //tboxFilterDefined.Text = defaultFilters;


    }

    public bool ValidateFields()
    {
        Validation vld = new Validation(tboxLogFile.Text);
        //if (cboxKeep.Checked)
       // {
            if (!vld.CheckFileName())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrKeep);

            if (cboxMaximumSizeLog.Checked)
            {
                vld.Value = tboxMaximumSizeLog.Text;
                if (!vld.CheckSize())
                    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.CongLdrMaximumSizeLog);
            }
       // }

        if(cboxMaximumCPUUsege.Checked)
        {
            vld.Value = tboxMaximumCPUUsege.Text;
            if (!vld.CheckPercent())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongMonitorMaximumCPUUsege);
        }

        if (cboxMaximumDiskActivity.Checked)
        {
            vld.Value = tboxMaximumDiskActivity.Text;
            if (!vld.CheckPercent())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongMonitorMaximumDiskActivity);
        }

        if (cboxMaximumDisplacement.Checked)
        {
            vld.Value = tboxMaximumDisplacement.Text;
            if (!vld.CheckSize())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
               + Resources.Resource.CongMonitorMaximumDisplacement);
        }

        if (cboxMinimumBattery.Checked)
        {
            vld.Value = tboxMinimumBattery.Text;
            if (!vld.CheckPercent())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongMonitorMinimumBattery);
        }

        //if ((ddlPrevInfected.SelectedIndex == 0) && (ddlInfected.SelectedIndex == 0))
        //    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": " +
        //        "Block and block action");

        return true;
    }

    private void LoadSource()
    {
        if (ddlInfected.Items.Count != 0) return;
        ddlInfected.Items.Add(Resources.Resource.Block);
        ddlInfected.Items.Add(Resources.Resource.Cure);
        ddlInfected.Items.Add(Resources.Resource.Delete);
        ddlInfected.Items.Add(Resources.Resource.Ask);

        ddlPrevInfected.Items.Add(Resources.Resource.Cure);
        ddlPrevInfected.Items.Add(Resources.Resource.Delete);
        ddlPrevInfected.Items.Add(Resources.Resource.Ask);

        ddlPrevPrevInfected.Items.Add(Resources.Resource.Delete);
        ddlPrevPrevInfected.Items.Add(Resources.Resource.Ask);

        ddlSuspicious.Items.Add(Resources.Resource.Skip);
        ddlSuspicious.Items.Add(Resources.Resource.Block);
        ddlSuspicious.Items.Add(Resources.Resource.Delete);

        ddlPrePrevSuspicious.Items.Add(Resources.Resource.Skip);
        ddlPrePrevSuspicious.Items.Add(Resources.Resource.Delete);

        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Disabled);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Optimal);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Maximum);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Excessive);

        tboxMaximumCPUUsege.Text = "20";
        tboxMaximumDiskActivity.Text = "20";
        tboxMaximumDisplacement.Text = "50";
        tboxMinimumBattery.Text = "90";

        tboxLogFile.Text = "Vba32mNt.log";
        tboxMaximumSizeLog.Text = "256";


    }

    /// <summary>
    /// Build xml
    /// </summary>
    /// <returns></returns>
    private string BuildXml()
    {
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("monitor");
        ARM2_dbcontrol.Generation.XmlBuilder xmlExclusions = new ARM2_dbcontrol.Generation.XmlBuilder("exclusions");
        ARM2_dbcontrol.Generation.XmlBuilder xmlIdlecheck = new ARM2_dbcontrol.Generation.XmlBuilder("idlecheck");
        xml.Top = String.Empty;
        xmlExclusions.Top = String.Empty;
        xmlIdlecheck.Top = String.Empty;

        //Набор файлов
        if (rbScanStandartSet.Checked)
            xml.AddNode("CHECK_MODE", "reg_dword:0");
        if (rbScanSelectedTypes.Checked)
        {
            xml.AddNode("CHECK_MODE", "reg_dword:1");
            
            string filters = tboxFilterDefined.Text;
            //Если пользователь не задал фильтры, используем набор по умолчанию
            if (String.IsNullOrEmpty(filters))
                filters = defaultFilters;

            xml.AddNode("FILTER_DEFINED", "reg_sz:" + filters);
        }
        if (rbMonitorScanAllTypes.Checked)
        {
            xml.AddNode("CHECK_MODE", "reg_dword:2");
            xml.AddNode("FILTER_EXCLUDE", "reg_sz:" + tboxFilterExclude.Text);
        }

        xml.AddNode("FAST_MODE", cboxScanOnlyNew.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("DETECT_RISKWARE", cboxDetectWare.Checked ? "reg_dword:1" : "reg_dword:0");
        
        //фоновая проверка
        xml.AddNode("IDLE_CHECK", cboxScanInBackGround.Checked ? "reg_dword:1" : "reg_dword:0");
        if (cboxScanInBackGround.Checked)
        {
            //cbox
            xml.AddNode("IDLE_PROCESSOR", cboxMaximumCPUUsege.Checked ? "reg_dword:1" : "reg_dword:0");
            xml.AddNode("IDLE_DISK", cboxMaximumDiskActivity.Checked ? "reg_dword:1" : "reg_dword:0");
            xml.AddNode("IDLE_MOUSE", cboxMaximumDisplacement.Checked ? "reg_dword:1" : "reg_dword:0");
            xml.AddNode("IDLE_NOTEBOOK", cboxMinimumBattery.Checked ? "reg_dword:1" : "reg_dword:0");
            //xml.AddNode("IDLE_AUTORUN", cboxScanStartupFiles.Checked ? "reg_dword:1" : "reg_dword:0");
            xml.AddNode("IDLE_USERFILES", cboxScanFilesByUser.Checked ? "reg_dword:1" : "reg_dword:0");

            //tbox
            if(cboxMaximumCPUUsege.Checked)
                xml.AddNode("IDLE_PROCESSOR_VALUE", "reg_dword:" + tboxMaximumCPUUsege.Text);
            if(cboxMaximumDiskActivity.Checked)
                xml.AddNode("IDLE_DISK_VALUE", "reg_dword:" + tboxMaximumDiskActivity.Text);
            if(cboxMaximumDisplacement.Checked)
                xml.AddNode("IDLE_MOUSE_VALUE", "reg_dword:" + tboxMaximumDisplacement.Text);
            if(cboxMinimumBattery.Checked)
                xml.AddNode("IDLE_NOTEBOOK_VALUE", "reg_dword:" + tboxMinimumBattery.Text);

        }
        
    
        xml.AddNode("NOTIFY", cboxNotifyOfMonitor.Checked ? "reg_dword:1" : "reg_dword:0");
        
        //лог-файл
        //xml.AddNode("REPORT", cboxKeep.Checked ? "reg_dword:1" : "reg_dword:0");
        //if (cboxKeep.Checked)
        //{
            xml.AddNode("REPORT_NAME", "reg_sz:" + tboxLogFile.Text);
           // xml.AddNode("ADD_TO_REPORT", cboxAddLog.Checked ? "reg_dword:1" : "reg_dword:0");
           
            xml.AddNode("LIMIT_REPORT", cboxMaximumSizeLog.Checked ? "reg_dword:1" : "reg_dword:0");
            if (cboxMaximumSizeLog.Checked)
            {
                xml.AddNode("LIMIT_REPORT_VALUE", "reg_dword:" + tboxMaximumSizeLog.Text);
            }
       // }

        xml.AddNode("SHOW_OK", cboxInformationAboutCleanFiles.Checked ? "reg_dword:1" : "reg_dword:0");



        // Действия над инфицированными
        xml.AddNode("InfectedAction1", "reg_dword:"+Convert.ToString(ddlInfected.SelectedIndex + 1));
        int index;
        if (ddlInfected.SelectedIndex < 2 && ddlPrevInfected.SelectedIndex == 0)
        {
            if (ddlInfected.SelectedIndex == 0) index = 2;
            else index = 1;
        }
        else index = ddlPrevInfected.SelectedIndex + 2;

        xml.AddNode("InfectedAction2", "reg_dword:" + Convert.ToString(index));
        xml.AddNode("InfectedAction3", "reg_dword:" + Convert.ToString(ddlPrevPrevInfected.SelectedIndex + 3));

     
        xml.AddNode("HEURISTIC", "reg_dword:" + Convert.ToString(ddlHeuristicAnalysis.SelectedIndex));

        if (ddlHeuristicAnalysis.SelectedIndex > 0)
        {
            xml.AddNode("BlockAutorunInf", "reg_dword:"+ (cboxBlockUSB.Checked ? "1" : "0"));
        }
        
        //Действия над подозрительными
        xml.AddNode("SuspiciousAction1", (ddlSuspicious.SelectedIndex == 2) ? "reg_dword:3" : ("reg_dword:" + Convert.ToString(ddlSuspicious.SelectedIndex)));
        xml.AddNode("SuspiciousAction2", (ddlPrePrevSuspicious.SelectedIndex == 0) ? "reg_dword:0" : "reg_dword:3");

        //пути фоновой проверки и список необрабатываемых путей        
        if (ddlExcludingFoldersAndFilesDelete.Items.Count != 0)
        {
            for (int i = 0; i < ddlExcludingFoldersAndFilesDelete.Items.Count; i++)
                xmlExclusions.AddNode((ddlExcludingFoldersAndFilesDelete.Items[i].Text.ToCharArray(0, 1)[0] == '-' ? "N" : "S") + Anchor.GetMd5Hash(ddlExcludingFoldersAndFilesDelete.Items[i].Text.Substring(1)),
                    "reg_sz:" + ddlExcludingFoldersAndFilesDelete.Items[i].Text.Substring(1));
        }
        xmlExclusions.Generate();

        
        if (ddlListOfPathToScan.Items.Count != 0)
        {
            for (int i = 0; i < ddlListOfPathToScan.Items.Count; i++)
                xmlIdlecheck.AddNode((ddlListOfPathToScan.Items[i].Text.ToCharArray(0, 1)[0] == '-' ? "N" : "S") + Anchor.GetMd5Hash(ddlListOfPathToScan.Items[i].Text.Substring(1)),
                    "reg_sz:" + ddlListOfPathToScan.Items[i].Text.Substring(1));                
        }
        xmlIdlecheck.Generate();

        //cbox to dll
        xml.AddNode("InfectedCopy1", cboxInfectedQuarantine.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("InfectedCopy2", cboxInfectedQua.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("InfectedCopy3",  cboxInfectedQuaPrev.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("SuspiciousCopy1",  cboxSuspiciousQua.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("SuspiciousCopy2",  cboxSuspiciousQuaPrev.Checked ? "reg_dword:1" : "reg_dword:0");

        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ConfigureMonitor");

        xml.Generate();

        return xml.Result + xmlExclusions.Result + xmlIdlecheck.Result;

    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureMonitor)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        ARM2_dbcontrol.Generation.XmlBuilder builder = new ARM2_dbcontrol.Generation.XmlBuilder();
        XmlTaskParser pars = new XmlTaskParser("<TaskMonitor>" + task.Param + "</TaskMonitor>");
                
        //cbox
        //cboxAddLog.Checked = pars.GetValue("ADD_TO_REPORT","reg_dword:") == "1" ? true : false;
        cboxScanOnlyNew.Checked = pars.GetValue("FAST_MODE", "reg_dword:") == "1" ? true : false;
        cboxDetectWare.Checked = pars.GetValue("DETECT_RISKWARE", "reg_dword:") == "1" ? true : false;

        cboxScanInBackGround.Checked = pars.GetValue("IDLE_CHECK", "reg_dword:") == "1" ? true : false;
        cboxScanInBackGround.Attributes.Add("onclick", "ChangeScanInBackGround();");
        cboxMaximumCPUUsege.Checked = pars.GetValue("IDLE_PROCESSOR", "reg_dword:") == "1" ? true : false;
        cboxMaximumCPUUsege.Attributes.Add("onclick", "ChangeMaximumCPUUsege();");
        cboxMaximumDiskActivity.Checked = pars.GetValue("IDLE_DISK", "reg_dword:") == "1" ? true : false;
        cboxMaximumDiskActivity.Attributes.Add("onclick", "ChangeMaximumDiskActivity();");
        cboxMaximumDisplacement.Checked = pars.GetValue("IDLE_MOUSE", "reg_dword:") == "1" ? true : false;
        cboxMaximumDisplacement.Attributes.Add("onclick", "ChangeMaximumDisplacement();");
        cboxMinimumBattery.Checked = pars.GetValue("IDLE_NOTEBOOK", "reg_dword:") == "1" ? true : false;
        cboxMinimumBattery.Attributes.Add("onclick", "ChangeMinimumBattery();");

        //cboxScanStartupFiles.Checked = pars.GetValue("IDLE_AUTORUN", "reg_dword:") == "1" ? true : false;
        cboxScanFilesByUser.Checked = pars.GetValue("IDLE_USERFILES", "reg_dword:") == "1" ? true : false;
        cboxNotifyOfMonitor.Checked = pars.GetValue("NOTIFY", "reg_dword:") == "1" ? true : false;
        //cboxKeep.Checked = pars.GetValue("REPORT", "reg_dword:") == "1" ? true : false;
        cboxMaximumSizeLog.Checked = pars.GetValue("LIMIT_REPORT", "reg_dword:") == "1" ? true : false;
        cboxInformationAboutCleanFiles.Checked = pars.GetValue("SHOW_OK", "reg_dword:") == "1" ? true : false;
        cboxInfectedQuarantine.Checked = pars.GetValue("InfectedCopy1", "reg_dword:") == "1" ? true : false;
        cboxInfectedQua.Checked = pars.GetValue("InfectedCopy2", "reg_dword:") == "1" ? true : false;
        cboxInfectedQuaPrev.Checked = pars.GetValue("InfectedCopy3", "reg_dword:") == "1" ? true : false;
        cboxSuspiciousQua.Checked = pars.GetValue("SuspiciousCopy1", "reg_dword:") == "1" ? true : false;
        cboxSuspiciousQuaPrev.Checked = pars.GetValue("SuspiciousCopy2", "reg_dword:") == "1" ? true : false;
        cboxBlockUSB.Checked = pars.GetValue("BlockAutorunInf", "reg_dword:") == "1" ? true : false;

        //tbox
        if (pars.GetValue("IDLE_PROCESSOR_VALUE", "reg_dword:") != String.Empty)
            tboxMaximumCPUUsege.Text = pars.GetValue("IDLE_PROCESSOR_VALUE", "reg_dword:");
        if (pars.GetValue("IDLE_DISK_VALUE", "reg_dword:") != String.Empty)
            tboxMaximumDiskActivity.Text = pars.GetValue("IDLE_DISK_VALUE", "reg_dword:");
        if (pars.GetValue("IDLE_MOUSE_VALUE", "reg_dword:") != String.Empty)
            tboxMaximumDisplacement.Text = pars.GetValue("IDLE_MOUSE_VALUE", "reg_dword:");
        if (pars.GetValue("IDLE_NOTEBOOK_VALUE", "reg_dword:") != String.Empty)
            tboxMinimumBattery.Text = pars.GetValue("IDLE_NOTEBOOK_VALUE", "reg_dword:");
        if (pars.GetValue("REPORT_NAME", "reg_sz:") != String.Empty)
            tboxLogFile.Text = pars.GetValue("REPORT_NAME", "reg_sz:");
        if (pars.GetValue("LIMIT_REPORT_VALUE", "reg_dword:") != String.Empty)
            tboxMaximumSizeLog.Text = pars.GetValue("LIMIT_REPORT_VALUE", "reg_dword:");
        if (pars.GetValue("FILTER_DEFINED", "reg_sz:") != String.Empty)
            tboxFilterDefined.Text = pars.GetValue("FILTER_DEFINED", "reg_sz:");
        else tboxFilterDefined.Text = defaultFilters;
        if (pars.GetValue("FILTER_EXCLUDE", "reg_sz:") != String.Empty)
            tboxFilterExclude.Text = pars.GetValue("FILTER_EXCLUDE", "reg_sz:");

        //enable disable textboxes checkboxes
        
        if (!cboxScanInBackGround.Checked)
        {
            cboxMaximumCPUUsege.InputAttributes.Add("disabled", "true");
            cboxMaximumDiskActivity.InputAttributes.Add("disabled", "true");
            cboxMaximumDisplacement.InputAttributes.Add("disabled", "true");
            cboxMinimumBattery.InputAttributes.Add("disabled", "true");
            cboxScanFilesByUser.InputAttributes.Add("disabled", "true");

            tboxMaximumCPUUsege.Attributes.Add("disabled", "true");
            tboxMaximumDiskActivity.Attributes.Add("disabled", "true");
            tboxMaximumDisplacement.Attributes.Add("disabled", "true");
            tboxMinimumBattery.Attributes.Add("disabled", "true");
        }
        else
        {
            if (!cboxMaximumCPUUsege.Checked)
            {
                tboxMaximumCPUUsege.Attributes.Add("disabled", "true");
            }
            if (!cboxMaximumDiskActivity.Checked)
            {
                tboxMaximumDiskActivity.Attributes.Add("disabled", "true");
            }
            if (!cboxMaximumDisplacement.Checked)
            {
                tboxMaximumDisplacement.Attributes.Add("disabled", "true");
            }
            if (!cboxMinimumBattery.Checked)
            {
                tboxMinimumBattery.Attributes.Add("disabled", "true");
            }
        }
         

        //ddl
        int index = 0;
        if (pars.GetValue("InfectedAction1", "reg_dword:") != String.Empty)
        {
            index = Convert.ToInt32(pars.GetValue("InfectedAction1", "reg_dword:"));
            index--;
        }
        ddlInfected.SelectedIndex = index;                
        
        switch (index)
        {
            case 0:
                ddlPrevInfected.Items.Clear();
                for (int i = 1; i < ddlInfected.Items.Count; i++)
                {
                    ddlPrevInfected.Items.Add(ddlInfected.Items[i].Text);                    
                }
                ddlPrevInfected.SelectedIndex = 0;
                ddlPrevPrevInfected.SelectedIndex = 0;
                break;
            case 1:
                ddlPrevInfected.Items.Clear();
                for (int i = 0; i < ddlInfected.Items.Count; i++)
                {
                    if (i != 1)
                    {
                        ddlPrevInfected.Items.Add(ddlInfected.Items[i].Text);                        
                    }
                }
                ddlPrevInfected.SelectedIndex = 0;
                ddlPrevPrevInfected.SelectedIndex = 0;
                break;
            case 2:
                ddlPrevInfected.SelectedIndex = 1;
                ddlPrevPrevInfected.SelectedIndex = 0;

                ddlPrevInfected.Enabled = false;
                ddlPrevPrevInfected.Enabled = false;
                cboxInfectedQua.InputAttributes.Add("disabled", "true");
                cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");
                break;
            case 3:
                ddlPrevInfected.SelectedIndex = 2;
                ddlPrevPrevInfected.SelectedIndex = 1;

                ddlPrevInfected.Enabled = false;
                ddlPrevPrevInfected.Enabled = false;
                cboxInfectedQua.InputAttributes.Add("disabled", "true");
                cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");

                cboxInfectedQuarantine.InputAttributes.Add("disabled", "true");
                break;
        }
        
        if (index < 2)
        {
            if (pars.GetValue("InfectedAction2", "reg_dword:") != String.Empty)
            {
                index = Convert.ToInt32(pars.GetValue("InfectedAction2", "reg_dword:"));
                ddlPrevInfected.SelectedIndex = index == 1 ? 0 : (index - 2);
                switch (ddlPrevInfected.SelectedIndex)
                {
                    case 0:
                        ddlPrevPrevInfected.SelectedIndex = 0;
                        break;
                    case 1:
                        ddlPrevPrevInfected.SelectedIndex = 0;
                        ddlPrevPrevInfected.Enabled = false;
                        cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");
                        break;
                    case 2:
                        ddlPrevPrevInfected.SelectedIndex = 1;
                        ddlPrevPrevInfected.Enabled = false;
                        cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");
                        cboxInfectedQua.InputAttributes.Add("disabled", "true");
                        break;
                }
            }
            else
                ddlPrevInfected.SelectedIndex = 0;

            if (ddlPrevInfected.SelectedIndex == 0)
            {
                if (pars.GetValue("InfectedAction3", "reg_dword:") != String.Empty)
                {
                    index = Convert.ToInt32(pars.GetValue("InfectedAction3", "reg_dword:"));
                    ddlPrevPrevInfected.SelectedIndex = index - 3;
                    if (ddlPrevPrevInfected.SelectedIndex == 1) cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");
                }
                else
                    ddlPrevPrevInfected.SelectedIndex = 0;
            }
        }


        if (pars.GetValue("SuspiciousAction1", "reg_dword:") != String.Empty)
        {
            index = Convert.ToInt32(pars.GetValue("SuspiciousAction1", "reg_dword:"));
            ddlSuspicious.SelectedIndex = index == 3 ? 2 : index;
        }
        else
            ddlSuspicious.SelectedIndex = 0;

        switch (ddlSuspicious.SelectedIndex)
        {
            case 0:
                ddlPrePrevSuspicious.Enabled= false;
                ddlPrePrevSuspicious.SelectedIndex = 0;
                cboxSuspiciousQuaPrev.InputAttributes.Add("disabled", "true");
                break;
            case 1:
                ddlPrePrevSuspicious.Enabled= true;
                ddlPrePrevSuspicious.SelectedIndex = 0;
                cboxSuspiciousQuaPrev.InputAttributes.Add("disabled", "false");
                break;
            case 2:
                ddlPrePrevSuspicious.Enabled= false;
                ddlPrePrevSuspicious.SelectedIndex = 1;
                cboxSuspiciousQuaPrev.InputAttributes.Add("disabled", "true");
                break;
        }

        if (pars.GetValue("SuspiciousAction2", "reg_dword:") != String.Empty)
        {
            index = Convert.ToInt32(pars.GetValue("SuspiciousAction2", "reg_dword:"));
            ddlPrePrevSuspicious.SelectedIndex = index == 3 ? 1 : 0;
        }
        else
            ddlPrePrevSuspicious.SelectedIndex = 0;

        if (pars.GetValue("HEURISTIC", "reg_dword:") != String.Empty)
        {
            ddlHeuristicAnalysis.SelectedIndex = Convert.ToInt32(pars.GetValue("HEURISTIC", "reg_dword:"));
        }
        else
            ddlHeuristicAnalysis.SelectedIndex = 0;

        if (ddlHeuristicAnalysis.SelectedIndex == 0)
        {
            cboxBlockUSB.InputAttributes.Add("disabled", "true");
            cboxSuspiciousQuaPrev.InputAttributes.Add("disabled", "true");
            ddlPrePrevSuspicious.Attributes.Add("disabled", "true");
            cboxSuspiciousQua.InputAttributes.Add("disabled", "true");
            ddlSuspicious.Attributes.Add("disabled", "true");
        }

        //Folders and files excluded from Monitor scanning 
        ddlExcludingFoldersAndFilesDelete.Items.Clear();
        string str = pars.GetXmlTagContent("exclusions");
        foreach (string item in ParseMD5Path(str))
        {
            ddlExcludingFoldersAndFilesDelete.Items.Add(item);
        }
        
        //List of paths to scan
        ddlListOfPathToScan.Items.Clear();        
        str = pars.GetXmlTagContent("idlecheck");
        foreach (string item in ParseMD5Path(str))
        {
            ddlListOfPathToScan.Items.Add(item);
        }

        //rb
        if (pars.GetValue("CHECK_MODE", "reg_dword:") != String.Empty)
            index = Convert.ToInt32(pars.GetValue("CHECK_MODE","reg_dword:"));
        else
            index = 0;

        if (index == 0)
            rbScanStandartSet.Checked = true;
        if (index == 1)
            rbScanSelectedTypes.Checked = true;
        if (index == 2)
            rbMonitorScanAllTypes.Checked = true;
    }

    private List<string> ParseMD5Path(string path)
    {        
        List<string> parsePath = new List<string>();
        path = path.Replace("reg_sz:", "");
        string tmp = String.Empty;

        while (path.Length != 0)
        {
            path = path.Remove(0, path.IndexOf(">") + 1);
            tmp = path.Substring(0, path.IndexOf("<"));
            path = path.Remove(0, path.IndexOf("<") + 2);
            tmp = (path.Substring(0, 1) == "N" ? "-" : "+") + tmp;
            parsePath.Add(tmp);
            path = path.Remove(0, path.IndexOf(">") + 1);
        }

        return parsePath;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.ConfigureMonitor;

        ARM2_dbcontrol.Generation.XmlBuilder xml =
            new ARM2_dbcontrol.Generation.XmlBuilder("task");

        ValidateFields();
        task.Param = BuildXml();

        return task;

    }

   /* protected void lbtnObjects_Click(object sender, EventArgs e)
    {
        tblObjects1.Visible = true;
        tblObjects2.Visible = true;
       // tblObjects3.Visible = true;

        tblActions1.Visible = false;
        tblReport1.Visible = false;
        tblReport2.Visible = false;
        tblScanning1.Visible = false;
        tblScanning2.Visible = false;
        tblScanning3.Visible = false;
        //Anchor.ScrollToObj(lbtnObjects.ClientID, Page);

    }

    protected void lbtnBackgroundScanning_Click(object sender, EventArgs e)
    {
        tblObjects1.Visible = false;
        tblObjects2.Visible = false;
        tblObjects3.Visible = false;

        tblActions1.Visible = false;
        tblReport1.Visible = false;
        tblReport2.Visible = false;
        tblScanning1.Visible = true;
        tblScanning2.Visible = true;
      //  tblScanning3.Visible = true;
        //Anchor.ScrollToObj(lbtnBackgroundScanning.ClientID, Page);
    }

    protected void lbtnActions_Click(object sender, EventArgs e)
    {
        tblObjects1.Visible = false;
        tblObjects2.Visible = false;
        tblObjects3.Visible = false;

        tblActions1.Visible = true;
        tblReport1.Visible = false;
        tblReport2.Visible = false;
        tblScanning1.Visible = false;
        tblScanning2.Visible = false;
        tblScanning3.Visible = false;
        //Anchor.ScrollToObj(lbtnActions.ClientID, Page);
    }

    protected void lbtnReport_Click(object sender, EventArgs e)
    {
        tblObjects1.Visible = false;
        tblObjects2.Visible = false;
        tblObjects3.Visible = false;

        tblActions1.Visible = false;
        tblReport1.Visible = true;
        tblReport2.Visible = true;
        tblScanning1.Visible = false;
        tblScanning2.Visible = false;
        tblScanning3.Visible = false;
        //Anchor.ScrollToObj(lbtnReport.ClientID, Page);
    }*/

    private void ScrollToObj(string controlId)
    {
        if (!Page.ClientScript.IsStartupScriptRegistered("close"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "close", "document.getElementById('" + controlId + "').scrollIntoView(true);", true);
    }

    protected void lbtnExcludingFoldersAndFilesAdd_Click(object sender, EventArgs e)
    {
        if (tboxExcludingFoldersAndFilesAdd.Text == String.Empty) return;
        string path = tboxExcludingFoldersAndFilesAdd.Text.Trim();
        if ((ddlExcludingFoldersAndFilesDelete.Items.FindByText("-" + path) != null) ||
            (ddlExcludingFoldersAndFilesDelete.Items.FindByText("+" + path) != null))
        {
        }
        else
        {
            ddlExcludingFoldersAndFilesDelete.Items.Add((cboxIncludingSubFolders.Checked ? "+" : "-") + path);
        }
        tboxExcludingFoldersAndFilesAdd.Text = String.Empty;
        // ddlExcludingFoldersAndFilesDelete.DataBind();
        //Anchor.ScrollToObj(tboxExcludingFoldersAndFilesAdd.ClientID, Page);
    }

    protected void lbntnExcludingFoldersAndFilesDelete_Click(object sender, EventArgs e)
    {
        ddlExcludingFoldersAndFilesDelete.Items.Remove(ddlExcludingFoldersAndFilesDelete.SelectedItem);
        
        //Anchor.ScrollToObj(ddlExcludingFoldersAndFilesDelete.ClientID, Page);
    }
    protected void lbtnListOfPathToScanAdd_Click(object sender, EventArgs e)
    {
        if (tboxListOfPathToScanAdd.Text == String.Empty) return;
        string path = tboxListOfPathToScanAdd.Text.Trim();
        if ((ddlListOfPathToScan.Items.FindByText("-" + path) != null) ||
            (ddlListOfPathToScan.Items.FindByText("+" + path) != null))
        {
        }
        else
        {
            ddlListOfPathToScan.Items.Add((cboxListOfPathToScanIncludingSubFolders.Checked ? "+" : "-") + path);
        }
        tboxListOfPathToScanAdd.Text = String.Empty;
        //Anchor.ScrollToObj(ddlListOfPathToScan.ClientID, Page);
    }
    protected void lbtnListOfPathToScanDelete_Click(object sender, EventArgs e)
    {
        ddlListOfPathToScan.Items.Remove(ddlListOfPathToScan.SelectedItem);
        //Anchor.ScrollToObj(ddlListOfPathToScan.ClientID, Page);
    }
}
