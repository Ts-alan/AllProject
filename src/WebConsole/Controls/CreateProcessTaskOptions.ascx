﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreateProcessTaskOptions.ascx.cs"
    Inherits="Controls_CreateProcessTaskOptions" %>
<div id="tskCreateProcess" runat="server" style="visibility: hidden; position: absolute;
    top: 0px;">
    <div class="x-dlg-hd">
        <%=Resources.Resource.CreateProcess %></div>
    <div class="x-dlg-bd">
        <table class="ListContrastTable" style="width: 560px">
            <tr>
                <td>
                    <%=Resources.Resource.CommandLine %>
                </td>
                <td>
                    <asp:TextBox ID="tboxCreateProcess" runat="server" Style="width: 300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tboxCreateProcess"
                        runat="server" ErrorMessage="RequiredFieldValidator" ValidationGroup="TaskValidation" Display="None"></asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender2 PopupPosition="BottomLeft" ID="ValidatorCalloutExtender21"
                        runat="server" TargetControlID="RequiredFieldValidator1" HighlightCssClass="highlight"
                        Width="300"/>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:CheckBox ID="cboxCommand" runat="server" />&nbsp;&nbsp;<%= Resources.Resource.ComSpec %>
                </td>
            </tr>
        </table>
    </div>
</div>
