<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureScanner.ascx.cs" Inherits="Controls_TaskConfigureScanner" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.TaskNameRunScanner%></div>
<script type="text/javascript" language="javascript">
    function pageLoad() {
        var defExts = ".COM.EXE.DLL.DRV.SYS.OV?.VXD.SCR.CPL.OCX.BPL.AX.PIF.DO?.XL?.HLP.RTF.WI?.WZ?.MSI.MSC.HT*.VB*.JS.JSE.ASP*.CGI.PHP*.?HTML.BAT.CMD.EML.NWS.MSG.XML.MSO.WPS.PPT.PUB.JPG.JPEG.ANI.INF.SWF.PDF";
        $("#ScannerTabs").tabs({ cookie: { expires: 30} });
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

<div id="ScannerTabs">
    <ul>
        <li><a href="#ScannerTab1"><%=Resources.Resource.ScanningObjects %> </a> </li>
        <li><a href="#ScannerTab2"><%=Resources.Resource.JournalEvents %></a> </li>
    </ul>

    <div id="ScannerTab1">
    
        <table   class="ListContrastTable" rules="all" style="width:600px">
            <tr>
                <td colSpan="2" style="width:600px">
                    <table width="599px" rules="groups">
                        <thead>
                            <th align="center" style="font-weight:bold"><%= Resources.Resource.CongScannerScanFileList %></th>
                        </thead>                        
                        <tr>
                            <td>
                                &nbsp;
                                <asp:RequiredFieldValidator ID="tboxScannerFileExtensionsValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                                    ControlToValidate="tboxScannerFileExtensions" Display="None" ValidationGroup="ScannerFileExtensionsValidationGroup" />
                                 <asp:RegularExpressionValidator id="ScannerFileExtensionsRegularExpressionValidator" ControlToValidate="tboxScannerFileExtensions" ValidationExpression="^(\.[\w|\?|\*]+)*$" ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>"  runat="server"/>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCallouttboxScannerFileExtensions" runat="server"
                                    TargetControlID="tboxScannerFileExtensionsValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="RegularValidatorCallouttboxScannerFileExtensions" runat="server"
                                    TargetControlID="ScannerFileExtensionsRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                                <asp:TextBox runat="server" ID="tboxScannerFileExtensions" style="width:435px"></asp:TextBox>
                                &nbsp;     
                                <asp:LinkButton runat="server" ID="lbtnScannerFileExtensionReset" SkinID="Button" OnClientClick="return false;"><%= Resources.Resource.DefaultFiles %></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:Label runat="server" style="width:100px;"><%=Resources.Resource.CongScannerExclude%></asp:Label>
                                &nbsp;&nbsp;
                               <asp:RegularExpressionValidator id="ScannerFilesExcludedRegularExpressionValidator" ControlToValidate="tboxScannerFilesExcluded" ValidationExpression="^(\.[\w|\?|\*]+)*$" ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>"  runat="server"/>
                               <ajaxToolkit:ValidatorCalloutExtender2 ID="ScannerFilesExcludedRegularExpressionValidatorCalloutExtender" runat="server"
                                    TargetControlID="ScannerFilesExcludedRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                                <asp:TextBox runat="server" ID="tboxScannerFilesExcluded" style="width:435px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox runat="server" ID="chkScannerCache"></asp:CheckBox>
                                <asp:Label runat="server"><%=Resources.Resource.CongScannerEnableCache %></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="width:600px">
                    <table width="590px">
                        <tr>
                            <td style="width:290px">
                                <asp:CheckBox ID="chkScannerScanMail" runat="server"/>
                                <asp:Label runat="server" ><%=Resources.Resource.CongScannerCheckMail%></asp:Label>
                            </td>
                            <td style="width:290px">
                                <asp:CheckBox ID="chkScannerFindPotential" runat="server"/>
                                <asp:Label  runat="server" ><%=Resources.Resource.CongScannerPotential%></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkScannerScanArchives" runat="server"/>
                                <asp:Label  runat="server" ><%=Resources.Resource.CongScannerCheckArchives%></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkScannerFindVirusInstalls" runat="server"/>
                                <asp:Label  runat="server" ><%=Resources.Resource.CongScannerSFX %></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;<asp:CheckBox ID="chkScannerMaxSize" runat="server" Enabled="false"/>
                                <asp:Label   runat="server" ><%=Resources.Resource.CongScannerArchivesSize%></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkScannerTrustAuthenCode" runat="server"/>
                                <asp:Label   runat="server" ><%=Resources.Resource.CongScannerTrustAuthCode %></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp; &nbsp; &nbsp;  &nbsp;  &nbsp;
                                <asp:RegularExpressionValidator id="ScannerMaxSizeRegularExpressionValidator" ControlToValidate="tboxScannerMaxSize" ValidationExpression="^([\d])*$" ErrorMessage="<%$Resources:Resource, WrongArchivesSize %>"  runat="server"/>
                               <ajaxToolkit:ValidatorCalloutExtender2 ID="ScannerMaxSizeRegularExpressionValidatorCalloutExtender" runat="server"
                                    TargetControlID="ScannerMaxSizeRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                                <asp:TextBox ID="tboxScannerMaxSize" runat="server"  Enabled="false"/>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr >
                <td style="width:300px">
                    <table rules="groups" >
                        <thead >
                            <td style="font-weight:bold">
                                 <asp:Label  runat="server" ><%=Resources.Resource.InfectedFiles %></asp:Label> 
                            </td>                           
                        </thead>
                        <tbody>
                            <tr style="height:25px">
                                <td>
                                    <asp:Label runat="server"><%= Resources.Resource.Actions %></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlInfectedActions">
                                        <asp:ListItem Text="<%$ Resources:Resource, CongScannerCure %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height:25px">
                                <td>
                                    <asp:Label  runat="server"><%= Resources.Resource.CongScannerCases %></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlInfectedCases">
                                        <asp:ListItem Text="<%$ Resources:Resource, CongScannerCure %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height:25px">
                                <td colspan="2">
                                    <asp:CheckBox runat="server" ID="chkInfectedSaveCopy" />
                                    <asp:Label runat="server"><%= Resources.Resource.CongScannerSaveCopyToQuarantine %></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="width:300px">
                    <table  rules="groups" >
                        <thead>
                            <td style="font-weight:bold">
                                <asp:Label  runat="server" ><%= Resources.Resource.SuspiciousFiles %></asp:Label>
                            </td>
                        </thead>                        
                        <tr style="height:25px">
                            <td >
                                <asp:Label  runat="server"><%= Resources.Resource.Actions %></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSuspiciousActions">
                                    <asp:ListItem Text="<%$ Resources:Resource, CongScannerCure %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:25px">
                            <td >
                                <asp:Label   runat="server"><%= Resources.Resource.CongScannerCases %></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSuspiciousCases">
                                    <asp:ListItem Text="<%$ Resources:Resource, CongScannerCure %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:25px">
                            <td colspan="2">
                                <asp:CheckBox runat="server" ID="chkSuspiciousSaveCopy" />
                                <asp:Label  runat="server"><%= Resources.Resource.CongScannerSaveCopyToQuarantine %></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    </div>
    <div id="ScannerTab2">
          <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                        <asp:Table ID="JournalEventTable" runat="server" CssClass="ListContrastTable">
                            <asp:TableHeaderRow  runat="server">
                                <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" class="listRulesHeader">
                                    <asp:Label  runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                                    <asp:Label  runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                                    <asp:Label  runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                                    <asp:Label runat="server"  ><%=Resources.Resource.CCJournal %></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>
</div>
