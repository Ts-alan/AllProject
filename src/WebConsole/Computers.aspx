<%@ Page Language="C#" MaintainScrollPositionOnPostback="true"  validateRequest=false MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Computers.aspx.cs" Inherits="Computers" Title="Untitled Page" EnableEventValidation="false" %>
<%@ Register Assembly="PagingControl" Namespace="PagingControls" TagPrefix="cc1" %>
<%@ Register Src="Controls/CompFiltersMain.ascx" TagName="CompFiltersMain" TagPrefix="uc1" %>
<%@ Register Src="Controls/CompFiltersExtra.ascx" TagName="CompFiltersExtra" TagPrefix="uc2" %>
<%@ Register Src="Controls/CompFiltersDate.ascx" TagName="CompFiltersDate" TagPrefix="uc3" %>
<%@ Register Src="Controls/CompFiltersBool.ascx" TagName="CompFiltersBool" TagPrefix="uc4" %>

<%@ Register Src="Controls/TaskCreateProcess.ascx" TagName="TaskUser" TagPrefix="tskCreateProcess" %>
<%@ Register Src="Controls/TaskSendFile.ascx" TagName="TaskUser" TagPrefix="tskSendFile" %>
<%@ Register Src="Controls/TaskListProcesses.ascx" TagName="TaskUser" TagPrefix="tskListProcesses" %>
<%@ Register Src="Controls/TaskSystemInfo.ascx" TagName="TaskUser" TagPrefix="tskSystemInfo" %>
<%@ Register Src="Controls/TaskConfigureLoader.ascx" TagName="TaskUser" TagPrefix="tskConfigureLoader" %>
<%@ Register Src="Controls/TaskConfigureMonitor.ascx" TagName="TaskUser" TagPrefix="tskConfigureMonitor" %>
<%@ Register Src="Controls/TaskRunScanner.ascx" TagName="TaskUser" TagPrefix="tskRunScanner" %>
<%@ Register Src="Controls/TaskComponentState.ascx" TagName="TaskUser" TagPrefix="tskComponentState" %>
<%@ Register Src="Controls/TaskConfigurePassword.ascx" TagName="TaskUser" TagPrefix="tskConfigurePassword" %>
<%@ Register Src="Controls/TaskConfigureQuarantine.ascx" TagName="TaskUser" TagPrefix="tskConfigureQuarantine" %>
<%@ Register Src="Controls/TaskRestoreFileFromQtn.ascx" TagName="TaskUser" TagPrefix="tskRestoreFileFromQtn" %>
<%--<%@ Register Src="~/Controls/TemporaryGroupPanel.ascx" TagName="GroupPanel" TagPrefix="tmpGroup" %>--%>
<%@ Register Src="~/Controls/TaskProActivProtection.ascx" TagName="TaskUser" TagPrefix="tskProactiveProtection" %>
<%@ Register Src="~/Controls/TaskFirewall.ascx" TagName="TaskUser" TagPrefix="tskFirewall" %>
<%@ Register Src="~/Controls/TaskChangeDeviceProtect.ascx" TagName="TaskUser" TagPrefix="tskChangeDeviceProtect" %>
<%@ Register Src="~/Controls/TaskChangeDeviceProtectEx.ascx" TagName="TaskUser" TagPrefix="tskDailyDeviceProtect" %>
<%@ Register Src="~/Controls/TaskRequestPolicy.ascx" TagName="TaskUser" TagPrefix="tskRequestPolicy" %>
<%@ Register Src="~/Controls/TaskConfigureScheduler.ascx" TagName="TaskUser" TagPrefix="tskConfigureScheduler" %>
<%@ Register Src="~/Controls/TaskUninstall.ascx" TagName="TaskUser" TagPrefix="tskUninstall" %>
<%@ Register Src="~/Controls/TaskConfigureAgent.ascx" TagName="TaskUser" TagPrefix="tskConfigureAgent" %>
<%@ Register Src="~/Controls/TaskDetachAgent.ascx" TagName="TaskUser" TagPrefix="tskDetachAgent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" />

