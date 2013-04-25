<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskUninstall.ascx.cs"
    Inherits="Controls_TaskUninstall" %>
<div class="tasksection" runat="server" id="HeaderName" style="width: 560px">
    <%=Resources.Resource.TaskUninstall%>
</div>
<div class="divSettings">
    <div class="ListContrastTable" style="width: 700px">
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
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:CheckBox ID="cbRebootAfterInstall" Checked="true" runat="server" />&nbsp;<%=Resources.Resource.RebootAfterInstall%>
                </td>
            </tr>
        </table>
    </div>
</div>
