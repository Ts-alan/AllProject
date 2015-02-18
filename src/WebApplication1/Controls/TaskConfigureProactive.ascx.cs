using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

public partial class Controls_TaskConfigureProactive : System.Web.UI.UserControl, ITask
{
    private static TaskConfigureProactive proactive;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || proactive == null)
            InitFields();
        InitFieldsJournalEvent(proactive.journalEvent);
    }

    private Boolean _hideHeader = false;
    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private Boolean _enabled = true;
    public Boolean Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    #region ITask Members

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        SetEnabled();

        if (proactive == null)
        {
            proactive = new TaskConfigureProactive(GetEvents());
            proactive.Vba32CCUser = Anchor.GetStringForTaskGivedUser();
        }
        else
        {
            proactive.Clear();
        }

        UpdateUserList();
        UpdateGeneral();
        UpdateUsers(proactive.UserRules[0].RuleName);
        UpdatePrinters();
        UpdateFields();
    }

    private String[] GetEvents()
    {
        String[] s = { "JE_VPP_APPLIED_RULES_FAILED",
                        "JE_VPP_APPLIED_RULES_OK",
                        "JE_VPP_AUDIT_DELETE",
                        "JE_VPP_AUDIT_EXECUTE",
                        "JE_VPP_AUDIT_OPEN_READ",
                        "JE_VPP_AUDIT_OPEN_WRITE",
                        "JE_VPP_AUDIT_READ",
                        "JE_VPP_AUDIT_WRITE",
                        "JE_VPP_START",
                        "JE_VPP_STOP",
                        "JE_VPP_PRINTER_GRANTED",
                        "JE_VPP_PRINTER_DENIED"
                     };
        return s;
    }

    private void SetEnabled()
    {
        tboxAppsProtected.Enabled = tboxAppsTrusted.Enabled = _enabled;
        tboxFolderExcluded.Enabled = tboxFolderProtected.Enabled = tboxFolderReadOnly.Enabled = _enabled;
        tboxFileExcluded.Enabled = tboxFileProtected.Enabled = tboxFileReadOnly.Enabled = _enabled;
        tboxKeyProtected.Enabled = tboxKeyReadOnly.Enabled = _enabled;
        tboxValueProtected.Enabled = tboxValueReadOnly.Enabled = _enabled;

        lboxAppsTrusted.Enabled = lboxAppsProtected.Enabled = _enabled;
        lboxFolderExcluded.Enabled = lboxFolderProtected.Enabled = lboxFolderReadOnly.Enabled = _enabled;
        lboxFileExcluded.Enabled = lboxFileProtected.Enabled = lboxFileReadOnly.Enabled = _enabled;
        lboxKeyProtected.Enabled = lboxKeyReadOnly.Enabled = _enabled;
        lboxValueProtected.Enabled = lboxValueReadOnly.Enabled = _enabled;
        
        tboxAppsTrustedUsers.Enabled = _enabled;
        tboxFolderProtectedUsers.Enabled = tboxFolderReadOnlyUsers.Enabled = _enabled;
        tboxFileProtectedUsers.Enabled = tboxFileReadOnlyUsers.Enabled = _enabled;
        tboxKeyProtectedUsers.Enabled = tboxKeyReadOnlyUsers.Enabled = _enabled;
        tboxPrinterTrustedUsers.Enabled = _enabled;

        lboxAppsTrustedUsers.Enabled = _enabled;
        lboxFolderProtectedUsers.Enabled = lboxFolderReadOnlyUsers.Enabled = _enabled;
        lboxFileProtectedUsers.Enabled = lboxFileReadOnlyUsers.Enabled = _enabled;
        lboxKeyProtectedUsers.Enabled = lboxKeyReadOnlyUsers.Enabled = _enabled;
        lboxPrinterTrustedUsers.Enabled = _enabled;

        tboxPrinterTrusted.Enabled = _enabled;
        lboxPrinterTrusted.Enabled = _enabled;

        cboxIsUserAudit.Enabled = tboxProcessedExtensions.Enabled = _enabled;

        cboxGeneralOn.Enabled = cboxUsersOn.Enabled = cboxPrintersOn.Enabled = _enabled;
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ProactiveProtection;
        SaveJournalEvents();
        SaveFields();
        task.Param = proactive.SaveToXml();

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ProactiveProtection)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        proactive.LoadFromXml(task.Param);

        UpdateUserList();
        UpdateGeneral();
        UpdateUsers(proactive.UserRules[0].RuleName);
        UpdatePrinters();
        UpdateFields();
        LoadJournalEvent(proactive.journalEvent);
    }

    #endregion

    private void SaveFields()
    {
        proactive.IsUserAudit = cboxIsUserAudit.Checked;
        proactive.LogProcessedExtensions = tboxProcessedExtensions.Text;

        proactive.IsEnabled = cboxGeneralOn.Checked;
        proactive.IsUserFiltering = cboxUsersOn.Checked;
        proactive.IsPrinterControl = cboxPrintersOn.Checked;
    }

    public String BuildTask()
    {
        SaveJournalEvents();
        SaveFields();
        return proactive.GetTask();        
    }

    #region Change ListBoxes

    #region General

    #region Application - Trusted

    protected void lbtnAddAppsTrusted_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxAppsTrusted.Text))
        {
            proactive.GeneralRule.TrustedApplications.Add(tboxAppsTrusted.Text);
            lboxAppsTrusted.Items.Add(tboxAppsTrusted.Text);
            tboxAppsTrusted.Text = String.Empty;
        }
    }

    protected void lbtnDeleteAppsTrusted_Click(Object sender, EventArgs e)
    {
        if (lboxAppsTrusted.SelectedIndex > -1 && lboxAppsTrusted.SelectedIndex < lboxAppsTrusted.Items.Count)
        {
            proactive.GeneralRule.TrustedApplications.RemoveAt(lboxAppsTrusted.SelectedIndex);
            lboxAppsTrusted.Items.RemoveAt(lboxAppsTrusted.SelectedIndex);            
        }
    }

    #endregion

    #region Application - Protected

    protected void lbtnAddAppsProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxAppsProtected.Text))
        {
            proactive.GeneralRule.ProtectedApplications.Add(tboxAppsProtected.Text);
            lboxAppsProtected.Items.Add(tboxAppsProtected.Text);
            tboxAppsProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteAppsProtected_Click(Object sender, EventArgs e)
    {
        if (lboxAppsProtected.SelectedIndex > -1 && lboxAppsProtected.SelectedIndex < lboxAppsProtected.Items.Count)
        {
            proactive.GeneralRule.ProtectedApplications.RemoveAt(lboxAppsProtected.SelectedIndex);
            lboxAppsProtected.Items.RemoveAt(lboxAppsProtected.SelectedIndex);
        }
    }

    #endregion

    #region Folder- Protected

    protected void lbtnAddFolderProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFolderProtected.Text))
        {
            proactive.GeneralRule.ProtectedFolders.Add(tboxFolderProtected.Text);
            lboxFolderProtected.Items.Add(tboxFolderProtected.Text);
            tboxFolderProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFolderProtected_Click(Object sender, EventArgs e)
    {
        if (lboxFolderProtected.SelectedIndex > -1 && lboxFolderProtected.SelectedIndex < lboxFolderProtected.Items.Count)
        {
            proactive.GeneralRule.ProtectedFolders.RemoveAt(lboxFolderProtected.SelectedIndex);
            lboxFolderProtected.Items.RemoveAt(lboxFolderProtected.SelectedIndex);
        }
    }

    #endregion

    #region Folder- Excluded

    protected void lbtnAddFolderExcluded_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFolderExcluded.Text))
        {
            proactive.GeneralRule.ExcludedFolders.Add(tboxFolderExcluded.Text);
            lboxFolderExcluded.Items.Add(tboxFolderExcluded.Text);
            tboxFolderExcluded.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFolderExcluded_Click(Object sender, EventArgs e)
    {
        if (lboxFolderExcluded.SelectedIndex > -1 && lboxFolderExcluded.SelectedIndex < lboxFolderExcluded.Items.Count)
        {
            proactive.GeneralRule.ExcludedFolders.RemoveAt(lboxFolderExcluded.SelectedIndex);
            lboxFolderExcluded.Items.RemoveAt(lboxFolderExcluded.SelectedIndex);
        }
    }

    #endregion

    #region Folder- ReadOnly

    protected void lbtnAddFolderReadOnly_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFolderReadOnly.Text))
        {
            proactive.GeneralRule.ReadOnlyFolders.Add(tboxFolderReadOnly.Text);
            lboxFolderReadOnly.Items.Add(tboxFolderReadOnly.Text);
            tboxFolderReadOnly.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFolderReadOnly_Click(Object sender, EventArgs e)
    {
        if (lboxFolderReadOnly.SelectedIndex > -1 && lboxFolderReadOnly.SelectedIndex < lboxFolderReadOnly.Items.Count)
        {
            proactive.GeneralRule.ReadOnlyFolders.RemoveAt(lboxFolderReadOnly.SelectedIndex);
            lboxFolderReadOnly.Items.RemoveAt(lboxFolderReadOnly.SelectedIndex);
        }
    }

    #endregion

    #region File- Protected

    protected void lbtnAddFileProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFileProtected.Text))
        {
            proactive.GeneralRule.ProtectedFiles.Add(tboxFileProtected.Text);
            lboxFileProtected.Items.Add(tboxFileProtected.Text);
            tboxFileProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFileProtected_Click(Object sender, EventArgs e)
    {
        if (lboxFileProtected.SelectedIndex > -1 && lboxFileProtected.SelectedIndex < lboxFileProtected.Items.Count)
        {
            proactive.GeneralRule.ProtectedFiles.RemoveAt(lboxFileProtected.SelectedIndex);
            lboxFileProtected.Items.RemoveAt(lboxFileProtected.SelectedIndex);
        }
    }

    #endregion

    #region File- Excluded

    protected void lbtnAddFileExcluded_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFileExcluded.Text))
        {
            proactive.GeneralRule.ExcludedFiles.Add(tboxFileExcluded.Text);
            lboxFileExcluded.Items.Add(tboxFileExcluded.Text);
            tboxFileExcluded.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFileExcluded_Click(Object sender, EventArgs e)
    {
        if (lboxFileExcluded.SelectedIndex > -1 && lboxFileExcluded.SelectedIndex < lboxFileExcluded.Items.Count)
        {
            proactive.GeneralRule.ExcludedFiles.RemoveAt(lboxFileExcluded.SelectedIndex);
            lboxFileExcluded.Items.RemoveAt(lboxFileExcluded.SelectedIndex);
        }
    }

    #endregion

    #region File- ReadOnly

    protected void lbtnAddFileReadOnly_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFileReadOnly.Text))
        {
            proactive.GeneralRule.ReadOnlyFiles.Add(tboxFileReadOnly.Text);
            lboxFileReadOnly.Items.Add(tboxFileReadOnly.Text);
            tboxFileReadOnly.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFileReadOnly_Click(Object sender, EventArgs e)
    {
        if (lboxFileReadOnly.SelectedIndex > -1 && lboxFileReadOnly.SelectedIndex < lboxFileReadOnly.Items.Count)
        {
            proactive.GeneralRule.ReadOnlyFiles.RemoveAt(lboxFileReadOnly.SelectedIndex);
            lboxFileReadOnly.Items.RemoveAt(lboxFileReadOnly.SelectedIndex);
        }
    }

    #endregion

    #region Key- Protected

    protected void lbtnAddKeyProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxKeyProtected.Text))
        {
            proactive.GeneralRule.ProtectedRegistryKeys.Add(tboxKeyProtected.Text);
            lboxKeyProtected.Items.Add(tboxKeyProtected.Text);
            tboxKeyProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteKeyProtected_Click(Object sender, EventArgs e)
    {
        if (lboxKeyProtected.SelectedIndex > -1 && lboxKeyProtected.SelectedIndex < lboxKeyProtected.Items.Count)
        {
            proactive.GeneralRule.ProtectedRegistryKeys.RemoveAt(lboxKeyProtected.SelectedIndex);
            lboxKeyProtected.Items.RemoveAt(lboxKeyProtected.SelectedIndex);
        }
    }

    #endregion

    #region Key- ReadOnly

    protected void lbtnAddKeyReadOnly_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxKeyReadOnly.Text))
        {
            proactive.GeneralRule.ReadOnlyRegistryKeys.Add(tboxKeyReadOnly.Text);
            lboxKeyReadOnly.Items.Add(tboxKeyReadOnly.Text);
            tboxKeyReadOnly.Text = String.Empty;
        }
    }

    protected void lbtnDeleteKeyReadOnly_Click(Object sender, EventArgs e)
    {
        if (lboxKeyReadOnly.SelectedIndex > -1 && lboxKeyReadOnly.SelectedIndex < lboxKeyReadOnly.Items.Count)
        {
            proactive.GeneralRule.ReadOnlyRegistryKeys.RemoveAt(lboxKeyReadOnly.SelectedIndex);
            lboxKeyReadOnly.Items.RemoveAt(lboxKeyReadOnly.SelectedIndex);
        }
    }

    #endregion

    #region Value- Protected

    protected void lbtnAddValueProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxValueProtected.Text))
        {
            proactive.GeneralRule.ProtectedRegistryValues.Add(tboxValueProtected.Text);
            lboxValueProtected.Items.Add(tboxValueProtected.Text);
            tboxValueProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteValueProtected_Click(Object sender, EventArgs e)
    {
        if (lboxValueProtected.SelectedIndex > -1 && lboxValueProtected.SelectedIndex < lboxValueProtected.Items.Count)
        {
            proactive.GeneralRule.ProtectedRegistryValues.RemoveAt(lboxValueProtected.SelectedIndex);
            lboxValueProtected.Items.RemoveAt(lboxValueProtected.SelectedIndex);
        }
    }

    #endregion

    #region Value- ReadOnly

    protected void lbtnAddValueReadOnly_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxValueReadOnly.Text))
        {
            proactive.GeneralRule.ReadOnlyRegistryValues.Add(tboxValueReadOnly.Text);
            lboxValueReadOnly.Items.Add(tboxValueReadOnly.Text);
            tboxValueReadOnly.Text = String.Empty;
        }
    }

    protected void lbtnDeleteValueReadOnly_Click(Object sender, EventArgs e)
    {
        if (lboxValueReadOnly.SelectedIndex > -1 && lboxValueReadOnly.SelectedIndex < lboxValueReadOnly.Items.Count)
        {
            proactive.GeneralRule.ReadOnlyRegistryValues.RemoveAt(lboxValueReadOnly.SelectedIndex);
            lboxValueReadOnly.Items.RemoveAt(lboxValueReadOnly.SelectedIndex);
        }
    }

    #endregion

    #endregion

    #region Users

    #region Application - Trusted

    protected void lbtnAddAppsTrustedUsers_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxAppsTrustedUsers.Text))
        {
            proactive.UserRules[GetSelectedIndex()].TrustedApplications.Add(tboxAppsTrustedUsers.Text);
            lboxAppsTrustedUsers.Items.Add(tboxAppsTrustedUsers.Text);
            tboxAppsTrustedUsers.Text = String.Empty;
        }
    }

    protected void lbtnDeleteAppsTrustedUsers_Click(Object sender, EventArgs e)
    {
        if (lboxAppsTrustedUsers.SelectedIndex > -1 && lboxAppsTrustedUsers.SelectedIndex < lboxAppsTrustedUsers.Items.Count)
        {
            proactive.UserRules[GetSelectedIndex()].TrustedApplications.RemoveAt(lboxAppsTrustedUsers.SelectedIndex);
            lboxAppsTrustedUsers.Items.RemoveAt(lboxAppsTrustedUsers.SelectedIndex);
        }
    }

    #endregion

    #region Folder- Protected

    protected void lbtnAddFolderProtectedUsers_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFolderProtectedUsers.Text))
        {
            proactive.UserRules[GetSelectedIndex()].ProtectedFolders.Add(tboxFolderProtectedUsers.Text);
            lboxFolderProtectedUsers.Items.Add(tboxFolderProtectedUsers.Text);
            tboxFolderProtectedUsers.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFolderProtectedUsers_Click(Object sender, EventArgs e)
    {
        if (lboxFolderProtectedUsers.SelectedIndex > -1 && lboxFolderProtectedUsers.SelectedIndex < lboxFolderProtectedUsers.Items.Count)
        {
            proactive.UserRules[GetSelectedIndex()].ProtectedFolders.RemoveAt(lboxFolderProtectedUsers.SelectedIndex);
            lboxFolderProtectedUsers.Items.RemoveAt(lboxFolderProtectedUsers.SelectedIndex);
        }
    }

    #endregion

    #region Folder- ReadOnly

    protected void lbtnAddFolderReadOnlyUsers_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFolderReadOnlyUsers.Text))
        {            
            proactive.UserRules[GetSelectedIndex()].ReadOnlyFolders.Add(tboxFolderReadOnlyUsers.Text);
            lboxFolderReadOnlyUsers.Items.Add(tboxFolderReadOnlyUsers.Text);
            tboxFolderReadOnlyUsers.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFolderReadOnlyUsers_Click(Object sender, EventArgs e)
    {
        if (lboxFolderReadOnlyUsers.SelectedIndex > -1 && lboxFolderReadOnlyUsers.SelectedIndex < lboxFolderReadOnlyUsers.Items.Count)
        {
            proactive.UserRules[GetSelectedIndex()].ReadOnlyFolders.RemoveAt(lboxFolderReadOnlyUsers.SelectedIndex);
            lboxFolderReadOnlyUsers.Items.RemoveAt(lboxFolderReadOnlyUsers.SelectedIndex);
        }
    }

    #endregion

    #region File- Protected

    protected void lbtnAddFileProtectedUsers_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFileProtectedUsers.Text))
        {
            proactive.UserRules[GetSelectedIndex()].ProtectedFiles.Add(tboxFileProtectedUsers.Text);
            lboxFileProtectedUsers.Items.Add(tboxFileProtectedUsers.Text);
            tboxFileProtectedUsers.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFileProtectedUsers_Click(Object sender, EventArgs e)
    {
        if (lboxFileProtectedUsers.SelectedIndex > -1 && lboxFileProtectedUsers.SelectedIndex < lboxFileProtectedUsers.Items.Count)
        {
            proactive.UserRules[GetSelectedIndex()].ProtectedFiles.RemoveAt(lboxFileProtectedUsers.SelectedIndex);
            lboxFileProtectedUsers.Items.RemoveAt(lboxFileProtectedUsers.SelectedIndex);
        }
    }

    #endregion

    #region File- ReadOnly

    protected void lbtnAddFileReadOnlyUsers_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFileReadOnlyUsers.Text))
        {
            proactive.UserRules[GetSelectedIndex()].ReadOnlyFiles.Add(tboxFileReadOnlyUsers.Text);
            lboxFileReadOnlyUsers.Items.Add(tboxFileReadOnlyUsers.Text);
            tboxFileReadOnlyUsers.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFileReadOnlyUsers_Click(Object sender, EventArgs e)
    {
        if (lboxFileReadOnlyUsers.SelectedIndex > -1 && lboxFileReadOnlyUsers.SelectedIndex < lboxFileReadOnlyUsers.Items.Count)
        {
            proactive.UserRules[GetSelectedIndex()].ReadOnlyFiles.RemoveAt(lboxFileReadOnlyUsers.SelectedIndex);
            lboxFileReadOnlyUsers.Items.RemoveAt(lboxFileReadOnlyUsers.SelectedIndex);
        }
    }

    #endregion

    #region Key- Protected

    protected void lbtnAddKeyProtectedUsers_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxKeyProtectedUsers.Text))
        {
            proactive.UserRules[GetSelectedIndex()].ProtectedRegistryKeys.Add(tboxKeyProtectedUsers.Text);
            lboxKeyProtectedUsers.Items.Add(tboxKeyProtectedUsers.Text);
            tboxKeyProtectedUsers.Text = String.Empty;
        }
    }

    protected void lbtnDeleteKeyProtectedUsers_Click(Object sender, EventArgs e)
    {
        if (lboxKeyProtectedUsers.SelectedIndex > -1 && lboxKeyProtectedUsers.SelectedIndex < lboxKeyProtectedUsers.Items.Count)
        {
            proactive.UserRules[GetSelectedIndex()].ProtectedRegistryKeys.RemoveAt(lboxKeyProtectedUsers.SelectedIndex);
            lboxKeyProtectedUsers.Items.RemoveAt(lboxKeyProtectedUsers.SelectedIndex);
        }
    }

    #endregion

    #region Key- ReadOnly

    protected void lbtnAddKeyReadOnlyUsers_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxKeyReadOnlyUsers.Text))
        {
            proactive.UserRules[GetSelectedIndex()].ReadOnlyRegistryKeys.Add(tboxKeyReadOnlyUsers.Text);
            lboxKeyReadOnlyUsers.Items.Add(tboxKeyReadOnlyUsers.Text);
            tboxKeyReadOnlyUsers.Text = String.Empty;
        }
    }

    protected void lbtnDeleteKeyReadOnlyUsers_Click(Object sender, EventArgs e)
    {
        if (lboxKeyReadOnlyUsers.SelectedIndex > -1 && lboxKeyReadOnlyUsers.SelectedIndex < lboxKeyReadOnlyUsers.Items.Count)
        {
            proactive.UserRules[GetSelectedIndex()].ReadOnlyRegistryKeys.RemoveAt(lboxKeyReadOnlyUsers.SelectedIndex);
            lboxKeyReadOnlyUsers.Items.RemoveAt(lboxKeyReadOnlyUsers.SelectedIndex);
        }
    }

    #endregion

    #region Printers - Trusted

    protected void lbtnAddPrinterTrustedUsers_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxPrinterTrustedUsers.Text))
        {
            proactive.UserRules[GetSelectedIndex()].TrustedPrinters.Add(tboxPrinterTrustedUsers.Text);
            lboxPrinterTrustedUsers.Items.Add(tboxPrinterTrustedUsers.Text);
            tboxPrinterTrustedUsers.Text = String.Empty;
        }
    }

    protected void lbtnDeletePrinterTrustedUsers_Click(Object sender, EventArgs e)
    {
        if (lboxPrinterTrustedUsers.SelectedIndex > -1 && lboxPrinterTrustedUsers.SelectedIndex < lboxPrinterTrustedUsers.Items.Count)
        {
            proactive.UserRules[GetSelectedIndex()].TrustedPrinters.RemoveAt(lboxPrinterTrustedUsers.SelectedIndex);
            lboxPrinterTrustedUsers.Items.RemoveAt(lboxPrinterTrustedUsers.SelectedIndex);
        }
    }

    #endregion

    private Int32 GetSelectedIndex()
    {
        return proactive.UserRules.FindIndex(
            delegate(ProactiveRule rule)
            {
                if (rule.RuleName == ddlUsers.SelectedValue)
                    return true;
                return false;
            }
            );
    }

    #endregion

    #region Printers

    #region Printer - Trusted

    protected void lbtnAddPrinterTrusted_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxPrinterTrusted.Text))
        {
            proactive.GeneralRule.TrustedPrinters.Add(tboxPrinterTrusted.Text);
            lboxPrinterTrusted.Items.Add(tboxPrinterTrusted.Text);
            tboxPrinterTrusted.Text = String.Empty;
        }
    }

    protected void lbtnDeletePrinterTrusted_Click(Object sender, EventArgs e)
    {
        if (lboxPrinterTrusted.SelectedIndex > -1 && lboxPrinterTrusted.SelectedIndex < lboxPrinterTrusted.Items.Count)
        {
            proactive.GeneralRule.TrustedPrinters.RemoveAt(lboxPrinterTrusted.SelectedIndex);
            lboxPrinterTrusted.Items.RemoveAt(lboxPrinterTrusted.SelectedIndex);
        }
    }

    #endregion

    #endregion

    #endregion

    #region Users Manager

    protected void lbtnAddUser_Click(object sender, EventArgs e)
    {
        String userName = Request["__EVENTARGUMENT"];
        if (!String.IsNullOrEmpty(userName))
        {
            proactive.UserRules.Add(proactive.UserRules[0].Clone(userName));
            ddlUsers.Items.Add(new ListItem(userName, userName));
            ddlUsers.SelectedIndex = ddlUsers.Items.Count - 1;
            UpdateUsers(userName);
        }
    }

    protected void lbtnDeleteUser_Click(object sender, EventArgs e)
    {
        if (ddlUsers.SelectedIndex >= 0)
        {
            proactive.UserRules.RemoveAt(ddlUsers.SelectedIndex);
            ddlUsers.Items.RemoveAt(ddlUsers.SelectedIndex);
            UpdateUsers(ddlUsers.SelectedValue);
        }
    }

    protected void ddlUsers_Changed(object sender, EventArgs e)
    {
        UpdateUsers(ddlUsers.SelectedValue);        
    }

    #endregion

    #region Updates

    private void UpdateFields()
    {
        cboxIsUserAudit.Checked = proactive.IsUserAudit;
        tboxProcessedExtensions.Text = proactive.LogProcessedExtensions;

        cboxGeneralOn.Checked = proactive.IsEnabled;
        cboxUsersOn.Checked = proactive.IsUserFiltering;
        cboxPrintersOn.Checked = proactive.IsPrinterControl;
    }

    private void UpdateUsers(String userName)
    {
        Int32 index = proactive.UserRules.FindIndex(
            delegate(ProactiveRule rule)
            {
                if (rule.RuleName == userName)
                    return true;
                return false;
            }
            );
        FillUserRules(proactive.UserRules[index]);

        tboxAppsTrustedUsers.Text = String.Empty;
        tboxFileProtectedUsers.Text = tboxFileReadOnlyUsers.Text = String.Empty;
        tboxFolderProtectedUsers.Text = tboxFolderReadOnlyUsers.Text = String.Empty;
        tboxKeyProtectedUsers.Text = tboxKeyReadOnlyUsers.Text = String.Empty;
        tboxPrinterTrustedUsers.Text = String.Empty;

        upnlApplicationTrustedUsers.Update();
        upnlFileReadOnlyUsers.Update();
        upnlFileProtectedUsers.Update();
        upnlFolderReadOnlyUsers.Update();
        upnlFolderProtectedUsers.Update();
        upnlKeyReadOnlyUsers.Update();
        upnlKeyProtectedUsers.Update();
        upnlPrinterTrustedUsers.Update();
    }

    private void UpdateUserList()
    {
        ddlUsers.Items.Clear();
        proactive.UserRules.ForEach(AddUserName);
    }

    private void AddUserName(ProactiveRule rule)
    {
        ddlUsers.Items.Add(new ListItem(rule.RuleName, rule.RuleName));
    }
    
    private void FillUserRules(ProactiveRule rule)
    {
        lboxAppsTrustedUsers.Items.Clear();
        lboxFileReadOnlyUsers.Items.Clear();
        lboxFileProtectedUsers.Items.Clear();
        lboxFolderReadOnlyUsers.Items.Clear();
        lboxFolderProtectedUsers.Items.Clear();
        lboxKeyReadOnlyUsers.Items.Clear();
        lboxKeyProtectedUsers.Items.Clear();
        lboxPrinterTrustedUsers.Items.Clear();

        if (rule == null)
            return;

        foreach (String str in rule.TrustedApplications)
        {
            lboxAppsTrustedUsers.Items.Add(new ListItem(str, str));
        }


        foreach (String str in rule.ReadOnlyFiles)
        {
            lboxFileReadOnlyUsers.Items.Add(new ListItem(str, str));
        }


        foreach (String str in rule.ProtectedFiles)
        {
            lboxFileProtectedUsers.Items.Add(new ListItem(str, str));
        }


        foreach (String str in rule.ReadOnlyFolders)
        {
            lboxFolderReadOnlyUsers.Items.Add(new ListItem(str, str));
        }


        foreach (String str in rule.ProtectedFolders)
        {
            lboxFolderProtectedUsers.Items.Add(new ListItem(str, str));
        }


        foreach (String str in rule.ReadOnlyRegistryKeys)
        {
            lboxKeyReadOnlyUsers.Items.Add(new ListItem(str, str));
        }


        foreach (String str in rule.ProtectedRegistryKeys)
        {
            lboxKeyProtectedUsers.Items.Add(new ListItem(str, str));
        }

        foreach (String str in rule.TrustedPrinters)
        {
            lboxPrinterTrustedUsers.Items.Add(new ListItem(str, str));
        }
    }

    private void UpdatePrinters()
    {
        lboxPrinterTrusted.Items.Clear();
        foreach (String str in proactive.GeneralRule.TrustedPrinters)
        {
            lboxPrinterTrusted.Items.Add(new ListItem(str, str));
        }
    }

    private void UpdateGeneral()
    {
        lboxAppsTrusted.Items.Clear();
        foreach (String str in proactive.GeneralRule.TrustedApplications)
        {
            lboxAppsTrusted.Items.Add(new ListItem(str, str));
        }

        lboxAppsProtected.Items.Clear();
        foreach (String str in proactive.GeneralRule.ProtectedApplications)
        {
            lboxAppsProtected.Items.Add(new ListItem(str, str));
        }

        lboxFileReadOnly.Items.Clear();
        foreach (String str in proactive.GeneralRule.ReadOnlyFiles)
        {
            lboxFileReadOnly.Items.Add(new ListItem(str, str));
        }

        lboxFileProtected.Items.Clear();
        foreach (String str in proactive.GeneralRule.ProtectedFiles)
        {
            lboxFileProtected.Items.Add(new ListItem(str, str));
        }

        lboxFileExcluded.Items.Clear();
        foreach (String str in proactive.GeneralRule.ExcludedFiles)
        {
            lboxFileExcluded.Items.Add(new ListItem(str, str));
        }

        lboxFolderReadOnly.Items.Clear();
        foreach (String str in proactive.GeneralRule.ReadOnlyFolders)
        {
            lboxFolderReadOnly.Items.Add(new ListItem(str, str));
        }

        lboxFolderProtected.Items.Clear();
        foreach (String str in proactive.GeneralRule.ProtectedFolders)
        {
            lboxFolderProtected.Items.Add(new ListItem(str, str));
        }

        lboxFolderExcluded.Items.Clear();
        foreach (String str in proactive.GeneralRule.ExcludedFolders)
        {
            lboxFolderExcluded.Items.Add(new ListItem(str, str));
        }

        lboxKeyReadOnly.Items.Clear();
        foreach (String str in proactive.GeneralRule.ReadOnlyRegistryKeys)
        {
            lboxKeyReadOnly.Items.Add(new ListItem(str, str));
        }

        lboxKeyProtected.Items.Clear();
        foreach (String str in proactive.GeneralRule.ProtectedRegistryKeys)
        {
            lboxKeyProtected.Items.Add(new ListItem(str, str));
        }

        lboxValueReadOnly.Items.Clear();
        foreach (String str in proactive.GeneralRule.ReadOnlyRegistryValues)
        {
            lboxValueReadOnly.Items.Add(new ListItem(str, str));
        }

        lboxValueProtected.Items.Clear();
        foreach (String str in proactive.GeneralRule.ProtectedRegistryValues)
        {
            lboxValueProtected.Items.Add(new ListItem(str, str));
        }
    }

    #endregion

    #region JournalEvents

    private void InitFieldsJournalEvent(JournalEvent _events)
    {
        if (_events == null)
            return;

        if (JournalEventTable.Rows.Count == 1)
        {
            for (Int32 i = 0; i < _events.Events.Length; i++)
            {
                JournalEventTable.Rows.Add(GenerateRow(_events.Events[i], i));
            }
        }
    }

    private void LoadJournalEvent(JournalEvent _events)
    {
        if (_events == null)
            return;

        Boolean isChecked = false;
        for (Int32 i = 0; i < _events.Events.Length; i++)
        {
            for (Int32 j = 0; j < 3; j++)
            {
                switch (j)
                {
                    case 0:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.WindowsJournal) == EventJournalFlags.WindowsJournal;
                        break;
                    case 1:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.LocalJournal) == EventJournalFlags.LocalJournal;
                        break;
                    case 2:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.CCJournal) == EventJournalFlags.CCJournal;
                        break;
                }
                (JournalEventTable.Rows[i + 1].Cells[j + 1].Controls[0] as CheckBox).Checked = isChecked;
            }
        }
    }

    private void SaveJournalEvents()
    {
        JournalEvent je = new JournalEvent(GetEvents());
        for (Int32 i = 0; i < JournalEventTable.Rows.Count - 1; i++)
        {

            if ((JournalEventTable.Rows[i + 1].Cells[1].Controls[0] as CheckBox).Checked == false)
            {
                je.Events[i].EventFlag ^= EventJournalFlags.WindowsJournal;
            }
            if ((JournalEventTable.Rows[i + 1].Cells[2].Controls[0] as CheckBox).Checked == false)
            {
                je.Events[i].EventFlag ^= EventJournalFlags.LocalJournal;
            }
            if ((JournalEventTable.Rows[i + 1].Cells[3].Controls[0] as CheckBox).Checked == false)
            {
                je.Events[i].EventFlag ^= EventJournalFlags.CCJournal;
            }
        }
        proactive.journalEvent = je;
    }

    private TableRow GenerateRow(SingleJournalEvent ev, Int32 rowNo)
    {
        String eventName = ev.EventName;
        EventJournalFlags val = ev.EventFlag;

        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.Attributes.Add("align", "center");
        Label l = new Label();
        l.Text = eventName;
        cell.Controls.Add(l);
        row.Cells.Add(cell);
        for (Int32 i = 0; i < 3; i++)
        {
            cell = new TableCell();
            CheckBox chk = new CheckBox();
            chk.Checked = false;

            cell.Controls.Add(chk);
            cell.Attributes.Add("align", "center");
            row.Cells.Add(cell);
        }

        return row;
    }

    #endregion
}