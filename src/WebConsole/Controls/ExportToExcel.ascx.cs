using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Common;
using System.Text;
using CustomControls;
using Common.Collection;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;

public partial class Controls_ExportToExcel : System.Web.UI.UserControl
{
    #region Public Methods
    public void InitializeExportToExcel(GridViewExtended associatedGridView,
        InformationListTypes informationListType)
    {
        initialized = true;
        if (associatedGridView == null)
        {
            throw new ArgumentNullException("associatedGridView", "InitializeExportToExcel does not support null argument");
        }
        if (informationListType == InformationListTypes.None)
        {
            throw new ArgumentException("InitializeExportToExcel does not accept InformationListTypes.None", "informationListType");
        }
        storage = new Storage(associatedGridView, informationListType);
    }
    #endregion

    #region Fields
    private bool initialized = false;
    private Storage storage = null;
    #endregion

    #region Event Handlers
    protected void lbtnExcel_Click(object sender, EventArgs e)
    {
        if (!initialized)
        {
            throw new HttpException(String.Format("ExportToExcel control {0} was never initialized ", ClientID));
        }
        storage.Update();
        if (!storage.Validate())
        {
            return;
        }

        Response.Clear();
        SetExcelMeta(storage.InformationListType);
        IList dataList = GetDataList(storage);
        GenerateTable(storage.Properties, dataList);
        Response.End();
    }
    #endregion

    #region Helper Storage Class
    protected class Storage
    {
        #region Fields
        private GridViewExtended associatedGridView;
        #endregion

        #region Properties
        private string where;
        public string Where
        {
            get { return where; }
            set { where = value; }
        }
        private string sortExpression;
        public string SortExpression
        {
            get { return sortExpression; }
            set { sortExpression = value; }
        }
        private SortDirection sortDirection;
        public SortDirection SortDirection
        {
            get { return sortDirection; }
            set { sortDirection = value; }
        }
        private SerializableDictionary<string, string> properties;
        public SerializableDictionary<string, string> Properties
        {
            get { return properties; }
            set { properties = value; }
        }
        private bool allowSorting;
        public bool AllowSorting
        {
            get { return allowSorting; }
            set { allowSorting = value; }
        }
        private InformationListTypes informationListType;
        public InformationListTypes InformationListType
        {
            get { return informationListType; }
            set { informationListType = value; }
        }
        #endregion

        #region Helper Methods

        private SerializableDictionary<string, string> GetDataControlFieldCollectionProperties
            (DataControlFieldCollection collection)
        {
            SerializableDictionary<string, string> dict = new SerializableDictionary<string, string>();
            foreach (DataControlField next in collection)
            {
                string header = next.HeaderText;
                string propertyName = null;

                if (next is BoundField)
                {
                    propertyName = (next as BoundField).DataField;
                }
                else if (next is ImageField)
                {
                    propertyName = (next as ImageField).DataAlternateTextField;
                }
                else if (next is HyperLinkField)
                {
                    propertyName = (next as HyperLinkField).DataTextField;
                }
                if (!String.IsNullOrEmpty(propertyName))
                {
                    dict.Add(propertyName, header);
                }
            }
            return dict;
        }

        #endregion

        #region Constructor
        public Storage(GridViewExtended associatedGridView,
        InformationListTypes informationListType)
        {
            this.associatedGridView = associatedGridView;
            this.informationListType = informationListType;
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            if (associatedGridView == null)
            {
                return;
            }
            where = associatedGridView.Where;
            if (where == String.Empty)
            {
                where = null;
            }
            allowSorting = associatedGridView.AllowSorting;
            sortExpression = associatedGridView.SortExpression;
            sortDirection = associatedGridView.SortDirection;
            properties = GetDataControlFieldCollectionProperties(associatedGridView.Columns);
        }

