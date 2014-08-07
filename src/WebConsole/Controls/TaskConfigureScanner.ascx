<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureScanner.ascx.cs" Inherits="Controls_TaskConfigureScanner" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.TaskNameRunScanner%></div>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#ScannerTabs").tabs({ cookie: { expires: 30} });
    });
    function pageLoad() {
        var defExts = ".COM.EXE.DLL.DRV.SYS.OV?.VXD.SCR.CPL.OCX.BPL.AX.PIF.DO?.XL?.HLP.RTF.WI?.WZ?.MSI.MSC.HT*.VB*.JS.JSE.ASP*.CGI.PHP*.?HTML.BAT.CMD.EML.NWS.MSG.XML.MSO.WPS.PPT.PUB.JPG.JPEG.ANI.INF.SWF.PDF";
        $("input[class='control']").button();
        $("input[type='button']").button();
        if($('#<%= tboxScannerFileExtensions.ClientID %>').val()=='')
            $('#<%= tboxScannerFileExtensions.ClientID %>').val(defExts);
        ScannerScanArchiveState();
        ScannerMaxSizeState();

        $('#<%= lbtnScannerFileExtensionReset.ClientID %>').on("click", function () {
            $('#<%= tboxScannerFileExtensions.ClientID %>').val(defExts);
        });

        $('#<%= chkScannerScanArchives.ClientID %>').on('click', function () {
            ScannerScanArchiveState();
        });
        $('#<%= chkScannerMaxSize.ClientID %>').on('click', function () {
            ScannerMaxSizeState();
        });
    };
    function ScannerScanArchiveState()
    {
        if ($('#<%= chkScannerScanArchives.ClientID %>').is(':checked') == true) {
            $('#<%= chkScannerMaxSize.ClientID %>').removeAttr('disabled');
            $('#<%= chkScannerMaxSize.ClientID %>').parent().removeAttr('disabled');
        }
        else {
            $('#<%= chkScannerMaxSize.ClientID %>').attr('disabled', 'disabled');
            $('#<%= chkScannerMaxSize.ClientID %>').parent().attr('disabled', 'disabled');
            $('#<%= chkScannerMaxSize.ClientID %>').attr('checked', false);
            $('#<%= tboxScannerMaxSize.ClientID %>').attr('disabled', 'disabled');
        }
    };
    function ScannerMaxSizeState()
    {
        if ($('#<%= chkScannerMaxSize.ClientID %>').is(':checked') == true) {
            $('#<%= tboxScannerMaxSize.ClientID %>').removeAttr('disabled');
        }
        else {
            $('#<%= tboxScannerMaxSize.ClientID %>').attr('disabled', 'disabled');
        }
    }
</script>