<script language="javascript" type="text/javascript">

    $('form').ready(function()
    {
        var w = $get('<%=hdnWidth.ClientID%>');
        var h = $get('<%=hdnHeight.ClientID%>');

        w.value = (window.innerWidth ? window.innerWidth : (document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.offsetWidth)); 
        h.value = (window.innerHeight ? window.innerHeight : (document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.offsetHeight));    
        
    });

    function OnKeyDown()
    {
        if (window.event.keyCode == 13)
        {
            var btn = $get('<%=lbtnGive.ClientID%>');
            
            if (btn != null)
            { //If we find the button click it            
                btn.click();
                event.keyCode = 0;
            }
        }
    }

</script>
<asp:HiddenField runat="server" ID="hdnWidth" Value="0"/>
<asp:HiddenField runat="server" ID="hdnHeight" Value="0"/>

<asp:WebPartManager ID="WebPartManager1" runat="server" />
<div class="title"><%=Resources.Resource.PageComputersTitle%></div>
<div class="divSettings">
<table class="subsection" width="100%"> 
        <tr>
			<td align=left>	
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
			                <%--<tmpGroup:GroupPanel Type=Computers runat=server id=tmpGroup />--%>
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
        <table class=area>
        <tr>
            <td>
                <asp:WebPartZone ID="WebPartZone1" runat="server" HeaderText="Filter 1" BorderColor="" 
                    BorderStyle="None" BorderWidth="" DragHighlightColor="Transparent" 
                    PartChromeType="TitleOnly" WebPartVerbRenderMode="TitleBar">
                    <ZoneTemplate>
                        <uc1:CompFiltersMain ID="cmpfltMain" runat="server"  />
                        <uc2:CompFiltersExtra ID="cmpfltExtra" runat="server" />
                        <uc3:CompFiltersDate ID="cmpfltDate" runat="server" />
                        <uc4:CompFiltersBool ID="cmpfltBool" runat="server" />
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

<div class="divSettings" runat=server id="divMainPolicy">
            <table class="subsection" width=100%> 
                <tr>
                    <td align="left">
                     <table>
			            <tr>
			                <td>
			                    <div class="GiveButton" style="width:90px; float:left" runat="server" id="divPolicyHeader">
				                    <asp:LinkButton ID="lbtnPolicyHeader" runat="server" ForeColor="white" Width="100%" OnClick="lbtnPolicyHeader_Click"><%=Resources.Resource.Policy %></asp:LinkButton>
				                </div>
				                <div style="width:5px; height:5px; float:left"></div>                                
                                <asp:DropDownList ID="ddlPolicyName" runat="server" SkinID="ddlFilterList" AutoPostBack="True" OnSelectedIndexChanged="ddlPolicyName_SelectedIndexChanged" />

                            </td>
                            <td>
                                   <div runat="server" id="divPolicyMenu" class="menuS" >
                                    <ul> 
                                        <li><a><%=Resources.Resource.Action%></a>
                                            <ul>
                                                <li><asp:LinkButton ID="lbtnApplyPolicyToComps" runat="server" OnClick="lbtnApplyPolicyToComps_Click">Применить политику</asp:LinkButton> </li>
                                                <li><asp:LinkButton ID="lbtnShowCompsByPolicy" runat="server" OnClick="lbtnShowCompsByPolicy_Click">Отобазить компьютеры</asp:LinkButton> </li>
                                                <li><asp:LinkButton ID="lbtnRemoveCompsFromPolicy" runat="server" OnClick="lbtnRemoveCompsFromPolicy_Click">Удалить компьютеры</asp:LinkButton></li>
                                            </ul>
                                        </li>     
                                    </ul> 
                                    </div>
                            </td>
                        </tr></table>
                     </td>
                    <td align="right">
                        <asp:LinkButton runat="server" ID="lbtnPolicyManagement" OnClick="lbtnPolicyManagement_Click"><%=Resources.Resource.CreatePolicy%></asp:LinkButton>&nbsp;
                        <asp:ImageButton ID="lbtnPolicyPanel" runat="server" OnClick="lbtnPolicyPanel_Click" style="visibility:hidden" />
                    </td>
                </tr>
            </table>  
       <div id="divPolicyPanel" runat="server" style="visibility: visible" enableviewstate="true">
            <table style="width:100%" class="area">
                <tr>
                    <td>
                        <asp:Panel ID="pnlPolicies" runat="server" style="width:100%">
                            <div>&nbsp;<asp:LinkButton runat="server" ID="lbtnCreatePolicy" OnClick="lbtnCreatePolicy_Click" /></div>
                            <div>&nbsp;<asp:LinkButton runat="server" ID="lbtnEditPolicy"  OnClick="lbtnEditPolicy_Click" /></div>
                            <div>&nbsp;<asp:LinkButton runat="server" ID="lbtnDeletePolicy" OnClick="lbtnDeletePolicy_Click" /></div>
                        </asp:Panel>
                    </td>
                     <td valign="top">
                        <%=Resources.Resource.DefaultPolicy%>&nbsp;&nbsp;<asp:ImageButton runat="server" ID="imbtnIsDefaultPolicy" OnClick="imbtnIsDefaultPolicy_Click" />
                    </td>
                </tr>
            </table>
        </div> 
    </div>

 <div class="divSettings" runat=server id=divMainTask>
     
