using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CustomControls;
using System.ComponentModel;
using VirusBlokAda.Vba32CC.Common.Collection;
using Filters.Primitive;
using Filters.Common;
using Filters.Composite;
using VirusBlokAda.Vba32CC.Common;

[ParseChildren(true)]
[PersistChildren(false)]
public partial class Controls_CompositeFilter : System.Web.UI.UserControl, ICompositeFilter
{
    #region Template
    private ITemplate _filtersTemplate;
    private Control _filtersTemplateContainer;

    [
        TemplateInstance(TemplateInstance.Single),
        PersistenceMode(PersistenceMode.InnerProperty),
        Browsable(false)
    ]
    public ITemplate FiltersTemplate
    {
        get
        {
            return _filtersTemplate;
        }
        set
        {
            if (!base.DesignMode)
            {
                if (_filtersTemplateContainer != null)
                {
                    throw new InvalidOperationException(String.Format("The FiltersTemplate of CompositeFilter with ID '{0}' cannot be set after the template has been instantiated or the filters template container has been created."
                    , ID));
                }
                _filtersTemplate = value;
            }
            else
            {
                _filtersTemplate = value;
                CreateFilters(true);
            }
        }

    }

    [Browsable(false)]
    public Control FiltersTemplateContainer
    {
        get
        {
            if (_filtersTemplateContainer == null)
            {
                _filtersTemplateContainer = CreateFiltersTemplateContainer();
                AddFiltersTemplateContainer();
            }
            return this._filtersTemplateContainer;
        }
    }

    protected virtual Control CreateFiltersTemplateContainer()
    {
        return new Control();
    }

    private void AddFiltersTemplateContainer()
    {
        FiltersPlaceHolder.Controls.Add(_filtersTemplateContainer);
    }

    private void ClearFiltersTemplateContainer()
    {
        FiltersPlaceHolder.Controls.Clear();
    }

    private void CreateFilters(bool recreate)
    {
        if (recreate)
        {
            FiltersTemplateContainer.Controls.Clear();
            _filtersTemplateContainer = null;
            ClearFiltersTemplateContainer();
        }
        if (_filtersTemplateContainer == null)
        {
            _filtersTemplateContainer = CreateFiltersTemplateContainer();
            if (_filtersTemplate != null)
            {
                _filtersTemplate.InstantiateIn(_filtersTemplateContainer);
            }
            AddFiltersTemplateContainer();
        }
        else if (FiltersTemplate != null)
        {
            throw new InvalidOperationException(String.Format("Cannot instantiate the FiltersTemplate in the Init event when the FiltersTemplateContainer was already created manually in CompositeFilter with ID '{0}'.",
                ID));
        }
    }
    #endregion

    #region LifeCycle

    private static readonly string temproraryFilterValue = "TemproraryFilter";
    private static readonly string temproraryGroupFilterValue = "TemproraryGroupFilter";

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        GenerateStorageNames();
        CreateFilters(base.DesignMode);
        CollapsiblePanelSwitch1.Initialize(divDetails);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitDDLUserFilters();
            InitSaveAsDialog();

            SetVisibleSaveDelete(false);
            FilteringOn = false;
            ddlUserFilters.SelectedValue = temproraryFilterValue;

