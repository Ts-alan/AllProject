<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskRunScanner.ascx.cs" Inherits="Controls_TaskRunScanner" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskNameRunScanner%></div>
<script type="text/javascript">
    function OnChangeScanPath() {
        var lboxId = '#<%=lboxPathes.ClientID %>';
        var lbox = $(lboxId);
        var option = $(lboxId + " option");
        var index = option.index($(lboxId + " option:selected"));
        if (index == -1) return;

        $('#<%=tboxPath.ClientID %>').val(lbox.val());
    }
    </script>
<div class="divSettings">
    <table style="width:560px;border-width:2px;border-color:#658eda;border-style:solid;">
    <tr>
        <td style="padding-left: 20px;padding-top: 10px;">
            <asp:CheckBox runat="server" ID="cboxCheckMemory" /> &nbsp; <%=Resources.Resource.ScanMemory%>
        </td>
    </tr>
    <tr>
        <td style="padding-left: 20px;padding-top: 10px;padding-bottom:10px;">
            <asp:UpdatePanel ID="upnlScanPathes" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 500px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.ScanPath %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxPath" style="width: 450px;"></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddPath" OnClick="lbtnAddPath_Click" OnClientClick="return Page_ClientValidate('ScanPathValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeletePath" OnClick="lbtnDeletePath_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxPathes" onchange="OnChangeScanPath()" style="width: 450px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqScanPathes" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxPath" Display="None" ValidationGroup="ScanPathValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="reqScanPathesCallout" HighlightCssClass="highlight" TargetControlID="reqScanPathes"></ajaxToolkit:ValidatorCalloutExtender>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
        </td>
    </tr>
    </table>  
</div>