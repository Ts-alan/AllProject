<%@ Page Language="C#" MaintainScrollPositionOnPostback="true"  validateRequest="false" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Groups.aspx.cs" Inherits="Groups" Title="Untitled Page" EnableEventValidation="false" %>
<%@ Register Assembly="PagingControl" Namespace="PagingControls" TagPrefix="cc1" %>
<%@ Register Src="Controls/GroupFiltersMain.ascx" TagName="GroupFiltersMain" TagPrefix="uc1" %>

<%@ Register Src="Controls/TaskCreateProcess.ascx" TagName="TaskUser" TagPrefix="tskCreateProcess" %>
<%@ Register Src="Controls/TaskSendFile.ascx" TagName="TaskUser" TagPrefix="tskSendFile" %>
<%@ Register Src="Controls/TaskListProcesses.ascx" TagName="TaskUser" TagPrefix="tskListProcesses" %>
<%@ Register Src="Controls/TaskSystemInfo.ascx" TagName="TaskUser" TagPrefix="tskSystemInfo" %>
<%@ Register Src="Controls/TaskConfigureLoader.ascx" TagName="TaskUser" TagPrefix="tskConfigureLoader" %>
<%@ Register Src="Controls/TaskConfigureMonitor.ascx" TagName="TaskUser" TagPrefix="tskConfigureMonitor" %>
<%@ Register Src="Controls/TaskConfigureScanner.ascx" TagName="TaskUser" TagPrefix="tskConfigureScanner" %>
<%@ Register Src="Controls/TaskComponentState.ascx" TagName="TaskUser" TagPrefix="tskComponentState" %>
<%@ Register Src="Controls/TaskConfigurePassword.ascx" TagName="TaskUser" TagPrefix="tskConfigurePassword" %>
<%@ Register Src="Controls/TaskConfigureQuarantine.ascx" TagName="TaskUser" TagPrefix="tskConfigureQuarantine" %>
<%@ Register Src="Controls/TaskRestoreFileFromQtn.ascx" TagName="TaskUser" TagPrefix="tskRestoreFileFromQtn" %>
<%@ Register Src="~/Controls/TaskConfigureProactive.ascx" TagName="TaskUser" TagPrefix="tskProactiveProtection" %>
<%@ Register Src="~/Controls/TaskFirewall.ascx" TagName="TaskUser" TagPrefix="tskFirewall" %>
<%@ Register Src="~/Controls/TaskChangeDeviceProtect.ascx" TagName="TaskUser" TagPrefix="tskChangeDeviceProtect" %>
<%@ Register Src="~/Controls/TaskRequestPolicy.ascx" TagName="TaskUser" TagPrefix="tskRequestPolicy" %>
<%@ Register Src="~/Controls/TaskConfigureScheduler.ascx" TagName="TaskUser" TagPrefix="tskConfigureScheduler" %>
<%@ Register Src="~/Controls/TaskProductUninstall.ascx" TagName="TaskUser" TagPrefix="tskUninstall" %>
<%@ Register Src="~/Controls/TaskAgentSettings.ascx" TagName="TaskUser" TagPrefix="tskAgentSettings" %>
<%@ Register Src="~/Controls/TaskMonitorOn.ascx" TagName="TaskUser" TagPrefix="tskMonitorOn" %>
<%@ Register Src="~/Controls/TaskMonitorOff.ascx" TagName="TaskUser" TagPrefix="tskMonitorOff" %>
<%@ Register Src="~/Controls/TaskRunScanner.ascx" TagName="TaskUser" TagPrefix="tskRunScanner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" />

<script language="javascript" type="text/javascript">

    $('form').ready(function () {
        var w = $get('<%=hdnWidth.ClientID%>');
        var h = $get('<%=hdnHeight.ClientID%>');

        w.value = (window.innerWidth ? window.innerWidth : (document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.offsetWidth));
        h.value = (window.innerHeight ? window.innerHeight : (document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.offsetHeight));

    });

    function OnKeyDown() {
        if (window.event.keyCode == 13) {
            var btn = $get('<%=lbtnGive.ClientID%>');

            if (btn != null) { //If we find the button click it            
                btn.click();
                event.keyCode = 0;
            }
        }
    }

