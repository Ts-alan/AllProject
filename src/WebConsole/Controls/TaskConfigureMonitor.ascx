<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureMonitor.ascx.cs" Inherits="Controls_TaskConfigureMonitor" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.CongLdrConfigureMonitor%></div>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $("#TabsMonitor").tabs({ cookie: { expires: 30} });

    });
function ChangeScanInBackGround() {
    var cbox = $get('<%=cboxScanInBackGround.ClientID%>');

    var cbox1 = $get('<%=cboxMaximumCPUUsege.ClientID%>');
    var cbox2 = $get('<%=cboxMaximumDiskActivity.ClientID%>');
    var cbox3 = $get('<%=cboxMaximumDisplacement.ClientID%>');
    var cbox4 = $get('<%=cboxMinimumBattery.ClientID%>');
    var cbox5 = $get('<%=cboxScanFilesByUser.ClientID%>');

    var tbox1 = $get('<%=tboxMaximumCPUUsege.ClientID%>');
    var tbox2 = $get('<%=tboxMaximumDiskActivity.ClientID%>');
    var tbox3 = $get('<%=tboxMaximumDisplacement.ClientID%>');
    var tbox4 = $get('<%=tboxMinimumBattery.ClientID%>');

    if (!$(cbox).is(':checked')) {
        cbox1.disabled = true;
        cbox2.disabled = true;
        cbox3.disabled = true;
        cbox4.disabled = true;
        cbox5.disabled = true;

        tbox1.disabled = true;
        tbox2.disabled = true;
        tbox3.disabled = true;
        tbox4.disabled = true;
    }
    else {
        cbox1.disabled = false;
        cbox2.disabled = false;
        cbox3.disabled = false;
        cbox4.disabled = false;
        cbox5.disabled = false;

        if ($(cbox1).is(':checked')) {
            tbox1.disabled = false;
        }
        if ($(cbox2).is(':checked')) {
            tbox2.disabled = false;
        }
        if ($(cbox3).is(':checked')) {
            tbox3.disabled = false;
        }
        if ($(cbox4).is(':checked')) {
            tbox4.disabled = false;
        }
    }
}

function ChangeMaximumCPUUsege() {
    var cbox = $get('<%=cboxMaximumCPUUsege.ClientID%>');
    var tbox = $get('<%=tboxMaximumCPUUsege.ClientID%>');

    if (!$(cbox).is(':checked')) {
        tbox.disabled = true;
    }
    else {
        tbox.disabled = false;
    }
}

function ChangeMaximumDiskActivity() {
    var cbox = $get('<%=cboxMaximumDiskActivity.ClientID%>');
    var tbox = $get('<%=tboxMaximumDiskActivity.ClientID%>');

    if (!$(cbox).is(':checked')) {
        tbox.disabled = true;
    }
    else {
        tbox.disabled = false;
    }
}

function ChangeMaximumDisplacement() {
    var cbox = $get('<%=cboxMaximumDisplacement.ClientID%>');
    var tbox = $get('<%=tboxMaximumDisplacement.ClientID%>');

    if (!$(cbox).is(':checked')) {
        tbox.disabled = true;
    }
    else {
        tbox.disabled = false;
    }
}

function ChangeMinimumBattery() {
    var cbox = $get('<%=cboxMinimumBattery.ClientID%>');
    var tbox = $get('<%=tboxMinimumBattery.ClientID%>');

    if (!$(cbox).is(':checked')) {
        tbox.disabled = true;
    }
    else {
        tbox.disabled = false;
    }
}

function ChangeInfected()
{
    var ddl1 = $get('<%=ddlInfected.ClientID%>');
    var ddl2 = $get('<%=ddlPrevInfected.ClientID%>');
    var ddl3 = $get('<%=ddlPrevPrevInfected.ClientID%>');
    var cbox1 = $get('<%=cboxInfectedQuarantine.ClientID%>');
    var cbox2 = $get('<%=cboxInfectedQua.ClientID%>');
    var cbox3 = $get('<%=cboxInfectedQuaPrev.ClientID%>');
   
    ddl2.disabled = true;
    ddl3.disabled = true;
    cbox1.disabled = false;
    cbox2.disabled = true;
    cbox3.disabled = true;
            
    switch(ddl1.selectedIndex)
    {
        case 0:
            ddl2.options[0].text = ddl1.options[1].text;
                        
            ddl2.disabled = false;
            ddl2.selectedIndex = 0;
            cbox2.disabled = false;
            
            ddl3.disabled = false;
            ddl3.selectedIndex = 0;
            cbox3.disabled = false;
            
            break;
        case 1:
            ddl2.options[0].text = ddl1.options[0].text;
                        
            ddl2.disabled = false;
            ddl2.selectedIndex = 0;
            cbox2.disabled = false;
            
            ddl3.disabled = false;
            ddl3.selectedIndex = 0;
            cbox3.disabled = false;
            break;
        default:
            ddl2.selectedIndex = ddl1.selectedIndex - 1;
            ddl3.selectedIndex = ddl1.selectedIndex - 2;
            
            if(ddl1.selectedIndex == 3) cbox1.disabled = true;
            break;
    }
}

