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
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ARM2_dbcontrol.Service.Vba32NS;
using ARM2_dbcontrol.Filters;
using VirusBlokAda.Vba32CC.DataBase;

public partial class Controls_TaskFirewall : System.Web.UI.UserControl, ITask
{
    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

        if (Firewall == null)
        {
            Firewall = new RuleConfigureFirewall();           
        }        
     
        cbxInvisibility.Text = Resources.Resource.InvisibilityMode;
        cbxMonitoring.Text = Resources.Resource.AntivirusMonitoring;
        
        cbxActivate.Text = Resources.Resource.ActivateRule;
        rbtnIP.Text = Resources.Resource.IPAddress;
        rbtnSubnetwork.Text = Resources.Resource.Subnetwork;
        cbxEventARM.Text = Resources.Resource.EventARM;
        
        ddlConnection.Items.Clear();
        ddlConnection.Items.Add(Resources.Resource.Allow);
        ddlConnection.Items.Add(Resources.Resource.Block);

        ddlProtocol.Items.Clear();
        ddlProtocol.Items.Add(Protocols.Any.ToString());
        ddlProtocol.Items.Add(Protocols.TCP.ToString());
        ddlProtocol.Items.Add(Protocols.UDP.ToString());

        ddlDirection.Items.Clear();
        ddlDirection.Items.Add(Directions.Any.ToString());
        ddlDirection.Items.Add(Directions.In.ToString());
        ddlDirection.Items.Add(Directions.Out.ToString());

        tabPanel1.HeaderText = Resources.Resource.Rules;
        tabPanel2.HeaderText = Resources.Resource.ZoneFriendlyIP;
        btnAddRule.Text = Resources.Resource.Save;
        btnCancelRule.Text = Resources.Resource.Close;
        btnAddNewRule.Text = Resources.Resource.Add;

        tabPanelRule1.HeaderText = Resources.Resource.Application;
        tabPanelRule2.HeaderText = Resources.Resource.IPAddress;
        tabPanelRule3.HeaderText = Resources.Resource.PortsAndProtocols;
        tabPanelRule4.HeaderText = Resources.Resource.EventARM;
        btnAddZoneIP.Text = Resources.Resource.Save;
        btnCancelZoneIP.Text = Resources.Resource.Close;
        btnAddZoneIPRule.Text = Resources.Resource.Add;

        TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
        ddlProfiles.Items.Clear();
        foreach (TaskUserEntity tsk in collection)
        {
            if (tsk.Type == TaskType.Firewall)
                ddlProfiles.Items.Add(tsk.Name.Substring(10));
        }

        if (ddlProfiles.Items.Count == 0)
        {
            lbtnRemove.Visible = lbtnEdit.Visible = false;
        }

