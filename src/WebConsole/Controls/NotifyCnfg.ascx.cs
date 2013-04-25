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

using System.IO;
using System.Collections.Generic;

using ARM2_dbcontrol.Service.Vba32NS;
using ARM2_dbcontrol.Filters;


public partial class Controls_NotifyCnfg : System.Web.UI.UserControl
{
    private const string notifyEventsCollectionFileName = "Settings\\Vba32NS.xml";
    private List<NotifyEvent> list = null;
    public static object synch = new object();          //объект для lock

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitFields();
        }
        
        Tabs.ActiveTabIndex = int.Parse(ActiveTab.Value);
    }

    private void InitFields()
    {
        cboxMailIsUse.Text = Resources.Resource.UseMail;
        cboxJabberIsUse.Text = Resources.Resource.UseJabber;
        cboxNetSendIsUse.Text = Resources.Resource.UseNetSend;
        rbPriority.Items.Add(Resources.Resource.Low);
        rbPriority.Items.Add(Resources.Resource.Normal);
        rbPriority.Items.Add(Resources.Resource.High);
        rbPriority.SelectedIndex = 0;

        lblMailAddresses.Text = Resources.Resource.Addresses;
        lblJabberAddresses.Text = Resources.Resource.Addresses;
        lblNetSendAddresses.Text = Resources.Resource.Addresses;

        lblMailBody.Text = Resources.Resource.Body;
        lblJabberBody.Text =  Resources.Resource.Body;
        lblNetSendBody.Text = Resources.Resource.Body;

        lblMailSubject.Text = Resources.Resource.Subject;

        tabPanel1.HeaderText = Resources.Resource.Mail;
        tabPanel2.HeaderText = Resources.Resource.Jabber;
        tabPanel3.HeaderText = "NetSend";

        //LoadState("vba32.virus.found");
        hdnJabber.Value = hdnMail.Value = hdnNetSend.Value = "";

    }

    private void Clear()
    {
        cboxMailIsUse.Checked = false;
        tboxMailSubject.Text = String.Empty;
        tboxMailBody.Text = String.Empty;
        lboxMailAddresses.Items.Clear();
        rbPriority.SelectedIndex = 0;

        //jabber
        cboxJabberIsUse.Checked = false;
        tboxJabberBody.Text = String.Empty;
        lboxJabberAddresses.Items.Clear();

        //NetSend
        cboxNetSendIsUse.Checked = false;
        tboxNetSendBody.Text = String.Empty; ;
        lboxNetSendAddresses.Items.Clear();

        hdnJabber.Value = hdnMail.Value = hdnNetSend.Value = "";
    }

    public void LoadState(string eventName)
    {
        if (list == null)
            list = GetList();

        if (list == null)
        {
            throw new Exception("list.Count==null");
        }

        Clear();

        list.Find(delegate(NotifyEvent ev)
        {
            if (ev.EventName == eventName)
            {
                //mail
                cboxMailIsUse.Checked = ev.Mail.IsUse;
                                
                tboxMailSubject.Text = ev.Mail.Subject;
                tboxMailBody.Text = ev.Mail.Message;
                foreach (string str in ev.Mail.AddrList)
                {
                    lboxMailAddresses.Items.Add(str);
                    hdnMail.Value += str + ";";
                }
                if (ev.Mail.Priority < 0 || ev.Mail.Priority > 2)
                    rbPriority.SelectedIndex = 0;
                else
                {
                    rbPriority.SelectedIndex = ev.Mail.Priority;
                }
                if (ev.Mail.IsUse)
                {
                    tboxMailAddresses.Enabled = tboxMailBody.Enabled = tboxMailSubject.Enabled = true;
                    tblMail.Disabled = false;
                }
                else
                {
                    tboxMailAddresses.Enabled = tboxMailBody.Enabled = tboxMailSubject.Enabled = false;
                    tblMail.Disabled = true;
                }

                //jabber
                cboxJabberIsUse.Checked = ev.Jabber.IsUse;
                tboxJabberBody.Text = ev.Jabber.Message;
                foreach (string str in ev.Jabber.AddrList)
                {
                    lboxJabberAddresses.Items.Add(str);
                    hdnJabber.Value += str + ";";
                }
                if (ev.Jabber.IsUse)
                {
                    tboxJabberAddresses.Enabled = tboxJabberBody.Enabled = true;
                    tblJabber.Disabled = false;
                }
                else
                {
                    tboxJabberAddresses.Enabled = tboxJabberBody.Enabled = false;
                    tblJabber.Disabled = true;
                }
                //NetSend
                cboxNetSendIsUse.Checked = ev.NetSend.IsUse;
                tboxNetSendBody.Text = ev.NetSend.Message;
                foreach (string str in ev.NetSend.AddrList)
                {
                    lboxNetSendAddresses.Items.Add(str);
                    hdnNetSend.Value += str + ";";
                }
                if (ev.NetSend.IsUse)
                {
                    tboxNetSendAddresses.Enabled = tboxNetSendBody.Enabled = true;
                    tblNetSend.Disabled = false;
                }
                else
                {
                    tboxNetSendAddresses.Enabled = tboxNetSendBody.Enabled = false;
                    tblNetSend.Disabled = true;
                }

                return true;
            }
            return false;
        });

    }
    public void SaveState(string eventName,bool notify)
    {
        GetClientState();
        ValidateFields();
        //Инициализация события
        NotifyEvent ev = new NotifyEvent();

        ev.EventName = eventName;
        ev.IsNotify = notify;

        //mail
        ev.Mail.IsUse = cboxMailIsUse.Checked;
        ev.Mail.Subject = tboxMailSubject.Text;
        ev.Mail.Message = tboxMailBody.Text;
        ev.Mail.Priority = rbPriority.SelectedIndex;
        foreach (ListItem item in lboxMailAddresses.Items)
            ev.Mail.AddrList.Add(item.Text);

        //jabber
        ev.Jabber.IsUse = cboxJabberIsUse.Checked;
        ev.Jabber.Message = tboxJabberBody.Text;

        foreach (ListItem item in lboxJabberAddresses.Items)
            ev.Jabber.AddrList.Add(item.Text);

        //NetSend
        ev.NetSend.IsUse = cboxNetSendIsUse.Checked;
        ev.NetSend.Message = tboxNetSendBody.Text;

        foreach (ListItem item in lboxNetSendAddresses.Items)
            ev.NetSend.AddrList.Add(item.Text);

        if (list == null)
            list = GetList();
        
        //Удаление старого
        list.RemoveAll(delegate(NotifyEvent tev)
        {
            if (tev.EventName == eventName)
                return true;
            return false;
        });

        //добавляем новый
        list.Add(ev);

        //сохраняем 
        SaveList();
    }

    /// <summary>
    /// Возвращает список событий и их настроек
    /// </summary>
    /// <returns></returns>
    private List<NotifyEvent> GetList()
    {
        List<NotifyEvent> tlist = null;
        lock (synch)
        {
            tlist =
                ObjectSerializer.XmlFileToObj<List<NotifyEvent>>(Server.MapPath(notifyEventsCollectionFileName));
        }
        if (tlist == null)
        {
            //throw new Exception("Список после десериализации пуст");
            tlist = new List<NotifyEvent>();
            if (!File.Exists(Server.MapPath(notifyEventsCollectionFileName)))
                ObjectSerializer.ObjToXmlStr(Server.MapPath(notifyEventsCollectionFileName), tlist);
        }
        return tlist;
    }

    /// <summary>
    /// Сохраняет список событий
    /// </summary>
    private void SaveList()
    {
        lock (synch)
        {
            ObjectSerializer.ObjToXmlStr(Server.MapPath(notifyEventsCollectionFileName), list);
        }
    }
 

   public bool ValidateFields()
    {
        System.Net.Mail.MailAddress ml;
        foreach (ListItem item in lboxMailAddresses.Items)
        {
            if (item.Text == String.Empty) break;
            try 
            {
                ml = new System.Net.Mail.MailAddress(item.Text);
            }
            catch 
            {
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.Mail);
            }
        }

        Validation vld;
        foreach (ListItem item in lboxNetSendAddresses.Items)
        {
            if (item.Text == String.Empty) break;
            vld = new Validation(item.Text);
            if (!vld.CheckIP())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                 + "NetSend");
        }


        return true;
    }

    private void GetClientState()
    {
        lboxMailAddresses.Items.Clear();
        lboxJabberAddresses.Items.Clear();
        lboxNetSendAddresses.Items.Clear();
        foreach (string str in hdnMail.Value.Split(';'))
        {
            if (str != String.Empty && str != null)
                lboxMailAddresses.Items.Add(str);
        }

        foreach (string str in hdnJabber.Value.Split(';'))
        {
            if (str != String.Empty && str != null)
                lboxJabberAddresses.Items.Add(str);
        }

        foreach (string str in hdnNetSend.Value.Split(';'))
        {
            if (str != String.Empty && str != null)
                lboxNetSendAddresses.Items.Add(str);
        }
    }
}
