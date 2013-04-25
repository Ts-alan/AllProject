<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Notification.aspx.cs" Inherits="Notification" Title="Untitled Page" %>
<%@ Register Src="Controls/NotifyCnfg.ascx" TagName="Notify" TagPrefix="NotifyCnfg" %>
<%@ Register Assembly="PagingControl" Namespace="PagingControls" TagPrefix="cc1" %>

<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" />
<div class="title"><%=Resources.Resource.Vba32NSSettings%></div>
<script type="text/javascript">
    $(document).ready(function(){
     $("#tabs").tabs({ cookie: { expires: 30 } });
     });
     
     function HideOptions()
     {
        var cbox = $get("<%=cboxUseFlowAnalysis.ClientID%>");
        var elem1 = $get("<%=tboxGlobalEpidemyLimit.ClientID%>");
        var elem2 = $get("<%=tboxGlobalEpidemyTimeLimit.ClientID%>");
        var elem3 = $get("<%=tboxGlobalEpidemyCompCount.ClientID%>");
        var elem4 = $get("<%=tboxLocalHearthLimit.ClientID%>");
        var elem5 = $get("<%=tboxLocalHearthTimeLimit.ClientID%>");
        var elem6 = $get("<%=tboxLimit.ClientID%>");
        var elem7 = $get("<%=tboxTimeLimit.ClientID%>");
        
        elem1.disabled = elem2.disabled = elem3.disabled = elem4.disabled = elem5.disabled = elem6.disabled = elem7.disabled = !cbox.checked;
     }
</script>

