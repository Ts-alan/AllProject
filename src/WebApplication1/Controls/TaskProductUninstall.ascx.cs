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
using ARM2_dbcontrol.Tasks;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.RemoteOperations.MsiInfo;

public partial class Controls_TaskProductUninstall : System.Web.UI.UserControl, ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
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

    private Boolean _hideBound = false;
    public Boolean HideBound
    {
        get { return _hideBound; }
        set
        {
            _hideBound = value;
            if (_hideBound)
            {
                tblInstall.Attributes.Add("class", "");
            }
            else
            {
                tblInstall.Attributes.Add("class", "ListContrastTable");
            }
        }
    }

    #region ITask Members

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        SetEnabled();
        List<String> list = new List<String>();
        list.Add(Resources.Resource.Antivirus);

        ddlProduct.DataSource = list;
        ddlProduct.DataBind();
    }

    private void SetEnabled()
    {
        ddlProduct.Enabled = _enabled;
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.Uninstall;
        task.Param = BuildXml();

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.Uninstall)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        VirusBlokAda.CC.Common.Xml.XmlTaskParser parser = new VirusBlokAda.CC.Common.Xml.XmlTaskParser(task.Param);
        Int32 value;
        try
        {
            value = Int32.Parse(parser.GetXmlTagContent("ProductType"));
        }
        catch
        {
            throw new Exception("Selected product for uninstallation is wrong.");
        }

        ddlProduct.SelectedIndex = value;
    }

    #endregion

    private String BuildXml()
    {
        StringBuilder content = new StringBuilder();
        content.Append("<UninstallProduct>");
        content.AppendFormat(@"<ProductType>{0}</ProductType>", ddlProduct.SelectedIndex);
        content.Append(@"</UninstallProduct>");

        VirusBlokAda.CC.Common.Xml.XmlBuilder xml = new VirusBlokAda.CC.Common.Xml.XmlBuilder("Uninstall");
        xml.Top = String.Empty;
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ProductUninstall");
        xml.AddNode("Content", content.ToString());
        xml.Generate();

        return xml.Result;
    }

    public String BuildTask(String osVersion)
    {        
        String version = String.Empty;        
        switch (ddlProduct.SelectedIndex)
        {
            case 0:
                version = Vba32VersionInfo.Vba32Antivirus;
                break;
        }

        return String.Format("<UninstallProduct><GUID>{0}</GUID></UninstallProduct>", Vba32VersionInfo.GetGuid(version));
    }
}