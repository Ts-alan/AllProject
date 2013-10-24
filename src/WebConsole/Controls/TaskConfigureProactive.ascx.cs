using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Tasks.ProactiveProtection;

public partial class Controls_TaskConfigureProactive : System.Web.UI.UserControl, ITask
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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
        Clear();
        SetEnabled();
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
    }

    private void Clear()
    {
        tboxAppsProtected.Text = tboxAppsTrusted.Text = String.Empty;
        tboxFolderExcluded.Text = tboxFolderProtected.Text = tboxFolderReadOnly.Text = String.Empty;
        tboxFileExcluded.Text = tboxFileProtected.Text = tboxFileReadOnly.Text = String.Empty;
        tboxKeyProtected.Text = tboxKeyReadOnly.Text = String.Empty;
        tboxValueProtected.Text = tboxValueReadOnly.Text = String.Empty;

        lboxAppsTrusted.Items.Clear();
        lboxAppsProtected.Items.Clear();
        lboxFolderExcluded.Items.Clear();
        lboxFolderProtected.Items.Clear();
        lboxFolderReadOnly.Items.Clear();
        lboxFileExcluded.Items.Clear();
        lboxFileProtected.Items.Clear();
        lboxFileReadOnly.Items.Clear();
        lboxKeyProtected.Items.Clear();
        lboxKeyReadOnly.Items.Clear();
        lboxValueProtected.Items.Clear();
        lboxValueReadOnly.Items.Clear();
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ProactiveProtection;

        ValidateFields();

        task.Param = BuildXml();

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ProactiveProtection)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        TaskConfigureProactive tsk = new TaskConfigureProactive();
        tsk.LoadFromXml(task.Param);

        lboxAppsTrusted.Items.Clear();
        foreach (String str in tsk.TrustedApplications)
        {
            lboxAppsTrusted.Items.Add(new ListItem(str, str));
        }

        lboxAppsProtected.Items.Clear();
        foreach (String str in tsk.ProtectedApplications)
        {
            lboxAppsProtected.Items.Add(new ListItem(str, str));
        }

        lboxFileReadOnly.Items.Clear();
        foreach (String str in tsk.ReadOnlyFiles)
        {
            lboxFileReadOnly.Items.Add(new ListItem(str, str));
        }

        lboxFileProtected.Items.Clear();
        foreach (String str in tsk.ProtectedFiles)
        {
            lboxFileProtected.Items.Add(new ListItem(str, str));
        }

        lboxFileExcluded.Items.Clear();
        foreach (String str in tsk.ExcludedFiles)
        {
            lboxFileExcluded.Items.Add(new ListItem(str, str));
        }

        lboxFolderReadOnly.Items.Clear();
        foreach (String str in tsk.ReadOnlyFolders)
        {
            lboxFolderReadOnly.Items.Add(new ListItem(str, str));
        }

        lboxFolderProtected.Items.Clear();
        foreach (String str in tsk.ProtectedFolders)
        {
            lboxFolderProtected.Items.Add(new ListItem(str, str));
        }

        lboxFolderExcluded.Items.Clear();
        foreach (String str in tsk.ExcludedFolders)
        {
            lboxFolderExcluded.Items.Add(new ListItem(str, str));
        }

        lboxKeyReadOnly.Items.Clear();
        foreach (String str in tsk.ReadOnlyRegistryKeys)
        {
            lboxKeyReadOnly.Items.Add(new ListItem(str, str));
        }

        lboxKeyProtected.Items.Clear();
        foreach (String str in tsk.ProtectedRegistryKeys)
        {
            lboxKeyProtected.Items.Add(new ListItem(str, str));
        }

        lboxValueReadOnly.Items.Clear();
        foreach (String str in tsk.ReadOnlyRegistryValues)
        {
            lboxValueReadOnly.Items.Add(new ListItem(str, str));
        }

        lboxValueProtected.Items.Clear();
        foreach (String str in tsk.ProtectedRegistryValues)
        {
            lboxValueProtected.Items.Add(new ListItem(str, str));
        }
    }

    #endregion

    private String BuildXml()
    {
        TaskConfigureProactive task = new TaskConfigureProactive();

        foreach (ListItem item in lboxAppsTrusted.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.TrustedApplications.Add(item.Value);
        }

        foreach (ListItem item in lboxAppsProtected.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ProtectedApplications.Add(item.Value);
        }

        foreach (ListItem item in lboxFileReadOnly.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ReadOnlyFiles.Add(item.Value);
        }

        foreach (ListItem item in lboxFileProtected.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ProtectedFiles.Add(item.Value);
        }

        foreach (ListItem item in lboxFileExcluded.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ExcludedFiles.Add(item.Value);
        }

        foreach (ListItem item in lboxFolderReadOnly.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ReadOnlyFolders.Add(item.Value);
        }

        foreach (ListItem item in lboxFolderProtected.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ProtectedFolders.Add(item.Value);
        }

        foreach (ListItem item in lboxFolderExcluded.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ExcludedFolders.Add(item.Value);
        }

        foreach (ListItem item in lboxKeyReadOnly.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ReadOnlyRegistryKeys.Add(item.Value);
        }

        foreach (ListItem item in lboxKeyProtected.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ProtectedRegistryKeys.Add(item.Value);
        }

        foreach (ListItem item in lboxValueReadOnly.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ReadOnlyRegistryValues.Add(item.Value);
        }

        foreach (ListItem item in lboxValueProtected.Items)
        {
            if (!String.IsNullOrEmpty(item.Value))
                task.ProtectedRegistryValues.Add(item.Value);
        }

        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task.SaveToXml();
    }

    public String BuildTask(TaskUserEntity task)
    {
        if (task.Type != TaskType.ProactiveProtection)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        TaskConfigureProactive tsk = new TaskConfigureProactive();
        tsk.LoadFromXml(task.Param);
        return tsk.BuildTaskXml();        
    }

    #region Change ListBoxes

    #region Application - Trusted

    protected void lbtnAddAppsTrusted_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxAppsTrusted.Text))
        {
            lboxAppsTrusted.Items.Add(tboxAppsTrusted.Text);
            tboxAppsTrusted.Text = String.Empty;
        }
    }

    protected void lbtnDeleteAppsTrusted_Click(Object sender, EventArgs e)
    {
        if (lboxAppsTrusted.SelectedIndex > -1 && lboxAppsTrusted.SelectedIndex < lboxAppsTrusted.Items.Count)
            lboxAppsTrusted.Items.RemoveAt(lboxAppsTrusted.SelectedIndex);
    }

    #endregion

    #region Application - Protected

    protected void lbtnAddAppsProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxAppsProtected.Text))
        {
            lboxAppsProtected.Items.Add(tboxAppsProtected.Text);
            tboxAppsProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteAppsProtected_Click(Object sender, EventArgs e)
    {
        if (lboxAppsProtected.SelectedIndex > -1 && lboxAppsProtected.SelectedIndex < lboxAppsProtected.Items.Count)
            lboxAppsProtected.Items.RemoveAt(lboxAppsProtected.SelectedIndex);
    }

    #endregion

    #region Folder- Protected

    protected void lbtnAddFolderProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFolderProtected.Text))
        {
            lboxFolderProtected.Items.Add(tboxFolderProtected.Text);
            tboxFolderProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFolderProtected_Click(Object sender, EventArgs e)
    {
        if (lboxFolderProtected.SelectedIndex > -1 && lboxFolderProtected.SelectedIndex < lboxFolderProtected.Items.Count)
            lboxFolderProtected.Items.RemoveAt(lboxFolderProtected.SelectedIndex);
    }

    #endregion

    #region Folder- Excluded

    protected void lbtnAddFolderExcluded_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFolderExcluded.Text))
        {
            lboxFolderExcluded.Items.Add(tboxFolderExcluded.Text);
            tboxFolderExcluded.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFolderExcluded_Click(Object sender, EventArgs e)
    {
        if (lboxFolderExcluded.SelectedIndex > -1 && lboxFolderExcluded.SelectedIndex < lboxFolderExcluded.Items.Count)
            lboxFolderExcluded.Items.RemoveAt(lboxFolderExcluded.SelectedIndex);
    }

    #endregion

    #region Folder- ReadOnly

    protected void lbtnAddFolderReadOnly_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFolderReadOnly.Text))
        {
            lboxFolderReadOnly.Items.Add(tboxFolderReadOnly.Text);
            tboxFolderReadOnly.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFolderReadOnly_Click(Object sender, EventArgs e)
    {
        if (lboxFolderReadOnly.SelectedIndex > -1 && lboxFolderReadOnly.SelectedIndex < lboxFolderReadOnly.Items.Count)
            lboxFolderReadOnly.Items.RemoveAt(lboxFolderReadOnly.SelectedIndex);
    }

    #endregion

    #region File- Protected

    protected void lbtnAddFileProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFileProtected.Text))
        {
            lboxFileProtected.Items.Add(tboxFileProtected.Text);
            tboxFileProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFileProtected_Click(Object sender, EventArgs e)
    {
        if (lboxFileProtected.SelectedIndex > -1 && lboxFileProtected.SelectedIndex < lboxFileProtected.Items.Count)
            lboxFileProtected.Items.RemoveAt(lboxFileProtected.SelectedIndex);
    }

    #endregion

    #region File- Excluded

    protected void lbtnAddFileExcluded_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFileExcluded.Text))
        {
            lboxFileExcluded.Items.Add(tboxFileExcluded.Text);
            tboxFileExcluded.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFileExcluded_Click(Object sender, EventArgs e)
    {
        if (lboxFileExcluded.SelectedIndex > -1 && lboxFileExcluded.SelectedIndex < lboxFileExcluded.Items.Count)
            lboxFileExcluded.Items.RemoveAt(lboxFileExcluded.SelectedIndex);
    }

    #endregion

    #region File- ReadOnly

    protected void lbtnAddFileReadOnly_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxFileReadOnly.Text))
        {
            lboxFileReadOnly.Items.Add(tboxFileReadOnly.Text);
            tboxFileReadOnly.Text = String.Empty;
        }
    }

    protected void lbtnDeleteFileReadOnly_Click(Object sender, EventArgs e)
    {
        if (lboxFileReadOnly.SelectedIndex > -1 && lboxFileReadOnly.SelectedIndex < lboxFileReadOnly.Items.Count)
            lboxFileReadOnly.Items.RemoveAt(lboxFileReadOnly.SelectedIndex);
    }

    #endregion

    #region Key- Protected

    protected void lbtnAddKeyProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxKeyProtected.Text))
        {
            lboxKeyProtected.Items.Add(tboxKeyProtected.Text);
            tboxKeyProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteKeyProtected_Click(Object sender, EventArgs e)
    {
        if (lboxKeyProtected.SelectedIndex > -1 && lboxKeyProtected.SelectedIndex < lboxKeyProtected.Items.Count)
            lboxKeyProtected.Items.RemoveAt(lboxKeyProtected.SelectedIndex);
    }

    #endregion

    #region Key- ReadOnly

    protected void lbtnAddKeyReadOnly_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxKeyReadOnly.Text))
        {
            lboxKeyReadOnly.Items.Add(tboxKeyReadOnly.Text);
            tboxKeyReadOnly.Text = String.Empty;
        }
    }

    protected void lbtnDeleteKeyReadOnly_Click(Object sender, EventArgs e)
    {
        if (lboxKeyReadOnly.SelectedIndex > -1 && lboxKeyReadOnly.SelectedIndex < lboxKeyReadOnly.Items.Count)
            lboxKeyReadOnly.Items.RemoveAt(lboxKeyReadOnly.SelectedIndex);
    }

    #endregion

    #region Value- Protected

    protected void lbtnAddValueProtected_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxValueProtected.Text))
        {
            lboxValueProtected.Items.Add(tboxValueProtected.Text);
            tboxValueProtected.Text = String.Empty;
        }
    }

    protected void lbtnDeleteValueProtected_Click(Object sender, EventArgs e)
    {
        if (lboxValueProtected.SelectedIndex > -1 && lboxValueProtected.SelectedIndex < lboxValueProtected.Items.Count)
            lboxValueProtected.Items.RemoveAt(lboxValueProtected.SelectedIndex);
    }

    #endregion
    
    #region Value- ReadOnly

    protected void lbtnAddValueReadOnly_Click(Object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(tboxValueReadOnly.Text))
        {
            lboxValueReadOnly.Items.Add(tboxValueReadOnly.Text);
            tboxValueReadOnly.Text = String.Empty;
        }
    }

    protected void lbtnDeleteValueReadOnly_Click(Object sender, EventArgs e)
    {
        if (lboxValueReadOnly.SelectedIndex > -1 && lboxValueReadOnly.SelectedIndex < lboxValueReadOnly.Items.Count)
            lboxValueReadOnly.Items.RemoveAt(lboxValueReadOnly.SelectedIndex);
    }

    #endregion
    
    #endregion
}