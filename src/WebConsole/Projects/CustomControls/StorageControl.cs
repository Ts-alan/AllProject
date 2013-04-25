﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace CustomControls
{
    [
        NonVisualControl,
        Bindable(false),
        DefaultProperty("ID"),
        ToolboxData("<{0}:StorageControl runat=server></{0}:StorageControl>")
    ]
    public class StorageControl : Control
    {
        #region LifeCycle
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
        }
        #endregion

        #region Properties
        private StorageTypeEnum _storageType = StorageTypeEnum.None;
        [
            Category("Behavior"),
            DefaultValue(StorageTypeEnum.None),
            Description("Storage type"),
            NotifyParentProperty(true),
        ]
        public StorageTypeEnum StorageType
        {
            get { return _storageType; }
            set { _storageType = value; }
        }


        private string _storageName = String.Empty;
        [
            Category("Behavior"),
            DefaultValue(""),
            Description("Storage name"),
            NotifyParentProperty(true),
        ]
        public string StorageName
        {
            get
            {
                if (String.IsNullOrEmpty(_storageName))
                {
                    _storageName = ID;
                }
                return _storageName;
            }
            set { _storageName = value; }
        }

        [
            Category("Data"),
            PersistenceMode(PersistenceMode.InnerProperty),
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public object Storage
        {
            get
            {
                if (_storageType == StorageTypeEnum.Session)
                {
                    return Context.Session[SessionStorageKey];
                }
                else if (_storageType == StorageTypeEnum.Application)
                {
                    return Context.Application[ApplicationStorageKey];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_storageType == StorageTypeEnum.Session)
                {
                    Context.Session[SessionStorageKey] = value;
                }
                else if (_storageType == StorageTypeEnum.Application)
                {
                    Context.Application[ApplicationStorageKey] = value;
                }

            }
        }
        #endregion

        #region Helper Attributes
        private string ClassName
        {
            get
            {
                return GetType().Name;
            }
        }

        private string _sessionStorageKey = null;
        private string SessionStorageKey
        {
            get
            {
                if (_sessionStorageKey == null)
                {
                    _sessionStorageKey = String.Format("{0}_{1}", StorageName, ClassName);
                }
                return _sessionStorageKey;
            }
        }

        private string _applicationStorageKey = null;
        private string ApplicationStorageKey
        {
            get
            {
                if (_applicationStorageKey == null)
                { 
                    _applicationStorageKey = String.Format("{0}_{1}_{2}", Page.User.Identity.Name,
                    StorageName, ClassName);
                }
                return _applicationStorageKey;
            }
        }
        #endregion

        #region ViewState
        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            _storageName = (string)ctlState[1];
            _storageType = (StorageTypeEnum)ctlState[2];
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[3];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = _storageName;
            ctlState[2] = _storageType;
            return ctlState;
        }
        #endregion

        #region Hidden Properties
        // Summary:
        //     Gets the server control identifier generated by ASP.NET.
        //
        // Returns:
        //     The server control identifier generated by ASP.NET.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ClientID { get { return String.Empty; } }
        //
        // Summary:
        //     Gets a System.Web.UI.ControlCollection object that represents the child controls
        //     for a specified server control in the UI hierarchy.
        //
        // Returns:
        //     The collection of child controls for the specified server control.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ControlCollection Controls { get { return null; } }
        //
        // Summary:
        //     Gets a value indicating whether this control supports themes.
        //
        // Returns:
        //     false in all cases.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     An attempt was made to set the value of the System.Web.UI.DataSourceControl.EnableTheming
        //     property.
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override bool EnableTheming
        {
            get { return base.EnableTheming; }
            set { base.EnableTheming = value; }
        }
        //
        // Summary:
        //     Gets the skin to apply to the System.Web.UI.DataSourceControl control.
        //
        // Returns:
        //     System.String.Empty.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     An attempt was made to set the value of the System.Web.UI.DataSourceControl.SkinID
        //     property.
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override string SkinID
        {
            get { return base.SkinID; }
            set { base.SkinID = value; }
        }
        //
        // Summary:
        //     Gets or sets a value indicating whether the control is visually displayed.
        //
        // Returns:
        //     Always false.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     An attempt was made to set the value of the System.Web.UI.DataSourceControl.Visible
        //     property.
        [Browsable(false)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }
        #endregion
    }
}