</script>
<asp:HiddenField runat="server" ID="hdnWidth" Value="0"/>
<asp:HiddenField runat="server" ID="hdnHeight" Value="0"/>

<asp:WebPartManager ID="WebPartManager1" runat="server" />
<div class="title"><%=Resources.Resource.PageGroupsTitle%></div>
<div class="divSettings">
<table class="subsection" width="100%"> 
        <tr>
			<td align="left">	
			    <table>
			    <tr>
			    <td>
				<div class="GiveButton" style="width:90px; float:left" runat="server" id="divFilterHeader">
				    <asp:LinkButton ID="lbtnFilterHeader" runat="server" ForeColor="white" Width="100%" OnClick="lbtnFilterHeader_Click"><%=Resources.Resource.Filter %></asp:LinkButton>
				</div>
				<div style="width:5px; height:5px; float:left"></div>
				<asp:dropdownlist id="ddlFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" SkinID="ddlFilterList"></asp:dropdownlist>
				</td>
			            <td>
			                
			            </td>
			        </tr>
			    </table>
            </td>
            <td align="right">
                <asp:ImageButton ID="imbtnTopControl" runat="server" OnClick="lbtnTopControl_Click" TabIndex="1" />
            </td>         
        </tr>
        </table>
<div id="divTop" runat="server" style="visibility: visible" enableviewstate="true">
        <table class="area">
        <tr>
            <td>
                <asp:WebPartZone ID="WebPartZone1" runat="server" HeaderText="Filter 1" BorderColor="" 
                    BorderStyle="None" BorderWidth="" DragHighlightColor="Transparent" 
                    PartChromeType="TitleOnly" WebPartVerbRenderMode="TitleBar">
                    <ZoneTemplate>
                        <uc1:GroupFiltersMain ID="cmpfltMain" runat="server"  />
                    </ZoneTemplate>               
                        <PartStyle CssClass="WebpartsBody" />
                        <PartTitleStyle CssClass="WebpartsTitle" />
                        <MenuVerbStyle Font-Names="Verdana,Tahoma" Font-Size="8pt"   />
                </asp:WebPartZone>
            </td>
            <td>
                <asp:WebPartZone ID="WebPartZone2" runat="server" HeaderText="Filter 2" BorderColor="" 
                    BorderStyle="None" BorderWidth="" DragHighlightColor="Transparent" 
                    PartChromeType="TitleOnly" WebPartVerbRenderMode="TitleBar">
                    <ZoneTemplate>   
                       
                    </ZoneTemplate>
                     <PartStyle CssClass="WebpartsBody" />
                     <PartTitleStyle CssClass="WebpartsTitle" />
                     <MenuVerbStyle Font-Names="Verdana,Tahoma" Font-Size="8pt"   />
                </asp:WebPartZone>
            </td>
            <td>
                <asp:WebPartZone ID="WebPartZone3" runat="server" HeaderText="Filter 3" BorderColor="" 
                    BorderStyle="None" BorderWidth="" DragHighlightColor="Transparent" 
                    PartChromeType="TitleOnly" WebPartVerbRenderMode="TitleBar">
                    <ZoneTemplate>
                    </ZoneTemplate>
                     <PartStyle CssClass="WebpartsBody" />
                     <PartTitleStyle CssClass="WebpartsTitle" />
                     <MenuVerbStyle Font-Names="Verdana,Tahoma" Font-Size="8pt"   />
                </asp:WebPartZone>
             </td>
             <td>
                <asp:WebPartZone ID="WebPartZone4" runat="server" HeaderText="Filter 4" BorderColor="" 
                    BorderStyle="None" BorderWidth="" DragHighlightColor="Transparent" 
                    PartChromeType="TitleOnly" WebPartVerbRenderMode="TitleBar">
                    <ZoneTemplate>
                    </ZoneTemplate>
                    <PartStyle CssClass="WebpartsBody" />
                     <PartTitleStyle CssClass="WebpartsTitle" />
                     <MenuVerbStyle Font-Names="Verdana,Tahoma" Font-Size="8pt"   />
                </asp:WebPartZone>
             </td>
             <td>
               <asp:CatalogZone ID="CatalogZone1" runat="server" HBorderColor="" 
                    BorderStyle="None" BorderWidth="" DragHighlightColor="Transparent" 
                    PartChromeType="TitleOnly">
                    <ZoneTemplate>
                        <asp:PageCatalogPart ID="PageCatalogPart1" runat="server" />
                    </ZoneTemplate>
                     <PartStyle CssClass="WebpartsBody" />
                     <PartTitleStyle CssClass="WebpartsTitle" />
                </asp:CatalogZone> 
             </td>
           </tr>
           <tr>
               <td colspan="5">
                    &nbsp;
                    <div class="GiveButton1" style="width: 100px; float:left">
                        <asp:LinkButton ID="btnApplyFilter" SkinID="FilterActions" runat="server" OnClick="btnApplyFilter_Click" ForeColor="White" />
                    </div>
                    <div class="GiveButton1" style="width: 100px; float:left">
                        <asp:LinkButton ID="lblDefaults" SkinID="FilterActions" runat="server" OnClick="lblDefaults_Click" ForeColor="White" />
                    </div>
                    <div class="GiveButton1" style="width: 100px; float:left">
                        <asp:LinkButton ID="lbtnFilter" SkinID="FilterActions" runat="server" OnClick="lbtnFilter_Click" ForeColor="White"></asp:LinkButton>
                    </div>
                    <div class="GiveButton1" style="width: 100px; float:left" runat="server" id="divEditFilter">
                        <asp:LinkButton ID="btnEditFilter" SkinID="FilterActions" runat="server" OnClick="btnEditFilter_Click" ForeColor="White"></asp:LinkButton>                    
                    </div>
                    <div class="GiveButton1" style="width: 100px; float:left" runat="server" id="divDelete">
                        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" ForeColor="White"></asp:LinkButton>
                    </div>
           </td>
           </tr>
     </table>
 </div>