function ChangeInfectedPrev()
{    
    var ddl2 = $get('<%=ddlPrevInfected.ClientID%>');
    var ddl3 = $get('<%=ddlPrevPrevInfected.ClientID%>');
    var cbox2 = $get('<%=cboxInfectedQua.ClientID%>');
    var cbox3 = $get('<%=cboxInfectedQuaPrev.ClientID%>');
       
    ddl3.disabled = true;
    cbox2.disabled = false;
    cbox3.disabled = true;
            
    switch(ddl2.selectedIndex)
    {
        case 0:            
            ddl3.disabled = false;
            ddl3.selectedIndex = 0;
            cbox3.disabled = false;            
            break;
        case 1:
            ddl3.selectedIndex = 0;            
            break;
        case 2:
            ddl3.selectedIndex = 1;
            cbox2.disabled = true;
            break;
    }
}

function ChangeInfectedPrevPrev()
{   
    var ddl3 = $get('<%=ddlPrevPrevInfected.ClientID%>'); 
    var cbox3 = $get('<%=cboxInfectedQuaPrev.ClientID%>');
    
    if( ddl3.selectedIndex == 0)
        cbox3.disabled = false;
    else cbox3.disabled = true;
}

function ChangeSuspicious() {
    var ddl2 = $get('<%=ddlSuspicious.ClientID%>');
    var ddl3 = $get('<%=ddlPrePrevSuspicious.ClientID%>');
    var cbox2 = $get('<%=cboxSuspiciousQua.ClientID%>');
    var cbox3 = $get('<%=cboxSuspiciousQuaPrev.ClientID%>');

    switch (ddl2.selectedIndex) {
        case 0:
            ddl3.disabled = true;
            ddl3.selectedIndex = 0;
            cbox3.disabled = true;
            break;
        case 1:
            ddl3.disabled = false;
            ddl3.selectedIndex = 0;
            cbox3.disabled = false;
            break;
        case 2:
            ddl3.disabled = true;
            ddl3.selectedIndex = 1;
            cbox3.disabled = true;
            break;
    }
}

