<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_NotifyCnfg" Codebehind="NotifyCnfg.ascx.cs" %>

<script language="javascript" type="text/javascript">
    function IsMail(value) {
        var mail = new RegExp(/^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$/);
        return mail.test(value);
    }

    //Select
    function AddToSelect(path, sel, hdn, isMail) {
       if(path.value == "")
           return;

       if (isMail && !IsMail(path.value)) {
           alert('<%=Resources.Resource.ErrorInvalidEmail%>');
           return;
       }
       
       for(var i=0;i<sel.options.length;i++)
            if(sel.options[i].value == path.value)
            {
                var message = "Value '" +
                  path.value + "' is exist in list";
                alert(message);
                return;
            }

        var newOpt = new Option();
        newOpt.text = path.value;
        newOpt.value = path.value;
        path.value = "";
        
        sel.options[sel.options.length] = newOpt;       
        
        SaveClickHandler(sel, hdn);
    }
    
    function DeleteFromSelect(sel, hdn)
    {
        if(sel.selectedIndex  == -1) return;
        
        sel.options[sel.selectedIndex] = null;  
        
        SaveClickHandler(sel, hdn);
    }
    
    function SaveClickHandler(listBox, hdn) 
    {
        var elements = ""; 
        var intCount = listBox.options.length; 

        for (i = 0; i < intCount; i++) 
        {
            elements += listBox.options[i].text + ';'; 
        } 

        hdn.value = elements; 
    }

   function ActiveTabChanged(sender, e) 
   {        
        hidden = $get('<%=ActiveTab.ClientID%>');
        
        hidden.value = sender.get_activeTab().get_tabIndex();
   }
   
   function HideOptionsMail()
     {
        var cbox = $get("<%=cboxMailIsUse.ClientID%>");
        
        var elem = $get("<%=tblMail.ClientID%>");
        var elem1 = $get("<%=tboxMailSubject.ClientID%>");
        var elem2 = $get("<%=tboxMailAddresses.ClientID%>");
        var elem3 = $get("<%=tboxMailBody.ClientID%>");
                
        elem.disabled = elem1.disabled = elem2.disabled = elem3.disabled = !cbox.checked;
     }
     
     function HideOptionsJabber()
     {
        var cbox = $get("<%=cboxJabberIsUse.ClientID%>");
        
        var elem = $get("<%=tblJabber.ClientID%>");
        var elem1 = $get("<%=tboxJabberAddresses.ClientID%>");
        var elem2 = $get("<%=tboxJabberBody.ClientID%>");       
                
        elem.disabled = elem1.disabled = elem2.disabled = !cbox.checked;
     }
     
     function HideOptionsNetSend()
     {
        var cbox = $get("<%=cboxNetSendIsUse.ClientID%>");
        
        var elem = $get("<%=tblNetSend.ClientID%>");
        var elem1 = $get("<%=tboxNetSendAddresses.ClientID%>");
        var elem2 = $get("<%=tboxNetSendBody.ClientID%>");
                
        elem.disabled = elem1.disabled = elem2.disabled = !cbox.checked;
     }
   
</script>
<asp:HiddenField runat="server" ID="ActiveTab" Value="0" />

<asp:HiddenField runat="server" ID="hdnMail" Value="" />
<asp:HiddenField runat="server" ID="hdnJabber" Value="" />
<asp:HiddenField runat="server" ID="hdnNetSend" Value="" />

<ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="540px" OnClientActiveTabChanged="ActiveTabChanged">
<ajaxToolkit:TabPanel runat="server" ID="tabPanel1">
 <ContentTemplate>
 <asp:CheckBox ID="cboxMailIsUse" runat="server" onclick="HideOptionsMail()" />
<table class="NotifyControl" runat="server" id="tblMail" disabled="true">
    <tr>
        <td>
            <asp:Label ID="lblMailSubject" runat="server">Subject</asp:Label>
        </td>
        <td style="width:170px;">
            <asp:Label ID="lblMailAddresses" runat="server">Addresses</asp:Label>
        </td>
        <td align=left> 
        </td>
    </tr>
    <tr>
        <td style="width:230px;">
            <asp:TextBox ID="tboxMailSubject" runat="server" style="width:260px" Enabled="false"  />
        </td>
        <td style="width:170px;">
            <asp:TextBox ID="tboxMailAddresses" runat="server" style="width:160px" Enabled="false"  />
        </td>
        <td valign="middle" align="left">
            <a id="lbtnMailAdd" onclick="AddToSelect($get('<%=tboxMailAddresses.ClientID%>'),$get('<%=lboxMailAddresses.ClientID%>'),$get('<%=hdnMail.ClientID%>'), 1);"><%=Resources.Resource.Add%></a>
        </td>
    </tr>
    <tr>
        <td style="width:270px;">
            <asp:Label ID="lblMailBody" runat="server">Body</asp:Label>
            <asp:TextBox ID="tboxMailBody" Enabled="false" runat="server" TextMode="MultiLine" style="width:260px;height:100px" />
            
        </td>
        <td style="width:170px;">
            <asp:ListBox ID="lboxMailAddresses" runat="server" Width="160px" Height="120px"></asp:ListBox>
        </td>
        <td valign=top align=left>
            <a id="lbtnMailDelete" onclick="DeleteFromSelect($get('<%=lboxMailAddresses.ClientID%>'),$get('<%=hdnMail.ClientID%>'));"  ><%=Resources.Resource.Delete%></a>
        </td>
    </tr>
    <tr>
        <td colspan=2>
            <asp:RadioButtonList ID="rbPriority" runat="server">
            </asp:RadioButtonList>
        </td>
         <td  align=left> 
        </td>
    </tr>