</div>

<div class="divSettings" runat="server" id="divMainTask">
     
<table class="subsection" width="100%"> 
        <tr>
            <td align="left">
             <table>
			    <tr>
			    <td>
              <asp:Label ID="lblTaskName" runat="server" SkinID="SubSectionLabel"></asp:Label>
              <asp:DropDownList ID="ddlTaskName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTaskName_SelectedIndexChanged" SkinID="ddlFilterList">
            </asp:DropDownList>
                </td>
                <td>
                <div class="GiveButton" style="float:left" >
                        <asp:LinkButton ID="lbtnGive" runat="server" OnClick="lbtnGive_Click" Width="100%" />
                </div>
                <div class="GiveButton" style="float:left; visibility:hidden">
                    <asp:LinkButton ID="lbtnUpdateInfo" runat="server" Width="100%" OnClick="lbtnUpdateInfo_Click"><%=Resources.Resource.UpdateInfo %></asp:LinkButton>
                </div>
                </td></tr></table>
             </td>
            <td align="right">
                &nbsp;<asp:LinkButton ID="lbtnSave"  runat="server" OnClick="lbtnSave_Click"></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="lbtnDelete" runat="server" OnClick="lbtnDelete_Click"></asp:LinkButton>
                &nbsp;<asp:ImageButton ID="imbtnBottomControl" runat="server"  OnClick="lbtnBottomControl_Click" />
            </td>
        </tr>
        </table>  