<table class="subsection" width="100%"> 
        <tr>
            <td align="left" style="min-width: 704px;">
             <table style="width: 100%; min-width: 704px;">
			    <tr>
			      <td style="min-width: 450px;">
                      <asp:Label ID="lblTaskName" runat="server" SkinID="SubSectionLabel"></asp:Label>
                      <asp:DropDownList ID="ddlTaskName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTaskName_SelectedIndexChanged" SkinID="ddlFilterList"></asp:DropDownList>
                  </td>
                  <td style="min-width: 254px;" align="left">
                    <div class="GiveButton" style="float:left" >
                            <asp:LinkButton ID="lbtnGive" runat="server" OnClick="lbtnGive_Click" Width="100%" />
                    </div>
                    <div class="GiveButton" visible="false" style="float:left; visibility:hidden; max-width: 1px;">
                        <asp:LinkButton ID="lbtnUpdateInfo" runat="server" Width="100%" OnClick="lbtnUpdateInfo_Click"><%=Resources.Resource.UpdateInfo %></asp:LinkButton>
                    </div>
                  </td>
                </tr>
             </table>
             </td>
            <td align="right" style="min-width: 160px;">
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
                          <tskConfigureLoader:TaskUser ID="tskConfigureLoader" runat="server" Visible="false" HideHeader="true"/>
                          <tskConfigureMonitor:TaskUser ID="tskConfigureMonitor" runat="server" Visible="false" HideHeader="true"/>
                          <tskRunScanner:TaskUser ID="tskRunScanner" runat="server" Visible="false" HideHeader="true"/>          
                          <tskComponentState:TaskUser ID="tskComponentState" runat="server" Visible="false" HideHeader="true"/>          
                          <tskConfigurePassword:TaskUser ID="tskConfigurePassword" runat="server" Visible="false" HideHeader="true"/>          
                          <tskConfigureQuarantine:TaskUser ID="tskConfigureQuarantine" runat="server" Visible="false" HideHeader="true"/>          
                          <tskRestoreFileFromQtn:TaskUser ID="tskRestoreFileFromQtn" runat="server" Visible="false" HideHeader="true"/>          
                          <tskProactiveProtection:TaskUser ID="tskProactiveProtection" runat="server" Visible="false" HideHeader="true"/>
                          <tskFirewall:TaskUser ID="tskFirewall" runat="server" Visible="false" HideHeader="true" />                    
                          <tskChangeDeviceProtect:TaskUser ID="tskChangeDeviceProtect" runat="server" HideHeader="true" Visible="false" />
                          <tskDailyDeviceProtect:TaskUser ID="tskDailyDeviceProtect" runat="server" HideHeader="true" Visible="false" />
                          <tskRequestPolicy:TaskUser ID="tskRequestPolicy" runat="server" HideHeader="true" Visible="false"></tskRequestPolicy:TaskUser>
                          <tskConfigureScheduler:TaskUser ID="tskConfigureScheduler" runat="server" HideHeader="true" Visible="false"></tskConfigureScheduler:TaskUser>                    
                          <tskUninstall:TaskUser ID="tskUninstall" runat="server" HideHeader="true" Visible="false"></tskUninstall:TaskUser>  
                          <tskConfigureAgent:TaskUser ID="tskConfigureAgent" runat="server" HideHeader="true" Visible="false" />
                          <tskDetachAgent:TaskUser ID="tskDetachAgent" runat="server" HideHeader="true" Visible="false" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
