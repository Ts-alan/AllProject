<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" validateRequest=false MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="SettingsExtra.aspx.cs" Inherits="SettingsExtra" Title="Untitled Page" %>

<%@ Register Assembly="PagingControl" Namespace="PagingControls" TagPrefix="cc1" %>
<%@ OutputCache Location="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" />

<script language="javascript" type="text/javascript" >
   
   $(document).ready(function(){
     $("#tabs").tabs({ cookie: { expires: 30 } });
     });
    //Select
    
    function UpOption(sel)
    {        
        var selectedIndex = sel.selectedIndex;
        if((selectedIndex == 0 )|| (selectedIndex == -1)) return;
              
        var selOpt = new Option();
        selOpt.text = sel.options[selectedIndex].value;
        selOpt.value = sel.options[selectedIndex].value;
        
        var revOpt = new Option();
        revOpt.text = sel.options[selectedIndex-1].value;
        revOpt.value = sel.options[selectedIndex-1].value;
        
        sel.options[selectedIndex] = revOpt;
        sel.options[selectedIndex - 1] = selOpt;
        
        sel.options[selectedIndex - 1].selected = true;
        
        SaveClickHandler(sel);
    }
    
    function DownOption(sel)
    {
        var selectedIndex = sel.selectedIndex;
        if((selectedIndex == (sel.length-1) )|| (selectedIndex == -1)) return;
              
        var selOpt = new Option();
        selOpt.text = sel.options[selectedIndex].value;
        selOpt.value = sel.options[selectedIndex].value;
        
        var revOpt = new Option();
        revOpt.text = sel.options[selectedIndex+1].value;
        revOpt.value = sel.options[selectedIndex+1].value;
        
        sel.options[selectedIndex] = revOpt;
        sel.options[selectedIndex + 1] = selOpt;
        
        sel.options[selectedIndex + 1].selected = true;
        
        SaveClickHandler(sel);
    }
    
    function SaveClickHandler(listBox) 
    { 
        var hdn = $get("<%=hdnBlockItems.ClientID%>");  

        var elements = ""; 
        var intCount = listBox.options.length; 

        for (i = 0; i < intCount; i++) 
        {
            elements += listBox.options[i].text + ';'; 
        } 

        hdn.value = elements; 
    }
    
    function SelectChanged()
    {
        var lbox = $get("<%=lboxPriority.ClientID%>");
     
        
        var pnlLU = $get("<%=pnlLastUpdate.ClientID%>");
        var pnlLA = $get("<%=pnlLastActive.ClientID%>");
        var pnlLI = $get("<%=pnlLastInfection.ClientID%>");
        var pnlK = $get("<%=pnlKey.ClientID%>");
        var pnlI = $get("<%=pnlIntegrity.ClientID%>");
        
        pnlLU.style.display = 'none';
        pnlLA.style.display = 'none';
        pnlLI.style.display = 'none';
        pnlK.style.display = 'none';
        pnlI.style.display = 'none';
        
        
        if(lbox.options[lbox.selectedIndex].text == '<%=Resources.Resource.LastUpdate%>')
        {
            pnlLU.style.display = '';
        }
        else
            if(lbox.options[lbox.selectedIndex].text == '<%=Resources.Resource.LastActive%>')
            {
                pnlLA.style.display = '';
            }
            else
                if(lbox.options[lbox.selectedIndex].text == '<%=Resources.Resource.LastInfected%>')
                {
                    pnlLI.style.display = '';
                }
                else
                    if(lbox.options[lbox.selectedIndex].text == '<%=Resources.Resource.KeyState%>')
                    {
                        pnlK.style.display = '';
                    }
                    else
                        if(lbox.options[lbox.selectedIndex].text == '<%=Resources.Resource.Integrity%>')
                        {
                            pnlI.style.display = '';
                        }        
    }   
   
</script>

 <div class="title"><%=Resources.Resource.Settings%></div>  
        
            
            
