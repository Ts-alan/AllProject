<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskFirewall.ascx.cs" Inherits="Controls_TaskFirewall" %>

<script language="javascript" type="text/javascript" >   
   
   function CheckEventARM()
   {
        element = $get('<%=txtEventARM.ClientID%>');
        cbox = $get('<%=cbxEventARM.ClientID%>');        
        
        CheckClick(cbox, element);
   }
   
   function CheckIP()
   {
        CheckIPAdress();
        CheckSubnetwork();
   }
    
   function CheckIPAdress()
   {
        element1 = $get('<%=txtIPSubnetwork.ClientID%>');
        element2 = $get('<%=txtMaskSubnetwork.ClientID%>');
        cbox = $get('<%=rbtnSubnetwork.ClientID%>');        
        
        CheckClick(cbox, element1);
        CheckClick(cbox, element2);
   }    
   
   function CheckSubnetwork()
   {
        element = $get('<%=txtIPAddress.ClientID%>');
        cbox = $get('<%=rbtnIP.ClientID%>');        
        
        CheckClick(cbox, element);
   }    
   
   function CheckClick(cbox, element)
   {     
     element.disabled =  !cbox.checked;
   }
 
   function ActiveTabChanged(sender, e) 
   {        
        hidden = $get('<%=ActiveTab.ClientID%>');
        
        hidden.value = sender.get_activeTab().get_tabIndex();
   }
        
</script>