        lbtnRemove.Attributes.Add("onclick", "return confirm('" + Resources.Resource.AreYouSureProfile + "');");
    }
   
    public bool ValidateFields()
    {
        if (Firewall.Rules.Count == 0 && Firewall.FriendlyIP.Count == 0)
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue);
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();        
        task.Type = TaskType.Firewall;
        task.Param = BuildXml();
        return task;
    }
    
     /// <summary>
     /// Метод конвертации данных в xml строку
     /// </summary>
     /// <returns>xml строка</returns>
     private string BuildXml()
     {
         ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("Firewall");
         xml.AddNode("Vba32CCUser", String.Format("{0} ({1} {2})", Profile.UserName, Profile.FirstName == "" ? Profile.UserName : Profile.FirstName, Profile.LastName == "" ? Profile.UserName : Profile.LastName) );
         xml.AddNode("Type", "Firewall");
         xml.AddNode("Content", Server.HtmlEncode(ObjectSerializer.ObjToXmlStr(Firewall).Remove(0, 39)));
         xml.Generate();
        
         return xml.Result;
     }

     /// <summary>
     /// Метод конвертации из xml в необходимый config файлу формат
     /// </summary>
     /// <param name="xmlParam">Сериализованные в xml данные</param>
     /// <returns>Содержимое config файла</returns>
     private string BuildConfig(string xmlParam)
     {   
         RuleConfigureFirewall firewall = ObjectSerializer.XmlStrToObj<RuleConfigureFirewall>(xmlParam);

         int count = 0;
         string result = "[_config_]\n";
         result += String.Format("invisible_mode={0}\n", firewall.ModeInvisibility == true ? 1 : 0);
         result += String.Format("save_log_days={0}\n", firewall.AntivirusMonitoring == true ? 1 : 0);
         result += "[_main_]\n";
         result += "_default_=trusted_area\n";
         foreach (FirewallRule rule in firewall.Rules)
         {
             if (rule.isActivate)
             {
                 if (rule.application != "*")
                     result += String.Format("{0}=trusted_area section{1}\n", rule.application, ++count);
                 else count++;
             }
         }
         count = 0;
         result += @"%SystemRoot%\vbagent.exe=allow_all_nolog";
         result += "\n[_users_]";
//!-- OPTM дописать генерацию конфига         
         return result;
     }

     /// <summary>
     /// Метод генерации задачи
     /// </summary>
     /// <param name="fullPath">полный путь config файла</param>
     /// <param name="xmlParam">xml настроек</param>
     /// <returns></returns>
     public string BuildTask(string fullPath, string xmlParam)
     {
         ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("CreateFile");
         xml.Top = String.Empty;

         XmlTaskParser pars = new XmlTaskParser(xmlParam);

         xml.AddNode("FullPath", fullPath);
         xml.AddNode("Content", BuildConfig(Server.HtmlDecode(pars.GetValue("Content"))));
         xml.Generate();       

         return xml.Result;
     }
 
    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.Firewall)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);        
        
        XmlTaskParser pars = new XmlTaskParser(task.Param);
        Firewall = ObjectSerializer.XmlStrToObj<RuleConfigureFirewall>(Server.HtmlDecode(pars.GetValue("Content")));
        
        UpdateData();        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitFields();
        }

        Tabs.ActiveTabIndex = int.Parse(ActiveTab.Value);
    }

    private void UpdateData()
    {
        ClearField();
        cbxInvisibility.Checked = Firewall.ModeInvisibility;
        cbxMonitoring.Checked = Firewall.AntivirusMonitoring;
        
        DataTable dt = new DataTable();
        CreateColumnsRule(dt);
        foreach (FirewallRule rule in Firewall.Rules)
        {
            AddNewItem(dt, rule);
        }
        dlRules.DataSource = new DataView(dt);
        dlRules.DataBind();

        for (int i = 0; i < Firewall.Rules.Count; i++)
        {
            if(dlRules.EditItemIndex != i)
                if (Firewall.Rules[i].isActivate)
                    (dlRules.Items[i].FindControl("trRules") as HtmlTableRow).Attributes.Add("class", "listRulesItemActive");
                else (dlRules.Items[i].FindControl("trRules") as HtmlTableRow).Attributes.Add("class", "listRulesItem");
        }
        
                
        dt = new DataTable();
        CreateColumnsFriendlyIP(dt);
        foreach (FriendlyIPRule rule in Firewall.FriendlyIP)
        {
            AddNewItem(dt, rule);
        }
        dlFriendlyIP.DataSource = new DataView(dt);
        dlFriendlyIP.DataBind();
    }
        

    protected void cbxInvisibility_CheckedChanged(object sender, EventArgs e)
    {
        Firewall.ModeInvisibility = cbxInvisibility.Checked;
    }
    protected void cbxMonitoring_CheckedChanged(object sender, EventArgs e)
    {
        Firewall.AntivirusMonitoring = cbxMonitoring.Checked;
    }

    public void ShowProfiles()
    {
        tblProfileMngmt.Visible = true;
        tblProfileOptions.Visible = false;
        tblProfile.Visible = false;
    }

    public void ShowProfileDetails(TaskUserEntity task)
    {
        ClearRule();
        LoadState(task);

        tblProfileMngmt.Visible = false;        
        tblProfileOptions.Visible = true;
        tblProfile.Visible = true;
    }

    private void ClearRule()
    {     
        Firewall.Rules.Clear();
        Firewall.FriendlyIP.Clear();
        ClearField();        
    }

    private void ClearField()
    {
        cbxInvisibility.Checked = cbxMonitoring.Checked = false;
        dlRules.DataBind();
        dlFriendlyIP.DataBind();
    }    


    #region Create/Edit profile

    #region Rule

    protected void lbtnRuleAdd_Click(object sender, EventArgs e)
    {
        FirewallRule rule = GetNewRule();
        //проверка на корректность        
        if (!ValidateRule(rule)) return;

        if (!isEditingRule)
        {
            Firewall.Rules.Add(rule);
        }
        else
        {
            Firewall.Rules[indexItem] = rule;
            isEditingRule = false;
        }

        UpdateData();
        InitializeRuleFields();

    }

    private bool ValidateRule(FirewallRule rule)
    {
        if (!rule.isSubnetwork)
        {
            if (!ValidateIPAddresses(rule.ip, false)) return false;
        }
        else
        {
            if (!ValidateIPAddresses(rule.ip, true) || !ValidateIPAddresses(rule.subnetwork_mask, true)) return false;
        }
        if (!ValidatePorts(rule.localPorts)) return false;
        if (!ValidatePorts(rule.remotePorts)) return false;
        
        if (rule.application == String.Empty) return false;
        foreach (FirewallRule fRule in Firewall.Rules)
        {
            if (fRule.application == rule.application) return false;
        }

        return true;
    }

    private void InitializeRuleFields()
    {
        cbxActivate.Checked = true;
        ddlConnection.SelectedIndex = ddlProtocol.SelectedIndex = ddlDirection.SelectedIndex = 0;
        fuApplication.Text = txtIPAddress.Text = txtLocalPorts.Text = txtRemotePorts.Text = "*";
        txtCommentApp.Text = txtEventARM.Text = String.Empty;
        txtMaskSubnetwork.Text = txtIPSubnetwork.Text = "0.0.0.0";
        cbxEventARM.Checked = rbtnSubnetwork.Checked = txtEventARM.Enabled = txtIPSubnetwork.Enabled = txtMaskSubnetwork.Enabled = false;
        rbtnIP.Checked = txtIPAddress.Enabled = true;
    }

    private bool ValidatePorts(string ports)
    {
        bool prov = true;
        try
        {
            int res = 0;
            while (ports != String.Empty)
            {
                if (ports.IndexOfAny(new char[2] { ',', '-' }) < 0)
                {
                    int.TryParse(ports, out res);
                    ports = String.Empty;
                }
                else
                {
                    int.TryParse(ports.Substring(0, ports.IndexOfAny(new char[2] { ',', '-' })), out res);
                    ports = ports.Substring(ports.IndexOfAny(new char[2] { ',', '-' }) + 1);
                }

            }
        }
        catch
        {
            prov = false;
            if (ports == "*") prov = true;
        }

        return prov;
    }

    protected void btnCancelRule_Click(object sender, EventArgs e)
    {
        isEditingRule = false;
        InitializeRuleFields();
    }

    protected void dlRules_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DeleteCommand":                
                Firewall.Rules.RemoveAt(e.Item.ItemIndex);
                UpdateData();
                break;
            case "EditCommand":                
                isEditingRule = true;
                indexItem = e.Item.ItemIndex;
                
                cbxActivate.Checked = Firewall.Rules[indexItem].isActivate;
                ddlConnection.SelectedValue = Firewall.Rules[indexItem].isAllow ? Resources.Resource.Allow : Resources.Resource.Block;
                fuApplication.Text = Firewall.Rules[indexItem].application;
                ddlDirection.SelectedIndex = (int)Firewall.Rules[e.Item.ItemIndex].direction;
                ddlProtocol.SelectedIndex = (int)Firewall.Rules[indexItem].protocol;

                rbtnIP.Checked = !Firewall.Rules[indexItem].isSubnetwork;
                rbtnSubnetwork.Checked = Firewall.Rules[indexItem].isSubnetwork;

                if (Firewall.Rules[indexItem].isSubnetwork)
                {
                    txtIPSubnetwork.Text = Firewall.Rules[indexItem].ip;
                    txtMaskSubnetwork.Text = Firewall.Rules[indexItem].subnetwork_mask;
                    txtIPAddress.Enabled = false;
                    txtIPSubnetwork.Enabled = txtMaskSubnetwork.Enabled = true;
                }
                else
                {
                    txtIPAddress.Text = Firewall.Rules[indexItem].ip;
                    txtIPAddress.Enabled = true;
                    txtIPSubnetwork.Enabled = txtMaskSubnetwork.Enabled = false;
                }

                txtLocalPorts.Text = Firewall.Rules[indexItem].localPorts;
                txtRemotePorts.Text = Firewall.Rules[indexItem].remotePorts;
                txtCommentApp.Text = Firewall.Rules[indexItem].comment;
                cbxEventARM.Checked = Firewall.Rules[indexItem].eventARM == String.Empty ? false : true;
                if (cbxEventARM.Checked) txtEventARM.Enabled = true;
                else txtEventARM.Enabled = false;
                txtEventARM.Text = Firewall.Rules[indexItem].eventARM;

                ModalPopupExtenderRule.Show();

                break;            
        }
    }

    private void AddNewItem(DataTable dt, FirewallRule rule)
    {
        DataRow dr = dt.NewRow();
        dr[0] = rule.isAllow ? Resources.Resource.Allow : Resources.Resource.Block;
        dr[1] = rule.application;
        dr[2] = rule.direction.ToString();
        dr[3] = rule.protocol.ToString();
        if (rule.isSubnetwork)
        {
            dr[4] = String.Format("{0}/{1}", rule.ip, rule.subnetwork_mask);
        }
        else
        {
            dr[4] = rule.ip;
        }
        dr[5] = rule.localPorts;
        dr[6] = rule.remotePorts;
        dr[7] = rule.eventARM;
        dr[8] = rule.comment;
        dt.Rows.Add(dr);
    }

    private void CreateColumnsRule(DataTable dt)
    {        
        dt.Columns.Add(new DataColumn("Connection", typeof(String)));
        dt.Columns.Add(new DataColumn("Application", typeof(String)));
        dt.Columns.Add(new DataColumn("Direction", typeof(String)));
        dt.Columns.Add(new DataColumn("Protocol", typeof(String)));
        dt.Columns.Add(new DataColumn("IPAddress", typeof(String)));
        dt.Columns.Add(new DataColumn("LocalPorts", typeof(String)));
        dt.Columns.Add(new DataColumn("RemotePorts", typeof(String)));
        dt.Columns.Add(new DataColumn("EventARM", typeof(String)));
        dt.Columns.Add(new DataColumn("Comment", typeof(String)));
    }

    private FirewallRule GetNewRule()
    {
        FirewallRule rule = new FirewallRule();

        rule.isActivate = cbxActivate.Checked;
        rule.isAllow = ddlConnection.SelectedValue.CompareTo(Resources.Resource.Allow) == 0;
        rule.application = fuApplication.Text;
        rule.comment = txtCommentApp.Text;

        rule.isSubnetwork = rbtnSubnetwork.Checked;
        if (rule.isSubnetwork)
        {
            rule.ip = txtIPSubnetwork.Text;
            rule.subnetwork_mask = txtMaskSubnetwork.Text;
        }
        else
        {
            rule.ip = txtIPAddress.Text;
            rule.subnetwork_mask = String.Empty;
        }

        switch (ddlProtocol.SelectedIndex)
        {
            case 0:
                rule.protocol = Protocols.Any;
                break;
            case 1:
                rule.protocol = Protocols.TCP;
                break;
            case 2:
                rule.protocol = Protocols.UDP;
                break;
        }
        switch (ddlDirection.SelectedIndex)
        {
            case 0:
                rule.direction = Directions.Any;
                break;
            case 1:
                rule.direction = Directions.In;
                break;
            case 2:
                rule.direction = Directions.Out;
                break;
        }
        rule.localPorts = txtLocalPorts.Text;
        rule.remotePorts = txtRemotePorts.Text;

        rule.eventARM = (!cbxEventARM.Checked) ? String.Empty : txtEventARM.Text;

        return rule;
    }

    #endregion

    #region FriendlyIP

    protected void lbtnAddFriendlyIPRule_Click(object sender, EventArgs e)
    {
        //проверка на корректность
        if (!ValidateIPAddresses(txtFriendlyIPAddress.Text, false)) return;

        if (!isEditingFriendlyIP)
        {
            Firewall.FriendlyIP.Add(GetNewFriendlyIP());
        }
        else
        {
            Firewall.FriendlyIP[indexItem] = GetNewFriendlyIP();
            isEditingFriendlyIP = false;
        }
        UpdateData();
        txtFriendlyIPAddress.Text = txtFriendlyComment.Text = String.Empty;
    }

    protected void btnCancelZoneIP_Click(object sender, EventArgs e)
    {
        isEditingFriendlyIP = false;
        txtFriendlyIPAddress.Text = txtFriendlyComment.Text = String.Empty;
    }

    protected void dlFriendlyIP_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DeleteCommand":                
                Firewall.FriendlyIP.RemoveAt(e.Item.ItemIndex);
                UpdateData();
                break;
            case "EditCommand":                
                isEditingFriendlyIP = true;
                indexItem = e.Item.ItemIndex;

                txtFriendlyIPAddress.Text = Firewall.FriendlyIP[indexItem].ip;
                txtFriendlyComment.Text = Firewall.FriendlyIP[indexItem].comment;

                ModalPopupExtender.Show();
                break;
        }
    }

    private void AddNewItem(DataTable dt, FriendlyIPRule rule)
    {
        DataRow dr = dt.NewRow();
        dr[0] = rule.ip;
        dr[1] = rule.comment;
        dt.Rows.Add(dr);
    }

    private void CreateColumnsFriendlyIP(DataTable dt)
    {
        dt.Columns.Add(new DataColumn("IPAddress", typeof(String)));
        dt.Columns.Add(new DataColumn("Comment", typeof(String)));
    }

    private FriendlyIPRule GetNewFriendlyIP()
    {
        FriendlyIPRule rule = new FriendlyIPRule();

        rule.ip = txtFriendlyIPAddress.Text;
        rule.comment = txtFriendlyComment.Text;

        return rule;
    }

    #endregion
    

    private bool ValidateIPAddresses(string ipAddresses, bool isOne)
    {
        if (ipAddresses == "*" && !isOne) return true;
        if (ipAddresses == String.Empty) return false;
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
        string[] listIP = ipAddresses.Trim(' ').Split(new char[3] { ',', ';', '-' });
        if (isOne && listIP.Length > 1) return false;
        foreach (string ip in listIP)
        {
            if (!regex.IsMatch(ip))
                return false;
        }
        return true;
    }
    
    #endregion

    #region Profiles

    protected void lbtnRemove_Click(object sender, EventArgs e)
    {
        TaskUserCollection collection =
            new TaskUserCollection(Profile.TasksList);

        collection.Delete(String.Format("Firewall: {0}", ddlProfiles.SelectedValue));
        ddlProfiles.Items.RemoveAt(ddlProfiles.SelectedIndex);
        if (ddlProfiles.Items.Count == 0)
        {
            lbtnRemove.Visible = lbtnEdit.Visible = false;
        }
        collection = collection.Deserialize();

        Profile.TasksList = collection.Serialize();
        Session["TaskUser"] = collection;
    }
    
    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
        EditClick(sender, e, collection.Get(String.Format("Firewall: {0}", ddlProfiles.SelectedValue)));
    }

    public void EditClick(object sender, EventArgs e, TaskUserEntity task)
    {
        string editing = "&Mode=Edit";
        string type = task.Type.ToString();
        Session["CurrentUserTask"] = task;
        Response.Redirect("TaskCreate.aspx?Type=" + type + editing);
    }

    protected void lbtnCreateProfile_Click(object sender, EventArgs e)
    {
        ClearRule();
        TaskUserEntity task = GetCurrentState();
        task.Name = String.Empty;

        string editing = "&Mode=Edit";
        string type = task.Type.ToString();
        Session["CurrentUserTask"] = task;
        Response.Redirect("TaskCreate.aspx?Type=" + type + editing);
    }

    #endregion

    private static RuleConfigureFirewall Firewall;
    private static bool isEditingRule = false;
    private static bool isEditingFriendlyIP = false;
    private static int indexItem;    
}