</div>
<div id="divBottom" runat="server" style="visibility: visible" enableviewstate="true">
        <table style="width:100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlTask" runat="server" style="width:100%">
                          <tskCreateProcess:TaskUser ID="tskCreateProcess" runat="server" HideHeader="true" Visible="false"/>
                          <tskSendFile:TaskUser ID="tskSendFile" runat="server" Visible="false"  HideHeader="true"/>
                          <tskListProcesses:TaskUser ID="tskListProcesses" runat="server" Visible="false"  HideHeader="true"/>
                          <tskSystemInfo:TaskUser ID="tskSystemInfo" runat="server" HideHeader="true" Visible="false"/>
                          <tskAgentSettings:TaskUser ID="tskAgentSettings" runat="server" HideHeader="true" Visible="false" />
                          <tskConfigureLoader:TaskUser ID="tskConfigureLoader" runat="server" Visible="false" HideHeader="true"/>
                          <tskConfigureMonitor:TaskUser ID="tskConfigureMonitor" runat="server" Visible="false" HideHeader="true"/>
                          <tskConfigureScanner:TaskUser ID="tskConfigureScanner" runat="server" Visible="false" HideHeader="true"/>          
                          <tskComponentState:TaskUser ID="tskComponentState" runat="server" Visible="false" HideHeader="true"/>          
                          <tskConfigurePassword:TaskUser ID="tskConfigurePassword" runat="server" Visible="false" HideHeader="true"/>          
                          <tskConfigureQuarantine:TaskUser ID="tskConfigureQuarantine" runat="server" Visible="false" HideHeader="true"/>          
                          <tskRestoreFileFromQtn:TaskUser ID="tskRestoreFileFromQtn" runat="server" Visible="false" HideHeader="true"/>          
                          <tskProactiveProtection:TaskUser ID="tskProactiveProtection" runat="server" Visible="false" HideHeader="true"/>
                          <tskFirewall:TaskUser ID="tskFirewall" runat="server" Visible="false" HideHeader="true" />                    
                          <tskChangeDeviceProtect:TaskUser ID="tskChangeDeviceProtect" runat="server" HideHeader="true" Visible="false" />
                          <tskRequestPolicy:TaskUser ID="tskRequestPolicy" runat="server" HideHeader="true" Visible="false"></tskRequestPolicy:TaskUser>
                          <tskConfigureScheduler:TaskUser ID="tskConfigureScheduler" runat="server" HideHeader="true" Visible="false"></tskConfigureScheduler:TaskUser>                    
                          <tskUninstall:TaskUser ID="tskUninstall" runat="server" HideHeader="true" Visible="false"></tskUninstall:TaskUser>
                          <tskMonitorOn:TaskUser ID="tskMonitorOn" runat="server" Visible="false" />
                          <tskMonitorOff:TaskUser ID="tskMonitorOff" runat="server" Visible="false" />  
                          <tskRunScanner:TaskUser ID="tskRunScanner" runat="server" Visible="false" HideHeader="true" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
</div>   
     <table width="100%" class="subWebParts">
        <tr>
            <td align="left" style="width:33%">
                <asp:CheckBox ID="cboxSelectAll" runat="server" Visible="false" />
     	      </td>
              <td align="center" style="width:33%">
				<%= Resources.Resource.PageSize %>&nbsp;<asp:dropdownlist id="ddlPageSize" runat="server" Width="96px" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" style="width:60px">
				</asp:dropdownlist>
			 </td>
			 <td align="right" style="width:33%">
			    <%= Resources.Resource.Design %>&nbsp;<asp:DropDownList ID="ddlWebPartState" runat="server" style="width:100px" AutoPostBack="True" OnSelectedIndexChanged="ddlWebPartState_SelectedIndexChanged">
                </asp:DropDownList>
			 </td>
        </tr>
     </table>    
     <table width="100%" style="table-layout: fixed;">
<tr><td>

 <asp:Panel runat="server" ScrollBars="Horizontal" ID="pnlComplist" Width="100%">
 
