using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Xml;

using ARM2_dbcontrol.Filters;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Generation;
using Filters.Composite;

/// <summary>
/// Import and export filters and task's from/to xml
/// </summary>
public partial class Portable : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Resources.Resource.PagePortableTitle;
        if (!IsPostBack)
        {
            InitFields();
        }
    }

    /// <summary>
    /// Initialization session
    /// </summary>
    protected override void InitFields()
    {
        lbtnImport.Text = Resources.Resource.Import;
        lbtnExport.Text = Resources.Resource.Export;

        cbList.Items.Add(Resources.Resource.PageComputersFilterTitle);
        cbList.Items.Add(Resources.Resource.PageEventsFilterTitle);

        cbList.Items.Add(Resources.Resource.PageComponentsFilterTitle);
        cbList.Items.Add(Resources.Resource.PageProcessFiltersTitle);

        if ((Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
        {
            cbList.Items.Add(Resources.Resource.InstallUninstallTasksFilters);
            cbList.Items.Add(Resources.Resource.PageTasksFilterTitle);
            cbList.Items.Add(Resources.Resource.UsersTasks);
        }

        lbtnDelete.Text = Resources.Resource.Delete;
        lbtnDelete.Attributes.Add("onclick", "return confirm('" + Resources.Resource.AreYouSureSelected + "');");
    }

    private bool IsAnyChecked()
    {
        //Тут вылетали в зависимости от роли, т.к. не для всех есть столько 
        //флажков. Можно сделать несколько иначе. 
        try
        {
            if ((cbList.Items[0].Selected) || (cbList.Items[1].Selected) || (cbList.Items[2].Selected) ||
                (cbList.Items[3].Selected) || (cbList.Items[4].Selected) ||
                (cbList.Items[5].Selected) || (cbList.Items[6].Selected))
                return true;
        }
        catch
        {

        }

        return false;
    }

    /// <summary>
    /// Import
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnImport_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsAnyChecked())
                throw new Exception(Resources.Resource.NoSelected);

            if (fuICompFilters.HasFile)
            {
                XmlTaskParser pars = new XmlTaskParser(UploadFile(fuICompFilters));

                if (pars.GetValue("ComputerFilters") != String.Empty)
                {
                    ImportComputersFilters(pars.GetValue("ComputerFilters").Replace("&", "&amp;"));
                }

                if (pars.GetValue("EventFilters") != String.Empty)
                {
                    ImportEventsFilters(pars.GetValue("EventFilters").Replace("&", "&amp;"));
                }

                if (pars.GetValue("CmpFilters") != String.Empty)
                {
                    ImportCmpFilters(pars.GetValue("CmpFilters").Replace("&", "&amp;"));
                }

                if (pars.GetValue("ProcFilters") != String.Empty)
                {
                    ImportProcFilters(pars.GetValue("ProcFilters").Replace("&", "&amp;"));
                }

                if (cbList.Items.Count > 4)
                {
                    if (pars.GetValue("TasksInstallFilters") != String.Empty)
                    {
                        ImportTasksInstallFilters(pars.GetValue("TasksInstallFilters").Replace("&", "&amp;"));
                    }

                    if (pars.GetValue("TaskFilters") != String.Empty)
                    {
                        ImportTasksFilters(pars.GetValue("TaskFilters").Replace("&", "&amp;"));
                    }

                    if (pars.GetValue("TaskUser") != String.Empty)
                    {
                        //string tp = Server.HtmlDecode(pars.GetValue("TaskUser"));
                        string tp = pars.GetValue("TaskUser");
                        //возможно ненужные 2 строки, без них все работает, а с ними не работает настройка Проактивной защиты
                        //tp = tp.Replace("&amp;lt;", "&lt;");
                        //tp = tp.Replace("&amp;gt;", "&gt;");
                        //tp = tp.Replace("&lt;", "<");
                        //tp = tp.Replace("&gt;", ">");
                        ImportTaskUser(tp);
                    }
                }
                lblMessage.Text = Resources.Resource.PortableSuccess;
            }
            else
            {
                lblMessage.Text = Resources.Resource.Error + ": " + Resources.Resource.SelectFile;
            }
        }
        catch (XmlException xmlEx)
        {
            lblMessage.Text = Resources.Resource.ErrorWrongXmlFile + ": " + xmlEx.Message;
        }
        catch (Exception ex)
        {
            lblMessage.Text = Resources.Resource.Error + ": " + ex.Message;
        }
    }

    /// <summary>
    /// Upload file(Import)
    /// </summary>
    /// <param name="control"></param>
    /// <returns>file content</returns>
    private string UploadFile(FileUpload control)
    {
        if (control.HasFile)
        {
            String fileExtension =
                System.IO.Path.GetExtension(control.FileName).ToLower();

            if (fileExtension == ".xml")
            {
                HttpPostedFile file = control.PostedFile;
                StreamReader reader = new StreamReader(file.InputStream);

                return reader.ReadToEnd();

            }
            else
                throw new Exception("Неверное расширение файла");
        }

        return String.Empty;
    }

    protected string CompareToExport(string str)
    {
        str = str.Replace("&amp;", "&");
        str = Server.HtmlEncode(str);
        str = str.Replace("&amp;lt;", "lt;");
        str = str.Replace("&amp;gt;", "gt;");

        return str;
    }

    /// <summary>
    /// Export
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnExport_Click(object sender, EventArgs e)
    {

        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("XmlContainer");

        if (cbList.Items[0].Selected)
        {
            xml.AddNode("ComputerFilters", CompareToExport(Profile.CompFilters));
        }

        if (cbList.Items[1].Selected)
        {
            xml.AddNode("EventFilters", CompareToExport(Profile.EventFilters));
        }

        if (cbList.Items[2].Selected)
        {
            xml.AddNode("CmpFilters", CompareToExport(Profile.ComponentFilters));
        }

        if (cbList.Items[3].Selected)
        {
            xml.AddNode("ProcFilters", CompareToExport(Profile.ProcessFilters));
        }

        if (cbList.Items.Count > 4)
        {
            if (cbList.Items[4].Selected)
            {
                xml.AddNode("TasksInstallFilters", CompareToExport(Profile.TasksInstallFilters));
            }
            if (cbList.Items[5].Selected)
            {
                xml.AddNode("TaskFilters", CompareToExport(Profile.TaskFilters));
            }

            if (cbList.Items[6].Selected)
            {
                xml.AddNode("TaskUser", Server.HtmlEncode(Profile.TasksList));
            }
        }

        xml.Generate();

        WriteXmlToResponse(xml.Result);

        lblMessage.Text = Resources.Resource.PortableSuccess;

    }

    /// <summary>
    /// Write to response xml content(Export)
    /// </summary>
    /// <param name="xmlstring"></param>
    private void WriteXmlToResponse(string xmlstring)
    {
        XmlDocument xml = new XmlDocument();

        xml.LoadXml(xmlstring);
        TextWriter writer = new StringWriter();
        xml.Save(writer);

        string str = writer.ToString();
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

        MemoryStream ms = new MemoryStream(encoding.GetBytes(str));
        Page.Visible = false;
        Response.Clear();
        Response.AppendHeader(
            "Content-Disposition",
            String.Format("attachment; filename=\"Vba32CC_settings_{0}.xml\"",
            Profile.UserName));

        Response.ContentType = "text/xml";
        ms.WriteTo(Response.OutputStream);
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        if (cbList.Items[0].Selected)
        {
            Profile.CompFilters = String.Empty;
            Session["CompFiltersTemp_StorageControl"] = null;
        }
        if (cbList.Items[1].Selected)
        {
            Profile.EventFilters = String.Empty;
            Session["EventFiltersTemp_StorageControl"] = null;
        }
        if (cbList.Items[2].Selected)
        {
            Profile.ComponentFilters = String.Empty;
            Session["ComponentFiltersTemp_StorageControl"] = null;
        }
        if (cbList.Items[3].Selected)
        {
            Profile.ProcessFilters = String.Empty;
            Session["ProcessFiltersTemp_StorageControl"] = null;
        }
        if (cbList.Items.Count > 4)
        {
            if (cbList.Items[4].Selected)
            {
                Profile.TasksInstallFilters = String.Empty;
                Session["TasksInstalFilltersTemp_StorageControl"] = null;
            }
            if (cbList.Items[5].Selected)
            {
                Profile.TaskFilters = String.Empty;
                Session["TaskFiltersTemp_StorageControl"] = null;
            }
            if (cbList.Items[6].Selected)
            {
                Profile.TasksList = String.Empty;
                Session["TaskUser"] = null;
            }
        }

        lblMessage.Text = Resources.Resource.PortableDelete;

        //Говнокод, конечно, но оставим так..
        try
        {
            if (!IsAnyChecked())
                throw new Exception(Resources.Resource.NoSelected);
        }
        catch (Exception ex)
        {
            lblMessage.Text = Resources.Resource.Error + ": " + ex.Message;
        }
    }

    //OLD Method!!!!
    private void ImportComputersFilters(string xml)
    {
        /* New code
        CompositeFilterStateCollection collectionProfile = CompositeFilterStateCollection.Deserialize(Profile.CompFilters);
        CompositeFilterStateCollection collectionXML = CompositeFilterStateCollection.Deserialize(xml);

        foreach(String nameXML in collectionXML.GetNames())
        {
            foreach (String nameProfile in collectionProfile.GetNames())
            {
                if (nameXML == nameProfile)
                {
                    collectionProfile.Delete(nameXML);
                    break;
                }
            }
            collectionProfile.Add(collectionXML.Get(nameXML));
        }

        Session["CompFiltersTemp_StorageControl"] = collectionProfile;
        Profile.CompFilters = collectionProfile.Serialize();
         */
        CompFilterCollection upCollection =
                   new CompFilterCollection(xml);

        CompFilterCollection collection =
            new CompFilterCollection(Profile.CompFilters);

        foreach (CompFilterEntity upComp in upCollection)
        {
            bool inColl = false;
            foreach (CompFilterEntity comp in collection)
            {
                if (comp.FilterName == upComp.FilterName)
                {
                    inColl = true;

                    break;
                }
            }
            if (inColl)
            {
                collection.Delete(upComp.FilterName);
                collection = collection.Deserialize();
            }
            collection.Add(upComp);
        }

        Session["CompFilters"] = collection;
        Profile.CompFilters = collection.Serialize();
    }

    private void ImportEventsFilters(String xml)
    {
        CompositeFilterStateCollection collectionProfile = CompositeFilterStateCollection.Deserialize(Profile.EventFilters);
        CompositeFilterStateCollection collectionXML = CompositeFilterStateCollection.Deserialize(xml);

        foreach (String nameXML in collectionXML.GetNames())
        {
            foreach (String nameProfile in collectionProfile.GetNames())
            {
                if (nameXML == nameProfile)
                {
                    collectionProfile.Delete(nameXML);
                    break;
                }
            }
            collectionProfile.Add(collectionXML.Get(nameXML));
        }

        Session["EventFiltersTemp_StorageControl"] = collectionProfile;
        Profile.EventFilters = collectionProfile.Serialize();
    }


    private void ImportCmpFilters(String xml)
    {
        CompositeFilterStateCollection collectionProfile = CompositeFilterStateCollection.Deserialize(Profile.ComponentFilters);
        CompositeFilterStateCollection collectionXML = CompositeFilterStateCollection.Deserialize(xml);

        foreach (String nameXML in collectionXML.GetNames())
        {
            foreach (String nameProfile in collectionProfile.GetNames())
            {
                if (nameXML == nameProfile)
                {
                    collectionProfile.Delete(nameXML);
                    break;
                }
            }
            collectionProfile.Add(collectionXML.Get(nameXML));
        }

        Session["ComponentFiltersTemp_StorageControl"] = collectionProfile;
        Profile.ComponentFilters = collectionProfile.Serialize();
    }

    private void ImportProcFilters(String xml)
    {
        CompositeFilterStateCollection collectionProfile = CompositeFilterStateCollection.Deserialize(Profile.ProcessFilters);
        CompositeFilterStateCollection collectionXML = CompositeFilterStateCollection.Deserialize(xml);

        foreach (String nameXML in collectionXML.GetNames())
        {
            foreach (String nameProfile in collectionProfile.GetNames())
            {
                if (nameXML == nameProfile)
                {
                    collectionProfile.Delete(nameXML);
                    break;
                }
            }
            collectionProfile.Add(collectionXML.Get(nameXML));
        }

        Session["ProcessFiltersTemp_StorageControl"] = collectionProfile;
        Profile.ProcessFilters = collectionProfile.Serialize();
    }

    private void ImportTasksInstallFilters(String xml)
    {
        CompositeFilterStateCollection collectionProfile = CompositeFilterStateCollection.Deserialize(Profile.TasksInstallFilters);
        CompositeFilterStateCollection collectionXML = CompositeFilterStateCollection.Deserialize(xml);

        foreach (String nameXML in collectionXML.GetNames())
        {
            foreach (String nameProfile in collectionProfile.GetNames())
            {
                if (nameXML == nameProfile)
                {
                    collectionProfile.Delete(nameXML);
                    break;
                }
            }
            collectionProfile.Add(collectionXML.Get(nameXML));
        }

        Session["TasksInstalFilltersTemp_StorageControl"] = collectionProfile;
        Profile.TasksInstallFilters = collectionProfile.Serialize();
    }

    private void ImportTasksFilters(String xml)
    {
        CompositeFilterStateCollection collectionProfile = CompositeFilterStateCollection.Deserialize(Profile.TaskFilters);
        CompositeFilterStateCollection collectionXML = CompositeFilterStateCollection.Deserialize(xml);

        foreach (String nameXML in collectionXML.GetNames())
        {
            foreach (String nameProfile in collectionProfile.GetNames())
            {
                if (nameXML == nameProfile)
                {
                    collectionProfile.Delete(nameXML);
                    break;
                }
            }
            collectionProfile.Add(collectionXML.Get(nameXML));
        }

        Session["TaskFiltersTemp_StorageControl"] = collectionProfile;
        Profile.TaskFilters = collectionProfile.Serialize();
    }

    private void ImportTaskUser(string xml)
    {
        TaskUserCollection upCollection =
                   new TaskUserCollection(xml);

        TaskUserCollection collection =
            new TaskUserCollection(Profile.TasksList);

        foreach (TaskUserEntity upComp in upCollection)
        {
            bool inColl = false;
            foreach (TaskUserEntity comp in collection)
            {
                if (comp.Name == upComp.Name)
                {
                    inColl = true;

                    break;
                }
            }
            if (inColl)
            {
                collection.Delete(upComp.Name);
                collection = collection.Deserialize();
            }

            List<TaskEntity> tasktypes = new List<TaskEntity>();
            //Что это за строчка и зачем она нужна была???
            // if (!tasktypes.Contains( upComp))
            {
                using (VlslVConnection conn = new VlslVConnection(
                   ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
                {
                    TaskManager db = new TaskManager(conn);
                    conn.OpenConnection();

                    tasktypes = db.ListTaskTypes();

                    db.GetTaskTypeID(upComp.Name, true);

                    conn.CloseConnection();
                }
            }

            collection.Add(upComp);

        }


        Session["TaskUser"] = collection;
        Profile.TasksList = collection.Serialize();
    }
}
