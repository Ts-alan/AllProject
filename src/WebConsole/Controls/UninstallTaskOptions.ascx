<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UninstallTaskOptions.ascx.cs"
    Inherits="Controls_UninstallTaskOptions" %>
<div runat="server" id="tskUninstall" style="display:none" title='<%$Resources:Resource, TaskUninstall %>'>
    <table class="ListContrastTable" style="width: 560px">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDomain" runat="server" Width="80px"><%=Resources.Resource.DomainName%></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tboxDomain" runat="server" Style="width: 190px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLogin" runat="server" Width="80px"><%=Resources.Resource.Login%></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tboxLogin" runat="server" Style="width: 190px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPass" runat="server" Width="80px"><%=Resources.Resource.PasswordLabelText%></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tboxPassword" runat="server" Style="width: 190px" TextMode="Password" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rbtnlProviders" AutoPostBack="false">
                                <asp:ListItem Text='<%$Resources:Resource, WMI %>' Value='0' Selected="True"></asp:ListItem>
                                <asp:ListItem Text='<%$Resources:Resource, RemoteService %>' Value='1'></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>                    
                </table>
            </td>
        </tr>
        <tr>
            <td>            
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbRebootAfterInstall" Checked="true" runat="server" />&nbsp;<%=Resources.Resource.RebootAfterInstall%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>     
    </table>
</div>