</div>   
     <table width="100%" class="subWebParts">
        <tr>
            <td align="left" style="width:33%">
                <asp:CheckBox ID="cboxSelectAll" runat="server" Visible=false />
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

 <asp:Panel runat=server ScrollBars=Horizontal ID=pnlComplist Width=100%>
 
<TABLE class="ListContrastTableMain"><TBODY><TR><TD align=left><cc1:PagingControl id="pcPagingTop" runat="server" OnLastPage="pcPaging_LastPage" OnHomePage="pcPaging_HomePage" OnPrevPage="pcPaging_PrevPage" OnNextPage="pcPaging_NextPage" ></cc1:PagingControl> </TD><TD align=right><a runat="server" ID="lbtnColorsTop" ><%=Resources.Resource.ColorMap %></a>&nbsp;&nbsp;&nbsp;<asp:Label id="lblCount" runat="server" SkinId="LabelContrast" ></asp:Label> </TD></TR><TR><TD colSpan=3><asp:datalist id="DataList1" runat="server" SkinId="ComputersDataList" OnItemDataBound="DataList1_ItemDataBound" OnItemCommand="DataList1_ItemCommand" border="1">
							<HeaderTemplate>
								<asp:LinkButton ID="lbtnSel" runat="server" CommandArgument="Select" CommandName="SelectCommand">
									<%#Resources.Resource.SelectText%>
								</asp:LinkButton>
								<td width="30" runat="server" id="tdID" class="HeaderCell">
									<asp:LinkButton id="lbtnID" runat="server" CommandArgument="ID" CommandName="SortCommand">
										<%#Resources.Resource.IDText%>
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdComputerName" class="HeaderCell">
									<asp:LinkButton id="lbtnComputerName" runat="server" CommandArgument="ComputerName" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdIPAdress" class="HeaderCell">
									<asp:LinkButton id="lbtnIPAddress" runat="server" CommandArgument="IPAddress" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdControlCenter" class="HeaderCell">
									<asp:LinkButton id="lbtnControlCenter" runat="server" CommandArgument="ControlCenter" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdCPUClock" class="HeaderCell">
									<asp:LinkButton id="lbtnCPUClock" runat="server" CommandArgument="CPUClock" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdDomainName" class="HeaderCell">
									<asp:LinkButton id="lbtnDomainName" runat="server" CommandArgument="DomainName" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdIntegrity" class="HeaderCell">
									<asp:LinkButton id="lbtnVba32Integrity" runat="server" CommandArgument="Vba32Integrity" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdLatestInfected" class="HeaderCell">
									<asp:LinkButton id="lbtnLatestInfected" runat="server" CommandArgument="LatestInfected" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdLatestMalware" class="HeaderCell">
									<asp:LinkButton id="lbtnLatestMalware" runat="server" CommandArgument="LatestMalware" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdLatestUpdate" class="HeaderCell">
									<asp:LinkButton id="lbtnLatestUpdate" runat="server" CommandArgument="LatestUpdate" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdOSType" class="HeaderCell">
									<asp:LinkButton id="lbtnOSName" runat="server" CommandArgument="OSName" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdRAM" class="HeaderCell">
									<asp:LinkButton id="lbtnRAM" runat="server" CommandArgument="RAM" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdRecentActive" class="HeaderCell">
									<asp:LinkButton id="lbtnRecentActive" runat="server" CommandArgument="RecentActive" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tduserLogin" class="HeaderCell">
									<asp:LinkButton id="lbtnUserLogin" runat="server" CommandArgument="UserLogin" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdKeyValid" class="HeaderCell">
									<asp:LinkButton id="lbtnVba32KeyValid" runat="server" CommandArgument="Vba32KeyValid" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdVersion" class="HeaderCell">
									<asp:LinkButton id="lbtnVba32Version" runat="server" CommandArgument="Vba32Version" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdPolicyName" class="HeaderCell">
									<asp:LinkButton id="lbtnPolicyName" runat="server" OnClientClick="return false;">
									</asp:LinkButton>
								</td>
								<td width="100" runat="server" id="tdDescription" class="HeaderCell">
									<asp:LinkButton id="lbtnDescription" runat="server" CommandArgument="Description" CommandName="SortCommand">
									</asp:LinkButton>
								</td>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox Runat="server" ID="check"></asp:CheckBox>	
								<td runat="server" id="tdCompName" >
								    <a runat="server" ID="aCompName" Visible="false">Link</a>
								</td>			
								<%#DataBinder.Eval(Container, "DataItem")%>
								<td runat="server" id="tdPolicyName" align="center">
								    <asp:LinkButton runat="server" ID="lbtnPolicyName" Visible="false" OnClick="lbtnPolicyName_Click" />
								</td>
								<td runat="server" id="tdDescription">
								    <asp:Label runat="server" ID="lbtnDescription" Visible="false" CommandName="EditCommand" />
								</td>
							</ItemTemplate>
						</asp:datalist> </TD>
						</TR>
						<TR>
						<TD align=left>
						    <cc1:PagingControl id="pcPaging" runat="server" OnLastPage="pcPaging_LastPage" OnHomePage="pcPaging_HomePage" OnPrevPage="pcPaging_PrevPage" OnNextPage="pcPaging_NextPage" ></cc1:PagingControl>
						 </TD>
						 <TD align=right>
						    <a ID="lbtnColorsBottom" runat="server"><%=Resources.Resource.ColorMap %></a>&nbsp;&nbsp;
						    <asp:LinkButton ID="lbtnDeleteComputers" runat=server OnClick="lbtnDeleteComputers_Click" />&nbsp;&nbsp;
						    <asp:LinkButton ID="lbtnExcel" runat="server" OnClick="lbtnExcel_Click"></asp:LinkButton>
						 </TD>
						</TR>
						</TABLE>
     </asp:Panel>
     </td></tr>
</table>

<asp:Panel ID="xmlShow" runat="server" style="display: none; z-index: 3; background-color:#DDD; border: thin solid navy;">
     <asp:Literal runat="server" ID="ltrlxmlShowValue"></asp:Literal>
</asp:Panel>
<ajaxToolkit:HoverMenuExtender ID="hm1" runat="server" TargetControlID="lbtnColorsBottom" PopupControlID="xmlShow" PopupPosition="Top" />
<ajaxToolkit:HoverMenuExtender ID="hm2" runat="server" TargetControlID="lbtnColorsTop" PopupControlID="xmlShow" PopupPosition="Left" />
    
<asp:Button runat="server" ID="btnHiddenMessage" style="visibility:hidden" />
<asp:Panel ID="pnlModalPopapMessage" runat="server" Style="display: none" CssClass="modalPopupMessage">
                                    <div runat="server" id ="mpPicture" class="ModalPopupPictureSuccess">
                                    </div>
                                    <div id="ModalPopupText">
                                        <p style="vertical-align:middle;">
                                            <center><asp:Label runat="server" ID="lblMessage" ></asp:Label></center>
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

