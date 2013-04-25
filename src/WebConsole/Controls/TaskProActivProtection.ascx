<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskProActivProtection.ascx.cs" Inherits="Controls_TaskProActivProtection" %>

<script language="javascript" type="text/javascript" >
   
    function CheckEventARMReg()
    {
        element = $get('<%=txtEventARMReg.ClientID%>');
        cbox = $get('<%=cbxEventARMReg.ClientID%>');        
        
        CheckClick(cbox, element);
    }
    
    function CheckEventARMFS()
    {
        element = $get('<%=txtEventARMFS.ClientID%>');
        cbox = $get('<%=cbxEventARMFS.ClientID%>');        
        
        CheckClick(cbox, element);
    }
    
    function CheckTypeRegistry()
    {
        element = $get('<%=cbxIncludeSubkeys.ClientID%>');
        cbox = $get('<%=rbtnKey.ClientID%>');        
        
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

<div class="tasksection" runat="server" id="HeaderName" style="width:670px"><%=Resources.Resource.TaskNameConfigureProactiveProtection %></div>
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
            <td style="padding-left:5px; width: 150px;" align="right" >
                <%=Resources.Resource.TypeApplications %>
            </td>
            <td align="left" style="width:220px;">                        
                <asp:DropDownList ID="ddlTypeApp" runat="server" AutoPostBack="True" style="width:220px;" OnSelectedIndexChanged="ddlTypeApp_SelectedIndexChanged" ></asp:DropDownList>
            </td>
            <td>
                <asp:LinkButton runat="server" ID="lbtnDeleteTypeApp" OnClick="lbtnDeleteTypeApp_Click" Visible="false"><%=Resources.Resource.Delete %></asp:LinkButton> &nbsp;
                <asp:LinkButton runat="server" ID="lbtnCreateTypeApp" OnClick="lbtnCreateTypeApp_Click"><%=Resources.Resource.CreatePrivateRule %></asp:LinkButton>
            </td>            
        </tr>    
    </table>
    
    <table  class="ListContrastTable" runat="server" id="tblProfile" visible="false" style="width:670px">
      <tr><td>
        <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="650px" OnClientActiveTabChanged="ActiveTabChanged">
            <ajaxToolkit:TabPanel runat="server" ID="tabPanel1" >
            <ContentTemplate>
                <table  class="ListContrastTable">
                 <tr valign="top">                            
                    <td style="width:350px;">
                        <asp:ListBox runat="server" ID="lbxApplications" Width="350px" Height="200px" />
                    </td>
                    <td style="width: 200px;">
                    <table> 
                        <tr>                        
                            <td>                            
                                <asp:TextBox runat="server" ID="fuApplicationAuthorized" />
                            </td>                        
                        </tr>    
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" ID="lbtnAddApp" OnClick="lbtnAddApp_Click"><%=Resources.Resource.Add %></asp:LinkButton><br />
                                <asp:LinkButton runat="server" ID="lbtnRemoveApp" OnClick="lbtnRemoveApp_Click"><%=Resources.Resource.Delete %></asp:LinkButton>                
                            </td>
                        </tr>                 
                    </table>                
                    </td>
                    <td></td>
                 </tr>          
                </table>
            </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel runat="server" ID="tabPanel2" Enabled="false" >
            <ContentTemplate>
                <table  class="ListContrastTable">
                    <tr valign="top">            
                       <td style="width:620px; padding-left: 5px;" align="left">
                          <asp:Panel runat="server" Width="620px" Height="200px" ID="pnlRegistry" ScrollBars="Both" BackColor="white">
                             <asp:DataList runat="server" ID="dlRegistryRules" BackColor="White" ShowFooter="False" Width="100%" GridLines="Both" CellPadding="1" CellSpacing="1" OnItemCommand="dlRegistryRules_ItemCommand" >
                                <HeaderTemplate>
                                <tr>
                                 <td runat="server" id="tdEventARM" style="width: 75px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label1"><%=Resources.Resource.EventARM %></asp:Label>                            
                                 </td>
                                 <td runat="server" id="tdPath" style="width: 200px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label2"><%=Resources.Resource.PathRegistry %></asp:Label>
                                 </td>
                                 <td runat="server" id="tdType" style="width: 50px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label3"><%=Resources.Resource.Type %></asp:Label>
                                 </td>
                                 <td runat="server" id="tdSubkeys" style="width: 50px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label4"><%=Resources.Resource.Subkeys %></asp:Label>
                                 </td>
                                 <td runat="server" id="tdLog" style="width: 30px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label5"><%=Resources.Resource.Log %></asp:Label>
                                 </td>
                                 <td runat="server" id="tdDelete" style="width: 25px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label6" />
                                 </td>
                                 <td runat="server" id="tdEdit" style="width: 25px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label7" />
                                 </td>
                                </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                <tr>
                                 <td class="listRulesItem">
                                    <asp:Literal runat="server" ID="lbl1" Text='<%# Eval("EventARM") %>' />
                                 </td>
                                 <td class="listRulesItem">
                                    <asp:Literal runat="server" ID="lbl2" Text='<%# Eval("Path") %>' />
                                 </td>
                                 <td class="listRulesItem">
                                    <asp:Literal runat="server" ID="lbl3" Text='<%# Eval("Type") %>' />
                                 </td>
                                 <td class="listRulesItem">
                                    <asp:Literal runat="server" ID="lbl4" Text='<%# Eval("Subkeys") %>' />
                                 </td>
                                 <td class="listRulesItem">
                                    <asp:Literal runat="server" ID="lbl5" Text='<%# Eval("Log") %>' />
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
                                <asp:Button runat="server" ID="lbtnAddReg" />
                                <br />
                                <asp:Panel ID="pnlModalPopapRegistry" runat="server" Style="display: none" CssClass="modalPopupRule">
                                <asp:Panel ID="pnlRuleContentRegistry" runat="server" Style="background-color:#DDDDDD;border:solid 1px Gray;color:Black">                                
                                    <table class="ListContrastTable">
                                        <tr>
                                            <td style="width:180px;"><asp:TextBox runat="server" ID="txtRegistry" SkinID="EditBox" Width="180px" ></asp:TextBox></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;"><asp:RadioButton runat="server" ID="rbtnKey" Checked="true" GroupName="groupOptions" onclick="CheckTypeRegistry()" /></td>
                                            <td style="padding-left: 5px; width: 180px;"><asp:CheckBox runat="server" ID="cbxIncludeSubkeys" Width="180px" /></td>
                                        </tr>
                                        <tr>                                        
                                            <td style="width: 180px;"><asp:RadioButton runat="server" ID="rbtnValue" GroupName="groupOptions" onclick="CheckTypeRegistry()" /></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td  style="width: 180px;"><asp:CheckBox runat="server" ID="cbxWriteReportReg" /></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td  style="width: 180px;"><asp:CheckBox runat="server" ID="cbxEventARMReg" Width="110px" onclick="CheckEventARMReg()" /></td>
                                            <td style="padding-left: 5px; width: 180px;">
                                                <asp:TextBox runat="server" ID="txtEventARMReg" style="width:70px;" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>                            
                                    </table>
                                </asp:Panel>
                                
                                     <div>                                        
                                        <p style="text-align: center;">
                                            <asp:Button ID="btnAddRuleReg" runat="server" OnClick="lbtnAddReg_Click" />
                                            <asp:Button ID="btnCancelRuleReg" runat="server" OnClick="btnCancelRuleReg_Click" />
                                        </p>
                                     </div>
                                </asp:Panel>
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderRule" runat="server" TargetControlID="lbtnAddReg" PopupControlID="pnlModalPopapRegistry" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="pnlModalPopapRegistry" X="100" Y="100" />
                                </td>
                    </tr>          
                    
                </table>
            </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel runat="server" ID="tabPanel3"  Enabled="false" >
            <ContentTemplate>
                <table  class="ListContrastTable">
                    <tr valign="top">
                         <td style="width:620px; padding-left: 5px;" align="left">
                            <asp:Panel runat="server" Width="620px" Height="200px" ID="pnlFileSystem" ScrollBars="Both" BackColor="white">
                                <asp:DataList runat="server" ID="dlFileSystemRules" BackColor="White" ShowFooter="False" Width="100%" GridLines="Both" CellPadding="1" CellSpacing="1" OnItemCommand="dlFileSystemRules_ItemCommand" >
                                    <HeaderTemplate>
                                    <tr>
                                        <td runat="server" id="tdEventARMFS" style="width: 75px;" class="listRulesHeader">
                                            <asp:Label runat="server" ID="Label1"><%=Resources.Resource.EventARM %></asp:Label>                            
                                        </td>
                                        <td runat="server" id="tdPathFS" style="width: 200px;" class="listRulesHeader">
                                            <asp:Label runat="server" ID="Label2"><%=Resources.Resource.Path %></asp:Label>
                                        </td>
                                        <td runat="server" id="tdAllowRead" style="width: 30px;" class="listRulesHeader">
                                            <asp:Label runat="server" ID="Label3"><%=Resources.Resource.AllowRead %></asp:Label>
                                        </td>
                                        <td runat="server" id="tdSubdirs" style="width: 50px;" class="listRulesHeader">
                                            <asp:Label runat="server" ID="Label4"><%=Resources.Resource.Subdirs %></asp:Label>
                                        </td>
                                        <td runat="server" id="tdLogFS" style="width: 40px;" class="listRulesHeader">
                                            <asp:Label runat="server" ID="Label5"><%=Resources.Resource.Log %></asp:Label>
                                        </td>
                                        <td runat="server" id="tdDeleteFS" style="width: 25px;" class="listRulesHeader">
                                            <asp:Label runat="server" ID="Label6" />
                                        </td>
                                        <td runat="server" id="tdEditFS" style="width: 25px;" class="listRulesHeader">
                                            <asp:Label runat="server" ID="Label7" />
                                        </td>
                                    </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <tr>
                                        <td class="listRulesItem">
                                            <asp:Literal runat="server" ID="lbl1" Text='<%# Eval("EventARM") %>' />
                                        </td>
                                        <td class="listRulesItem">
                                            <asp:Literal runat="server" ID="lbl2" Text='<%# Eval("Path") %>' />
                                        </td>
                                        <td class="listRulesItem">
                                            <asp:Literal runat="server" ID="lbl3" Text='<%# Eval("AllowRead") %>' />
                                        </td>
                                        <td class="listRulesItem">
                                            <asp:Literal runat="server" ID="lbl4" Text='<%# Eval("Subdirs") %>' />
                                        </td>
                                        <td class="listRulesItem">
                                            <asp:Literal runat="server" ID="lbl5" Text='<%# Eval("Log") %>' />
                                        </td>
                                        <td class="listRulesItem">
                                            <asp:LinkButton runat="server" ID="lbtnEdit" CommandName="EditCommand" ><%=Resources.Resource.Edit %></asp:LinkButton>
                                        </td>                        
                                        <td class="listRulesItem">
                                            <asp:LinkButton runat="server" ID="lbtnDelete" CommandName="DeleteCommand" ><%=Resources.Resource.Delete %></asp:LinkButton>
                                        </td>
                                    </tr>
                                    </ItemTemplate>                                                                         
                                </asp:DataList>       
                            </asp:Panel>              
                         </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Button runat="server" ID="lbtnAddFS" />
                        <br />
                        <asp:Panel ID="pnlModalPopapFS" runat="server" Style="display: none" CssClass="modalPopupRule">
                        <asp:Panel ID="pnlRuleContentFS" runat="server" Style="background-color:#DDDDDD;border:solid 1px Gray;color:Black">                                
                        <table class="ListContrastTable">
                            <tr>
                                <td style="width:180px;"><asp:TextBox runat="server" ID="txtNameDir" SkinID="EditBox" Width="180px"></asp:TextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 180px;"><asp:CheckBox runat="server" ID="cbxAllowRead"/></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 180px;"><asp:CheckBox runat="server" ID="cbxIncludeSubDir"/></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 180px;"><asp:CheckBox runat="server" ID="cbxWriteReportFS" /></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width:180px;">
                                    <asp:CheckBox runat="server" ID="cbxEventARMFS" Width="110px" onclick="CheckEventARMFS()" />
                                </td>
                                <td style="padding-left:5px; width:150px;">
                                    <asp:TextBox runat="server" ID="txtEventARMFS" style="width:70px;" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                        
                                    <div>                                        
                                        <p style="text-align: center;">
                                            <asp:Button ID="btnAddRuleFS" runat="server" OnClick="lbtnAddFS_Click" />
                                            <asp:Button ID="btnCancelRuleFS" runat="server" OnClick="btnCancelRuleFS_Click" />
                                        </p>
                                    </div>
                        </asp:Panel>
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderRule1" runat="server" TargetControlID="lbtnAddFS" PopupControlID="pnlModalPopapFS" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="pnlModalPopapFS" X="100" Y="100" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
      </td></tr>
    </table>
    
    