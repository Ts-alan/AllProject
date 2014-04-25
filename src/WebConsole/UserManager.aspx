<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true"
    CodeFile="UserManager.aspx.cs" Inherits="UserManager" %>

<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <div class="title">
        <%=Resources.Resource.UserManaging%></div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
    </ajaxToolkit:ToolkitScriptManager>
    <script type="text/javascript" language="javascript">
        // common
        function UserEntity(username, password, email, firstname, lastname, role) {
            this.username = username;
            this.password = password;
            this.email = email;
            this.firstname = firstname;
            this.lastname = lastname;
            this.role = role;
        }
    </script>
    <%--Users Grid View--%>
    <asp:UpdatePanel ID="updatePanelUsersGrid" runat="server" RenderMode="Inline" UpdateMode="Always">
        <ContentTemplate>
            <asp:ObjectDataSource ID="objectDataSourceUsers" runat="server" SelectMethod="Get"
                TypeName="UsersDataContainer" SelectCountMethod="Count" EnablePaging="True" SortParameterName="sortExpression">
            </asp:ObjectDataSource>
            <asp:GridView ID="gridViewUsers" runat="server" DataSourceID="objectDataSourceUsers"
                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" EnableModelValidation="True"
                CssClass="gridViewStyle" RowStyle-CssClass="gridViewRow" AlternatingRowStyle-CssClass="gridViewRowAlternating"
                HeaderStyle-CssClass="gridViewHeader" OnRowDataBound="gridViewUsers_RowDataBound"
                OnRowCommand="gridViewUsers_RowCommand">
                <AlternatingRowStyle CssClass="gridViewRowAlternating" />
                <Columns>
                    <asp:BoundField DataField="Login" HeaderText="<%$ Resources:Resource, Login %>" SortExpression="Login" />
                    <asp:BoundField DataField="FirstName" HeaderText="<%$ Resources:Resource, FirstName %>"
                        SortExpression="FirstName" />
                    <asp:BoundField DataField="LastName" HeaderText="<%$ Resources:Resource, LastName %>"
                        SortExpression="LastName" />
                    <asp:BoundField DataField="CreationDate" HeaderText="<%$ Resources:Resource, CreationDate %>"
                        SortExpression="CreationDate" />
                    <asp:BoundField DataField="Email" HeaderText="<%$ Resources:Resource, Email %>" SortExpression="Email" />
                    <asp:BoundField DataField="LastLoginDate" HeaderText="<%$ Resources:Resource, LastLoginDate %>"
                        SortExpression="LastLoginDate" />
                    <asp:TemplateField SortExpression="Role" HeaderText="<%$ Resources:Resource, Role %>">
                        <ItemTemplate>
                            <asp:Literal ID="lRole" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Resource, Edit %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="False" CommandName="DeleteX"
                                CommandArgument='<%# Eval("Login") %>' ToolTip="<%$ Resources:Resource, Delete %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridViewHeader" />
                <PagerSettings Visible="true" Position="TopAndBottom" />
                <PagerTemplate>
                    <paging:Paging runat="server" ID="Paging1" />
                </PagerTemplate>
                <RowStyle CssClass="gridViewRow" />
            </asp:GridView>
            <custom:GridViewStorageControl StorageName="Users" ID="gridViewStorageControlUsers" runat="server" StorageType="Application"
                GridViewID="gridViewUsers" />
            <asp:Button ID="btnHiddenRefresh" runat="server" OnClick="btnHiddenRefresh_Click"
                OnInit="btnHiddenRefresh_Init" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Users Grid View ended--%>
    <div class="GiveButton1">
        <asp:LinkButton runat="server" ID="lbtnCreateUserInit" ForeColor="white" Width="100%"><%=Resources.Resource.CreateUserButtonText %></asp:LinkButton>
    </div>
    <%--Create User Modal--%>
    <asp:Panel runat="server" ID="panelCreateUser" Style="display: none;" CssClass="modalPopupUsers">
        <div runat="server" id="divPopupCreateUser" class="modalPopupUsersHeader">
            <div style="float:left;width:350px;">
                <asp:Label ID="lblCreateUserHeader" SkinID="SubSectionLabel" Style="font-size: 14px;
                    font-weight: bold; width: 350px; margin-top: 9px;" runat="server"><%=Resources.Resource.RegistrationNewUser%></asp:Label>
            </div>
            <div style="float:right; cursor:pointer;">
                <asp:ImageButton ID="btnCancelCreateUser" runat="server" />
            </div>
        </div>        
        <%--Create User Input--%>
        <table style="margin: 0 auto;">
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblUserName" runat="server" AssociatedControlID="tbUserName"><%=Resources.Resource.UserLogin %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbUserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredUserName" runat="server" ControlToValidate="tbUserName"
                        ErrorMessage="<%$ Resources:Resource, UserNameRequiredErrorMessage %>" ValidationGroup="CreateUserValidation"
                        Display="None"></asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredUserNameCalloutExtender" runat="server"
                        TargetControlID="requiredUserName" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="tbPassword"><%=Resources.Resource.PasswordLabelText %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredPassword" runat="server" ControlToValidate="tbPassword"
                        ErrorMessage="<%$ Resources:Resource, PasswordRequiredErrorMessage %>" ValidationGroup="CreateUserValidation"
                        Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regexPassword" runat="server" ControlToValidate="tbPassword"
                        Display="None" ErrorMessage="<%$ Resources:Resource, InvalidPasswordErrorMessage %>"
                        ValidationGroup="CreateUserValidation">
                    </asp:RegularExpressionValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredPasswordCalloutExtender" runat="server"
                        TargetControlID="requiredPassword" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                    <ajaxToolkit:ValidatorCalloutExtender ID="regexPasswordCalloutExtender" runat="server"
                        TargetControlID="regexPassword" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="tbConfirmPassword"><%=Resources.Resource.PasswordLabelText%>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredConfirmPassword" runat="server" ControlToValidate="tbConfirmPassword"
                        ErrorMessage="<%$ Resources:Resource, ConfirmPasswordRequiredErrorMessage %>"
                        ValidationGroup="CreateUserValidation" Display="None"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comparePassword" runat="server" ControlToCompare="tbPassword"
                        ControlToValidate="tbConfirmPassword" Display="None" ErrorMessage="<%$ Resources:Resource, ConfirmPasswordCompareErrorMessage %>"
                        ValidationGroup="CreateUserValidation"></asp:CompareValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredConfirmPasswordCalloutExtender"
                        runat="server" TargetControlID="requiredConfirmPassword" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                    <ajaxToolkit:ValidatorCalloutExtender ID="comparePasswordCalloutExtender" runat="server"
                        TargetControlID="comparePassword" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblEmail" runat="server" AssociatedControlID="tbEmail"><%=Resources.Resource.Email %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredEmail" runat="server" ControlToValidate="tbEmail"
                        ErrorMessage="<%$ Resources:Resource, EmailRequiredErrorMessage %>" ValidationGroup="CreateUserValidation"
                        Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regexEmail" runat="server" ControlToValidate="tbEmail"
                        Display="None" ErrorMessage="<%$ Resources:Resource, ErrorInvalidEmail  %>" ValidationGroup="CreateUserValidation">
                    </asp:RegularExpressionValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredEmailCalloutExtender" runat="server"
                        TargetControlID="requiredEmail" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                    <ajaxToolkit:ValidatorCalloutExtender ID="regexEmailCalloutExtender" runat="server"
                        TargetControlID="regexEmail" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="tbFirstname"><%=Resources.Resource.FirstName %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredFirstName" runat="server" ControlToValidate="tbFirstName"
                        ErrorMessage="<%$ Resources:Resource, FirstNameRequiredErrorMessage %>" ValidationGroup="CreateUserValidation"
                        Display="None"></asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredFirstNameCalloutExtender" runat="server"
                        TargetControlID="requiredFirstName" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblLastName" runat="server" AssociatedControlID="tbLastName"><%=Resources.Resource.LastName %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbLastName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredLastName" runat="server" ControlToValidate="tbLastName"
                        ErrorMessage="<%$ Resources:Resource, LastNameRequiredErrorMessage %>" ValidationGroup="CreateUserValidation"
                        Display="None"></asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredLastNameCalloutExtender" runat="server"
                        TargetControlID="requiredLastName" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblRole" runat="server" AssociatedControlID="ddlRole"><%=Resources.Resource.Role %>:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRole" runat="server">
                        <asp:ListItem Text="<%$ Resources:Resource, Viewer %>" Value="Viewer"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, Operator %>" Value="Operator"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, Administrator %>" Value="Administrator"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <%--Create User Input ended--%>
        <div style="width: 100%; height: 50px;">
            <div id="divCreateUserProgress" style="width: 100%; height: 100%; display: none">
                <table style="margin: 0 auto;">
                    <tr>
                        <td>
                            <asp:Image ID="imgProcessingCreate" AlternateText="Processing" runat="server" OnInit="imgProcessing_Init" />                        
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divCreateUserResult" style="background-color: #dcdcdc; width: 99%; height: 150px; border-width: 1px; border-color: Black; border-style:solid;">
        </div>
        
        <div class="GiveButton1" style="margin: 0 auto; margin-bottom: 5px; margin-top: 5px;">
            <asp:LinkButton runat="server" ID="btnCreateUser" ForeColor="white" Width="100%" 
            OnClientClick="setTimeout(btnCreateUser_ClientClick, 0); return false;">
            <%=Resources.Resource.Create %></asp:LinkButton>
        </div>        
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="modalPopupExtCreateUser" runat="server" TargetControlID="lbtnCreateUserInit"
        CancelControlID="btnCancelCreateUser" PopupControlID="panelCreateUser" BackgroundCssClass="modalBackgroundBottom" />
    <script type="text/javascript" language="javascript">
        var tbUserName = '<%= tbUserName.ClientID %>';
        var tbPassword = '<%= tbPassword.ClientID %>';
        var tbConfirmPassword = '<%= tbConfirmPassword.ClientID %>';
        var tbEmail = '<%= tbEmail.ClientID %>';
        var tbFirstName = '<%= tbFirstName.ClientID %>';
        var tbLastName = '<%= tbLastName.ClientID %>';
        var ddlRole = '<%= ddlRole.ClientID %>';
        var modalPopupExtCreateUser = '<%= modalPopupExtCreateUser.ClientID %>';
        var btnHiddenRefresh = '<%= btnHiddenRefresh.ClientID %>';
        var btnCreateUser = '<%= btnCreateUser.ClientID %>';

        function clearCreateUserFields() {
            $get(tbUserName).value = "";
            $get(tbPassword).value = "";
            $get(tbConfirmPassword).value = "";
            $get(tbEmail).value = "";
            $get(tbFirstName).value = "";
            $get(tbLastName).value = "";
            $get(ddlRole).selectedIndex = 0;
            $get('divCreateUserResult').innerHTML = "&nbsp";
        }

        Sys.Application.add_load(hideCreate);

        function hideCreate(sender, args) {
            $addHandler(document, "keydown", function (args) {
                if (args.keyCode == Sys.UI.Key.esc) {
                    $find(modalPopupExtCreateUser).hide();
                }
            });
        }

        function btnCreateUser_ClientClick() {
            if (Page_ClientValidate('CreateUserValidation')) {
                //Validation is successful
                disableCreate();
                var username = $get(tbUserName).value;
                var password = $get(tbPassword).value;
                var email = $get(tbEmail).value;
                var firstname = $get(tbFirstName).value;
                var lastname = $get(tbLastName).value;
                var role = $get(ddlRole).options[$get(ddlRole).selectedIndex].value;
                var user = new UserEntity(username, password, email, firstname, lastname, role);
                PageMethods.CreateUser(user, btnCreateUser_Callback, btnCreateUser_Error);
            }
        }

        function enableCreate() {
            $get(tbUserName).disabled = false;
            $get(tbPassword).disabled = false;
            $get(tbConfirmPassword).disabled = false;
            $get(tbEmail).disabled = false;
            $get(tbFirstName).disabled = false;
            $get(tbLastName).disabled = false;
            $get(ddlRole).disabled = false;
            $get(btnCreateUser).disabled = false;
            $get('divCreateUserProgress').style.display = 'none';
        }

        function disableCreate() {
            $get(tbUserName).disabled = true;
            $get(tbPassword).disabled = true;
            $get(tbConfirmPassword).disabled = true;
            $get(tbEmail).disabled = true;
            $get(tbFirstName).disabled = true;
            $get(tbLastName).disabled = true;
            $get(ddlRole).disabled = true;
            $get(btnCreateUser).disabled = true;
            $get('divCreateUserProgress').style.display = 'block';
        }

        function btnCreateUser_Callback(result) {
            if (result.Success) {
                $get(btnHiddenRefresh).click();
                clearCreateUserFields();
                alert(result.Message);
            }
            else {
                $get('divCreateUserResult').innerHTML = result.Message;
            }
            enableCreate();
        }

        function btnCreateUser_Error(errors) {
            $get('divCreateUserResult').innerHTML = errors.get_Message();
            enableCreate();
        }
    </script>
    <%--Create User Modal ended--%>
    <%--Edit User Modal--%>
    <asp:Panel runat="server" ID="panelEditUser" Style="display: none;" CssClass="modalPopupUsers">
        <div runat="server" id="divPopupEditUser" class="modalPopupUsersHeader">
            <div style="float:left;width:350px;">
                <asp:Label ID="lblEditUserHeader" SkinID="SubSectionLabel" Style="font-size: 14px;
                    font-weight: bold; width: 350px; margin-top: 9px;" runat="server"><%=Resources.Resource.EditUser%></asp:Label>
            </div>
            <div style="float:right; cursor:pointer;">
                <asp:ImageButton ID="btnCancelEditUser" runat="server" />
            </div>
        </div> 
        
        <%--Edit User Input--%>
        <table style="margin: 0 auto;">
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblUserNameEdit" runat="server"><%=Resources.Resource.UserLogin %>:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblUserNameEditActive" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblEmailEdit" runat="server" AssociatedControlID="tbEmailEdit"><%=Resources.Resource.Email %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbEmailEdit" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredEmailEdit" runat="server" ControlToValidate="tbEmailEdit"
                        ErrorMessage="<%$ Resources:Resource, EmailRequiredErrorMessage %>" ValidationGroup="EditUserValidation"
                        Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regexEmailEdit" runat="server" ControlToValidate="tbEmailEdit"
                        Display="None" ErrorMessage="<%$ Resources:Resource, ErrorInvalidEmail  %>" ValidationGroup="EditUserValidation">
                    </asp:RegularExpressionValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredEmailEditCalloutExtender" runat="server"
                        TargetControlID="requiredEmailEdit" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                    <ajaxToolkit:ValidatorCalloutExtender ID="regexEmailEditCalloutExtender" runat="server"
                        TargetControlID="regexEmailEdit" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblFirstNameEdit" runat="server" AssociatedControlID="tbFirstnameEdit"><%=Resources.Resource.FirstName %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbFirstNameEdit" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredFirstNameEdit" runat="server" ControlToValidate="tbFirstNameEdit"
                        ErrorMessage="<%$ Resources:Resource, FirstNameRequiredErrorMessage %>" ValidationGroup="EditUserValidation"
                        Display="None"></asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredFirstNameEditCalloutExtender" runat="server"
                        TargetControlID="requiredFirstNameEdit" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblLastNameEdit" runat="server" AssociatedControlID="tbLastNameEdit"><%=Resources.Resource.LastName %>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbLastNameEdit" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="requiredLastNameEdit" runat="server" ControlToValidate="tbLastNameEdit"
                        ErrorMessage="<%$ Resources:Resource, LastNameRequiredErrorMessage %>" ValidationGroup="EditUserValidation"
                        Display="None"></asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredLastNameEditCalloutExtender" runat="server"
                        TargetControlID="requiredLastNameEdit" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 15px;">
                    <asp:Label ID="lblRoleEdit" runat="server" AssociatedControlID="ddlRoleEdit"><%=Resources.Resource.Role %>:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRoleEdit" runat="server">
                        <asp:ListItem Text="<%$ Resources:Resource, Viewer %>" Value="Viewer"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, Operator %>" Value="Operator"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, Administrator %>" Value="Administrator"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <%--Edit User Input ended--%>
        <div style="width: 100%; height: 50px;">
            <div id="divEditUserProgress" style="width: 100%; height: 100%; display: none">
                <table style="margin: 0 auto;">
                    <tr>
                        <td>
                            <asp:Image ID="imgProcessingEdit" AlternateText="Processing" runat="server" OnInit="imgProcessing_Init" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divLitEditResult" style="background-color: #dcdcdc; width: 99%; height: 150px; border-width: 1px; border-color: Black; border-style:solid;">
        </div>
        
        <div class="GiveButton1" style="margin: 0 auto; margin-bottom: 5px; margin-top: 5px;">
            <asp:LinkButton runat="server" ID="btnEditUser" ForeColor="white" Width="100%" ValidationGroup="EditUserValidation" 
            OnClientClick="setTimeout(btnEditUser_ClientClick, 0); return false;">
            <%=Resources.Resource.Edit %></asp:LinkButton>
        </div>
        
    </asp:Panel>
    <asp:Button ID="btnShowEditPopup" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="modalPopupExtEditUser" runat="server" CancelControlID="btnCancelEditUser"
        TargetControlID="btnShowEditPopup" PopupControlID="panelEditUser" BackgroundCssClass="modalBackgroundBottom" />
    <script type="text/javascript" language="javascript">
        var modalPopupExtEditUser = '<%= modalPopupExtEditUser.ClientID %>';
        var gridViewUsers = '<%= gridViewUsers.ClientID %>';
        var lblUserNameEditActive = '<%= lblUserNameEditActive.ClientID %>';
        var tbEmailEdit = '<%= tbEmailEdit.ClientID %>';
        var tbFirstNameEdit = '<%= tbFirstNameEdit.ClientID %>';
        var tbLastNameEdit = '<%= tbLastNameEdit.ClientID %>';
        var ddlRoleEdit = '<%= ddlRoleEdit.ClientID %>';
        var btnEditUser = '<%= btnEditUser.ClientID %>';

        String.prototype.trim = function () {
            return this.replace(/^\s*/, "").replace(/\s*$/, "");
        }

        //populates edit popup with corresponding user info
        function openEditPopup(rowIndex, disableRole) {
            var row = $get(gridViewUsers).rows[parseInt(rowIndex) + 2];
            var username = row.cells[0].childNodes[0].data;
            $get('divLitEditResult').innerHTML = '&nbsp;';
            $get(lblUserNameEditActive).innerHTML = username;

            var loadEditRequestUser = false;
            var user = null;
            for (i = 0; i < editRequestUsers.length; i++) {
                if (editRequestUsers[i].username == username) {
                    loadEditRequestUser = true;
                    user = editRequestUsers[i];
                    break;
                }
            }

            var firstname;
            var lastname;
            var email;
            var roleIndex;

            var options = $get(ddlRoleEdit).options;
            if (loadEditRequestUser) {
                firstname = user.firstname;
                lastname = user.lastname;
                email = user.email;
                for (i = 0; i < options.length; i++) {
                    if (options[i].value == user.role) {
                        roleIndex = i;
                        break;
                    }
                }
            }
            else {
                firstname = row.cells[1].childNodes[0].data;
                lastname = row.cells[2].childNodes[0].data;
                email = row.cells[4].childNodes[0].data;
                var role = row.cells[6].childNodes[0].data.trim();                
                for (i = 0; i < options.length; i++) {
                    if (options[i].innerHTML == role) {
                        roleIndex = i;
                        break;
                    }
                }
            }

            $get(tbEmailEdit).value = email;
            $get(tbFirstNameEdit).value = firstname;
            $get(tbLastNameEdit).value = lastname;
            $get(ddlRoleEdit).selectedIndex = roleIndex;

            if (loadEditRequestUser) {
                disableEdit();
            }
            else {
                enableEdit(disableRole);
            }
            Page_ClientValidate('EditUserValidation');
            $find(modalPopupExtEditUser).show();
        }

        Sys.Application.add_load(hideEdit);

        function hideEdit(sender, args) {
            $addHandler(document, "keydown", function (args) {
                if (args.keyCode == Sys.UI.Key.esc) {
                    $find(modalPopupExtEditUser).hide();
                }
            });
        }

        var editRequestUsers = new Array();

        function EditUserContext(username, disableRole) {
            this.username = username;
            this.disableRole = disableRole;
        }

        function disableEdit() {
            $get(tbEmailEdit).disabled = true;
            $get(tbFirstNameEdit).disabled = true;
            $get(tbLastNameEdit).disabled = true;
            $get(ddlRoleEdit).disabled = true;
            $get(btnEditUser).disabled = true;
            $get('divEditUserProgress').style.display = 'block';
        }

        function enableEdit(disableRole) {
            $get(tbEmailEdit).disabled = false;
            $get(tbFirstNameEdit).disabled = false;
            $get(tbLastNameEdit).disabled = false;
            if (disableRole) {
                $get(ddlRoleEdit).disabled = true;
            }
            else {
                $get(ddlRoleEdit).disabled = false;
            }
            $get(btnEditUser).disabled = false;
            $get('divEditUserProgress').style.display = 'none';
        }


        function btnEditUser_ClientClick() {
            if (Page_ClientValidate('EditUserValidation')) {
                //Validation is successful
                var disableRole = $get(ddlRoleEdit).disabled;
                disableEdit();
                var username = $get(lblUserNameEditActive).innerHTML;
                var email = $get(tbEmailEdit).value;
                var firstname = $get(tbFirstNameEdit).value;
                var lastname = $get(tbLastNameEdit).value;
                var role = $get(ddlRoleEdit).options[$get(ddlRoleEdit).selectedIndex].value;
                var user = new UserEntity(username, "", email, firstname, lastname, role);
                editRequestUsers.push(user);
                var objEditUserContext = new EditUserContext(username,
                   disableRole);
                PageMethods.UpdateUser(user, btnEditUser_Callback, btnEditUser_Error, objEditUserContext);
                
            }
        }

        function btnEditUser_Callback(result, userContext) {
            if (result.Success) {
                $get(btnHiddenRefresh).click();
            }
            for (i = 0; i < editRequestUsers.length; i++) {
                if (userContext.username == editRequestUsers[i].username) {
                    editRequestUsers.splice(i, 1);
                }
            }
            if (userContext.username == $get(lblUserNameEditActive).innerHTML) {
                $get('divLitEditResult').innerHTML = result.Message;
                enableEdit(userContext.disableRole);
            }
        }

        function btnEditUser_Error(errors, userContext) {            
            for (i = 0; i < editRequestUsers.length; i++) {
                if (userContext.username == editRequestUsers[i].username) {
                    editRequestUsers.splice(i, 1);
                }
            }
            if (userContext.username == $get(lblUserNameEditActive).innerHTML) {
                $get('divLitEditResult').innerHTML = errors.get_Message();
                enableEdit(userContext.disableRole);
            }
        }
    </script>
    <%--Edit User Modal ended--%>
</asp:Content>
