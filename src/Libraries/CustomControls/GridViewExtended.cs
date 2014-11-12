using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using VirusBlokAda.CC.Common;

namespace VirusBlokAda.CC.CustomControls
{
    public class GridViewExtended : GridView
    {
        #region Properties
        private StorageTypeEnum _storageType = StorageTypeEnum.None;
        [
            Category("Extension"),
            DefaultValue(StorageTypeEnum.None),
            Description("Storage type"),
        ]
        public StorageTypeEnum StorageType
        {
            get { return _storageType; }
            set { _storageType = value; }
        }


        private string _storageName = String.Empty;
        [
            Category("Extension"),
            DefaultValue(""),
            Description("Storage name"),
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

        private string _where = String.Empty;
        [
            DefaultValue(""),
            Description("Where"),
        ]
        public string Where
        {
            get { return _where;  }
            set { _where = value; }
        }

        #endregion

        #region Storage
        #region Common
        private object Storage
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

        private GridViewStorage GridViewStorage
        {
            get
            {
                if (Storage == null)
                {
                    GridViewStorage newStorage = new GridViewStorage();
                    Storage = newStorage;
                    return newStorage;
                }
                else
                {
                    return (Storage as GridViewStorage);
                }
            }
            set
            {
                Storage = value;
            }
        }
        #endregion

        #region Data
        protected int PageIndexStored
        {
            get
            {
                if (GridViewStorage != null)
                {
                    return GridViewStorage.PageIndex;
                }
                return 0;
            }
            set
            {
                if (GridViewStorage != null)
                {
                    GridViewStorage.PageIndex = value;
                }
            }
        }

        protected int PageSizeStored
        {
            get
            {
                if (GridViewStorage != null)
                {
                    return GridViewStorage.PageSize;
                }
                return 0;
            }
            set
            {
                if (GridViewStorage != null)
                {
                    GridViewStorage.PageSize = value;
                }
            }
        }

        protected string SortExpressionStored
        {
            get
            {
                if (GridViewStorage != null)
                {
                    return GridViewStorage.SortExpression;
                }
                return String.Empty;
            }
            set
            {
                if (GridViewStorage != null)
                {
                    GridViewStorage.SortExpression = value;
                }
            }
        }

        protected SortDirection SortDirectionStored
        {
            get
            {
                if (GridViewStorage != null)
                {
                    return GridViewStorage.SortDirection;
                }
                return SortDirection.Ascending;
            }
            set
            {
                if (GridViewStorage != null)
                {
                    GridViewStorage.SortDirection = value;
                }
            }
        }

        protected string WhereStored
        {
            get
            {
                if (GridViewStorage != null)
                {
                    return GridViewStorage.Where;
                }
                return String.Empty;
            }
            set
            {
                if (GridViewStorage != null)
                {
                    GridViewStorage.Where = value;
                }
            }
        }
        #endregion
        #endregion

        #region Helper
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
            _where = (string)ctlState[1];
            _storageName = (string)ctlState[2];
            _storageType = (StorageTypeEnum)ctlState[3];
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[4];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = _where;
            ctlState[2] = _storageName;
            ctlState[3] = _storageType;
            return ctlState;
        }
        #endregion

        #region LifeCycle
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                Page.LoadComplete += new EventHandler(delegate(object sender, EventArgs e_Load)
                {
                    LoadGridViewState();
                });
            }

            this.DataBound += new EventHandler(delegate(object sender, EventArgs e_DataBound)
            {
                SaveGridViewState();
            });

            ObjectDataSource ods = (this.DataSourceObject as ObjectDataSource);
            if (ods != null)
            {
                ods.Selecting += new ObjectDataSourceSelectingEventHandler(
                    delegate(object sender, ObjectDataSourceSelectingEventArgs e1)
                    {
                        if (!String.IsNullOrEmpty(this.Where))
                        {
                            e1.InputParameters["where"] = this.Where;
                        }
                    });
                return;
            }
            SqlDataSource sds = (this.DataSourceObject as SqlDataSource);
            if (sds != null)
            {
                //untested
                sds.Selecting += new SqlDataSourceSelectingEventHandler(
                    delegate(object sender, SqlDataSourceSelectingEventArgs e1)
                    {
                        if (!String.IsNullOrEmpty(this.Where))
                        {
                            
                            sds.SelectParameters.Add("where", this.Where);
                        }
                    });
                return;
            }
        }


        private void SaveGridViewState()
        {
            this.PageIndexStored = this.PageIndex;
            this.PageSizeStored = this.PageSize;
            if (this.AllowSorting)
            {
                this.SortExpressionStored = this.SortExpression;
                this.SortDirectionStored = this.SortDirection;
            }
            this.WhereStored = this.Where;
        }

        private void LoadGridViewState()
        {
            if (this.AllowSorting)
            {
                this.Sort(this.SortExpressionStored, this.SortDirectionStored);
            }
            this.PageIndex = this.PageIndexStored;
            this.PageSize = this.PageSizeStored;
            this.Where = this.WhereStored;
            this.DataBind();
        }
        #endregion
    }
}