</table>
</ContentTemplate>
</ajaxToolkit:TabPanel>
<ajaxToolkit:TabPanel runat="server" ID="tabPanel2">
 <ContentTemplate>
 <asp:CheckBox ID="cboxJabberIsUse" runat="server" onclick="HideOptionsJabber()"/>
<table class="NotifyControl" runat="server" id="tblJabber" disabled="true">
    <tr>
        <td>
        </td>
        <td style="width:170px;">
            <asp:Label ID="lblJabberAddresses" runat="server">Addresses</asp:Label>
        </td>
        <td align="left">
        </td>
    </tr>
    <tr>
        <td style="width:230px;">
            
        </td>
        <td style="width:170px;">
            <asp:TextBox ID="tboxJabberAddresses" runat="server" style="width:160px" Enabled="false"/>
        </td>
        <td valign=middle align=left>
            <a id="lbtnJabberAdd" onclick="AddToSelect($get('<%=tboxJabberAddresses.ClientID%>'),$get('<%=lboxJabberAddresses.ClientID%>'),$get('<%=hdnJabber.ClientID%>'), 0);"><%=Resources.Resource.Add%></a>
        </td>
    </tr>
    <tr>
        <td style="width:270px;">
            <asp:Label ID="lblJabberBody" runat=server>Body</asp:Label><br/>
            <asp:TextBox ID="tboxJabberBody" Enabled="false" runat="server" TextMode=MultiLine style="width:260px;height:100px"  />
        </td>
        <td style="width:170px;">
            <asp:ListBox ID="lboxJabberAddresses" runat="server" Width="160px" Height="120px"></asp:ListBox>
        </td>
        <td valign=top align=left>
            <a id="lbtnJabberDelete" onclick="DeleteFromSelect($get('<%=lboxJabberAddresses.ClientID%>'),$get('<%=hdnJabber.ClientID%>'));"  ><%=Resources.Resource.Delete%></a>
        </td>
    </tr>
</table>
</ContentTemplate>
</ajaxToolkit:TabPanel>
<ajaxToolkit:TabPanel runat="server" ID="tabPanel3">
 <ContentTemplate>
 <asp:CheckBox ID="cboxNetSendIsUse" runat="server" onclick="HideOptionsNetSend()"/>
<table class="NotifyControl" runat="server" id="tblNetSend" disabled="true" >
    <tr>
        <td>
        </td>
        <td style="width:170px;">
            <asp:Label ID="lblNetSendAddresses" runat=server>Addresses</asp:Label>
        </td>
        <td align=left>
        </td>
    </tr>
    <tr>
        <td style="width:230px;">
        </td>
        <td style="width:170px;">
            <asp:TextBox ID="tboxNetSendAddresses" runat="server"  style="width:160px" Enabled="false" />
        </td>
        <td valign=middle align=left>
            <a id="lbtnNetSendAdd" onclick="AddToSelect($get('<%=tboxNetSendAddresses.ClientID%>'),$get('<%=lboxNetSendAddresses.ClientID%>'),$get('<%=hdnNetSend.ClientID%>'), 0);"><%=Resources.Resource.Add%></a>
        </td>
    </tr>
    <tr>
        <td style="width:270px;">
            <asp:Label ID="lblNetSendBody" runat=server>Body</asp:Label><br/>
            <asp:TextBox ID="tboxNetSendBody" runat="server" Enabled="false" TextMode=MultiLine style="width:260px;height:100px" />
        </td>
        <td style="width:170px;">
            <asp:ListBox ID="lboxNetSendAddresses" runat="server" Width="160px" Height="120px"></asp:ListBox>
        </td>
        <td valign=top align=left>
            <a id="lbtnNetSendDelete" onclick="DeleteFromSelect($get('<%=lboxNetSendAddresses.ClientID%>'),$get('<%=hdnNetSend.ClientID%>'));"  ><%=Resources.Resource.Delete%></a>
        </td>
    </tr>
</table>
</ContentTemplate>
</ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>