        public bool Validate()
        {
            if (properties == null)
            {
                return false;
            }
            if (properties.Count == 0)
            {
                return false;
            }
            if (informationListType == InformationListTypes.Computers ||
                informationListType == InformationListTypes.None)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
    #endregion

    #region Excel Generation
    #region Meta
    private void SetExcelMeta(InformationListTypes type)
    {        
        string attachment = String.Format("attachment; filename={0}.xls", type.ToString());
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        Response.Charset = Encoding.UTF8.WebName;
        Response.Write("<head><meta http-equiv=Content-Type content=:\"text/html; charset=utf-8\"></head>");
    }
    #endregion

    #region GetterCache
    private delegate object GetterDelegate(object instance);
    private readonly Dictionary<string, GetterDelegate> getterCache = new Dictionary<string, GetterDelegate>();
    private void GenerateGetterCache(object representative, string[] properties)
    {
        foreach (string sProperty in properties)
        {
            //get property
            PropertyInfo propertyInfo = representative.GetType().GetProperty(sProperty, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                throw new HttpException("ExportToExcel: GenerateGetterCache can't find corresponding property in datacontainer");
            }

            //get getMethod
            MethodInfo getterMethodInfo = propertyInfo.GetGetMethod();
            //create dynamic method
            DynamicMethod getter = new DynamicMethod(getterMethodInfo.Name, typeof(object),
                new Type[] { typeof(object) }, getterMethodInfo.Module, true);
            ILGenerator getterIl = getter.GetILGenerator();

            //getMethod is not static
            getterIl.Emit(OpCodes.Ldarg_0);
            if (getterMethodInfo.DeclaringType.IsValueType)
            {
                getterIl.Emit(OpCodes.Unbox, getterMethodInfo.DeclaringType);
            }

            //call corresponding method
            if (getterMethodInfo.IsFinal || !getterMethodInfo.IsVirtual)
            {
                getterIl.Emit(OpCodes.Call, getterMethodInfo);
            }
            else
            {
                getterIl.Emit(OpCodes.Callvirt, getterMethodInfo);
            }

            //box return type if needed
            if (getterMethodInfo.ReturnType.IsValueType)
            {
                getterIl.Emit(OpCodes.Box, getterMethodInfo.ReturnType);
            }

            getterIl.Emit(OpCodes.Ret);

            //create delegate
            GetterDelegate getterDelegate = (GetterDelegate)getter.CreateDelegate(typeof(GetterDelegate));

            //add to cache
            getterCache.Add(propertyInfo.Name, getterDelegate);
        }
    }
    #endregion

    #region DataList
    private IList GetDataList(Storage storage)
    {
        string sort = String.Empty;
        if (storage.AllowSorting)
        {
            if (!String.IsNullOrEmpty(storage.SortExpression))
            {
                sort = storage.SortExpression + " " + storage.SortDirection.ToString();
            }
        }
        string where = storage.Where;
        InformationListTypes type = storage.InformationListType;

        IList list = null;
        switch (type)
        {
            case InformationListTypes.Components:
                list = ComponentsDataContainer.Get(where, sort, ComponentsDataContainer.Count(where), 0);
                break;
            case InformationListTypes.Events:
                list = EventsDataContainer.Get(where, sort, EventsDataContainer.Count(where), 0);
                break;
            case InformationListTypes.Processes:
                list = ProcessDataContainer.Get(where, sort, ProcessDataContainer.Count(where), 0);
                break;
            case InformationListTypes.Tasks:
                list = TasksDataContainer.Get(where, sort, TasksDataContainer.Count(where), 0);
                break;
            case InformationListTypes.TasksInstall:
                list = InstallTasksDataContainer.Get(where, sort, InstallTasksDataContainer.Count(where), 0);
                break;
        }
        return list;
    }
    #endregion

    #region Table
    private readonly StringBuilder builder = new StringBuilder();
    private void GenerateTable(Dictionary<string, string> properties, IList dataList)
    {
        Response.Write(TableStartTag());
        string[] propertiesHeaders = new string[properties.Count];
        properties.Values.CopyTo(propertiesHeaders, 0);

        string sHeader = GenerateTableHeader(propertiesHeaders);
        Response.Write(sHeader);

        if (dataList.Count == 0)
        {
            return;
        }

        string[] propertiesNames = new string[properties.Count];
        properties.Keys.CopyTo(propertiesNames, 0);

        object representative = dataList[0];
        GenerateGetterCache(representative, propertiesNames);

        foreach (object instance in dataList)
        {
            string sRow = GenerateTableRow(propertiesNames, instance);
            Response.Write(sRow);
        }
        Response.Write(TableEndTag());
    }

    private string TableStartTag()
    {
        return "<table rules=\"all\" border=\"1\">";
    }

    private string TableEndTag()
    {
        return "</table>";
    }

    private string GenerateTableRow(string[] properties, object instance)
    {
        builder.Remove(0, builder.Length);
        builder.Append("<tr>");
        foreach (string property in properties)
        {
            GetterDelegate getter = getterCache[property];
            object value = getter(instance);
            builder.Append(String.Format("<td>{0}</td>", value));
        }
        builder.AppendLine("</tr>");
        return builder.ToString();
    }

    private string GenerateTableHeader(string[] propertiesHeaders)
    {
        builder.Remove(0, builder.Length);
        builder.Append("<tr>");
        foreach (string sHeader in propertiesHeaders)
        {
            builder.Append(String.Format("<th scope=\"col\">{0}</th>", sHeader));
        }
        builder.AppendLine("</tr>");
        return builder.ToString();
    }
    #endregion

    #endregion
}