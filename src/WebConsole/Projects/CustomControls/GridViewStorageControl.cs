using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControls
{
    [
        NonVisualControl,
        Bindable(false),
        DefaultProperty("ID"),
        ToolboxData("<{0}:GridViewStorageControl runat=server></{0}:GridViewStorageControl>"),
        Obsolete("Use GridViewExtended instead", false)
    ]

    public class GridViewStorageControl : StorageControl
    {
        #region LifeCycle
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FindGridView();

            if (!Page.IsPostBack)
            {
                Page.LoadComplete += new EventHandler(delegate(object sender, EventArgs e_Load)
                {
                    LoadGridViewState();
                });
            } 

            gridView.DataBound += new EventHandler(delegate(object sender, EventArgs e_DataBound)
                {
                    SaveGridViewState();
                });
        }

        private void FindGridView()
        {
            gridView = NamingContainer.FindControl(_gridViewID) as GridView;
            if (gridView == null)
            {
                String errorInfo = String.Format("The GridViewID of '{0}' must be the ID of a control of type  GridView.  A control with ID '{1}' could not be found",
                    ID, GridViewID);
                throw new HttpException(errorInfo);
            }
        }

        private void SaveGridViewState()
        {
            PageIndex = gridView.PageIndex;
            PageSize = gridView.PageSize;
            if (gridView.AllowSorting)
            {
                SortExpression = gridView.SortExpression;
                SortDirection = gridView.SortDirection;
            }
            Where = gridView.Attributes["where"];
        }

        private void LoadGridViewState()
        {
            if (gridView.AllowSorting)
            {
                gridView.Sort(SortExpression, SortDirection);
            }
            gridView.PageIndex = PageIndex;
            gridView.PageSize = PageSize;
            gridView.Attributes.Add("where", Where);
            gridView.DataBind();
        }
        #endregion

        #region Properties
        #region Data
        [
            Category("Data"),
            PersistenceMode(PersistenceMode.InnerProperty),
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        protected GridViewStorage GridViewStorage
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

        [
            Category("Data"),
            PersistenceMode(PersistenceMode.InnerProperty),
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public int PageIndex
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

        [
            Category("Data"),
            PersistenceMode(PersistenceMode.InnerProperty),
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public int PageSize
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
        [
            Category("Data"),
            PersistenceMode(PersistenceMode.InnerProperty),
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public string SortExpression
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

        [
            Category("Data"),
            PersistenceMode(PersistenceMode.InnerProperty),
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public SortDirection SortDirection
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

        [
            Category("Data"),
            PersistenceMode(PersistenceMode.InnerProperty),
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public string Where
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

        #region Behaviour
        protected GridView gridView;

        private string _gridViewID;
        [
            Category("Behavior"),
            DefaultValue(""),
            Description("GridView ID"),
            NotifyParentProperty(true),
        ]
        public string GridViewID
        {
            get { return _gridViewID; }
            set
            {
                _gridViewID = value;
            }
        }
        #endregion
        #endregion

        #region ViewState
        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            _gridViewID = (string)ctlState[1];
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[2];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = _gridViewID;
            return ctlState;
        }
        #endregion
    }
}