<div id="ScannerTabs" style="width:600px">
    <ul>
        <li><a href="#ScannerTab1"><%=Resources.Resource.ScanningObjects %> </a> </li>
        <li><a href="#ScannerTab2"><%=Resources.Resource.JournalEvents %></a> </li>
    </ul>

    <div id="ScannerTab1">
        <div class="ListContrastTable">
            <div style="padding-bottom:5px;padding-left:5px">
                <p><b><%= Resources.Resource.CongScannerScanFileList %></b></p>
                <p>
                    <asp:TextBox runat="server" ID="tboxScannerFileExtensions" Style="width: 420px"></asp:TextBox>
                     &nbsp;
                    <asp:LinkButton runat="server" ID="lbtnScannerFileExtensionReset" SkinID="Button"
                        OnClientClick="return false;"><%= Resources.Resource.DefaultFiles %></asp:LinkButton>
                    <asp:RequiredFieldValidator ID="tboxScannerFileExtensionsValidator" runat="server"
                        ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>' ControlToValidate="tboxScannerFileExtensions"
                        Display="None" ValidationGroup="ScannerFileExtensionsValidationGroup" />
                    <asp:RegularExpressionValidator ID="ScannerFileExtensionsRegularExpressionValidator"
                        ControlToValidate="tboxScannerFileExtensions" ValidationExpression="^(\.[\w|\?|\*]+)*$"
                        ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>" runat="server" />
                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCallouttboxScannerFileExtensions"
                        runat="server" TargetControlID="tboxScannerFileExtensionsValidator" HighlightCssClass="highlight"
                        PopupPosition="BottomRight" />
                    <ajaxToolkit:ValidatorCalloutExtender2 ID="RegularValidatorCallouttboxScannerFileExtensions"
                        runat="server" TargetControlID="ScannerFileExtensionsRegularExpressionValidator"
                        HighlightCssClass="highlight" PopupPosition="BottomRight" />
                </p>
                <p>
                    <b> <%=Resources.Resource.CongMonitorExcluding%></b>
                    &nbsp;
                    <asp:TextBox runat="server" ID="tboxScannerFilesExcluded" Style="width: 470px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="ScannerFilesExcludedRegularExpressionValidator"
                        ControlToValidate="tboxScannerFilesExcluded" ValidationExpression="^(\.[\w|\?|\*]+)*$"
                        ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>" runat="server" />
                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ScannerFilesExcludedRegularExpressionValidatorCalloutExtender"
                        runat="server" TargetControlID="ScannerFilesExcludedRegularExpressionValidator"
                        HighlightCssClass="highlight" PopupPosition="BottomRight" />
                </p>
                    <asp:CheckBox runat="server" ID="chkScannerCache"></asp:CheckBox><%=Resources.Resource.CongScannerEnableCache %>
            </div>
            <div style="padding-bottom:5px;padding-left:5px">
                <table width="545px" border="1px" rules="groups">
                    <tr>
                        <td style="width:270px">
                            <asp:CheckBox ID="chkScannerScanMail" runat="server"/> <%=Resources.Resource.CongScannerCheckMail%>
                        </td>
                        <td style="width:270px">
                            <asp:CheckBox ID="chkScannerFindPotential" runat="server"/><%=Resources.Resource.CongScannerPotential%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkScannerScanArchives" runat="server"/><%=Resources.Resource.CongScannerCheckArchives%>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkScannerFindVirusInstalls" runat="server"/> <%=Resources.Resource.CongScannerSFX %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:15px">
                            <asp:CheckBox ID="chkScannerMaxSize" runat="server" Enabled="false" /><%=Resources.Resource.CongScannerArchivesSize%>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkScannerTrustAuthenCode" runat="server"/><%=Resources.Resource.CongScannerTrustAuthCode %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:30px"> 
                            <asp:TextBox ID="tboxScannerMaxSize" runat="server"  Enabled="false"/>
                            <asp:RegularExpressionValidator id="ScannerMaxSizeRegularExpressionValidator" ControlToValidate="tboxScannerMaxSize" ValidationExpression="^([\d])*$" ErrorMessage="<%$Resources:Resource, WrongArchivesSize %>"  runat="server"/>
                            <ajaxToolkit:ValidatorCalloutExtender2 ID="ScannerMaxSizeRegularExpressionValidatorCalloutExtender" runat="server"
                                TargetControlID="ScannerMaxSizeRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="padding-bottom:5px;padding-left:5px">
                <table width="550px" style="padding: 10px" >
                    <tr>
                        <td >
                            <table style="width:270px" rules="groups" border="1px">
                                <thead>
                                    <td colspan="2">
                                        <b><%=Resources.Resource.InfectedFiles %></b>
                                    </td>
                                </thead>
                                <tbody>
                                    <tr style="height: 25px">
                                        <td>
                                            <%= Resources.Resource.Actions %>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInfectedActions" style="width:120px">
                                                <asp:ListItem Text="<%$ Resources:Resource, CongScannerCure %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 25px">
                                        <td>
                                            <%= Resources.Resource.CongScannerCases %>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInfectedCases" style="width:120px">
                                                <asp:ListItem Text="<%$ Resources:Resource, CongScannerCure %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 25px">
                                        <td colspan="2">
                                            <asp:CheckBox runat="server" ID="chkInfectedSaveCopy" /><%= Resources.Resource.CongScannerSaveCopyToQuarantine %></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td >
                            <table style="width:270px" rules="groups" border="1px" >
                                <thead>
                                    <td  colspan="2"><b><%= Resources.Resource.SuspiciousFiles %></b>
                                    </td>
                                </thead>
                                <tbody>
                                    <tr style="height: 25px">
                                        <td>
                                            <%= Resources.Resource.Actions %>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlSuspiciousActions" style="width:120px">
                                                <asp:ListItem Text="<%$ Resources:Resource, CongScannerCure %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 25px">
                                    <td></td>                                        
                                    </tr>
                                    <tr style="height: 25px">
                                        <td colspan="2">
                                            <asp:CheckBox runat="server" ID="chkSuspiciousSaveCopy" />
                                            <asp:Label ID="Label4" runat="server"><%= Resources.Resource.CongScannerSaveCopyToQuarantine %></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div id="ScannerTab2">
          <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                        <asp:Table ID="JournalEventTable" runat="server" CssClass="ListContrastTable" rules="cols">
                            <asp:TableHeaderRow  runat="server" CssClass="gridViewHeader">
                                <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" >
                                    <asp:Label  runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" >
                                    <asp:Label  runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;">
                                    <asp:Label  runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;">
                                    <asp:Label runat="server"  ><%=Resources.Resource.CCJournal %></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>
</div>