<div class="tasksection" runat="server" id="HeaderName" style="width:670px"><%=Resources.Resource.TaskNameConfigureFirewall %></div>
<asp:HiddenField runat="server" ID="ActiveTab" Value="0" />

    <table  class="ListContrastTable" runat="server" id="tblProfileMngmt" visible="true" style="width:670px">         
        <tr>
            <td style="padding-left:5px; width: 150px;" align="right" >
                <%=Resources.Resource.Profiles %>
            </td>
            <td style="width: 200px;">            
                <asp:DropDownList ID="ddlProfiles" runat="server" style="width:185px;" ></asp:DropDownList>
            </td>
            <td>            
                <asp:LinkButton runat="server" ID="lbtnCreateProfile" OnClick="lbtnCreateProfile_Click"><%=Resources.Resource.Create %></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lbtnEdit" OnClick="lbtnEdit_Click"><%=Resources.Resource.Edit %></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lbtnRemove" OnClick="lbtnRemove_Click"><%=Resources.Resource.Delete %></asp:LinkButton>
            </td>
        </tr>        
    </table>
    
    <table  class="ListContrastTable" runat="server" id="tblProfileOptions" visible="false" style="width:670px">
        <tr>
            <td style="padding-left:50px; width: 200px">
                <asp:CheckBox runat="server" ID="cbxInvisibility" Checked="false" AutoPostBack="true" OnCheckedChanged="cbxInvisibility_CheckedChanged" />
            </td>
            <td style="width: 200px;">
                <asp:CheckBox runat="server" ID="cbxMonitoring" Checked="false" AutoPostBack="true" OnCheckedChanged="cbxMonitoring_CheckedChanged" />
            </td>              
        </tr>
    </table>
    
    <table  class="ListContrastTable" runat="server" id="tblProfile" visible="false" style="width:670px">
        <tr>
            <td>
                <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="650px" OnClientActiveTabChanged="ActiveTabChanged">
                    <ajaxToolkit:TabPanel runat="server" ID="tabPanel1" >
                    <ContentTemplate>
                        <table  class="ListContrastTable">
                            <tr valign="top">            
                                <td style="width:620px; padding-left: 5px;" align="left">
                                <asp:Panel runat="server" Width="620px" Height="200px" ID="pnlRules" ScrollBars="Both" BackColor="white">
                                    <asp:DataList runat="server" ID="dlRules" BackColor="White" ShowFooter="False" Width="100%" GridLines="Both" CellPadding="1" CellSpacing="1" OnItemCommand="dlRules_ItemCommand" >
                                    <HeaderTemplate>
                                        <tr>                                            
                                            <td runat="server" id="tdConnection" style="width: 70px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label1"><%=Resources.Resource.Connection %></asp:Label>                            
                                            </td>
                                            <td runat="server" id="tdApplication" style="width: 120px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label2"><%=Resources.Resource.Application %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdDirection" style="width: 40px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label3"><%=Resources.Resource.Direction %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdProtocol" style="width: 40px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label4"><%=Resources.Resource.Protocol %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdIPAddress" style="width: 80px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label5"><%=Resources.Resource.IPAddress %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdLocalPorts" style="width: 80px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label6"><%=Resources.Resource.LocalPorts %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdRemotePorts" style="width: 80px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label7"><%=Resources.Resource.RemotePorts %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdEventARM" style="width: 40px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label8"><%=Resources.Resource.EventARM %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdComment" style="width: 70px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label9"><%=Resources.Resource.Comment %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdDelete" style="width: 25px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label10" />
                                            </td>
                                            <td runat="server" id="tdEdit" style="width: 25px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label11" />
                                            </td>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="trRules" runat="server">                                            
                                            <td>
                                                <asp:Literal runat="server" ID="lbl1" Text='<%# Eval("Connection") %>' />
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="lbl2" Text='<%# Eval("Application") %>' />
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="lbl3" Text='<%# Eval("Direction") %>' />
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="lbl4" Text='<%# Eval("Protocol") %>' />
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="lbl5" Text='<%# Eval("IPAddress") %>' />
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="lbl6" Text='<%# Eval("LocalPorts") %>' />
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="lbl7" Text='<%# Eval("RemotePorts") %>' />
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="lbl8" Text='<%# Eval("EventARM") %>' />
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="lbl9" Text='<%# Eval("Comment") %>' />
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lbtnEdit" CommandName="EditCommand"><%=Resources.Resource.Edit %></asp:LinkButton>
                                            </td>                        
                                            <td>
                                                <asp:LinkButton runat="server" ID="lbtnDelete" CommandName="DeleteCommand"><%=Resources.Resource.Delete %></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    </asp:DataList>
                                </asp:Panel>              
                                </td>
                            </tr>
                            <tr>
                            <td>
                                <asp:Button runat="server" ID="btnAddNewRule" />
                                <br />
                                
                                
                                
                                <asp:Panel ID="pnlModalPopapRule" runat="server" Style="display: none" CssClass="modalPopupRule">
                                    <asp:Panel ID="pnlRuleContent" runat="server" Style="background-color:#DDDDDD;border:solid 1px Gray;color:Black">
                                   
                                    <table class="ListContrastTable">
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbxActivate" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <ajaxToolkit:TabContainer runat="server" ID="TabsRule" ActiveTabIndex="0" Width="380px" Height="130px" >
                                                    <ajaxToolkit:TabPanel runat="server" ID="tabPanelRule1" >
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 150px;" align="right"><asp:Label runat="server" ID="lblConnection"><%=Resources.Resource.Connection %></asp:Label> </td>
                                                                <td><asp:DropDownList runat="server" ID="ddlConnection" SkinID="ddlEdit" Width="100px" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 150px;" align="right"><asp:Label runat="server" ID="lblApplication"><%=Resources.Resource.Application %></asp:Label></td>
                                                                <td><asp:TextBox runat="server" ID="fuApplication" SkinID="editBox" Width="213px" Text="*" /></td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td style="padding-left: 5px; width: 150px;" align="right"><asp:Label runat="server" ID="lblComment" ><%=Resources.Resource.Comment %></asp:Label></td>
                                                                <td><asp:TextBox runat="server" ID="txtCommentApp" SkinID="EditBox" TextMode="MultiLine" Width="213px" Height="60px" /></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    </ajaxToolkit:TabPanel>
                                                    
                                                    <ajaxToolkit:TabPanel runat="server" ID="tabPanelRule2" >
                                                    <ContentTemplate>
                                                        <TABLE>                            
                                                            <TR>
                                                                <TD style="PADDING-LEFT: 5px; WIDTH: 100px" align="left">
                                                                    <asp:RadioButton id="rbtnIP" runat="server" Checked="true" GroupName="Address" onclick="CheckIP()"></asp:RadioButton>
                                                                </TD>
                                                                <TD>
                                                                    <asp:TextBox id="txtIPAddress" runat="server" Width="120px" Text="*" SkinID="EditBox"></asp:TextBox>                                            
                                                                </TD>
                                                            </TR>
                                                            <TR>
                                                                <TD style="PADDING-LEFT: 5px; WIDTH: 100px" align="left">
                                                                    <asp:RadioButton id="rbtnSubnetwork" runat="server" GroupName="Address" onclick="CheckIP()"></asp:RadioButton>                                                                </TD>
                                                                <TD>
                                                                    <asp:TextBox id="txtIPSubnetwork" runat="server" Width="120px" Text="0.0.0.0" SkinID="EditBox" Enabled="false"></asp:TextBox>
                                                                </TD>
                                                            </TR>
                                                            <TR>
                                                                <TD style="PADDING-LEFT: 5px; WIDTH: 100px" align=left></TD>
                                                                <TD>
                                                                    <asp:TextBox id="txtMaskSubnetwork" runat="server" Width="120px" Text="0.0.0.0" SkinID="EditBox" Enabled="false"></asp:TextBox>
                                                                </TD>
                                                            </TR>
                                                        </TABLE>
                                                    </ContentTemplate>
                                                    </ajaxToolkit:TabPanel>
                                                    
                                                    <ajaxToolkit:TabPanel runat="server" ID="tabPanelRule3" >
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td  style="padding-left: 5px; width: 150px;" align="right">
                                                                    <asp:Label runat="server" ID="lblProtocol"><%=Resources.Resource.Protocol %></asp:Label>                                            
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList runat="server" ID="ddlProtocol" SkinID="ddlEdit" Width="80px" />&nbsp;&nbsp;                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 150px;" align="right">
                                                                    <asp:Label runat="server" ID="lblDirection"><%=Resources.Resource.Direction %></asp:Label>
                                                               </td>
                                                               <td>
                                                                    <asp:DropDownList runat="server" ID="ddlDirection" SkinID="ddlEdit" Width="80px" />
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                                <td  style="padding-left: 5px; width: 150px;" align="right"><asp:Label runat="server" ID="lblLocalPorts"><%=Resources.Resource.LocalPorts %></asp:Label></td>
                                                                <td><asp:TextBox runat="server" ID="txtLocalPorts" SkinID="EditBox" Width="213px" Text="*" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td  style="padding-left: 5px; width: 150px;" align="right"><asp:Label runat="server" ID="lblRemotePorts"><%=Resources.Resource.RemotePorts %></asp:Label></td>
                                                                <td><asp:TextBox runat="server" ID="txtRemotePorts" SkinID="EditBox" Width="213px" Text="*" /></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    </ajaxToolkit:TabPanel>
                                                    
                                                    <ajaxToolkit:TabPanel runat="server" ID="tabPanelRule4" >
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 150px;" align="left"><asp:CheckBox runat="server" ID="cbxEventARM" Checked="false" onclick="CheckEventARM()" /></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 150px;" align="left"><%=Resources.Resource.Identifier %></td>
                                                                <td><asp:TextBox runat="server" ID="txtEventARM" SkinID="EditBox" Width="120px" Enabled="false" /></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    </ajaxToolkit:TabPanel>
                                                </ajaxToolkit:TabContainer>
                                            </td>
                                        </tr>
                                    </table>
            
                                    </asp:Panel>
                                    
                                    <div>                                        
                                        <p style="text-align: center;">
                                            <asp:Button ID="btnAddRule" runat="server" OnClick="lbtnRuleAdd_Click" />
                                            <asp:Button ID="btnCancelRule" runat="server" OnClick="btnCancelRule_Click" />
                                        </p>
                                    </div>
                            </asp:Panel>
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderRule" runat="server" TargetControlID="btnAddNewRule" PopupControlID="pnlModalPopapRule" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="pnlModalPopapRule" X="100" Y="100" />
                            </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    
                    <ajaxToolkit:TabPanel runat="server" ID="tabPanel2" >
                    <ContentTemplate>
                        <table  class="ListContrastTable">
                            <tr valign="top">            
                                <td style="width:620px; padding-left: 5px;" align="left">
                                <asp:Panel runat="server" Width="620px" Height="200px" ID="pnlFriendlyIP" ScrollBars="Both" BackColor="white">
                                    <asp:DataList runat="server" ID="dlFriendlyIP" BackColor="White" ShowFooter="False" Width="100%" GridLines="Both" CellPadding="1" CellSpacing="1" OnItemCommand="dlFriendlyIP_ItemCommand" >
                                    <HeaderTemplate>
                                        <tr>
                                            <td runat="server" id="tdIPAddress" style="width: 260px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label1"><%=Resources.Resource.IPAddress %></asp:Label>                            
                                            </td>                      
                                            <td runat="server" id="tdComment" style="width: 250px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label2"><%=Resources.Resource.Comment %></asp:Label>
                                            </td>
                                            <td runat="server" id="tdDelete" style="width: 50px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label3" />
                                            </td>
                                            <td runat="server" id="tdEdit" style="width: 50px;" class="listRulesHeader">
                                                <asp:Label runat="server" ID="Label4" />
                                            </td>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="listRulesItem">
                                                <asp:Literal runat="server" ID="lbl1" Text='<%# Eval("IPAddress") %>' />
                                            </td>                        
                                            <td class="listRulesItem">
                                                <asp:Literal runat="server" ID="lbl2" Text='<%# Eval("Comment") %>' />
                                            </td>
                                            <td class="listRulesItem">
                                                <asp:LinkButton runat="server" ID="lbtnEdit" CommandName="EditCommand"><%=Resources.Resource.Edit %></asp:LinkButton>
                                            </td>                        
                                            <td class="listRulesItem">
                                                <asp:LinkButton runat="server" ID="lbtnDelete" CommandName="DeleteCommand"><%=Resources.Resource.Delete %></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>                                                                       
                                    </asp:DataList>       
                                </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                            <td>
                                <asp:Button runat="server" ID="btnAddZoneIPRule" />
                                <br />
                                
                                <asp:Panel ID="pnlModalPopapZoneIP" runat="server" Style="display: none" CssClass="modalPopup">
                                    <asp:Panel ID="pnlZoneIPContent" runat="server" Style="background-color:#DDDDDD;border:solid 1px Gray;color:Black">
                                   
                                    <table class="ListContrastTable">
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblFriendlyIPAddress"><%=Resources.Resource.IPAddress %></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtFriendlyIPAddress" Width="183px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblFriendlyComment"><%=Resources.Resource.Comment %></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtFriendlyComment" TextMode="MultiLine" Width="183px" Height="60px" />
                                            </td>
                                        </tr>
                                    </table>
            
                                    </asp:Panel>
                                    
                                    <div>                                        
                                        <p style="text-align: center;">
                                            <asp:Button ID="btnAddZoneIP" runat="server" OnClick="lbtnAddFriendlyIPRule_Click" />
                                            <asp:Button ID="btnCancelZoneIP" runat="server" OnClick="btnCancelZoneIP_Click" />
                                        </p>
                                    </div>
                            </asp:Panel>
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnAddZoneIPRule" PopupControlID="pnlModalPopapZoneIP" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="pnlModalPopapZoneIP" X="200" Y="200" />
                            </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </td>
        </tr>
    </table>