<div id='tabs'>
      <ul>
        <li><a href='#0'><span><%=Resources.Resource.Mail %></span></a></li>
        <li><a href='#1'><span><%=Resources.Resource.Jabber %></span></a></li>
        <li><a href='#2'><span><%=Resources.Resource.Notification %></span></a></li>
        <li><a href='#3'><span><%=Resources.Resource.RegisteredEvents%></span></a></li>
      </ul>
      <div id='0'>
        <table  class="ListContrastTableMain" style="width:700px" >
                <tr>
                    <td> 
                        <asp:Label ID="lblMailServer" runat="server"></asp:Label>
                    </td>
                    <td align=center>
                        <asp:TextBox ID="tboxMailServer" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                      <td>
                        <asp:Label ID="lblMailFrom" runat="server"></asp:Label>
                    </td>
                    <td align=center>
                        <asp:TextBox ID="tboxMailFrom" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                      <td>
                        <asp:Label ID="lblDisplayName" runat="server"></asp:Label>
                    </td> 
                    <td align=center>
                        <asp:TextBox ID="tboxMailDisplayName" runat="server"></asp:TextBox>
                    </td>
                </tr>
             </table>
      </div>
      <div id='1'>
         <table class="ListContrastTableMain" style="width:700px" >
                <tr>
                   <td>
                      <asp:Label ID="lblJabberSever" runat="server"></asp:Label>
                   </td>
                   <td align=center>
                      <asp:TextBox ID="tboxJabberServer" runat="server"></asp:TextBox>
                   </td>
                </tr>
                <tr>
                   <td>
                      <asp:Label ID="lblJabberFrom" runat="server"></asp:Label>
                    </td>
                    <td align=center>
                       <asp:TextBox ID="tboxJabberFrom" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Label ID="lblJabberPassword" runat="server"></asp:Label>
                    </td>
                     <td align=center>
                       <asp:TextBox ID="tboxJabberPassword" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
             </table>
      </div>
      <div id='2'>
           <table class="ListContrastTableMain" style="width:700px">
            <tr>
                <td colspan=4>
                    <asp:CheckBox ID="cboxUseFlowAnalysis" runat="server" onclick="HideOptions()"  />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <%=Resources.Resource.EventsCount %>
                </td>
                <td>
                    <%=Resources.Resource.TimeInterval %>
                </td>
                <td>
                    <%=Resources.Resource.ComputersCount %>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Epidemy %>
                </td>
                <td>
                     <asp:TextBox ID="tboxGlobalEpidemyLimit" style="width:40px" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="tboxGlobalEpidemyTimeLimit" style="width:40px" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="tboxGlobalEpidemyCompCount" style="width:40px" runat=server />
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.LocalHearth %>
                </td>
                <td>
                     <asp:TextBox ID="tboxLocalHearthLimit" style="width:40px" runat="server"/>
                </td>
                <td>
                    <asp:TextBox ID="tboxLocalHearthTimeLimit" style="width:40px" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.EventFlow%>
                </td>
                <td>
                    <asp:TextBox ID="tboxLimit" style="width:40px" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="tboxTimeLimit" style="width:40px" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            </table>
      </div>
      <div id='3'>
        <table class="ListContrastTableMain" style="width:700px">
	    <tr>
		    <td valign=top>
			    <asp:datalist id="dlEvents" class="ListContrastTable" Border=0 runat="server" OnItemCommand="dlEvents_ItemCommand" OnItemDataBound="dlEvents_ItemDataBound">
				    <HeaderTemplate>
					  <tr class="subsection">
					    <td align="center" style="width: 150px;">
						    <asp:LinkButton ID="lbtnNotify" runat=server CommandName="SortCommand" CommandArgument="Notify" />
					    </td>
					    <td>
						    <asp:LinkButton ID="lbtnEventName" runat="server" CommandName="SortCommand" CommandArgument="EventName"></asp:LinkButton>
					    </td>
					  </tr>
				    </HeaderTemplate>
				    <ItemTemplate>
				      <tr>
					    <td align=center>
						    <asp:ImageButton ID="ibtnNotify" runat=server CommandName="SelectCommand" CommandArgument="Notify" />
					    </td>
					    <td>
						    <asp:Label id="lblID" runat="server" Visible=False Text='<%# DataBinder.Eval(Container.DataItem, "ID")%>'></asp:Label>						
						    <asp:LinkButton id="lbtnEventName" CommandName="SelectCommand" CommandArgument="EventName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EventName")%>' SkinId="LabelContrast"></asp:LinkButton>
					    </td>
					  </tr>
				    </ItemTemplate>
			    </asp:datalist>  
			    <cc1:PagingControl ID="pcPaging" runat="server" OnNextPage="pcPaging_NextPage" OnPrevPage="pcPaging_PrevPage" OnHomePage="pcPaging_HomePage" OnLastPage="pcPaging_LastPage"/>
             </td>
	    </tr>
    </table>

    <asp:Panel runat="server" ID="pnlModalPopap" Style="display: none" CssClass="modalPopupNotify">
    <div runat="server" id="divPopupHeader" class="modalPopupNotifyHeader"><asp:Label ID="lblSelectedEventName" SkinID="SubSectionLabel" style="font-size: 14px; font-weight:bold" runat="server" /></div>    
			    <NotifyCnfg:Notify ID="notify" runat="server" />			    
        <div style="vertical-align:middle; height:30px">            
                <div class="GiveButton1" style="float:left">
                    <asp:LinkButton runat="server" ID="lbtnSave" SkinID="LeftLink" OnClick="lbtnSave_Click" ForeColor="white" Width="100%" />
                </div>  
                <div class="GiveButton1" style="float:left">
                    <asp:LinkButton ID="btnCancel" runat="server" ForeColor="white" Width="100%"/>  
                </div>                
                <asp:Button ID="btnModalPopup" runat="server" Style="visibility:hidden" />            
        </div>
    </asp:Panel>

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnModalPopup" X="400" Y="200" PopupControlID="pnlModalPopap" CancelControlID="btnCancel" BackgroundCssClass="modalBackground" PopupDragHandleControlID="divPopupHeader" />

          </div>
    <div class="GiveButton1">
        <asp:LinkButton runat=server ID=lbtnSaveAll SkinID="LeftLink" OnClick="lbtnSaveAll_Click" ForeColor="white" Width="100%" />
    </div>
</div>

</asp:Content>