function ChangeHeuristics() {
    var ddl = $get('<%=ddlHeuristicAnalysis.ClientID%>');
    var ddl2 = $get('<%=ddlSuspicious.ClientID%>');
    var ddl3 = $get('<%=ddlPrePrevSuspicious.ClientID%>');
    var cbox2 = $get('<%=cboxSuspiciousQua.ClientID%>');
    var cbox3 = $get('<%=cboxSuspiciousQuaPrev.ClientID%>');
    var cbox4 = $get('<%=cboxBlockUSB.ClientID%>');

    if (ddl.selectedIndex == 0) {
        ddl2.disabled = true;
        ddl3.disabled = true;
        cbox2.disabled = true;
        cbox3.disabled = true;
        cbox4.disabled = true;
    }
    else {
        ddl2.disabled = false;
        cbox2.disabled = false;
        cbox4.disabled = false;
        switch (ddl2.selectedIndex) {
            case 0:
                ddl3.disabled = true;
                cbox3.disabled = true;
                break;
            case 1:
                ddl3.disabled = false;
                cbox3.disabled = false;
                break;
            case 2:
                ddl3.disabled = true;
                cbox3.disabled = true;
                break;
        }
    }
}
</script>
<div id="TabsMonitor" style="width:560px">
    <ul>
        <li><a href="#tab1"><%=Resources.Resource.CongMonitorObjects%></a> </li>
        <li><a href="#tab2"><%=Resources.Resource.BackgroundScanning%></a> </li>
        <li><a href="#tab3"><%=Resources.Resource.Actions%></a> </li>
        <li><a href="#tab4"><%=Resources.Resource.CongMonitorReport%></a> </li>
    </ul>
    <div id="tab1" class="divSettings">
        <table class="ListContrastTable">
            <tr>
                <td>
                    <asp:RadioButton ID="rbScanStandartSet" runat="server" GroupName="ScanMode" />&nbsp;<%=Resources.Resource.CongMonitorScanStandartSet%>
                </td>
            </tr>
            <tr>   
                <td>
                    <asp:RadioButton ID="rbScanSelectedTypes" runat="server" GroupName="ScanMode" />&nbsp;<%=Resources.Resource.CongMonitorScanSelectedTypes%><br/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="tboxFilterDefined" runat="server" style="width:495px"></asp:TextBox>
                </td>
            </tr>
            <tr> 
                <td>
                    <asp:RadioButton ID="rbMonitorScanAllTypes" runat="server" GroupName="ScanMode" />&nbsp;<%=Resources.Resource.CongMonitorScanAllTypes%><br/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=Resources.Resource.CongMonitorExcluding%>&nbsp;<asp:TextBox ID="tboxFilterExclude" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cboxScanOnlyNew" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorScanOnlyNew%>
                </td>
            </tr>
        </table>
        <table class="ListContrastTable">
            <tr>
                <td>
                    <asp:CheckBox ID="cboxDetectWare" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorDetectWare %>   
                </td>
            </tr>
        </table>
        <table class="ListContrastTable" >
            <tr>
                <td style="width: 547px">
                    <%=Resources.Resource.CongMonitorExcludingFoldersAndFiles %>
                </td>
            </tr>
            <tr>
                <td style="width: 547px">
                    <asp:TextBox ID="tboxExcludingFoldersAndFilesAdd" runat="server" style="width:300px"></asp:TextBox>&nbsp;&nbsp;<asp:LinkButton ID="lbtnExcludingFoldersAndFilesAdd" runat="server" OnClick="lbtnExcludingFoldersAndFilesAdd_Click"><%=Resources.Resource.Add%></asp:LinkButton> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="cboxIncludingSubFolders" />&nbsp;<%=Resources.Resource.CongMonitorIncludingSubFolders %><br/>
                    <asp:DropDownList ID="ddlExcludingFoldersAndFilesDelete" style="width:300px" runat="server"></asp:DropDownList>&nbsp;&nbsp;<asp:LinkButton ID="lbntnExcludingFoldersAndFilesDelete" runat="server" OnClick="lbntnExcludingFoldersAndFilesDelete_Click"><%=Resources.Resource.Delete%></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div id="tab2" class="divSettings">
         <table id="Table1" class="ListContrastTable" runat="server">
            <tr>
                <td>
                  <asp:CheckBox ID="cboxScanInBackGround" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorScanInBackGround %>  
                </td>
            </tr>
            <tr>
                <td>
                     &nbsp;&nbsp;&nbsp;&nbsp;<%=Resources.Resource.CongMonitorFuncConditions %>
                     <table width="75%" border="1" >
                        <tr>
                            <td>
                                <asp:CheckBox ID="cboxMaximumCPUUsege" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorMaximumCPUUsege %>                            </td>
                            <td align="right">
                               <asp:TextBox ID="tboxMaximumCPUUsege" runat="server" style="width:40px"/> 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cboxMaximumDiskActivity" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorMaximumDiskActivity %>
                            </td>
                            <td align="right">
                               <asp:TextBox ID="tboxMaximumDiskActivity" runat="server" style="width:40px"/> 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cboxMaximumDisplacement" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorMaximumDisplacement %>
                            </td>
                            <td align="right">
                               <asp:TextBox ID="tboxMaximumDisplacement" runat="server" style="width:40px"/> 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cboxMinimumBattery" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorMinimumBattery %>
                            </td>
                            <td align="right">
                               <asp:TextBox ID="tboxMinimumBattery" runat="server" style="width:40px"/> 
                            </td>
                        </tr>
                    </table>
                </td>
              </tr>
         </table>
         <table class="ListContrastTable" >
              <!--<tr>
                <td>
                    <asp:CheckBox ID="cboxScanStartupFiles" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorScanStartupFiles %>
                </td>
              </tr>-->
              <tr>
                <td>
                    <asp:CheckBox ID="cboxScanFilesByUser" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorScanFilesByUser %>
                </td>
              </tr>
         </table>
         <table class="ListContrastTable" id="tblScanning3" runat="server">
            <tr>
                <td>
                    <%=Resources.Resource.CongMonitorListOfPathToScan %><br/>
                    <asp:TextBox ID="tboxListOfPathToScanAdd" runat="server" style="width:300px"></asp:TextBox>&nbsp;&nbsp;<asp:LinkButton ID="lbtnListOfPathToScanAdd" runat="server" OnClick="lbtnListOfPathToScanAdd_Click"><%=Resources.Resource.Add%></asp:LinkButton> <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="cboxListOfPathToScanIncludingSubFolders" />&nbsp;<%=Resources.Resource.CongMonitorIncludingSubFolders %><br/>
                    <asp:DropDownList ID="ddlListOfPathToScan" style="width:310px" runat="server"></asp:DropDownList>&nbsp;&nbsp;<asp:LinkButton ID="lbtnListOfPathToScanDelete" runat="server" OnClick="lbtnListOfPathToScanDelete_Click"><%=Resources.Resource.Delete%></asp:LinkButton>
                </td>
            </tr>
         </table>
    </div>
    <div id="tab3" class="divSettings">
        <table class="ListContrastTable">
            <tr>
                <td width="50%">
                    <%=Resources.Resource.CongMonitorInfected %><br/>
                    <asp:DropDownList ID="ddlInfected" runat="server" onchange="ChangeInfected()">
                    </asp:DropDownList><br/>
                    <asp:CheckBox ID="cboxInfectedQuarantine" runat="server" />&nbsp;&nbsp;<%=Resources.Resource.CongMonitorSaveCopyQuarantine %>
                </td> 
                 <td width="50%">
                    <%=Resources.Resource.CongMonitorHeuristicAnalysis %><br/>
                    <asp:DropDownList ID="ddlHeuristicAnalysis" runat="server" onchange="ChangeHeuristics()">
                    </asp:DropDownList>
                     <br />
                     <asp:CheckBox ID="cboxBlockUSB" runat="server" /><%=Resources.Resource.BlockAutorunUSB%>
                 </td> 
            </tr>
            <tr>
                <td width="50%">
                    <br/><%=Resources.Resource.CongMonitorIfPreviousFails %><br/>
                    <asp:DropDownList ID="ddlPrevInfected" runat="server" onchange="ChangeInfectedPrev()">
                    </asp:DropDownList><br/>
                    <asp:CheckBox ID="cboxInfectedQua" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorSaveCopyQuarantine %>
                </td> 
                 <td width="50%">
                    <%=Resources.Resource.CongMonitorSuspicious %><br/>
                    <asp:DropDownList ID="ddlSuspicious" runat="server" onchange="ChangeSuspicious()">
                    </asp:DropDownList><br/>
                    <asp:CheckBox ID="cboxSuspiciousQua" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorSaveCopyQuarantine %>
                </td> 
            </tr>
            <tr>
                <td width="50%">
                    <br/><%=Resources.Resource.CongMonitorIfPreviousFails %><br/>
                    <asp:DropDownList ID="ddlPrevPrevInfected" runat="server" onchange="ChangeInfectedPrevPrev()">
                    </asp:DropDownList><br/>
                    <asp:CheckBox ID="cboxInfectedQuaPrev" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorSaveCopyQuarantine %>
                </td> 
                 <td width="50%">
                    <br/><%=Resources.Resource.CongMonitorIfPreviousFails %><br/>
                    <asp:DropDownList ID="ddlPrePrevSuspicious" runat="server">
                    </asp:DropDownList><br/>
                    <asp:CheckBox ID="cboxSuspiciousQuaPrev" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorSaveCopyQuarantine %>
                </td> 
            </tr>
         </table>
    </div>
    <div id="tab4" class="divSettings">
        <table class="ListContrastTable">
            <tr>
                <td>
                   <asp:CheckBox ID="cboxNotifyOfMonitor" runat="server" />&nbsp;<%=Resources.Resource.CongMonitorNotifyOfMonitor %>
                </td>
            </tr>
        </table>
        <table class="ListContrastTable">
            <tr>
                <td>
                    <asp:Label runat='server' ID='lblLogFile'><%=Resources.Resource.Name %></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tboxLogFile" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cboxMaximumSizeLog" runat="server" /><%=Resources.Resource.CongLdrMaximumSizeLog%>
                </td>
                <td>
                     <asp:TextBox ID="tboxMaximumSizeLog" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan='2'>
                 <asp:CheckBox ID="cboxInformationAboutCleanFiles" runat="server" /><%=Resources.Resource.CongMonitorInformationAboutCleanFiles %>
                </td>
            </tr>
        </table>    
    </div>
</div>