<table class="ListContrastTable">
    <tr>
        <td align="left">
            <cc1:PagingControl id="pcPagingTop" runat="server" OnLastPage="pcPaging_LastPage" OnHomePage="pcPaging_HomePage" OnPrevPage="pcPaging_PrevPage" OnNextPage="pcPaging_NextPage" ></cc1:PagingControl>
        </td>
        <td align="right">
            <asp:Label id="lblCount" runat="server" SkinId="LabelContrast" ></asp:Label> 
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:datalist id="DataList1" runat="server" SkinId="ComputersDataList" OnItemDataBound="DataList1_ItemDataBound" OnItemCommand="DataList1_ItemCommand" border="1">
							<HeaderTemplate>
								<asp:LinkButton ID="lbtnSel" runat="server" CommandArgument="Select" CommandName="SelectCommand">
									<%#Resources.Resource.SelectText%>
								</asp:LinkButton>
								<td width="30" runat="server" id="tdID" class="HeaderCell">
									<asp:LinkButton id="lbtnID" runat="server" CommandArgument="ID" CommandName="SortCommand">
										<%#Resources.Resource.IDText%>
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdGroupName" class="HeaderCell">
									<asp:LinkButton id="lbtnGroupName" runat="server" CommandArgument="GroupName" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdTotalCount" class="HeaderCell">
									<asp:LinkButton id="lbtnTotalCount" runat="server" CommandArgument="TotalCount" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdActiveCount" class="HeaderCell">
									<asp:LinkButton id="lbtnActiveCount" runat="server" CommandArgument="ActiveCount" CommandName="SortCommand">
									</asp:LinkButton>
								</td>						
                                <td width="100" runat="server" id="tdParentName" class="HeaderCell">
									<asp:LinkButton id="lbtnParentName" runat="server" CommandArgument="ParentName" CommandName="SortCommand">
								</asp:LinkButton>
								</td>		
								<td width="100" runat="server" id="tdDescription" class="HeaderCell">
									<asp:LinkButton id="lbtnGroupComment" runat="server" CommandArgument="GroupComment" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox Runat="server" ID="check"></asp:CheckBox>			
								<%#DataBinder.Eval(Container, "DataItem")%>								
								<td runat="server" id="tdParentName" align="center">
								    <asp:Label runat="server" ID="lbtnParentName" />
								</td>
								<td runat="server" id="tdDescription" align="center">
								    <asp:Label runat="server" ID="lbtnGroupComment" Visible="false" CommandName="EditCommand" />
								</td>
							</ItemTemplate>
						</asp:datalist> </td>
						</tr>
						<tr>
						<td align="left">
						    <cc1:PagingControl id="pcPaging" runat="server" OnLastPage="pcPaging_LastPage" OnHomePage="pcPaging_HomePage" OnPrevPage="pcPaging_PrevPage" OnNextPage="pcPaging_NextPage" ></cc1:PagingControl>
						 </td>
						 <td align="right">					    
						    <asp:LinkButton ID="lbtnExcel" runat="server" OnClick="lbtnExcel_Click"></asp:LinkButton>
						 </td>
						</tr>
						</table>
     </asp:Panel>
     </td></tr>
</table>

<asp:Panel ID="xmlShow" runat="server" style="display: none; z-index: 3; background-color:#DDD; border: thin solid navy;">
     <asp:Literal runat="server" ID="ltrlxmlShowValue"></asp:Literal>
</asp:Panel>   
<asp:Button runat="server" ID="btnHiddenMessage" style="visibility:hidden" />
<asp:Panel ID="pnlModalPopapMessage" runat="server" Style="display: none" CssClass="modalPopupMessage">
                                    <div runat="server" id ="mpPicture" class="ModalPopupPictureSuccess">
                                    </div>
                                    <div id="ModalPopupText">
                                        <p style="vertical-align:middle;">
                                            <asp:Label runat="server" ID="lblMessage" ></asp:Label>
                                        </p>
                                    </div>
                                    <div id="ModalPopupButton">
                                        <p style="text-align: center; vertical-align:bottom;">
                                            <asp:Button ID="btnClose" runat="server" />                                            
                                        </p>
                                    </div>
                            </asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnHiddenMessage" PopupControlID="pnlModalPopapMessage" CancelControlID="btnClose" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="pnlModalPopapMessage" />

</asp:Content>



