<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" Inherits="UserManager" Codebehind="UserManager.aspx.cs" %>

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
                            <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="False" ToolTip="<%$ Resources:Resource, Edit %>" OnClientClick="return false;" />
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
    <asp:LinkButton runat="server" ID="lbtnCreateUserInit" SkinID="Button" OnClientClick="return false;" ><%=Resources.Resource.CreateUserButtonText %></asp:LinkButton>
    <%--Create User Modal--%>
    <div id="panelCreateUser" style="display: none;">
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredUserNameCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredPasswordCalloutExtender" runat="server" PopupPosition="BottomLeft"
                        TargetControlID="requiredPassword" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                    <ajaxToolkit:ValidatorCalloutExtender ID="regexPasswordCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredConfirmPasswordCalloutExtender" PopupPosition="BottomLeft"
                        runat="server" TargetControlID="requiredConfirmPassword" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                    <ajaxToolkit:ValidatorCalloutExtender ID="comparePasswordCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredEmailCalloutExtender" runat="server" PopupPosition="BottomLeft"
                        TargetControlID="requiredEmail" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                    <ajaxToolkit:ValidatorCalloutExtender ID="regexEmailCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredFirstNameCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredLastNameCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
    </div>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var tbUserName = '#' + '<%= tbUserName.ClientID %>';
            var tbPassword = '#' + '<%= tbPassword.ClientID %>';
            var tbConfirmPassword = '#' + '<%= tbConfirmPassword.ClientID %>';
            var tbEmail = '#' + '<%= tbEmail.ClientID %>';
            var tbFirstName = '#' + '<%= tbFirstName.ClientID %>';
            var tbLastName = '#' + '<%= tbLastName.ClientID %>';
            var ddlRole = '#' + '<%= ddlRole.ClientID %>';

            var btnHiddenRefresh = '<%= btnHiddenRefresh.ClientID %>';
            var btnCreateUserInit = '#' + '<%= lbtnCreateUserInit.ClientID %>';
            var divCreateUserResult = '#divCreateUserResult';
            var divCreateUserProgress = '#divCreateUserProgress';

            $("#panelCreateUser").dialog({ autoOpen: false });

            $(document).on("click", btnCreateUserInit, function () {
                $("#panelCreateUser").dialog('destroy');
                var d = $("#panelCreateUser");
                var dOpt = {
                    title: '<%=Resources.Resource.RegistrationNewUser%>',
                    width: 320,
                    modal: true,
                    resizable: false,
                    buttons: [{
                        id: "button-create",
                        text: '<%=Resources.Resource.Create %>',
                        click: function () {
                            if (Page_ClientValidate('CreateUserValidation')) {
                                //Validation is successful
                                disableCreate();

                                var username = $(tbUserName).val();
                                var password = $(tbPassword).val();
                                var email = $(tbEmail).val();
                                var firstname = $(tbFirstName).val();
                                var lastname = $(tbLastName).val();
                                var role = $(ddlRole + " :selected").val();
                                var user = new UserEntity(username, password, email, firstname, lastname, role);
                                PageMethods.CreateUser(user, btnCreateUser_Callback, btnCreateUser_Error);
                            }
                        }
                    }]
                };
                d.dialog(dOpt);
            });

            function disableCreate() {
                $(tbUserName).prop("disabled", true);
                $(tbPassword).prop("disabled", true);
                $(tbConfirmPassword).prop("disabled", true);
                $(tbEmail).prop("disabled", true);
                $(tbFirstName).prop("disabled", true);
                $(tbLastName).prop("disabled", true);
                $(ddlRole).prop("disabled", true);
                $("#button-create").button("disable");
                $(divCreateUserProgress).css('display', 'block');
            }

            function clearCreateUserFields() {
                $(tbUserName).val("");
                $(tbPassword).val("");
                $(tbConfirmPassword).val("");
                $(tbEmail).val("");
                $(tbFirstName).val("");
                $(tbLastName).val("");
                $('select' + ddlRole).prop('selectedIndex', 0);
                $(divCreateUserResult).html("&nbsp");
            }

            function enableCreate() {
                $(tbUserName).prop("disabled", false);
                $(tbPassword).prop("disabled", false);
                $(tbConfirmPassword).prop("disabled", false);
                $(tbEmail).prop("disabled", false);
                $(tbFirstName).prop("disabled", false);
                $(tbLastName).prop("disabled", false);
                $(ddlRole).prop("disabled", false);
                $("#button-create").button("enable");
                $(divCreateUserProgress).css('display', 'none');
            }

            function btnCreateUser_Callback(result) {
                if (result.Success) {
                    $get(btnHiddenRefresh).click();
                    clearCreateUserFields();
                    alert(result.Message);
                }
                else {
                    $(divCreateUserResult).html(result.Message);
                }
                enableCreate();
            }

            function btnCreateUser_Error(errors) {
                $(divCreateUserResult).html(errors.get_Message());
                enableCreate();
            }
        });
    </script>
    <%--Create User Modal ended--%>
    <%--Edit User Modal--%>
    <div id="panelEditUser" style="display: none;">
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredEmailEditCalloutExtender" runat="server" PopupPosition="BottomLeft"
                        TargetControlID="requiredEmailEdit" HighlightCssClass="highlight">
                    </ajaxToolkit:ValidatorCalloutExtender>
                    <ajaxToolkit:ValidatorCalloutExtender ID="regexEmailEditCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredFirstNameEditCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredLastNameEditCalloutExtender" runat="server" PopupPosition="BottomLeft"
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
    </div>
    
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var gridViewUsers = '<%= gridViewUsers.ClientID %>';
            var lblUserNameEditActive = '#' + '<%= lblUserNameEditActive.ClientID %>';
            var tbEmailEdit = '#' + '<%= tbEmailEdit.ClientID %>';
            var tbFirstNameEdit = '#' + '<%= tbFirstNameEdit.ClientID %>';
            var tbLastNameEdit = '#' + '<%= tbLastNameEdit.ClientID %>';
            var ddlRoleEdit = '#' + '<%= ddlRoleEdit.ClientID %>';

            var divLitEditResult = '#divLitEditResult';
            var divEditUserProgress = '#divEditUserProgress';
            var btnHiddenRefresh = '<%= btnHiddenRefresh.ClientID %>';

            $("#panelEditUser").dialog({ autoOpen: false });

            $(document).on("click", "input[type*='image'][id$='_imgEdit']", function () {
                var rowIndex = $(this).attr('rowIndex');
                var disableRole = $(this).attr('disableRole');
                openEditPopup(rowIndex, disableRole == "true");

                $("#panelEditUser").dialog('destroy');
                var d = $("#panelEditUser");
                var dOpt = {
                    title: '<%=Resources.Resource.EditUser%>',
                    width: 320,
                    modal: true,
                    resizable: false,
                    buttons: [{
                        id: "button-edit",
                        text: '<%=Resources.Resource.Edit %>',
                        click: function () {
                            if (Page_ClientValidate('EditUserValidation')) {
                                //Validation is successful
                                var disableRole = $(ddlRoleEdit).prop("disabled");
                                disableEdit();
                                var username = $(lblUserNameEditActive).html();
                                var email = $(tbEmailEdit).val();
                                var firstname = $(tbFirstNameEdit).val();
                                var lastname = $(tbLastNameEdit).val();
                                var role = $(ddlRoleEdit + " :selected").val();
                                var user = new UserEntity(username, "", email, firstname, lastname, role);
                                editRequestUsers.push(user);
                                var objEditUserContext = new EditUserContext(username, disableRole);
                                PageMethods.UpdateUser(user, btnEditUser_Callback, btnEditUser_Error, objEditUserContext);
                            }
                        }
                    }]
                };
                d.dialog(dOpt);
            });

            function disableEdit() {
                $(tbEmailEdit).prop("disabled", true);
                $(tbFirstNameEdit).prop("disabled", true);
                $(tbLastNameEdit).prop("disabled", true);
                $(ddlRoleEdit).prop("disabled", true);
                $("#button-edit").button("disable");
                $(divEditUserProgress).css('display', 'block');
            }

            function enableEdit(disableRole) {
                $(tbEmailEdit).prop("disabled", false);
                $(tbFirstNameEdit).prop("disabled", false);
                $(tbLastNameEdit).prop("disabled", false);
                if (disableRole) {
                    $(ddlRoleEdit).prop("disabled", true);
                }
                else {
                    $(ddlRoleEdit).prop("disabled", false);
                }
                $("#button-edit").button("enable");
                $(divEditUserProgress).css('display', 'none');
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
                if (userContext.username == $(lblUserNameEditActive).html()) {
                    $(divLitEditResult).html(result.Message);
                    enableEdit(userContext.disableRole);
                }
            }

            function btnEditUser_Error(errors, userContext) {
                for (i = 0; i < editRequestUsers.length; i++) {
                    if (userContext.username == editRequestUsers[i].username) {
                        editRequestUsers.splice(i, 1);
                    }
                }
                if (userContext.username == $(lblUserNameEditActive).html()) {
                    $(divLitEditResult).html(errors.get_Message());
                    enableEdit(userContext.disableRole);
                }
            }

            var editRequestUsers = new Array();

            function EditUserContext(username, disableRole) {
                this.username = username;
                this.disableRole = disableRole;
            }

            String.prototype.trim = function () {
                return this.replace(/^\s*/, "").replace(/\s*$/, "");
            }

            //populates edit popup with corresponding user info
            function openEditPopup(rowIndex, disableRole) {
                var row = $get(gridViewUsers).rows[parseInt(rowIndex) + 2];
                var username = row.cells[0].childNodes[0].data;
                $(divLitEditResult).html('&nbsp;');
                $(lblUserNameEditActive).html(username);

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

                if (loadEditRequestUser) {
                    firstname = user.firstname;
                    lastname = user.lastname;
                    email = user.email;
                    i = 0;
                    $(ddlRoleEdit + " option").each(function () {
                        if ($(this).val() == user.role) {
                            roleIndex = i;
                        }
                        i++;
                    });
                }
                else {
                    firstname = row.cells[1].childNodes[0].data;
                    lastname = row.cells[2].childNodes[0].data;
                    email = row.cells[4].childNodes[0].data;
                    var role = row.cells[6].childNodes[0].data.trim();
                    i = 0;
                    $(ddlRoleEdit + " option").each(function () {
                        if ($(this).html() == role) {
                            roleIndex = i;
                        }
                        i++;
                    });
                }
                $(tbEmailEdit).val(email);
                $(tbFirstNameEdit).val(firstname);
                $(tbLastNameEdit).val(lastname);
                $('select' + ddlRoleEdit).prop('selectedIndex', roleIndex);

                if (loadEditRequestUser) {
                    disableEdit();
                }
                else {
                    enableEdit(disableRole);
                }
            }
        });
    </script>
    <%--Edit User Modal ended--%>
</asp:Content>
