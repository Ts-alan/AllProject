<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureProactive.ascx.cs" Inherits="Controls_TaskConfigureProactive" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskNameConfigureProactiveProtection%></div>
<div class="divSettings">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#MainTabsProactive").tabs({ cookie: { expires: 30} });
            $("#TabsProactive").tabs({ cookie: { expires: 30} });
            $("#TabsProactiveUsers").tabs({ cookie: { expires: 30} });
            $("#AccordionFileSystem").accordion({ collapsible: false, active: false, heightStyle: 'content' });
            $("#AccordionRegistry").accordion({ collapsible: false, active: false, heightStyle: 'content' });
            $("#AccordionFileSystemUsers").accordion({ collapsible: false, active: false, heightStyle: 'content' });
    });

    function OnChange(lboxId, tboxAttr) {
        var lbox = $('#' + lboxId.id);            
        var option = $("#" + lboxId.id + " option");
        var index = option.index($("#" + lboxId.id + " option:selected"));
        if (index == -1) return;
            
        $("[" + tboxAttr + "]").val(lbox.val());
    }

    function AddPath(tboxAttr) {
        if (!Page_ClientValidate(tboxAttr + "Validation"))
            return false;
        
        var newPath = $("[" + tboxAttr + "]").val();

        var isExist = false;
        $('[' + tboxAttr + "List] option").each(function () {
            if ($(this).val() == newPath) {
                isExist = true;
            }
        });
        if (isExist) {
            alert('<%=Resources.Resource.DuplicateValue %>');
            return false;
        }
        return true;
    }

    </script>
    <div id="MainTabsProactive">
        <ul>
            <li><a href="#mainTabProactive1"><%=Resources.Resource.General %></a> </li>
            <li><a href="#mainTabProactive2"><%=Resources.Resource.UserManaging%></a> </li>
            <li><a href="#mainTabProactive5"><%=Resources.Resource.Printers%></a> </li>
            <li><a href="#mainTabProactive4"><%=Resources.Resource.Audit%></a> </li>
            <li><a href="#mainTabProactive3"><%=Resources.Resource.JournalEvents%></a> </li>
        </ul>
        <div id="mainTabProactive1">
            <div><asp:CheckBox runat="server" ID="cboxGeneralOn" /> &nbsp;<%=Resources.Resource.VPP_General_On%></div>
            <div id="TabsProactive" style="width:900px">
                <ul>
                    <li><a href="#tabProactive1"><%=Resources.Resource.Applications %></a> </li>
                    <li><a href="#tabProactive2"><%=Resources.Resource.FileSystem %></a> </li>
                    <li><a href="#tabProactive3"><%=Resources.Resource.Registry %></a> </li>
                </ul>
                <div id='tabProactive1'>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddAppsTrusted" OnClick="lbtnAddAppsTrusted_Click" OnClientClick="return AddPath('AppsTrusted');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteAppsTrusted" OnClick="lbtnDeleteAppsTrusted_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxAppsTrusted" onchange="OnChange(this, 'AppsTrusted');" style="width: 350px;height: 100px;" AppsTrustedList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddAppsProtected" OnClick="lbtnAddAppsProtected_Click" OnClientClick="return AddPath('AppsProtected');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteAppsProtected" OnClick="lbtnDeleteAppsProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxAppsProtected" onchange="OnChange(this, 'AppsProtected');" style="width: 350px;height: 100px;" AppsProtectedList></asp:ListBox>
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
                <div id='tabProactive2'>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddFolderReadOnly" OnClick="lbtnAddFolderReadOnly_Click" OnClientClick="return AddPath('FolderReadOnly');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFolderReadOnly" OnClick="lbtnDeleteFolderReadOnly_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFolderReadOnly" onchange="OnChange(this, 'FolderReadOnly');" style="width: 350px;height: 100px;" FolderReadOnlyList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddFileReadOnly" OnClick="lbtnAddFileReadOnly_Click" OnClientClick="return AddPath('FileReadOnly');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFileReadOnly" OnClick="lbtnDeleteFileReadOnly_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFileReadOnly" onchange="OnChange(this, 'FileReadOnly');" style="width: 350px;height: 100px;" FileReadOnlyList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddFolderProtected" OnClick="lbtnAddFolderProtected_Click" OnClientClick="return AddPath('FolderProtected');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFolderProtected" OnClick="lbtnDeleteFolderProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFolderProtected" onchange="OnChange(this, 'FolderProtected');" style="width: 350px;height: 100px;" FolderProtectedList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddFileProtected" OnClick="lbtnAddFileProtected_Click" OnClientClick="return AddPath('FileProtected');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFileProtected" OnClick="lbtnDeleteFileProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFileProtected" onchange="OnChange(this, 'FileProtected');" style="width: 350px;height: 100px;" FileProtectedList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddFolderExcluded" OnClick="lbtnAddFolderExcluded_Click" OnClientClick="return AddPath('FolderExcluded');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFolderExcluded" OnClick="lbtnDeleteFolderExcluded_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFolderExcluded" onchange="OnChange(this, 'FolderExcluded');" style="width: 350px;height: 100px;" FolderExcludedList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddFileExcluded" OnClick="lbtnAddFileExcluded_Click" OnClientClick="return AddPath('FileExcluded');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFileExcluded" OnClick="lbtnDeleteFileExcluded_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFileExcluded" onchange="OnChange(this, 'FileExcluded');" style="width: 350px;height: 100px;" FileExcludedList></asp:ListBox>
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
                <div id='tabProactive3'>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddKeyReadOnly" OnClick="lbtnAddKeyReadOnly_Click" OnClientClick="return AddPath('KeyReadOnly');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteKeyReadOnly" OnClick="lbtnDeleteKeyReadOnly_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxKeyReadOnly" onchange="OnChange(this, 'KeyReadOnly');" style="width: 350px;height: 100px;" KeyReadOnlyList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddValueReadOnly" OnClick="lbtnAddValueReadOnly_Click" OnClientClick="return AddPath('ValueReadOnly');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteValueReadOnly" OnClick="lbtnDeleteValueReadOnly_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxValueReadOnly" onchange="OnChange(this, 'ValueReadOnly');" style="width: 350px;height: 100px;" ValueReadOnlyList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddKeyProtected" OnClick="lbtnAddKeyProtected_Click" OnClientClick="return AddPath('KeyProtected');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteKeyProtected" OnClick="lbtnDeleteKeyProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxKeyProtected" onchange="OnChange(this, 'KeyProtected');" style="width: 350px;height: 100px;" KeyProtectedList></asp:ListBox>
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
                                                <asp:LinkButton runat="server" ID="lbtnAddValueProtected" OnClick="lbtnAddValueProtected_Click" OnClientClick="return AddPath('ValueProtected');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteValueProtected" OnClick="lbtnDeleteValueProtected_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxValueProtected" onchange="OnChange(this, 'ValueProtected');" style="width: 350px;height: 100px;" ValueProtectedList></asp:ListBox>
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
        <div id="mainTabProactive2">
            <div><asp:CheckBox runat="server" ID="cboxUsersOn" /> &nbsp;<%=Resources.Resource.VPP_Users_On%></div>
            <div>
                <asp:UpdatePanel runat="server" ID="upnlUsers">
                <ContentTemplate>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlUsers" OnSelectedIndexChanged="ddlUsers_Changed" AutoPostBack="true"></asp:DropDownList>
                        <asp:LinkButton runat="server" ID="lbtnAddUser" OnClientClick="return false;" SkinID="Button"><%=Resources.Resource.Add %></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lbtnDeleteUser" OnClick="lbtnDeleteUser_Click" OnClientClick="return DeleteUser();" SkinID="Button"><%=Resources.Resource.Delete %></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lbtnAddUserHidden" style="display: none; width: 1px;" OnClick="lbtnAddUser_Click"></asp:LinkButton>
                    </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                
                <div id="divAddUser" style="display: none;">
                    <table style="margin:5px;">
                        <tr>
                            <td><%=Resources.Resource.Login %></td>
                            <td style="padding-left: 10px;padding-bottom:10px;">
                                <asp:TextBox ID="tboxUserName" runat="server" Style="width: 180px" />
                                <asp:RequiredFieldValidator ID="requiredUserName" ControlToValidate="tboxUserName" runat="server"
                                    Display="None" ErrorMessage='<%$ Resources:Resource, LoginRequiredErrorMessage %>'
                                    ValidationGroup="AddUserValidation">
                                </asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="requiredUserNameExt" runat="server" TargetControlID="requiredUserName"
                                    HighlightCssClass="highlight" PopupPosition="BottomLeft">
                                </ajaxToolkit:ValidatorCalloutExtender2>
                            </td>
                        </tr>
                        <tr>
                            <td><%=Resources.Resource.DomainName %></td>
                            <td style="padding-left: 10px;padding-bottom:10px;">
                                <asp:TextBox ID="tboxDomain" runat="server" Style="width: 180px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <script type="text/javascript" language="javascript">
                    $(document).ready(function () {
                        var ddlUsers = '#' + '<%= ddlUsers.ClientID %>';
                        var tboxUserName = '#' + '<%= tboxUserName.ClientID %>';
                        var tboxDomain = '#' + '<%= tboxDomain.ClientID %>';
                        var btnAddUser = '#' + '<%= lbtnAddUser.ClientID %>';
                        var btnAddUserRefresh = '<%=lbtnAddUserHidden.UniqueID %>';

                        $("#divAddUser").dialog({ autoOpen: false });

                        $(document).on("click", btnAddUser, function () {
                            $("#divAddUser").dialog('destroy');
                            var d = $("#divAddUser");
                            var dOpt = {
                                title: '',
                                width: 320,
                                modal: true,
                                resizable: false,
                                buttons: {
                                    '<%=Resources.Resource.Add %>': function () {
                                        if (Page_ClientValidate('AddUserValidation')) {                                            
                                            var fullUserName = $(tboxUserName).val();
                                            if ($(tboxDomain).val() != '')
                                                fullUserName = $(tboxDomain).val() + '\\' + fullUserName;

                                            var isExist = false;
                                            $(ddlUsers + " option").each(function () {
                                                if ($(this).val() == fullUserName) {
                                                    isExist = true;
                                                }
                                            });
                                            if (isExist) {
                                                alert('<%=Resources.Resource.DuplicateUserName %>');
                                                return;
                                            }

                                            $(tboxUserName).val('');
                                            $(tboxDomain).val('');
                                            d.dialog('close');
                                            __doPostBack(btnAddUserRefresh, fullUserName);
                                        }
                                    }
                                }
                            };
                            d.dialog(dOpt);
                        });
                    });

                    function DeleteUser() {
                        if ($('#' + '<%= ddlUsers.ClientID %>').prop('selectedIndex') == "0") {
                            alert('<%=Resources.Resource.NoDeleteDefault %>');
                            return false;
                        }

                        return true;
                    }
                </script>
            </div>
            <div id="TabsProactiveUsers" style="width:900px">
                <ul>
                    <li><a href="#tabUsersProactive1"><%=Resources.Resource.Applications %></a> </li>
                    <li><a href="#tabUsersProactive2"><%=Resources.Resource.FileSystem %></a> </li>
                    <li><a href="#tabUsersProactive3"><%=Resources.Resource.Registry %></a> </li>
                    <li><a href="#tabUsersProactive4"><%=Resources.Resource.Printers %></a> </li>
                </ul>
                <div id='tabUsersProactive1'>
                    <asp:UpdatePanel ID="upnlApplicationTrustedUsers" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                        <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                            <%=Resources.Resource.Trusted %>
                                        </h3>                        
                                        <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                            <div>
                    
                                                <asp:TextBox runat="server" ID="tboxAppsTrustedUsers" style="width: 350px;" AppsTrustedUsers></asp:TextBox>                                
                                                <br />
                                                <asp:LinkButton runat="server" ID="lbtnAddAppsTrustedUsers" OnClick="lbtnAddAppsTrustedUsers_Click" OnClientClick="return AddPath('AppsTrustedUsers');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteAppsTrustedUsers" OnClick="lbtnDeleteAppsTrustedUsers_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxAppsTrustedUsers" onchange="OnChange(this, 'AppsTrustedUsers');" style="width: 350px;height: 100px;" AppsTrustedUsersList></asp:ListBox>
                                                <asp:RequiredFieldValidator ID="reqAppsTrustedUsers" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                            ControlToValidate="tboxAppsTrustedUsers" Display="None" ValidationGroup="AppsTrustedUsersValidation">
                                                </asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqAppsTrustedUsersCallout" HighlightCssClass="highlight" TargetControlID="reqAppsTrustedUsers"></ajaxToolkit:ValidatorCalloutExtender2>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                </div>
                <div id='tabUsersProactive2'>
                <div id='AccordionFileSystemUsers'>
                <h3><a><span><%=Resources.Resource.ReadOnly %></span></a></h3>
                <div>
                    <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnlFolderReadOnlyUsers" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                        <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                            <%=Resources.Resource.Folders %>
                                        </h3>                        
                                        <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                            <div>
                    
                                                <asp:TextBox runat="server" ID="tboxFolderReadOnlyUsers" style="width: 350px;" FolderReadOnlyUsers></asp:TextBox>                                
                                                <br />
                                                <asp:LinkButton runat="server" ID="lbtnAddFolderReadOnlyUsers" OnClick="lbtnAddFolderReadOnlyUsers_Click" OnClientClick="return AddPath('FolderReadOnlyUsers');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFolderReadOnlyUsers" OnClick="lbtnDeleteFolderReadOnlyUsers_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFolderReadOnlyUsers" onchange="OnChange(this, 'FolderReadOnlyUsers');" style="width: 350px;height: 100px;" FolderReadOnlyUsersList></asp:ListBox>
                                                <asp:RequiredFieldValidator ID="reqFolderReadOnlyUsers" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                            ControlToValidate="tboxFolderReadOnlyUsers" Display="None" ValidationGroup="FolderReadOnlyUsersValidation">
                                                </asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFolderReadOnlyUsersCallout" HighlightCssClass="highlight" TargetControlID="reqFolderReadOnlyUsers"></ajaxToolkit:ValidatorCalloutExtender2>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="padding-left: 20px;">
                            <asp:UpdatePanel ID="upnlFileReadOnlyUsers" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                        <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                            <%=Resources.Resource.Files %>
                                        </h3>                        
                                        <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                            <div>
                    
                                                <asp:TextBox runat="server" ID="tboxFileReadOnlyUsers" style="width: 350px;" FileReadOnlyUsers></asp:TextBox>                                
                                                <br />
                                                <asp:LinkButton runat="server" ID="lbtnAddFileReadOnlyUsers" OnClick="lbtnAddFileReadOnlyUsers_Click" OnClientClick="return AddPath('FileReadOnlyUsers');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFileReadOnlyUsers" OnClick="lbtnDeleteFileReadOnlyUsers_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFileReadOnlyUsers" onchange="OnChange(this, 'FileReadOnlyUsers');" style="width: 350px;height: 100px;" FileReadOnlyUsersList></asp:ListBox>
                                                <asp:RequiredFieldValidator ID="reqFileReadOnlyUsers" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                            ControlToValidate="tboxFileReadOnlyUsers" Display="None" ValidationGroup="FileReadOnlyUsersValidation">
                                                </asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFileReadOnlyUsersCallout" HighlightCssClass="highlight" TargetControlID="reqFileReadOnlyUsers"></ajaxToolkit:ValidatorCalloutExtender2>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
	                </tr>
	                </table>
                </div>
                <h3><a><span><%=Resources.Resource.FullAccess %></span></a></h3>
                <div>
                    <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnlFolderProtectedUsers" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                        <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                            <%=Resources.Resource.Folders %>
                                        </h3>                        
                                        <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                            <div>
                    
                                                <asp:TextBox runat="server" ID="tboxFolderProtectedUsers" style="width: 350px;" FolderProtectedUsers></asp:TextBox>                                
                                                <br />
                                                <asp:LinkButton runat="server" ID="lbtnAddFolderProtectedUsers" OnClick="lbtnAddFolderProtectedUsers_Click" OnClientClick="return AddPath('FolderProtectedUsers');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFolderProtectedUsers" OnClick="lbtnDeleteFolderProtectedUsers_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFolderProtectedUsers" onchange="OnChange(this, 'FolderProtectedUsers');" style="width: 350px;height: 100px;" FolderProtectedUsersList></asp:ListBox>
                                                <asp:RequiredFieldValidator ID="reqFolderProtectedUsers" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                            ControlToValidate="tboxFolderProtectedUsers" Display="None" ValidationGroup="FolderProtectedUsersValidation">
                                                </asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFolderProtectedUsersCallout" HighlightCssClass="highlight" TargetControlID="reqFolderProtectedUsers"></ajaxToolkit:ValidatorCalloutExtender2>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="padding-left: 20px;">
                            <asp:UpdatePanel ID="upnlFileProtectedUsers" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                        <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                            <%=Resources.Resource.Files %>
                                        </h3>                        
                                        <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                            <div>
                    
                                                <asp:TextBox runat="server" ID="tboxFileProtectedUsers" style="width: 350px;" FileProtectedUsers></asp:TextBox>                                
                                                <br />
                                                <asp:LinkButton runat="server" ID="lbtnAddFileProtectedUsers" OnClick="lbtnAddFileProtectedUsers_Click" OnClientClick="return AddPath('FileProtectedUsers');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteFileProtectedUsers" OnClick="lbtnDeleteFileProtectedUsers_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxFileProtectedUsers" onchange="OnChange(this, 'FileProtectedUsers');" style="width: 350px;height: 100px;" FileProtectedUsersList></asp:ListBox>
                                                <asp:RequiredFieldValidator ID="reqFileProtectedUsers" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                            ControlToValidate="tboxFileProtectedUsers" Display="None" ValidationGroup="FileProtectedUsersValidation">
                                                </asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqFileProtectedUsersCallout" HighlightCssClass="highlight" TargetControlID="reqFileProtectedUsers"></ajaxToolkit:ValidatorCalloutExtender2>
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
                <div id='tabUsersProactive3'>
                    <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnlKeyReadOnlyUsers" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                        <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                            <%=Resources.Resource.ReadOnly%>
                                        </h3>                        
                                        <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                            <div>
                    
                                                <asp:TextBox runat="server" ID="tboxKeyReadOnlyUsers" style="width: 350px;" KeyReadOnlyUsers></asp:TextBox>                                
                                                <br />
                                                <asp:LinkButton runat="server" ID="lbtnAddKeyReadOnlyUsers" OnClick="lbtnAddKeyReadOnlyUsers_Click" OnClientClick="return AddPath('KeyReadOnlyUsers');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteKeyReadOnlyUsers" OnClick="lbtnDeleteKeyReadOnlyUsers_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxKeyReadOnlyUsers" onchange="OnChange(this, 'KeyReadOnlyUsers');" style="width: 350px;height: 100px;" KeyReadOnlyUsersList></asp:ListBox>
                                                <asp:RequiredFieldValidator ID="reqKeyReadOnlyUsers" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                            ControlToValidate="tboxKeyReadOnlyUsers" Display="None" ValidationGroup="KeyReadOnlyUsersValidation">
                                                </asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqKeyReadOnlyUsersCallout" HighlightCssClass="highlight" TargetControlID="reqKeyReadOnlyUsers"></ajaxToolkit:ValidatorCalloutExtender2>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="padding-left: 20px;">
                            <asp:UpdatePanel ID="upnlKeyProtectedUsers" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                        <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                            <%=Resources.Resource.FullAccess%>
                                        </h3>                        
                                        <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                            <div>
                    
                                                <asp:TextBox runat="server" ID="tboxKeyProtectedUsers" style="width: 350px;" KeyProtectedUsers></asp:TextBox>                                
                                                <br />
                                                <asp:LinkButton runat="server" ID="lbtnAddKeyProtectedUsers" OnClick="lbtnAddKeyProtectedUsers_Click" OnClientClick="return AddPath('KeyProtectedUsers');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton runat="server" ID="lbtnDeleteKeyProtectedUsers" OnClick="lbtnDeleteKeyProtectedUsers_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                <br />
                                                <asp:ListBox runat="server" ID="lboxKeyProtectedUsers" onchange="OnChange(this, 'KeyProtectedUsers');" style="width: 350px;height: 100px;" KeyProtectedUsersList></asp:ListBox>
                                                <asp:RequiredFieldValidator ID="reqKeyProtectedUsers" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                            ControlToValidate="tboxKeyProtectedUsers" Display="None" ValidationGroup="KeyProtectedUsersValidation">
                                                </asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqKeyProtectedUsersCallout" HighlightCssClass="highlight" TargetControlID="reqKeyProtectedUsers"></ajaxToolkit:ValidatorCalloutExtender2>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>    
                        </td>
	                </tr>
	                </table>
                </div>
                <div id='tabUsersProactive4'>
                    <asp:UpdatePanel ID="upnlPrinterTrustedUsers" runat="server" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                                <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                            ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                    <%=Resources.Resource.Trusted %>
                                </h3>                        
                                <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                    <div>
                    
                                        <asp:TextBox runat="server" ID="tboxPrinterTrustedUsers" style="width: 350px;" PrinterTrustedUsers></asp:TextBox>                                
                                        <br />
                                        <asp:LinkButton runat="server" ID="lbtnAddPrinterTrustedUsers" OnClick="lbtnAddPrinterTrustedUsers_Click" OnClientClick="return AddPath('PrinterTrustedUsers');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lbtnDeletePrinterTrustedUsers" OnClick="lbtnDeletePrinterTrustedUsers_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxPrinterTrustedUsers" onchange="OnChange(this, 'PrinterTrustedUsers');" style="width: 350px;height: 100px;" PrinterTrustedUsersList></asp:ListBox>
                                        <asp:RequiredFieldValidator ID="reqPrinterTrustedUsers" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                                    ControlToValidate="tboxPrinterTrustedUsers" Display="None" ValidationGroup="PrinterTrustedUsersValidation">
                                        </asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqPrinterTrustedUsersCallout" HighlightCssClass="highlight" TargetControlID="reqPrinterTrustedUsers"></ajaxToolkit:ValidatorCalloutExtender2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="mainTabProactive5">
            <div><asp:CheckBox runat="server" ID="cboxPrintersOn" /> &nbsp;<%=Resources.Resource.VPP_Printers_On%></div>
            <asp:UpdatePanel ID="upnlPrinterTrusted" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <div class="ui-accordion ui-widget ui-helper-reset" style="width: 400px;">
                        <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                            <%=Resources.Resource.Trusted %>
                        </h3>                        
                        <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                            <div>
                    
                                <asp:TextBox runat="server" ID="tboxPrinterTrusted" style="width: 350px;" PrinterTrusted></asp:TextBox>                                
                                <br />
                                <asp:LinkButton runat="server" ID="lbtnAddPrinterTrusted" OnClick="lbtnAddPrinterTrusted_Click" OnClientClick="return AddPath('PrinterTrusted');" style="color: Blue;"><%=Resources.Resource.Add %></asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton runat="server" ID="lbtnDeletePrinterTrusted" OnClick="lbtnDeletePrinterTrusted_Click" style="color: Blue;"><%=Resources.Resource.Delete %></asp:LinkButton>
                                <br />
                                <asp:ListBox runat="server" ID="lboxPrinterTrusted" onchange="OnChange(this, 'PrinterTrusted');" style="width: 350px;height: 100px;" PrinterTrustedList></asp:ListBox>
                                <asp:RequiredFieldValidator ID="reqPrinterTrusted" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                                            ControlToValidate="tboxPrinterTrusted" Display="None" ValidationGroup="PrinterTrustedValidation">
                                </asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="reqPrinterTrustedCallout" HighlightCssClass="highlight" TargetControlID="reqPrinterTrusted"></ajaxToolkit:ValidatorCalloutExtender2>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="mainTabProactive4">
            <div><asp:CheckBox runat="server" ID="cboxIsUserAudit" /> &nbsp;<%=Resources.Resource.AuditOn%></div>
            <div style="padding-top: 10px;"><b><%=Resources.Resource.ProcessedFileTypes%></b></div>
            <div>
                <asp:TextBox runat="server" ID="tboxProcessedExtensions" style="width: 300px;"></asp:TextBox>
                <asp:RegularExpressionValidator id="tboxProcessedExtensionsRegularExpressionValidator" ControlToValidate="tboxProcessedExtensions" 
                    ValidationExpression="^([\w|.\?|\*]+)*$" ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>"  runat="server" Display="None">
                </asp:RegularExpressionValidator>
                <ajaxToolkit:ValidatorCalloutExtender2 ID="tboxProcessedExtensionsRegularExpressionValidatorCalloutExtender" runat="server"
                        TargetControlID="tboxProcessedExtensionsRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="Right" >
                </ajaxToolkit:ValidatorCalloutExtender2>
            </div>
        </div>
        <div id="mainTabProactive3">
            <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                        <asp:Table ID="JournalEventTable" runat="server" CssClass="ListContrastTable" rules="cols">
                            <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" CssClass="gridViewHeader">
                                <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;">
                                    <asp:Label ID="Label1" runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;">
                                    <asp:Label ID="Label2" runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;">
                                    <asp:Label ID="Label3" runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;">
                                    <asp:Label ID="Label4" runat="server"  ><%=Resources.Resource.CCJournal %></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>