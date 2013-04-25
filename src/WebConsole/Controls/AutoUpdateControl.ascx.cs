using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Common;
using ARM2_dbcontrol.DataBase;
using System.Reflection;

public partial class Controls_AutoUpdateControl : System.Web.UI.UserControl
{
    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFields();
        }
    }

    protected void InitFields()
    {
        cboxAutoUpdate.Text = Resources.Resource.Fix;
        cboxAutoUpdate.Checked = enableAutoUpdate;
        timerAutoUpdate.Interval = intervalAutoUpdate * 1000;
        timerAutoUpdate.Enabled = enableAutoUpdate;
    }
    #endregion

    #region Properties
    #region Data
    private InformationListTypes _informationListType = InformationListTypes.None;
    public InformationListTypes InformationListType
    {
        get { return _informationListType; }
        set { _informationListType = value; }
    }
    #endregion

    #region Events
    public event EventHandler<EventArgs> AutoUpdate;
    protected void OnAutoUpdate()
    {
        EventHandler<EventArgs> temp = AutoUpdate;
        if (temp != null)
        {

            temp(this, new EventArgs());
        }
    }
    #endregion

    #region AutoUpdate
    private const string intervalAutoUpdateCommon = "IntervalAutoUpdate";
    private PropertyInfo _intervalAutoUpdateProperty;
    private PropertyInfo intervalAutoUpdateProperty
    {
        get
        {
            if (_intervalAutoUpdateProperty == null)
            {
                string strInformationListType = Enum.GetName(typeof(InformationListTypes),
                    _informationListType);
                string intervalAutoUpdateProperyName = intervalAutoUpdateCommon + strInformationListType;
                _intervalAutoUpdateProperty = (typeof(SettingsEntity)).GetProperty(intervalAutoUpdateProperyName, typeof(int));
                if (_intervalAutoUpdateProperty == null)
                {
                    throw new InvalidOperationException(String.Format("Can't find auto update interval corresponding to {0}'s InformationListType",
                        ClientID));
                }
            }
            return _intervalAutoUpdateProperty;
        }
    }
    protected int intervalAutoUpdate
    {
        get
        {
            SettingsEntity settings = SettingsEntityAccessor.GetSettingsEntity();
            return (int)intervalAutoUpdateProperty.GetValue(settings, null);
        }
    }

    private const string enableAutoUpdateCommon = "EnableAutoUpdate";
    private PropertyInfo _enableAutoUpdateProperty;
    private PropertyInfo enableAutoUpdateProperty
    {
        get
        {
            if (_enableAutoUpdateProperty == null)
            {
                string strInformationListType = Enum.GetName(typeof(InformationListTypes),
                    _informationListType);
                string enableAutoUpdateProperyName = enableAutoUpdateCommon + strInformationListType;
                _enableAutoUpdateProperty = (typeof(SettingsEntity)).GetProperty(enableAutoUpdateProperyName, typeof(bool));
                if (_enableAutoUpdateProperty == null)
                {
                    throw new InvalidOperationException(String.Format("Can't find auto update enable corresponding to {0}'s InformationListType",
                        ClientID));
                }
            }
            return _enableAutoUpdateProperty;
        }
    }
    protected bool enableAutoUpdate
    {
        get
        {
            SettingsEntity settings = SettingsEntityAccessor.GetSettingsEntity();
            return (bool)enableAutoUpdateProperty.GetValue(settings, null);
        }
        set 
        {
            SettingsEntity settings = SettingsEntityAccessor.GetSettingsEntity();
            enableAutoUpdateProperty.SetValue(settings, value, null);
            SettingsEntityAccessor.SetSettingsEntity(settings);
        }
    }
    #endregion
    #endregion

    #region Autoupdate
    protected void timerAutoUpdate_Tick(object sender, EventArgs e)
    {
        OnAutoUpdate();
    }
    protected void cboxAutoUpdate_CheckedChanged(object sender, EventArgs e)
    {       
        enableAutoUpdate = cboxAutoUpdate.Checked;
        timerAutoUpdate.Enabled = cboxAutoUpdate.Checked;
    }
    #endregion
}