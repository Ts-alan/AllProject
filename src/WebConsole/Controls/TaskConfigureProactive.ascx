<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureProactive.ascx.cs" Inherits="Controls_TaskConfigureProactive" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskNameConfigureProactiveProtection%></div>
<div class="divSettings">
    <script type="text/javascript">
     $(document).ready(function () {
            $("#TabsProactive").tabs({ cookie: { expires: 30} });
            $("#AccordionFileSystem").accordion({ collapsible: false, active: false, heightStyle: 'content' });
            $("#AccordionRegistry").accordion({ collapsible: false, active: false, heightStyle: 'content' });
    });

    function OnChange(lboxId, tboxAttr) {
        var lbox = $('#' + lboxId.id);            
        var option = $("#" + lboxId.id + " option");
        var index = option.index($("#" + lboxId.id + " option:selected"));
        if (index == -1) return;
            
        $("[" + tboxAttr + "]").val(lbox.val());
    }
    </script>

    <div id="TabsProactive" style="width:900px">
        <ul>
            <li><a href="#tab1"><%=Resources.Resource.Applications %></a> </li>
            <li><a href="#tab2"><%=Resources.Resource.FileSystem %></a> </li>
            <li><a href="#tab3"><%=Resources.Resource.Registry %></a> </li>
        </ul>
        <div id='tab1'>
            <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlApplicationTrusted" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Trusted %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxAppsTrusted" style="width: 350px;" AppsTrusted></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddAppsTrusted" OnClick="lbtnAddAppsTrusted_Click" OnClientClick="return Page_ClientValidate('AppsTrustedValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteAppsTrusted" OnClick="lbtnDeleteAppsTrusted_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxAppsTrusted" onchange="OnChange(this, 'AppsTrusted');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqAppsTrusted" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxAppsTrusted" Display="None" ValidationGroup="AppsTrustedValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqAppsTrustedCallout" HighlightCssClass="highlight" TargetControlID="reqAppsTrusted"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="padding-left: 20px;">
                    <asp:UpdatePanel ID="upnlApplicationProtected" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Protected %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxAppsProtected" style="width: 350px;" AppsProtected></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddAppsProtected" OnClick="lbtnAddAppsProtected_Click" OnClientClick="return Page_ClientValidate('AppsProtectedValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteAppsProtected" OnClick="lbtnDeleteAppsProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxAppsProtected" onchange="OnChange(this, 'AppsProtected');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqAppsProtected" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxAppsProtected" Display="None" ValidationGroup="AppsProtectedValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqAppsProtectedCallout" HighlightCssClass="highlight" TargetControlID="reqAppsProtected"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            </table>
        </div>
        <div id='tab2'>
        <div id='AccordionFileSystem'>
        <h3><a><span><%=Resources.Resource.ReadOnly %></span></a></h3>
        <div>
            <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlFolderReadOnly" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Folders %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxFolderReadOnly" style="width: 350px;" FolderReadOnly></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddFolderReadOnly" OnClick="lbtnAddFolderReadOnly_Click" OnClientClick="return Page_ClientValidate('FolderReadOnlyValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteFolderReadOnly" OnClick="lbtnDeleteFolderReadOnly_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxFolderReadOnly" onchange="OnChange(this, 'FolderReadOnly');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqFolderReadOnly" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxFolderReadOnly" Display="None" ValidationGroup="FolderReadOnlyValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFolderReadOnlyCallout" HighlightCssClass="highlight" TargetControlID="reqFolderReadOnly"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="padding-left: 20px;">
                    <asp:UpdatePanel ID="upnlFileReadOnly" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Files %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxFileReadOnly" style="width: 350px;" FileReadOnly></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddFileReadOnly" OnClick="lbtnAddFileReadOnly_Click" OnClientClick="return Page_ClientValidate('FileReadOnlyValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteFileReadOnly" OnClick="lbtnDeleteFileReadOnly_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxFileReadOnly" onchange="OnChange(this, 'FileReadOnly');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqFileReadOnly" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxFileReadOnly" Display="None" ValidationGroup="FileReadOnlyValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFileReadOnlyCallout" HighlightCssClass="highlight" TargetControlID="reqFileReadOnly"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
	        </tr>
	        </table>
        </div>
        <h3><a><span><%=Resources.Resource.Protected %></span></a></h3>
        <div>
            <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlFolderProtected" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Folders %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxFolderProtected" style="width: 350px;" FolderProtected></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddFolderProtected" OnClick="lbtnAddFolderProtected_Click" OnClientClick="return Page_ClientValidate('FolderProtectedValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteFolderProtected" OnClick="lbtnDeleteFolderProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxFolderProtected" onchange="OnChange(this, 'FolderProtected');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqFolderProtected" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxFolderProtected" Display="None" ValidationGroup="FolderProtectedValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFolderProtectedCallout" HighlightCssClass="highlight" TargetControlID="reqFolderProtected"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="padding-left: 20px;">
                    <asp:UpdatePanel ID="upnlFileProtected" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Files %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxFileProtected" style="width: 350px;" FileProtected></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddFileProtected" OnClick="lbtnAddFileProtected_Click" OnClientClick="return Page_ClientValidate('FileProtectedValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteFileProtected" OnClick="lbtnDeleteFileProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxFileProtected" onchange="OnChange(this, 'FileProtected');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqFileProtected" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxFileProtected" Display="None" ValidationGroup="FileProtectedValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFileProtectedCallout" HighlightCssClass="highlight" TargetControlID="reqFileProtected"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
	        </table>
        </div>
        <h3><a><span><%=Resources.Resource.Excluded %></span></a></h3>
        <div>
            <table>    
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlFolderExcluded" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Folders %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxFolderExcluded" style="width: 350px;" FolderExcluded></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddFolderExcluded" OnClick="lbtnAddFolderExcluded_Click" OnClientClick="return Page_ClientValidate('FolderExcludedValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteFolderExcluded" OnClick="lbtnDeleteFolderExcluded_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxFolderExcluded" onchange="OnChange(this, 'FolderExcluded');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqFolderExcluded" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxFolderExcluded" Display="None" ValidationGroup="FolderExcludedValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFolderExcludedCallout" HighlightCssClass="highlight" TargetControlID="reqFolderExcluded"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="padding-left: 20px;">
                    <asp:UpdatePanel ID="upnlFileExcluded" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Files %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxFileExcluded" style="width: 350px;" FileExcluded></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddFileExcluded" OnClick="lbtnAddFileExcluded_Click" OnClientClick="return Page_ClientValidate('FileExcludedValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteFileExcluded" OnClick="lbtnDeleteFileExcluded_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxFileExcluded" onchange="OnChange(this, 'FileExcluded');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqFileExcluded" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxFileExcluded" Display="None" ValidationGroup="FileExcludedValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFileExcludedCallout" HighlightCssClass="highlight" TargetControlID="reqFileExcluded"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            </table>
        </div>
        </div>
        </div>
        <div id='tab3'>
        <div id='AccordionRegistry'>
        <h3><a><span><%=Resources.Resource.ReadOnly %></span></a></h3>
        <div>
            <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlKeyReadOnly" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Keys %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxKeyReadOnly" style="width: 350px;" KeyReadOnly></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddKeyReadOnly" OnClick="lbtnAddKeyReadOnly_Click" OnClientClick="return Page_ClientValidate('KeyReadOnlyValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteKeyReadOnly" OnClick="lbtnDeleteKeyReadOnly_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxKeyReadOnly" onchange="OnChange(this, 'KeyReadOnly');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqKeyReadOnly" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxKeyReadOnly" Display="None" ValidationGroup="KeyReadOnlyValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqKeyReadOnlyCallout" HighlightCssClass="highlight" TargetControlID="reqKeyReadOnly"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="padding-left: 20px;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Values %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxValueReadOnly" style="width: 350px;" ValueReadOnly></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddValueReadOnly" OnClick="lbtnAddValueReadOnly_Click" OnClientClick="return Page_ClientValidate('ValueReadOnlyValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteValueReadOnly" OnClick="lbtnDeleteValueReadOnly_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxValueReadOnly" onchange="OnChange(this, 'ValueReadOnly');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqValueReadOnly" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxValueReadOnly" Display="None" ValidationGroup="ValueReadOnlyValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqValueReadOnlyCallout" HighlightCssClass="highlight" TargetControlID="reqValueReadOnly"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
	        </tr>
	        </table>
        </div>
        <h3><a><span><%=Resources.Resource.Protected %></span></a></h3>
        <div>
            <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlKeyProtected" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Keys %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxKeyProtected" style="width: 350px;" KeyProtected></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddKeyProtected" OnClick="lbtnAddKeyProtected_Click" OnClientClick="return Page_ClientValidate('KeyProtectedValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteKeyProtected" OnClick="lbtnDeleteKeyProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxKeyProtected" onchange="OnChange(this, 'KeyProtected');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqKeyProtected" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxKeyProtected" Display="None" ValidationGroup="KeyProtectedValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqKeyProtectedCallout" HighlightCssClass="highlight" TargetControlID="reqKeyProtected"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="padding-left: 20px;">
                    <asp:UpdatePanel ID="upnlValueProtected" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Values %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxValueProtected" style="width: 350px;" ValueProtected></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddValueProtected" OnClick="lbtnAddValueProtected_Click" OnClientClick="return Page_ClientValidate('ValueProtectedValidation');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeleteValueProtected" OnClick="lbtnDeleteValueProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxValueProtected" onchange="OnChange(this, 'ValueProtected');" style="width: 350px;height: 100px;"></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqValueProtected" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxValueProtected" Display="None" ValidationGroup="ValueProtectedValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqValueProtectedCallout" HighlightCssClass="highlight" TargetControlID="reqValueProtected"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
	        </table>
        </div>        
        </div>  
        </div>
    </div>     
</div>