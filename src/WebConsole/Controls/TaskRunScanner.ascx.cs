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
/// Задача запуска консольного сканера
/// </summary>
public partial class Controls_TaskRunScanner : System.Web.UI.UserControl, ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();

        //if (tboxCheckObjects.Text == String.Empty)
        //    tboxCheckObjects.Text = "*:";

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
        LoadSource();


        tabPanel1.HeaderText = Resources.Resource.Actions;
        tabPanel2.HeaderText = Resources.Resource.CongMonitorObjects;
        tabPanel3.HeaderText = Resources.Resource.CongMonitorReport;
        tabPanel4.HeaderText = Resources.Resource.CongScannerAdditional;

        lblExclude.Text = Resources.Resource.CongScannerExclude;
        lblSet.Text = Resources.Resource.CongScannerSet;
        lblAddArch.Text = Resources.Resource.CongScannerAdd;

        cboxMultyThreading.Text = " " + Resources.Resource.EnableDisableMultithreading;
        cboxShowProgressScan.Text = " " + Resources.Resource.ShowProgressScan;

    }

    public bool ValidateFields()
    {
        Validation vld = new Validation(tboxSet.Text);
        if(cboxSet.Checked)
        {
            if (!vld.CheckStringToExtension())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongScannerSet);
        }

        if (cboxAddArch.Checked)
        {
            vld.Value = tboxAddArch.Text;
            if (!vld.CheckStringToExtension())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongScannerAdd);
        }

        if (cboxExclude.Checked)
        {
            vld.Value = tboxExclude.Text;
            if (!vld.CheckStringToExtension())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongScannerExclude);
        }

        vld.Value = tboxCheckObjects.Text;
        if (!vld.CheckPath())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongMonitorObjects);

        if (cboxIsArchiveSize.Checked)
        {
            vld.Value = tboxArchiveSize.Text;
            if (!vld.CheckSize())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongScannerArchivesSize);
        }

        if (rbRemove.Checked)
        {
            vld.Value = tboxRemove.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongScannerRemovePathToInfected);
        }

        if (cboxKeep.Checked)
        {
            vld.Value = tboxKeep.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongScannerKeep);
        }

        if (cboxSaveInfectedToReport.Checked)
        {
            vld.Value = tboxSaveInfectedToReport.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongScannerSaveInfectedToReport);
        }

        vld.Value = tboxPathToScanner.Text;
        if (!vld.CheckPath())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        + Resources.Resource.CongScannerPathToScanner);

        return true;
    }

    private string BuildXml()
    {

        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("task");

        //cbox
        xml.AddNode("IsCheckArchives", cboxCheckArchives.Checked ? "1" : "0");
        xml.AddNode("IsCheckMacros", cboxCheckMacros.Checked ? "1" : "0");
        xml.AddNode("IsCheckMail", cboxCheckMail.Checked ? "1" : "0");
        xml.AddNode("IsCheckMemory", cboxCheckMemory.Checked ? "1" : "0");
        xml.AddNode("IsCleanFiles", cboxCleanFiles.Checked ? "1" : "0");
        xml.AddNode("IsCheckCure", cboxCure.Checked ? "1" : "0");
        xml.AddNode("IsCheckCureBoot", cboxCureBoot.Checked ? "1" : "0");
        xml.AddNode("IsDeleteArchives", cboxDeleteArchives.Checked ? "1" : "0");
        xml.AddNode("IsDeleteMail", cboxDeleteMail.Checked ? "1" : "0");
        xml.AddNode("IsDetectAdware", cboxDetectAdware.Checked ? "1" : "0");
        xml.AddNode("IsEnableCach", cboxEnableCach.Checked ? "1" : "0");
        xml.AddNode("IsExclude", cboxExclude.Checked ? "1" : "0");
        xml.AddNode("IsSaveInfectedToQuarantine", cboxSaveInfectedToQuarantine.Checked ? "1" : "0");
        xml.AddNode("IsSaveInfectedToReport", cboxSaveInfectedToReport.Checked ? "1" : "0");
        xml.AddNode("IsSaveSusToQuarantine", cboxSaveSusToQuarantine.Checked ? "1" : "0");
        xml.AddNode("IsScanBootSectors", cboxScanBootSectors.Checked ? "1" : "0");
        xml.AddNode("IsSFX", cboxScannerSFX.Checked ? "1" : "0");
        xml.AddNode("IsScanStartup", cboxScanStartup.Checked ? "1" : "0");
        xml.AddNode("IsSet", cboxSet.Checked ? "1" : "0");
        xml.AddNode("IsKeep", cboxKeep.Checked ? "1" : "0");
        xml.AddNode("IsAdd", cboxAdd.Checked ? "1" : "0");
        xml.AddNode("IsAddArch", cboxAddArch.Checked ? "1" : "0");
        xml.AddNode("IsAddInf", cboxAddInf.Checked ? "1" : "0");
        //update base
        xml.AddNode("IsUpdateBase", cboxUpdate.Checked ? "1" : "0");

        //tbox
        xml.AddNode("CheckObjects", tboxCheckObjects.Text);
       

        if (cboxKeep.Checked)
            xml.AddNode("Keep", tboxKeep.Text);
        xml.AddNode("PathToScanner", tboxPathToScanner.Text);
        if (cboxSaveInfectedToReport.Checked)
            xml.AddNode("SaveInfectedToReport", tboxSaveInfectedToReport.Text);

        if(cboxIsArchiveSize.Checked)
            xml.AddNode("ArchiveSize", tboxArchiveSize.Text);

        if(cboxAddArch.Checked)
            xml.AddNode("AddArch", tboxAddArch.Text);
        if (cboxExclude.Checked)
            xml.AddNode("Exclude", tboxExclude.Text);
        if (cboxSet.Checked)
            xml.AddNode("Set", tboxSet.Text);

        //ddl
        xml.AddNode("Mode", Convert.ToString(ddlMode.SelectedIndex));
        xml.AddNode("HeuristicAnalysis", ddlHeuristicAnalysis.SelectedIndex.ToString());

        //Multithreading
        if (cboxMultyThreading.Checked)
        {
            xml.AddNode("MultyThreading", ddlCountThread.SelectedItem.Text);
        }

        //Show scan progress
        xml.AddNode("IsShowScanProgress", cboxShowProgressScan.Checked ? "1" : "0");


        int index = 0;
       /* if (cboxCure.Checked)
        {
        }*/
            if (rbSkip.Checked) index = 1;
            if (rbDelete.Checked) index = 2;
            if (rbRename.Checked) index = 3;
            if (rbRemove.Checked) index = 4;

        xml.AddNode("Remove", tboxRemove.Text);
        
        xml.AddNode("IfCureChecked", index.ToString());
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "RunScanner");

        xml.Generate();

        return xml.Result;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.RunScanner)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser pars = new XmlTaskParser(task.Param);

        cboxCheckArchives.Checked = pars.GetValue("IsCheckArchives") == "1" ? true : false;
        cboxCheckMacros.Checked = pars.GetValue("IsCheckMacros") == "1" ? true : false;
        cboxCheckMail.Checked = pars.GetValue("IsCheckMail") == "1" ? true : false;
        cboxCheckMemory.Checked = pars.GetValue("IsCheckMemory") == "1" ? true : false;
        cboxCleanFiles.Checked = pars.GetValue("IsCleanFiles") == "1" ? true : false;
        cboxCure.Checked = pars.GetValue("IsCheckCure") == "1" ? true : false;
        cboxCureBoot.Checked = pars.GetValue("IsCheckCureBoot") == "1" ? true : false;
        cboxDeleteArchives.Checked = pars.GetValue("IsDeleteArchives") == "1" ? true : false;
        cboxDeleteMail.Checked = pars.GetValue("IsDeleteMail") == "1" ? true : false;
        cboxDetectAdware.Checked = pars.GetValue("IsDetectAdware") == "1" ? true : false;
        cboxEnableCach.Checked = pars.GetValue("IsEnableCach") == "1" ? true : false;
        cboxExclude.Checked = pars.GetValue("IsExclude") == "1" ? true : false;
        cboxSaveInfectedToQuarantine.Checked = pars.GetValue("IsSaveInfectedToQuarantine") == "1" ? true : false;
        cboxSaveInfectedToReport.Checked = pars.GetValue("IsSaveInfectedToReport") == "1" ? true : false;
        cboxSaveSusToQuarantine.Checked = pars.GetValue("IsSaveSusToQuarantine") == "1" ? true : false;
        cboxScanBootSectors.Checked = pars.GetValue("IsScanBootSectors") == "1" ? true : false;
        cboxScannerSFX.Checked = pars.GetValue("IsSFX") == "1" ? true : false;
        cboxScanStartup.Checked = pars.GetValue("IsScanStartup") == "1" ? true : false;
        cboxSet.Checked = pars.GetValue("IsSet") == "1" ? true : false;
        cboxKeep.Checked = pars.GetValue("IsKeep") == "1" ? true : false;
        cboxAdd.Checked = pars.GetValue("IsAdd") == "1" ? true : false;
        cboxAddArch.Checked = pars.GetValue("IsAddArch") == "1" ? true : false;
        cboxAddInf.Checked = pars.GetValue("IsAddInf") == "1" ? true : false;
        //Update base
        cboxUpdate.Checked = pars.GetValue("IsUpdateBase") == "1"? true : false;

        cboxIsArchiveSize.Checked = pars.GetValue("ArchiveSize") == String.Empty ? false : true;

        tboxCheckObjects.Text = pars.GetValue("CheckObjects");
        if (tboxCheckObjects.Text == String.Empty)
            tboxCheckObjects.Text = "*:";

        tboxKeep.Text = pars.GetValue("Keep");
       
        tboxPathToScanner.Text = pars.GetValue("PathToScanner");
        if (tboxPathToScanner.Text == String.Empty)
            tboxPathToScanner.Text = "%VBA32%";

        tboxRemove.Text = pars.GetValue("Remove");
        tboxSaveInfectedToReport.Text = pars.GetValue("SaveInfectedToReport");

        tboxArchiveSize.Text = pars.GetValue("ArchiveSize");

        tboxAddArch.Text = pars.GetValue("AddArch");
        tboxExclude.Text = pars.GetValue("Exclude");
        tboxSet.Text = pars.GetValue("Set");

        string value = pars.GetValue("IfCureChecked");
        int index = 0;
        if (value != String.Empty)
            index = Convert.ToInt32(value);

        if (index == 1) rbSkip.Checked = true;
        if (index == 2) rbDelete.Checked = true;
        if (index == 3) rbRename.Checked = true;
        if (index == 4) rbRemove.Checked = true;

        value = pars.GetValue("HeuristicAnalysis");
        if (value != String.Empty)
            ddlHeuristicAnalysis.SelectedIndex = Convert.ToInt32(value);

        value = pars.GetValue("Mode");
        if (value != String.Empty)
            ddlMode.SelectedIndex = Convert.ToInt32(value);

        //Multithreading
        value = pars.GetValue("MultyThreading");
        if (value != String.Empty)
        {
            ddlCountThread.SelectedValue = value;
            cboxMultyThreading.Checked = true;

            ddlCountThread.Enabled = true;
            lblCountThread.Enabled = true;
        }
        else
        {
            ddlCountThread.SelectedIndex = 0;
            cboxMultyThreading.Checked = false;

            ddlCountThread.Enabled = false;
            lblCountThread.Enabled = false;
        }

        //Show scan progress
        cboxShowProgressScan.Checked = pars.GetValue("IsShowScanProgress") == "1" ? true : false;
        
    }

    public string GenerateCommandLine(TaskUserEntity task)
    {

        if (task.Type != TaskType.RunScanner)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser pars = new XmlTaskParser(task.Param);

        string fileName = cboxUpdate.Checked ? "VbaControlAgent.exe\" LCSU" : "vba32w.exe\"";

        string commandLine = '"' + tboxPathToScanner.Text + fileName + " " + '"' + tboxCheckObjects.Text + '"';

        if (pars.GetValue("IsCheckArchives") == "1")
            commandLine += " /AR+";//AR
        else
            commandLine += " /AR-";


        if (pars.GetValue("IsCheckMacros") == "1")
            commandLine += " /VM+";
        else
            commandLine += " /VM-";
        //VM


        if (pars.GetValue("IsCheckMail") == "1") 
            commandLine += " /ML+";
        else
            commandLine += " /ML-";
        //ML


        if (pars.GetValue("IsCheckMemory") == "1")
            commandLine += " /MR+";
        else
            commandLine += " /MR-";
        //MR


        if (pars.GetValue("IsCleanFiles") == "1")
            commandLine += " /OK+";
        else
            commandLine += " /OK-";
        //OK


        if (pars.GetValue("IsCheckCure") == "1")
            commandLine += " /FC+";
        else
            commandLine += " /FC-";
        //FC


        if (pars.GetValue("IsCheckCureBoot") == "1")
            commandLine += " /BC+";
        else
            commandLine += " /BC-";
        //BC


        if (pars.GetValue("IsDeleteArchives") == "1")
            commandLine += " /AD+";
        else
            commandLine += " /AD-";
        //AD


        if (pars.GetValue("IsDeleteMail") == "1")
            commandLine += " /MD+";
        else
            commandLine += " /MD-";
        //MD

        if (pars.GetValue("IsDetectAdware") == "1")
            commandLine += " /RW+";
        else
            commandLine += " /RW-";
        //RW


        if (pars.GetValue("IsEnableCach") == "1")
            commandLine += " /CH+";
        else
            commandLine += " /CH-";
        //CH


        if (pars.GetValue("IsExclude") == "1")
        {
            //!!!Проверить
            commandLine += " /EXT-" + tboxExclude.Text;
        }


        if (pars.GetValue("IsSaveInfectedToQuarantine") == "1")
            commandLine += " /QI+[" + tboxRemove.Text + "]";
        else
            commandLine += " /QI-";
        ////QI[+[каталог]|-]          

        if (pars.GetValue("IsSaveSusToQuarantine") == "1")
            commandLine += " /QS+[" + tboxRemove.Text + "]";
        else
            commandLine += " /QS-";
        //QS[+[каталог]|-]

        if (pars.GetValue("IsScanBootSectors") == "1")
            commandLine += " /BT+";
        else
            commandLine += " /BT-";
        ////BT

        if (pars.GetValue("IsSFX") == "1")
            commandLine += " /SFX+";
        else
            commandLine += " /SFX-";
        ////SFX


        if (pars.GetValue("IsScanStartup") == "1")
            commandLine += " /AS+";
        else
            commandLine += " /AS-";
        ////AS


        if (pars.GetValue("IsSet") == "1")
        {
            commandLine += " /EXT=" + tboxSet.Text;
        }

        if (pars.GetValue("IsKeep") == "1")
        {   
            if (pars.GetValue("IsAdd") == "1")
                commandLine += " /R+" + '"' + tboxKeep.Text +'"';
            else
                commandLine += " /R=" + '"' + tboxKeep.Text + '"';
        }
        //else
        //    commandLine += " /CH-";
        ////EXT+


        if (pars.GetValue("IsAddArch") == "1")
            commandLine += " /EXT+" + tboxAddArch.Text;
       
        ////EXT+

        if (pars.GetValue("IsSaveInfectedToReport") == "1")
        {
            if (pars.GetValue("IsAddInf") == "1")
                commandLine += " /L+" + tboxSaveInfectedToReport.Text;
            else
                commandLine += " /L=" + tboxSaveInfectedToReport.Text;
        }


        string value = pars.GetValue("IfCureChecked");
        int index = 0;
        if (value != String.Empty)
        {
            ///HA=[0|1|2|3]
            index = Convert.ToInt32(value);
            //     if (index == 1) rbSkip.Checked = true;
            if (index == 2) commandLine += " /FD+";
            if (index == 3) commandLine += " /FR+";

            if (index == 4) commandLine += " /FM+" + '"'+ tboxRemove.Text+'"';

        }

        value = pars.GetValue("HeuristicAnalysis");
        if (value != String.Empty)
        {
            index = Convert.ToInt32(value);
            commandLine += " /HA=" + index.ToString();
        }

        value = pars.GetValue("Mode");

        if (value != String.Empty)
        {
            index = Convert.ToInt32(value);
            index++;
            commandLine += " /M=" + index.ToString();
        }

         value = pars.GetValue("ArchiveSize");

         if (value != String.Empty)
         {
             index = Convert.ToInt32(value);
             commandLine += " /AL=" + index.ToString();
         }

        //Multithreading
         value = pars.GetValue("MultyThreading");

         if (value != String.Empty)
         {
             commandLine += " /J=" + value;
         }

        //Show progress
         if (pars.GetValue("IsShowScanProgress") == "1")
             commandLine += " /SP+";
         else
             commandLine += " /SP-";        

       
        return commandLine;
    }

    private void LoadSource()
    {
        if (ddlHeuristicAnalysis.Items.Count != 0) return;
        if ((!rbDelete.Checked) && (!rbRemove.Checked) && (!rbRename.Checked) && (!rbSkip.Checked))
            rbSkip.Checked = true;

        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Disabled);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Optimal);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Maximum);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Excessive);
        ddlHeuristicAnalysis.SelectedIndex = 2;

        ddlMode.Items.Add(Resources.Resource.Fast);
        ddlMode.Items.Add(Resources.Resource.Safe);
        ddlMode.Items.Add(Resources.Resource.Excessive);

        ddlCountThread.Items.Add("Auto");
        for (int i = 2; i < 16; i++)
        {
            ddlCountThread.Items.Add(i.ToString());
        }
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.RunScanner;

        ARM2_dbcontrol.Generation.XmlBuilder xml =
            new ARM2_dbcontrol.Generation.XmlBuilder("task");

        ValidateFields();
        task.Param = BuildXml();

        return task;
    }
}
