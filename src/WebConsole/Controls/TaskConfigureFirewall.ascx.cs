using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Service.Vba32NS;
using System.Data;
using System.Text;
using System.Xml;
using System.IO;
using ARM2_dbcontrol.Tasks.ConfigureFirewall;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

public partial class Controls_TaskConfigureFirewall : System.Web.UI.UserControl, ITask
{
    private static TaskConfigureFirewall firewall;
    private static List<FirewallRuleProtocol> RuleProtocols;
    private Int32 IP4AddedRulesCount = 0;
    private Int32 IP6AddedRulesCount = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitFields();
        }
        InitFieldsJournalEvent(firewall.journalEvent);
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

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        SetEnabled();
        if (firewall == null)
        {
            firewall = new TaskConfigureFirewall(GetEvents());
            firewall.Vba32CCUser = Anchor.GetStringForTaskGivedUser();
        }
        else
        {
            firewall.IP4Rules.Clear();
            firewall.IP6Rules.Clear();
            firewall.journalEvent.ClearEvents();
        }

        if (RuleProtocols == null)
        {
            RuleProtocols = new List<FirewallRuleProtocol>();
            LoadRuleProtocols();
        }
        else
        {
            RuleProtocols.Clear();
            LoadRuleProtocols();
        }

        IP4dlAddRules.DataSource = RuleProtocols;
        IP4dlAddRules.DataBind();
        IP6dlAddRules.DataSource = RuleProtocols;
        IP6dlAddRules.DataBind();

        IP4UpdateData();
        IP6UpdateData();
        UpdateGeneralSettings();
    }

    private void UpdateGeneralSettings()
    {
        chkFirewallOn.Checked = firewall.firewall_On;
        ddlFirewallNetworkType.SelectedIndex = (Int32)firewall.NetworkType;
    }

    private void SetEnabled()
    {
        chkFirewallOn.Enabled = ddlFirewallNetworkType.Enabled = tblIP4UpdatePanel.Enabled = tblIP6UpdatePanel.Enabled = JournalEventTable.Enabled = _enabled;
    }

    private String[] GetEvents()
    {
        String[] s = { "JE_VND_APPLIED_RULES_FAILED", "JE_VND_APPLIED_RULES_OK", "JE_VND_AUIDIT_OTHER", "JE_VND_AUIDIT_TCP", "JE_VND_AUIDIT_UDP", "JE_VND_START", "JE_VND_STOP" };
        return s;
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.Firewall;
        SaveGeneralSettings();
        SaveJournalEvents();
        task.Param = firewall.SaveToXml();
        return task;
    }

    private void SaveGeneralSettings()
    {
        firewall.firewall_On = chkFirewallOn.Checked;
        firewall.NetworkType = (FirewallNetworkType)ddlFirewallNetworkType.SelectedIndex;
    }

    private void SaveJournalEvents()
    {
        JournalEvent je = new JournalEvent(GetEvents());
        for (int i = 0; i < JournalEventTable.Rows.Count - 1; i++)
        {

            if ((JournalEventTable.Rows[i + 1].Cells[1].Controls[0] as CheckBox).Checked == true)
            {
                je.Events[i].EventFlag |= EventJournalFlags.WindowsJournal;
            }
            if ((JournalEventTable.Rows[i + 1].Cells[2].Controls[0] as CheckBox).Checked == true)
            {
                je.Events[i].EventFlag |= EventJournalFlags.LocalJournal;
            }
            if ((JournalEventTable.Rows[i + 1].Cells[3].Controls[0] as CheckBox).Checked == true)
            {
                je.Events[i].EventFlag |= EventJournalFlags.CCJournal;
            }
        }
        firewall.journalEvent = je;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.Firewall)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
        firewall.LoadFromXml(task.Param);
        IP4UpdateData();
        IP6UpdateData();
        UpdateGeneralSettings();
        LoadJournalEvent(firewall.journalEvent);
    }

    public String BuildTask()
    {
        return firewall.GetTask();
    }

    #region IP4
    private void IP4UpdateData()
    {
        IP4dlRules.DataSource = firewall.IP4Rules;
        IP4dlRules.DataBind();
    }

    protected void IP4ApplyTcpUdpRuleDialogButtonClick(object sender, EventArgs e)
    {
        FirewallRule rule = new FirewallRule();
        rule.Name = IP4txtNameDialog.Text;
        rule.LocalIP = IP4txtLocalIPDialog.Text;
        rule.LocalPort = IP4txtLocalPortDialog.Text;
        rule.DestinationIP = IP4txtDestinationIPDialog.Text;
        rule.DestinationPort = IP4txtDestinationPortDialog.Text;
        rule.Audit = IP4chkAuditTcpUdpDialog.Checked;
        rule.Protocol = IP4selectProtocolDialog.SelectedValue;
        rule.Rule = (RulesEnum)IP4selectRuleDialog.SelectedIndex;
        firewall.IP4Rules.Add(rule);
        IP4UpdateData();
    }

    protected void IP4ApplyRuleDialogButtonClick(object sender, EventArgs e)
    {
        FirewallRule rule = new FirewallRule();
        rule.Name = IP4AddRuleDialogName.Text;
        rule.LocalIP = IP4addRuleLocalIP.Text;
        rule.LocalPort = "*";
        rule.DestinationIP = IP4addRuleDestinationIP.Text;
        rule.DestinationPort = "*";
        rule.Audit = IP4chkAuditDialog.Checked;
        String protocols = "";
        for (int i = 0; i < IP4dlAddRules.Items.Count; i++)
        {
            if ((IP4dlAddRules.Items[i].FindControl("IP4chkAddRuleProtocol") as CheckBox).Checked)
            {
                protocols += i.ToString() + ",";
            }
        }
        protocols += IP4addRuleOtherProtocol.Text;
        if (protocols.Length > 0)
        {
            if (protocols[protocols.Length - 1] == ',') protocols = protocols.Remove(protocols.Length - 1, 1);
        }
        rule.Protocol = protocols;
        rule.Rule = (RulesEnum)IP4addRuleActionSelect.SelectedIndex;
        firewall.IP4Rules.Add(rule);
        IP4UpdateData();
    }

    protected void IP4dlRules_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            String protocol = ((FirewallRule)e.Item.DataItem).Protocol;
            if (protocol == "TCP" || protocol == "UDP")
            {
                (e.Item.FindControl("IP4trFirewallItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("ruleType", "TcpUdp");
            }
            else
            {
                (e.Item.FindControl("IP4trFirewallItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("ruleType", "NotTcpUdp");
            }
            (e.Item.FindControl("IP4hdnRowNo") as HiddenField).Value = (++IP4AddedRulesCount).ToString();
            (e.Item.FindControl("IP4hdnAudit") as HiddenField).Value = ((FirewallRule)e.Item.DataItem).Audit.ToString();
            (e.Item.FindControl("IP4chkEnable") as CheckBox).Checked = ((FirewallRule)e.Item.DataItem).Enable;
            (e.Item.FindControl("IP4lblName") as Label).Text = ((FirewallRule)e.Item.DataItem).Name;
            (e.Item.FindControl("IP4lblLocalIP") as Label).Text = ((FirewallRule)e.Item.DataItem).LocalIP;
            (e.Item.FindControl("IP4lblLocalPort") as Label).Text = ((FirewallRule)e.Item.DataItem).LocalPort;
            (e.Item.FindControl("IP4lblDestinationIP") as Label).Text = ((FirewallRule)e.Item.DataItem).DestinationIP;
            (e.Item.FindControl("IP4lblDestinationPort") as Label).Text = ((FirewallRule)e.Item.DataItem).DestinationPort;
            (e.Item.FindControl("IP4lblProtocol") as Label).Text = ((FirewallRule)e.Item.DataItem).Protocol;
            (e.Item.FindControl("IP4lblRule") as Label).Text = ResourceControl.GetStringForCurrentCulture(((FirewallRule)e.Item.DataItem).Rule.ToString());
            (e.Item.FindControl("IP4lblRule") as Label).Attributes.Add("valueRule", ((FirewallRule)e.Item.DataItem).Rule.ToString());
            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("IP4trFirewallItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("IP4trFirewallItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void IP4dlRules_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        IP4dlRules.EditItemIndex = e.Item.ItemIndex;
        IP4dlRules.SelectedIndex = e.Item.ItemIndex;
        IP4UpdateData();
    }

    protected void IP4DeleteButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP4hdnActiveRowNo.Value);
        if (index != 0)
        {
            firewall.IP4Rules.RemoveAt(index - 1);
            IP4hdnActiveRowNo.Value = Convert.ToString(0);
            IP4UpdateData();
        }
    }

    protected void IP4MoveUpButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP4hdnActiveRowNo.Value);
        if (index != 0 & index != 1)
        {
            firewall.IP4Rules.Reverse(index - 2, 2);
            IP4hdnActiveRowNo.Value = Convert.ToString(index - 1);
            IP4UpdateData();
        }
    }

    protected void IP4MoveDownButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP4hdnActiveRowNo.Value);
        if (index != 0 & index != firewall.IP4Rules.Count)
        {
            firewall.IP4Rules.Reverse(index - 1, 2);
            IP4hdnActiveRowNo.Value = Convert.ToString(index + 1);
            IP4UpdateData();
        }
    }

    protected void IP4ChangeTcpUdpRuleDialogButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP4hdnActiveRowNo.Value);
        if (index != 0)
        {
            FirewallRule rule = firewall.IP4Rules[index - 1];
            rule.Name = IP4txtNameDialog.Text;
            rule.LocalIP = IP4txtLocalIPDialog.Text;
            rule.LocalPort = IP4txtLocalPortDialog.Text;
            rule.DestinationIP = IP4txtDestinationIPDialog.Text;
            rule.DestinationPort = IP4txtDestinationPortDialog.Text;
            rule.Audit = IP4chkAuditTcpUdpDialog.Checked;
            rule.Protocol = IP4selectProtocolDialog.SelectedValue;
            rule.Rule = (RulesEnum)IP4selectRuleDialog.SelectedIndex;
            firewall.IP4Rules.Insert(index - 1, rule);
            firewall.IP4Rules.RemoveAt(index);
            IP4UpdateData();
        }
    }

    protected void IP4ChangeRulesDialogButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP4hdnActiveRowNo.Value);
        if (index != 0)
        {
            FirewallRule rule = firewall.IP4Rules[index - 1];
            rule.Name = IP4AddRuleDialogName.Text;
            rule.LocalIP = IP4addRuleLocalIP.Text;
            rule.LocalPort = "*";
            rule.DestinationIP = IP4addRuleDestinationIP.Text;
            rule.DestinationPort = "*";
            rule.Audit = IP4chkAuditDialog.Checked;
            String protocols = "";
            for (int i = 0; i < IP4dlAddRules.Items.Count; i++)
            {
                if ((IP4dlAddRules.Items[i].FindControl("IP4chkAddRuleProtocol") as CheckBox).Checked)
                {
                    protocols += i.ToString() + ",";
                }
            }
            protocols += IP4addRuleOtherProtocol.Text;
            if (protocols[protocols.Length - 1] == ',') protocols = protocols.Remove(protocols.Length - 1, 1);
            rule.Protocol = protocols;
            rule.Rule = (RulesEnum)IP4addRuleActionSelect.SelectedIndex;
            firewall.IP4Rules.Insert(index - 1, rule);
            firewall.IP4Rules.RemoveAt(index);
            IP4UpdateData();
        }
    }

    protected void IP4dlAddRules_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            (e.Item.FindControl("IP4chkAddRuleProtocol") as CheckBox).Checked = ((FirewallRuleProtocol)e.Item.DataItem).IsChecked;
            (e.Item.FindControl("IP4lblAddRuleProtocolName1") as Label).Text = ((FirewallRuleProtocol)e.Item.DataItem).Name;
            (e.Item.FindControl("IP4lblAddRuleProtocolName2") as Label).Text = ((FirewallRuleProtocol)e.Item.DataItem).FullName;
            (e.Item.FindControl("IP4chkAddRuleProtocol") as CheckBox).Attributes.Add("no", ((FirewallRuleProtocol)e.Item.DataItem).No.ToString());
            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("tr_IP4dlAddRules") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("tr_IP4dlAddRules") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void IP4chkEnable_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        int index = Convert.ToInt32((chk.Parent.Controls[3] as HiddenField).Value);
        FirewallRule rule = firewall.IP4Rules[index - 1];
        rule.Enable = chk.Checked;
        firewall.IP4Rules.Insert(index - 1, rule);
        firewall.IP4Rules.RemoveAt(index);
        IP4UpdateData();
    }

    #endregion

    #region IP6
    private void IP6UpdateData()
    {
        IP6dlRules.DataSource = firewall.IP6Rules;
        IP6dlRules.DataBind();
    }

    protected void IP6ApplyTcpUdpRuleDialogButtonClick(object sender, EventArgs e)
    {
        FirewallRule rule = new FirewallRule();
        rule.Name = IP6txtNameDialog.Text;
        rule.LocalIP = IP6txtLocalIPDialog.Text;
        rule.LocalPort = IP6txtLocalPortDialog.Text;
        rule.DestinationIP = IP6txtDestinationIPDialog.Text;
        rule.DestinationPort = IP6txtDestinationPortDialog.Text;
        rule.Audit = IP6chkAuditTcpUdpDialog.Checked;
        rule.Protocol = IP6selectProtocolDialog.SelectedValue;
        rule.Rule = (RulesEnum)IP6selectRuleDialog.SelectedIndex;
        firewall.IP6Rules.Add(rule);
        IP6UpdateData();
    }

    protected void IP6ApplyRuleDialogButtonClick(object sender, EventArgs e)
    {
        FirewallRule rule = new FirewallRule();
        rule.Name = IP6AddRuleDialogName.Text;
        rule.LocalIP = IP6addRuleLocalIP.Text;
        rule.LocalPort = "*";
        rule.DestinationIP = IP6addRuleDestinationIP.Text;
        rule.DestinationPort = "*";
        rule.Audit = IP6chkAuditDialog.Checked;
        String protocols = "";
        for (int i = 0; i < IP6dlAddRules.Items.Count; i++)
        {
            if ((IP6dlAddRules.Items[i].FindControl("IP6chkAddRuleProtocol") as CheckBox).Checked)
            {
                protocols += i.ToString() + ",";
            }
        }
        protocols += IP6addRuleOtherProtocol.Text;
        if (protocols.Length > 0)
        {
            if (protocols[protocols.Length - 1] == ',') protocols = protocols.Remove(protocols.Length - 1, 1);
        }
        rule.Protocol = protocols;
        rule.Rule = (RulesEnum)IP6addRuleActionSelect.SelectedIndex;
        firewall.IP6Rules.Add(rule);
        IP6UpdateData();
    }

    protected void IP6dlRules_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            String protocol = ((FirewallRule)e.Item.DataItem).Protocol;
            if (protocol == "TCP" || protocol == "UDP")
            {
                (e.Item.FindControl("IP6trFirewallItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("ruleType", "TcpUdp");
            }
            else
            {
                (e.Item.FindControl("IP6trFirewallItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("ruleType", "NotTcpUdp");
            }
            (e.Item.FindControl("IP6hdnRowNo") as HiddenField).Value = (++IP6AddedRulesCount).ToString();
            (e.Item.FindControl("IP6hdnAudit") as HiddenField).Value = ((FirewallRule)e.Item.DataItem).Audit.ToString();
            (e.Item.FindControl("IP6chkEnable") as CheckBox).Checked = ((FirewallRule)e.Item.DataItem).Enable;
            (e.Item.FindControl("IP6lblName") as Label).Text = ((FirewallRule)e.Item.DataItem).Name;
            (e.Item.FindControl("IP6lblLocalIP") as Label).Text = ((FirewallRule)e.Item.DataItem).LocalIP;
            (e.Item.FindControl("IP6lblLocalPort") as Label).Text = ((FirewallRule)e.Item.DataItem).LocalPort;
            (e.Item.FindControl("IP6lblDestinationIP") as Label).Text = ((FirewallRule)e.Item.DataItem).DestinationIP;
            (e.Item.FindControl("IP6lblDestinationPort") as Label).Text = ((FirewallRule)e.Item.DataItem).DestinationPort;
            (e.Item.FindControl("IP6lblProtocol") as Label).Text = ((FirewallRule)e.Item.DataItem).Protocol;
            (e.Item.FindControl("IP6lblRule") as Label).Text = ResourceControl.GetStringForCurrentCulture(((FirewallRule)e.Item.DataItem).Rule.ToString());
            (e.Item.FindControl("IP6lblRule") as Label).Attributes.Add("valueRule", ((FirewallRule)e.Item.DataItem).Rule.ToString());
            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("IP6trFirewallItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("IP6trFirewallItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void IP6dlRules_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        IP6dlRules.EditItemIndex = e.Item.ItemIndex;
        IP6dlRules.SelectedIndex = e.Item.ItemIndex;
        IP6UpdateData();
    }

    protected void IP6DeleteButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP6hdnActiveRowNo.Value);
        if (index != 0)
        {
            firewall.IP6Rules.RemoveAt(index - 1);
            IP6hdnActiveRowNo.Value = Convert.ToString(0);
            IP6UpdateData();
        }
    }

    protected void IP6MoveUpButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP6hdnActiveRowNo.Value);
        if (index != 0 & index != 1)
        {
            firewall.IP6Rules.Reverse(index - 2, 2);
            IP6hdnActiveRowNo.Value = Convert.ToString(index - 1);
            IP6UpdateData();
        }
    }

    protected void IP6MoveDownButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP6hdnActiveRowNo.Value);
        if (index != 0 & index != firewall.IP6Rules.Count)
        {
            firewall.IP6Rules.Reverse(index - 1, 2);
            IP6hdnActiveRowNo.Value = Convert.ToString(index + 1);
            IP6UpdateData();
        }
    }

    protected void IP6ChangeTcpUdpRuleDialogButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP6hdnActiveRowNo.Value);
        if (index != 0)
        {
            FirewallRule rule = firewall.IP6Rules[index - 1];
            rule.Name = IP6txtNameDialog.Text;
            rule.LocalIP = IP6txtLocalIPDialog.Text;
            rule.LocalPort = IP6txtLocalPortDialog.Text;
            rule.DestinationIP = IP6txtDestinationIPDialog.Text;
            rule.DestinationPort = IP6txtDestinationPortDialog.Text;
            rule.Audit = IP6chkAuditTcpUdpDialog.Checked;
            rule.Protocol = IP6selectProtocolDialog.SelectedValue;
            rule.Rule = (RulesEnum)IP6selectRuleDialog.SelectedIndex;
            firewall.IP6Rules.Insert(index - 1, rule);
            firewall.IP6Rules.RemoveAt(index);
            IP6UpdateData();
        }
    }

    protected void IP6ChangeRulesDialogButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(IP6hdnActiveRowNo.Value);
        if (index != 0)
        {
            FirewallRule rule = firewall.IP6Rules[index - 1];
            rule.Name = IP6AddRuleDialogName.Text;
            rule.LocalIP = IP6addRuleLocalIP.Text;
            rule.LocalPort = "*";
            rule.DestinationIP = IP6addRuleDestinationIP.Text;
            rule.DestinationPort = "*";
            rule.Audit = IP6chkAuditDialog.Checked;
            String protocols = "";
            for (int i = 0; i < IP6dlAddRules.Items.Count; i++)
            {
                if ((IP6dlAddRules.Items[i].FindControl("IP6chkAddRuleProtocol") as CheckBox).Checked)
                {
                    protocols += i.ToString() + ",";
                }
            }
            protocols += IP6addRuleOtherProtocol.Text;
            if (protocols[protocols.Length - 1] == ',')
                protocols = protocols.Remove(protocols.Length - 1, 1);
            rule.Protocol = protocols;
            rule.Rule = (RulesEnum)IP6addRuleActionSelect.SelectedIndex;
            firewall.IP6Rules.Insert(index - 1, rule);
            firewall.IP6Rules.RemoveAt(index);
            IP6UpdateData();
        }
    }

    protected void IP6dlAddRules_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            (e.Item.FindControl("IP6chkAddRuleProtocol") as CheckBox).Checked = ((FirewallRuleProtocol)e.Item.DataItem).IsChecked;
            (e.Item.FindControl("IP6lblAddRuleProtocolName1") as Label).Text = ((FirewallRuleProtocol)e.Item.DataItem).Name;
            (e.Item.FindControl("IP6lblAddRuleProtocolName2") as Label).Text = ((FirewallRuleProtocol)e.Item.DataItem).FullName;
            (e.Item.FindControl("IP6chkAddRuleProtocol") as CheckBox).Attributes.Add("no", ((FirewallRuleProtocol)e.Item.DataItem).No.ToString());
            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("tr_IP6dlAddRules") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("tr_IP6dlAddRules") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void IP6chkEnable_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        int index = Convert.ToInt32((chk.Parent.Controls[3] as HiddenField).Value);
        FirewallRule rule = firewall.IP6Rules[index - 1];
        rule.Enable = chk.Checked;
        firewall.IP6Rules.Insert(index - 1, rule);
        firewall.IP6Rules.RemoveAt(index);
        IP6UpdateData();
    }

    #endregion

    #region RuleProtocols
    private void LoadRuleProtocols()
    {
        FirewallRuleProtocol rule = new FirewallRuleProtocol();
        for (int i = 0; i < TaskConfigureFirewall.ProtocolsName.Length; i++)
        {
            rule = new FirewallRuleProtocol();
            rule.No = i;
            rule.Name = CreateProtocolName(TaskConfigureFirewall.ProtocolsName[i]);
            rule.FullName = ResourceControl.GetStringForCurrentCulture(TaskConfigureFirewall.ProtocolsName[i]);
            RuleProtocols.Add(rule);
        }
    }

    private string CreateProtocolName(string pnName)
    {
        if (pnName == "pnAX_25") return "AX.25";
        if (pnName == "pnTP") return "TP++";
        string Name = pnName.Remove(0, 2);
        Name = Name.Replace('_', '-');
        return Name;
    }
    #endregion

    #region JournalEvents

    private void InitFieldsJournalEvent(JournalEvent _events)
    {
        if (_events == null)
            return;

        if (JournalEventTable.Rows.Count == 1)
        {
            for (Int32 i = 0; i < _events.Events.Length; i++)
            {
                JournalEventTable.Rows.Add(GenerateRow(_events.Events[i], i));
            }
        }
    }

    private void LoadJournalEvent(JournalEvent _events)
    {
        if (_events == null)
            return;

        Boolean isChecked = false;
        for (Int32 i = 0; i < _events.Events.Length; i++)
        {
            for (Int32 j = 0; j < 3; j++)
            {
                switch (j)
                {
                    case 0:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.WindowsJournal) == EventJournalFlags.WindowsJournal;
                        break;
                    case 1:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.LocalJournal) == EventJournalFlags.LocalJournal;
                        break;
                    case 2:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.CCJournal) == EventJournalFlags.CCJournal;
                        break;
                }
                (JournalEventTable.Rows[i + 1].Cells[j + 1].Controls[0] as CheckBox).Checked = isChecked;
            }
        }
    }

    private TableRow GenerateRow(SingleJournalEvent ev, int rowNo)
    {
        String eventName = ev.EventName;
        EventJournalFlags val = ev.EventFlag;

        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.Attributes.Add("align", "center");
        Label l = new Label();
        l.Text = eventName;
        cell.Controls.Add(l);
        row.Cells.Add(cell);
        for (Int32 i = 0; i < 3; i++)
        {
            cell = new TableCell();
            CheckBox chk = new CheckBox();
            chk.Checked = false;
   //         chk.Attributes.Add("rowNo", rowNo.ToString());
   //         chk.Attributes.Add("colNo", i.ToString());


            cell.Controls.Add(chk);
            cell.Attributes.Add("align", "center");
            row.Cells.Add(cell);
        }

        return row;
    }




    #endregion
}