<div id='tabs'>
      <ul>
        <li><a href='#0'><span><%= Resources.Resource.ShowComputerTableFieldsText%></span></a></li>
        <li><a href='#1'><span><%=Resources.Resource.TimerSettings %></span></a></li>
        <li><a href='#2'><span><%=Resources.Resource.ColorSettings %></span></a></li>
        <li runat="server" id="liEventColor"><a href='#3'><span><%= Resources.Resource. EventColorTableTitle %></span></a></li>
      </ul>
      <div id='0'>            
            <table class="ListContrastTable" style="width: 700px;">
				<tr>
				    <td><asp:checkbox id="cboxShowControlCenter" runat="server"></asp:checkbox><%=Resources.Resource.ControlCenter%>
					</td>					
					<td><asp:checkbox id="cboxRecentActive" runat="server"></asp:checkbox><%=Resources.Resource.RecentActive%>
					</td>
					<td><asp:checkbox id="cboxCPUClock" runat="server"></asp:checkbox><%=Resources.Resource.CPU%>
					</td>
				</tr>
				<tr>
					<td><asp:checkbox id="cboxShowDomainName" runat="server"></asp:checkbox><%=Resources.Resource.DomainName%>
					</td>
					<td><asp:checkbox id="cboxLatestUpdate" runat="server"></asp:checkbox><%=Resources.Resource.LatestUpdate%>
					</td>
					<td><asp:checkbox id="cboxVba32KeyValid" runat="server"></asp:checkbox><%=Resources.Resource.VBA32KeyValid%>
					</td>
				</tr>
				<tr>
					<td><asp:checkbox id="cboxUserLogin" runat="server"></asp:checkbox><%=Resources.Resource.UserLogin%>
					</td>
					<td><asp:checkbox id="cboxVba32Version" runat="server"></asp:checkbox><%=Resources.Resource.VBA32Version%>
					</td>
					<td><asp:checkbox id="cboxRAM" runat="server"></asp:checkbox><%=Resources.Resource.RAM%>
					</td>
				</tr>
				<tr>
					<td><asp:checkbox id="cboxLatestMalware" runat="server"></asp:checkbox><%=Resources.Resource.LatestMalware%>
					</td>
					<td><asp:checkbox id="cboxLatestInfected" runat="server"></asp:checkbox><%=Resources.Resource.LatestInfected%>
					</td>
					<td><asp:checkbox id="cboxVba32Integrity" runat="server"></asp:checkbox><%=Resources.Resource.VBA32Integrity%>
					</td>
				</tr>
				<tr>
					<td><asp:checkbox id="cboxOSType" runat="server"></asp:checkbox><%=Resources.Resource.OSType%>
					</td>				
				    <td><asp:checkbox id="cboxDescription" runat="server"></asp:checkbox><%=Resources.Resource.Description%>
					</td>
					<td><asp:checkbox id="cboxPolicyName" runat="server"></asp:checkbox><%=Resources.Resource.Policy%>
					</td>		
				</tr>
			</table>
      </div>
        
      <div id='1'>        
        <table class="ListContrastTable" style="width: 700px;">                    
                <tr>
                    <td><asp:Label ID="lblEvents" runat="server" SkinId="LeftLabel"><%=Resources.Resource.Events %>&nbsp; (<%=Resources.Resource.Seconds %>)</asp:Label><asp:TextBox ID="tboxIntervalEvents" SkinID="TimerBox" runat="server" ></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="tboxIntervalEventsFilteredExtender"   runat="server" FilterType="Numbers" TargetControlID="tboxIntervalEvents">
                    </ajaxToolkit:FilteredTextBoxExtender>
                        <asp:RangeValidator ID="tboxIntervalEventsRangeValidator"  runat="server" ErrorMessage="<%$ Resources:Resource, ErrorSettingsAutoUpdateIntervalRange %>" ControlToValidate="tboxIntervalEvents" MaximumValue="1800"  MinimumValue="30" Type="Integer"></asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="tboxIntervalEventsRequiredFieldValidator"  runat="server" ErrorMessage="*" ControlToValidate="tboxIntervalEvents"></asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                   <td><asp:Label ID="lblTasks" runat="server" SkinId="LeftLabel"><%=Resources.Resource.Tasks %>&nbsp; (<%=Resources.Resource.Seconds %>)</asp:Label><asp:TextBox ID="tboxIntervalTasks" SkinID="TimerBox" runat="server" ></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender  ID="tboxIntervalTasksFilteredExtender"   runat="server" FilterType="Numbers" TargetControlID="tboxIntervalTasks">
                    </ajaxToolkit:FilteredTextBoxExtender>
                    <asp:RangeValidator ID="tboxIntervalTasksRangeValidator"  runat="server" ErrorMessage="<%$ Resources:Resource, ErrorSettingsAutoUpdateIntervalRange %>" ControlToValidate="tboxIntervalTasks" MaximumValue="1800" MinimumValue="30"  Type="Integer"></asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="tboxIntervalTasksRequiredFieldValidator"  runat="server" ErrorMessage="*" ControlToValidate="tboxIntervalTasks"></asp:RequiredFieldValidator>
                   </td>
                </tr>            
        </table>
      </div>
        
      <div id='2'>
        <asp:UpdatePanel runat="server" ID="upnlCompColors">
        <ContentTemplate>
        <table class="ListContrastTable" style="width: 700px;">
            <tr>
                <td style="width: 300px;">
                    <table>
                    <tr>
                        <td align="left">
                            <asp:ListBox runat="server" ID="lboxPriority" Width="200px" Height="85px" onchange="SelectChanged()" />                          
                            <asp:HiddenField ID="hdnBlockItems" runat="server" />
                        </td>
                        <td valign="top">
                            &nbsp;&nbsp;<a id="lbtnUpSelect" style="cursor:pointer;" onclick="UpOption($get('<%=lboxPriority.ClientID%>'));"><%=Resources.Resource.Up%></a><br/>
                            &nbsp;&nbsp;<a id="lbtnDownSelect" style="cursor:pointer;" onclick="DownOption($get('<%=lboxPriority.ClientID%>'));"><%=Resources.Resource.Down%></a>
                        </td>
                    </tr>
                    </table>
                </td>
                <td valign="top">
                    <asp:Panel runat="server" ID="pnlLastUpdate" Width="200px" style="display:none">
                        <%=Resources.Resource.LastUpdate%>:
                        <table>
                            <tr>
                                <td>
                                    1.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLU1" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLU1" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLU1"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    2.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLU2" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLU2" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLU2"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    3.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLU3" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLU3" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLU3"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlLastInfection" Width="200px" style="display:none">
                        <%=Resources.Resource.LastInfected%>:
                        <table>
                            <tr>
                                <td>
                                    1.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLI1" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLI1" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLI1"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    2.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLI2" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLI2" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLI2"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    3.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLI3" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLI3" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLI3"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlLastActive" Width="200px" style="display:none">
                        <%=Resources.Resource.LastActive%>:
                        <table>
                            <tr>
                                <td>
                                    1.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLA1" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLA1" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLA1"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    2.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLA2" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLA2" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLA2"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    3.
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeLA3" SkinID="ddlEdit" Width="50px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTypeLA3" SkinID="ddlEdit" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorLA3"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>   
                    <asp:Panel runat="server" ID="pnlIntegrity" Width="200px" style="display:none">                        
                        <table>
                            <tr>
                                <td>
                                    <%=Resources.Resource.Integrity%>:
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorIntegrity"></asp:DropDownList>
                                </td>                                
                            </tr>                                                      
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlKey" Width="200px" style="display:none">                        
                        <table>                            
                            <tr>
                                <td>
                                    <%=Resources.Resource.KeyState%>:
                                </td>                                    
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlColorKey"></asp:DropDownList>
                                </td>                                
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 300px;"><%=Resources.Resource.GoodState%>: <asp:DropDownList runat="server" ID="ddlGoodStateColor"></asp:DropDownList></td>
                <td></td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>       
      </div>
      
      <div id='3'>
        <asp:UpdatePanel runat="server" ID="upnlEvents">
        <ContentTemplate>
            <table class="ListContrastTable" style="width: 700px;" runat="server" id="tblEvent" >
				<tr>
					<td><asp:datalist id="dlEvents" border=1 runat="server" Height="80px" OnItemCommand="dlEvents_ItemCommand" OnItemDataBound="dlEvents_ItemDataBound">
							<ItemTemplate>
								<td>
								    <asp:LinkButton id="lbtnNameSel" runat="server" CommandName="EditCommand">Edit</asp:LinkButton>
								</td>
							    <td>
								   <asp:LinkButton ID="lbtnColor" runat=server CommandName="EditCommand" >Empty</asp:LinkButton>
								</td>
							</ItemTemplate>
							<EditItemTemplate>
							<td>
								<asp:Label id="lblID" runat="server" Visible=False Text='<%# DataBinder.Eval(Container.DataItem, "ID")%>'></asp:Label>
								<asp:Label id="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EventName")%>' SkinId="LabelContrast"></asp:Label>
							</td>
							<td>	
								<asp:DropDownList ID="ddlMultiColor" runat="server" Width="200px"></asp:DropDownList>
								<asp:LinkButton id="lbtnUpdate" runat="server" CommandName="UpdateCommand" SkinId="LbtnContrast">Update</asp:LinkButton>&nbsp;
								<asp:LinkButton id="lbtnCancel" runat="server" CommandName="CancelCommand" SkinId="LbtnContrast">Cancel</asp:LinkButton>
							</td>
							</EditItemTemplate>
						</asp:datalist>  
						<cc1:PagingControl ID="pcPaging" runat="server" OnNextPage="pcPaging_NextPage" OnPrevPage="pcPaging_PrevPage" OnHomePage="pcPaging_HomePage" OnLastPage="pcPaging_LastPage"/>          
					</td>
				</tr>
			</table>
        </ContentTemplate>
        </asp:UpdatePanel>
      </div>
      
      <div class="divSettings">
	      <asp:LinkButton id="btnSave" runat="server" SkinID="Button" onclick="btnSave_Click">
            <%=Resources.Resource.Save%>
          </asp:LinkButton>
      </div>
</div>  
            
</asp:Content>

