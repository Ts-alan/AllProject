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
using System.ComponentModel;
using VirusBlokAda.CC.Filters.Primitive;
using System.Collections.Generic;
using System.Diagnostics;

[ParseChildren(true)]
[PersistChildren(false)]
public partial class Controls_PrimitiveFilterTemplate : System.Web.UI.UserControl, IPrimitiveTemplateFilter
{
    #region Template
    private ITemplate _filterTemplate;
    private Control _filterTemplateContainer;

    [
        TemplateInstance(TemplateInstance.Single),
        PersistenceMode(PersistenceMode.InnerProperty),
        Browsable(false)
    ]
    public ITemplate FilterTemplate
    {
        get
        {
            return _filterTemplate;
        }
        set
        {
            if (!base.DesignMode)
            {
                if (_filterTemplateContainer != null)
                {
                    throw new InvalidOperationException(String.Format("The FilterTemplate of PrimitiveFilterTemplate with ID '{0}' cannot be set after the template has been instantiated or the filter template container has been created."
                    , ID));
                }
                _filterTemplate = value;
            }
            else
            {
                _filterTemplate = value;
                CreateFilter(true);
            }
        }

    }

    [Browsable(false)]
    public Control FilterTemplateContainer
    {
        get
        {
            if (_filterTemplateContainer == null)
            {
                _filterTemplateContainer = CreateFilterTemplateContainer();
                AddFilterTemplateContainer();
            }
            return this._filterTemplateContainer;
        }
    }

    protected virtual Control CreateFilterTemplateContainer()
    {
        return new Control();
    }

    private void AddFilterTemplateContainer()
    {
        placeHolderFilter.Controls.Add(_filterTemplateContainer);
    }

    private void ClearFilterTemplateContainer()
    {
        placeHolderFilter.Controls.Clear();
    }

    private void CreateFilter(bool recreate)
    {
        if (recreate)
        {
            FilterTemplateContainer.Controls.Clear();
            _filterTemplateContainer = null;
            ClearFilterTemplateContainer();
        }
        if (_filterTemplateContainer == null)
        {
            _filterTemplateContainer = CreateFilterTemplateContainer();
            if (_filterTemplate != null)
            {
                _filterTemplate.InstantiateIn(_filterTemplateContainer);
            }
            AddFilterTemplateContainer();
        }
        else if (FilterTemplate != null)
        {
            throw new InvalidOperationException(String.Format("Cannot instantiate the FiltersTemplate in the Init event when the FiltersTemplateContainer was already created manually in CompositeFilter with ID '{0}'.",
                ID));
        }
    }
    #endregion

    #region LifeCycle

    protected void Page_Load(object sender, EventArgs e)
    {        
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        CreateFilter(base.DesignMode);
        if (!IsPostBack)
        {
            InitFields();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        EnableFields();
    }

    private void EnableFields()
    {
        bool enable = cboxUseFilter.Checked;
        RecursiveEnable(placeHolderFilter, enable);
        if (enable)
        {
            lblNameFilter.Attributes.Remove("disabled");
            ddlAndOr.Attributes.Remove("disabled");
            ddlNot.Attributes.Remove("disabled");
        }
        else
        {
            lblNameFilter.Attributes.Add("disabled", "disabled");
            ddlAndOr.Attributes.Add("disabled", "disabled");
            ddlNot.Attributes.Add("disabled", "disabled");
        }
    }

    private void RecursiveEnable(Control control, bool enable)
    {        
        //validators
        BaseValidator validator = control as BaseValidator;
        if (validator != null)
        {
            if (validator.Attributes["radioDisabled"] != "")
            {                
                validator.Enabled = enable;
            }
        }
        //controls
        WebControl webcontrol = control as WebControl;
        if (webcontrol != null)
        {
            if (webcontrol.Attributes["radioDisabled"] != "")
            {
                if (enable)
                {
                    webcontrol.Attributes.Remove("disabled");
                }
                else
                {
                    webcontrol.Attributes.Add("disabled", "disabled");
                }
            }
        }
        
        //for html controls
        HtmlControl htmlcontrol = control as HtmlControl;
        if (htmlcontrol != null)
        {
            if (htmlcontrol.Attributes["radioDisabled"] != "")
            {
                if (enable)
                {
                    htmlcontrol.Attributes.Remove("disabled");
                }
                else
                {
                    htmlcontrol.Attributes.Add("disabled", "disabled");
                }
            }
        }

        if (control.Controls != null)
        {
            foreach (Control next in control.Controls)
            {
                RecursiveEnable(next, enable);
            }
        }
    }

    protected void InitFields()
    {
        lblNameFilter.Text = TextFilter;        
        ddlAndOr.Items.Add(Resources.Resource.AND);
        ddlAndOr.Items.Add(Resources.Resource.OR);
        ddlNot.Items.Add(String.Empty);
        ddlNot.Items.Add(Resources.Resource.NOT);
        ddlAndOr.Visible = ddlNot.Visible = _isVisibleLogic;
        divPrimitiveFilterTemplate.Style.Remove(HtmlTextWriterStyle.Height);
        divPrimitiveFilterTemplate.Style.Add(HtmlTextWriterStyle.Height, _height);
    }

    #endregion

    #region IPrimitiveTemplateFilter Members

    public PrimitiveFilterState SaveState()
    {
        return new PrimitiveFilterState(ddlAndOr.SelectedIndex == 1, ddlNot.SelectedIndex == 1, cboxUseFilter.Checked, String.Empty);
    }

    public void LoadState(PrimitiveFilterState savedState)
    {
        if (savedState != null)
        {
            ddlAndOr.SelectedIndex = savedState.Or ? 1 : 0;
            ddlNot.SelectedIndex = savedState.Not ? 1 : 0;
            cboxUseFilter.Checked = savedState.IsSelected;
        }
    }

    public string GetID()
    {
        return NameFieldDB;
    }

    public void Clear()
    {
        cboxUseFilter.Checked = false;
        ddlAndOr.SelectedIndex = 0;
        ddlNot.SelectedIndex = 0;
    }

    #endregion

    #region Properties
    private String _height = "30px";
    public String Height
    {
        get { return _height; }
        set
        {
            _height = value;
            divPrimitiveFilterTemplate.Style.Remove(HtmlTextWriterStyle.Height);
            divPrimitiveFilterTemplate.Style.Add(HtmlTextWriterStyle.Height, value);
        }
    }
    
    private String _textFilter = String.Empty;
    public String TextFilter
    {
        get { return _textFilter; }
        set { _textFilter = value; }
    }

    private String _nameFieldDB = String.Empty;
    public String NameFieldDB
    {
        get { return _nameFieldDB; }
        set { _nameFieldDB = value; }
    }

    public bool IsSelected
    {
        get { return cboxUseFilter.Checked; }
    }

    public bool UseOR
    {
        get { return ddlAndOr.SelectedIndex == 1; }
    }
    
    public bool UseNOT
    {
        get { return ddlNot.SelectedIndex == 1; }
    }

    private Boolean _isVisibleLogic = true;
    public Boolean IsVisibleLogic
    {
        get { return _isVisibleLogic; }
        set { _isVisibleLogic = value; }
    }
    #endregion


}
