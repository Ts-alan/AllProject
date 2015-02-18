<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" Inherits="SettingsMain" Title="Untitled Page" Codebehind="SettingsMain.aspx.cs" %>
<%@ OutputCache Location="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
 <div class="title"><%=Resources.Resource.Settings%></div>
<div class="divSettings">
<div class="subsection"><%=Resources.Resource.PrivateInformation%>/<%=Resources.Resource.Appearance%></div>
        <table class="ListContrastTableMain">
        <tr>
            <td>
                <asp:Label ID="lblUserLogin" runat="server" SkinId="LeftLabel"><%=Resources.Resource.UserLogin %></asp:Label>
                <asp:Label runat="server" SkinId="LabelContrast" ID="lblLogin" Width="154px"></asp:Label>&nbsp;&nbsp;
          </td>
          <tr>
            <td>
                <asp:Label ID="lblFirstName" runat="server" SkinId="LeftLabel"><%=Resources.Resource.FirstName %></asp:Label>
                <asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLastName" runat="server" SkinId="LeftLabel"><%=Resources.Resource.LastName %></asp:Label>
                <asp:TextBox ID="tbLastName" runat="server" ></asp:TextBox> 
            </td>
        </tr>
    <tr>
        <td>
                        <br />
        <asp:Label ID="lblLanguage" runat="server" SkinId="LeftLabel">
            <%=Resources.Resource.Language%>
        </asp:Label>
            <asp:DropDownList ID="ddlLanguage" runat="server" ></asp:DropDownList>
           </td>
    </tr> 
     <tr>
        <td>
            <asp:Label ID="lblMasterPage" runat="server" SkinId="LeftLabel">
                <%=Resources.Resource.MasterPage%>
            </asp:Label>
            <asp:DropDownList ID="ddlMasterPage" runat="server"  ></asp:DropDownList>
        </td> 
    </tr>
    <tr>
        <td>
        <asp:Label ID="lblTheme" runat="server" SkinId="LeftLabel">
            <%=Resources.Resource.Theme%>
        </asp:Label>
        <asp:DropDownList ID="ddlTheme" runat="server"  ></asp:DropDownList>
        </td>
    </tr> 
     <tr>
         <td>
            <br/>
            <asp:LinkButton ID="btnSave" runat="server" SkinID="Button" OnClick="btnSave_Click" >
                <%=Resources.Resource.Save%>
            </asp:LinkButton>
         </td>
     </tr>
</table>  
</div>
<div class="divSettings">
<div class="subsection"><%=Resources.Resource.ChangePasswordTitleText%></div>
        <table class="ListContrastTableMain">
        <tr>
            <td colspan=2>
                <asp:ChangePassword ID="chpwdPassword" runat="server" SuccessPageUrl="~/Default.aspx" CancelButtonType="Link" ChangePasswordButtonType="Link">
                    <TitleTextStyle HorizontalAlign="Left" />
                    <LabelStyle BorderColor="White" ForeColor="Black" HorizontalAlign="Right" />
                    <ChangePasswordTemplate>
                                    <table border="0" cellpadding="0">
                                        <tr>
                                            <td class="LeftLabel">
                                                <asp:Label ID="CurrentPasswordLabel" SkinId="LeftLabel" runat="server" AssociatedControlID="CurrentPassword">
                                                    <%=Resources.Resource.PasswordLabelText%>
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;
                                                <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="chpwdPassword">*</asp:RequiredFieldValidator>
                                                
                                             </td>
                                        </tr>
                                        <tr>
                                            <td class="LeftLabel">
                                                <asp:Label ID="NewPasswordLabel" SkinId="LeftLabel" runat="server" AssociatedControlID="NewPassword">
                                                    <%=Resources.Resource.NewPasswordLabelText%>
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;
                                                <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                                    ErrorMessage="New Password is required." ToolTip="New Password is required."
                                                    ValidationGroup="chpwdPassword">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="LeftLabel">
                                                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword"  Width="200px" SkinId="LeftLabel">
                                                    <%=Resources.Resource.ConfirmNewPasswordLabelText%>
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;
                                                <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                                    ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                                    ValidationGroup="chpwdPassword">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                                    ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                                    ValidationGroup="chpwdPassword"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="color: red">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan=2> 
                                            <br />
                                                <asp:LinkButton ID="ChangePasswordLinkButton" SkinID="Button" runat="server" CommandName="ChangePassword"
                                                    ValidationGroup="chpwdPassword">
                                                    <%=Resources.Resource.ChangePasswordButtonText%>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                    </ChangePasswordTemplate>
                    <InstructionTextStyle BorderColor="White" />
                </asp:ChangePassword>
            </td>
        </tr>
        </table> 
    </div>
</asp:Content>

