<%@ Page Language="C#" validateRequest=false MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" Title="Untitled Page" %>
<%@ OutputCache Location="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
     <div class="title"><%=Resources.Resource.RegistrationNewUser%></div>
    <div  class="divSettings">
    <asp:CreateUserWizard Width=100% ID="CreateUserWizard1" runat="server" ContinueDestinationPageUrl="~/Logins.aspx" LoginCreatedUser="False" CreateUserButtonType="Link" OnCreatingUser="CreateUserWizard1_CreatingUser" OnCreatedUser="CreateUserWizard1_CreatedUser">
        <WizardSteps>
            <asp:CreateUserWizardStep runat="server">
                <ContentTemplate>
                    <table class="ListContrastTable" width="100%">
                        <tr>
                            <td align="center" colspan="2">
                                </td>
                        </tr>
                        <tr>
                            <td align="right">
                                </td>
                            <td>
                                <asp:TextBox ID="UserName" runat="server" Width="145px"></asp:TextBox>&nbsp;&nbsp;
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>&nbsp;
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" style="height: 26px">
                                </td>
                            <td style="height: 26px">
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="145px"></asp:TextBox>&nbsp;&nbsp;
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" >Password:</asp:Label>&nbsp;
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" style="height: 26px">
                                </td>
                            <td style="height: 26px">
                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" Width="145px"></asp:TextBox>&nbsp;&nbsp;
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>&nbsp;
                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                    ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                                    ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" style="height: 26px">
                            </td>
                            <td style="height: 26px">
                                <asp:TextBox ID="tboxFirstName" runat="server" Width="145px"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblFirstName"
                                    runat="server" Text="First name" AssociatedControlID="tboxFirstName"></asp:Label><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                        runat="server" ControlToValidate="tboxFirstName" ErrorMessage="First name is required."
                                        ToolTip="First name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" style="height: 26px">
                            </td>
                            <td style="height: 26px">
                                <asp:TextBox ID="tboxLastName" runat="server" Width="145px"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblLastName"
                                    runat="server" Text="Last name" AssociatedControlID="tboxLastName"></asp:Label><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                        runat="server" ControlToValidate="tboxLastName" ErrorMessage="Last name is required."
                                        ToolTip="Last name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right">
                                </td>
                            <td>
                                <asp:TextBox ID="Email" runat="server" Width="145px"></asp:TextBox>&nbsp;&nbsp;
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>&nbsp;
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                    ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator></td>
                        </tr>              
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rbRoles" runat="server">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                    ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                                    ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="color: red">
                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <CustomNavigationTemplate>
                    <table border="0" cellspacing="5" style="width: 100%; height: 100%;">
                        <tr align="right">
                            <td align="left" colspan="0">
                                <asp:LinkButton ID="StepNextButton" runat="server" CommandName="MoveNext" ValidationGroup="CreateUserWizard1"><%= Resources.Resource.Create%></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </CustomNavigationTemplate>
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep runat="server">
                <ContentTemplate>
                    <table border="0">
                        <tr>
                            <td align="center">
                                <%=Resources.Resource.AcountCreate%>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:CompleteWizardStep>
        </WizardSteps>
        <FinishNavigationTemplate>
            <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                Text="Previous" />
            <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="Finish" />
        </FinishNavigationTemplate>
    </asp:CreateUserWizard>
    </div>
</asp:Content>