            if (CurrentUserFilterName == temproraryFilterValue)
            {
                FilteringOn = true;
            }
            else if (CurrentUserFilterName == temproraryGroupFilterValue)
            {
                TemporaryGroupFilter1.FilteringOn = true;
            }
            else if (!String.IsNullOrEmpty(CurrentUserFilterName))
            {
                ddlUserFilters.SelectedValue = CurrentUserFilterName;
                FilteringOn = true;
                SetVisibleSaveDelete(true);
            }
            LoadCurrentFilterState();
        }
        RegisterScripts();
    }


    private void SetVisibleSaveDelete(bool visible)
    {
        divDelete.Visible = visible;
        divSave.Visible = visible;
    }

    protected void InitDDLUserFilters()
    {
        ddlUserFilters.Items.Clear();
        ddlUserFilters.Items.Add(new ListItem(Resources.Resource.TemporaryFilter, temproraryFilterValue));
        foreach (string userFilterName in UserFilters.GetNames())
        {
            ddlUserFilters.Items.Add(new ListItem(userFilterName, userFilterName));
        }
    }

    private void RegisterScripts()
    {
        Page.ClientScript.RegisterClientScriptInclude("Ext-All", @"js/Groups/ext-4.1.1/ext-all-debug.js");
        Page.ClientScript.RegisterClientScriptInclude("PageRequestManagerHelper", @"js/PageRequestManagerHelper.js");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (InformationListType == InformationListTypes.None)
        {
            TemporaryGroupFilter1.Visible = false;
        }
    }

    private void InitSaveAsDialog()
    {
        saveAsDialogFilter.RestrictedNames = String.Format("['{0}']", Resources.Resource.TemporaryFilter);
        if (UserFilters.GetNames().Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            bool isFirst = true;
            foreach (string userFilterName in UserFilters.GetNames())
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                }
                else
                {
                    isFirst = false;
                }
                sb.AppendFormat("'{0}'", userFilterName);
            }
            sb.Append("]");
            saveAsDialogFilter.UsedNames = sb.ToString();
        }
    }
    #endregion

    #region ChildFilters
    private bool enumeratedChildFilters = false;

    protected void EnumerateChildFilters()
    {
        EnumerateChildFilters(false);
    }
    protected void EnumerateChildFilters(bool force)
    {
        if (enumeratedChildFilters && !force)
        {
            return;
        }
        childFilters = new List<IPrimitiveFilter>();
        if (FiltersPlaceHolder.Controls != null)
        {
            EnumerateChildFiltersRecursive(FiltersPlaceHolder.Controls);
        }
        enumeratedChildFilters = true;
    }

    protected void EnumerateChildFiltersRecursive(ControlCollection controls)
    {
        foreach (Control next in controls)
        {
            if (next.Controls != null)
            {
                EnumerateChildFiltersRecursive(next.Controls);
            }
            IPrimitiveFilter filter = next as IPrimitiveFilter;
            if (filter != null)
            {
                childFilters.Add(filter);
            }
        }
    }

    private List<IPrimitiveFilter> childFilters = null;
    #endregion

    #region Properties
    #region Data
    private void GenerateStorageNames()
    {
        CurrentFilterStateStorage.StorageName = PageName + "_CurrentFilterState";
        CurrentUserFilterNameStorage.StorageName = PageName + "_CurrentUserFilterName";
    }

    private string PageName
    {
        get { return Page.GetType().Name; }
    }

    protected SerializableDictionary<string, PrimitiveFilterState> CurrentFilterState
    {
        get
        {
            if (CurrentFilterStateStorage.Storage == null)
            {
                CurrentFilterStateStorage.Storage = new SerializableDictionary<string, PrimitiveFilterState>();
            }
            return (CurrentFilterStateStorage.Storage as SerializableDictionary<string, PrimitiveFilterState>);
        }
    }

    protected string CurrentUserFilterName
    {
        get
        {
            if (CurrentUserFilterNameStorage.Storage == null)
            {
                CurrentUserFilterNameStorage.Storage = String.Empty;
            }
            return (CurrentUserFilterNameStorage.Storage as string);
        }
        set
        {
            CurrentUserFilterNameStorage.Storage = value;
        }
    }

    [
        Category("Data"),
        DefaultValue(""),
        Description("User filters temprorary storage name"),
    ]
    public string UserFiltersTemproraryStorageName
    {
        get
        {
            return UserFiltersTemproraryStorage.StorageName;
        }
        set
        {
            UserFiltersTemproraryStorage.StorageName = value;
        }
    }

    protected CompositeFilterStateCollection UserFilters
    {
        get
        {
            if (UserFiltersTemproraryStorage.Storage == null)
            {
                UserFiltersTemproraryStorage.Storage =
                    CompositeFilterStateCollection.Deserialize(Profile.GetPropertyValue(userFiltersProfileKey) as string);
            }
            return (UserFiltersTemproraryStorage.Storage as CompositeFilterStateCollection);
        }
    }

    private string userFiltersProfileKey;
    [
        Category("Data"),
        DefaultValue(""),
        Description("User filters profile key"),
    ]
    public string UserFiltersProfileKey
    {
        get
        {
            return userFiltersProfileKey;
        }
        set
        {
            userFiltersProfileKey = value;
        }
    }

    public InformationListTypes InformationListType
    {
        get { return TemporaryGroupFilter1.InformationListType; }
        set { TemporaryGroupFilter1.InformationListType = value; }
    }
    #endregion

    #region Events
    [
       Category("Action"),
       Description("Active filter change event"),
    ]
    public event EventHandler<FilterEventArgs> ActiveFilterChange;
    protected void OnActiveFilterChange(string where)
    {
        TemporaryGroupFilter1.Where = where;
        EventHandler<FilterEventArgs> temp = ActiveFilterChange;
        if (temp != null)
        {

            temp(this, new FilterEventArgs(where));
        }
    }
    #endregion

    protected bool FilteringOn
    {
        get
        {
            return (divFilterHeader.Attributes["class"] == "GiveButton1");
        }
        set
        {
            divFilterHeader.Attributes["class"] = String.Format("GiveButton{0}", value ? "1" : "");
        }
    }

    #endregion

    #region Drop Down List
    protected void ddlUserFilters_SelectedIndexChanged(object sender, EventArgs e)
    {
        string userFilterName = ddlUserFilters.SelectedValue;
        if (userFilterName == temproraryFilterValue)
        {
            ClearFilter();
        }
        else
        {
            SetVisibleSaveDelete(true);

            CompositeFilterState entity = UserFilters.Get(userFilterName);
            LoadCurrentFilterStateFromDictionary(entity.State);
            if (FilteringOn)
            {
                CurrentUserFilterName = userFilterName;
                SaveCurrentFilterState();
                OnActiveFilterChange(GenerateSQL());
            }
        }
    }
    #endregion

    #region Helper Methods
    protected void ClearFilter()
    {
        Clear();
        ClearCurrentFilterState();
        OnActiveFilterChange("");
        CurrentUserFilterName = String.Empty;
        SetVisibleSaveDelete(false);
        ddlUserFilters.SelectedIndex = 0;
        FilteringOn = false;
        TemporaryGroupFilter1.FilteringOn = false;
    }

    protected void ApplyFilter()
    {
        if (Validate())
        {
            SaveCurrentFilterState();
            OnActiveFilterChange(GenerateSQL());
            CurrentUserFilterName = ddlUserFilters.SelectedValue;
            FilteringOn = true;
            TemporaryGroupFilter1.FilteringOn = false;
        }
    }

    private void UpdateUserFiltersProfile()
    {
        Profile.SetPropertyValue(userFiltersProfileKey, UserFilters.Serialize());
    }

    #endregion

    #region Buttons
    protected void lbtnApply_Click(object sender, EventArgs e)
    {
        ApplyFilter();
    }

    protected void lbtnSaveAs_Click(object sender, EventArgs e)
    {
        if (!Validate()) return;
        string userFilterName = saveAsDialogFilter.SaveAsName;
        SerializableDictionary<string, PrimitiveFilterState> state = null;
        if (FilteringOn)
        {
            SaveCurrentFilterState();
            state = new SerializableDictionary<string, PrimitiveFilterState>(CurrentFilterState);
        }
        else
        {
            state = SaveCurrentFilterStateToDictionary();
        }

        UserFilters.Update(new CompositeFilterState(userFilterName, state));
        UpdateUserFiltersProfile();
        InitDDLUserFilters();
        InitSaveAsDialog();
        SetVisibleSaveDelete(true);
        ddlUserFilters.SelectedValue = userFilterName;

        if (FilteringOn)
        {
            CurrentUserFilterName = userFilterName;
            OnActiveFilterChange(GenerateSQL());
        }
    }

    protected void lbtnClear_Click(object sender, EventArgs e)
    {
        ClearFilter();
    }

    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        if (!Validate()) return;
        SerializableDictionary<string, PrimitiveFilterState> state = null;
        if (FilteringOn)
        {
            SaveCurrentFilterState();
            state = new SerializableDictionary<string, PrimitiveFilterState>(CurrentFilterState);
        }
        else
        {
            state = SaveCurrentFilterStateToDictionary();
        }

        UserFilters.Update(new CompositeFilterState(ddlUserFilters.SelectedValue, state));
        UpdateUserFiltersProfile();

        if (FilteringOn)
        {
            OnActiveFilterChange(GenerateSQL());
        }
    }



    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        UserFilters.Delete(ddlUserFilters.SelectedValue);
        UpdateUserFiltersProfile();
        InitDDLUserFilters();
        InitSaveAsDialog();
        ClearFilter();
    }

    protected void lbtnFilterHeader_Click(object sender, EventArgs e)
    {
        if (!FilteringOn)
        {
            ApplyFilter();
        }
        else
        {
            ClearFilter();
        }
    }
    #endregion

    #region ICompositeFilterMembers
    public void Clear()
    {
        EnumerateChildFilters();
        if (childFilters == null) return;
        foreach (IPrimitiveFilter filter in childFilters)
        {
            filter.Clear();
        }
    }

    public bool Validate()
    {
        EnumerateChildFilters();
        if (childFilters == null) return true;
        bool valid = true;
        foreach (IPrimitiveFilter filter in childFilters)
        {
            if (!filter.Validate())
            {
                valid = false;
                break;
            }
        }
        return valid;
    }

    public string GenerateSQL()
    {
        EnumerateChildFilters();
        if (childFilters == null) return String.Empty;
        if (childFilters.Count == 0) return String.Empty;

        StringBuilder where = new StringBuilder();
        string nextStr = String.Empty;

        for (int i = 0; i < childFilters.Count; i++)
        {
            if (where.ToString() == String.Empty)
            {
                nextStr = childFilters[i].GenerateSQL().TrimStart();
                if (nextStr.ToUpper().StartsWith("AND"))
                {
                    nextStr = nextStr.Remove(0, 3);
                }
                else if (nextStr.ToUpper().StartsWith("OR"))
                {
                    nextStr = nextStr.Remove(0, 2);
                }
            }
            else
            {
                nextStr = childFilters[i].GenerateSQL();
            }
            where.Append(nextStr);
        }
        return where.ToString();
    }


    protected void SaveCurrentFilterState()
    {
        EnumerateChildFilters();
        if (childFilters == null) return;
        CurrentFilterState.Clear();
        foreach (IPrimitiveFilter filter in childFilters)
        {
            CurrentFilterState.Add(filter.GetID(), filter.SaveState());
        }
    }

    protected SerializableDictionary<string, PrimitiveFilterState> SaveCurrentFilterStateToDictionary()
    {
        SerializableDictionary<string, PrimitiveFilterState> state =
            new SerializableDictionary<string, PrimitiveFilterState>();
        EnumerateChildFilters();
        if (childFilters == null) return state;
        foreach (IPrimitiveFilter filter in childFilters)
        {
            state.Add(filter.GetID(), filter.SaveState());
        }
        return state;
    }

    protected void ClearCurrentFilterState()
    {
        CurrentFilterState.Clear();
    }

    protected void LoadCurrentFilterState()
    {
        EnumerateChildFilters();
        if (childFilters == null) return;
        foreach (IPrimitiveFilter filter in childFilters)
        {
            PrimitiveFilterState value = null;
            if (CurrentFilterState.TryGetValue(filter.GetID(), out value))
            {
                filter.LoadState(value);
            }
        }
    }

    protected void LoadCurrentFilterStateFromDictionary(SerializableDictionary<string, PrimitiveFilterState> dict)
    {
        EnumerateChildFilters();
        if (childFilters == null) return;
        foreach (IPrimitiveFilter filter in childFilters)
        {
            PrimitiveFilterState value = null;
            if (dict.TryGetValue(filter.GetID(), out value))
            {
                filter.LoadState(value);
            }
        }
    }

    #endregion

    #region Temprorary Group Filter
    protected void TemporaryGroupFilter1_TemporaryGroupFilterChanged(object sender, FilterEventArgs e)
    {
        FilteringOn = false;
        ClearCurrentFilterState();
        if (!String.IsNullOrEmpty(e.Where))
        {
            CurrentUserFilterName = temproraryGroupFilterValue;
        }
        else
        {
            CurrentUserFilterName = "";
        }
        OnActiveFilterChange(e.Where);
    }
    #endregion
}