﻿<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Controls_CreateProcessTaskOptions" Codebehind="CreateProcessTaskOptions.ascx.cs" %>
<div id="tskCreateProcess" runat="server" style="display:none;" title='<%$Resources:Resource, CreateProcess %>'>
      <table class="ListContrastTable" style="width: 560px" runat="server">
        <tr>
            <td>
                <%=Resources.Resource.CommandLine %>
            </td>
            <td>
                <asp:TextBox ID="tboxCreateProcess" runat="server" Style="width: 300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tboxCreateProcess"
                    runat="server" ErrorMessage="RequiredFieldValidator" ValidationGroup="TaskValidation" Display="None"></asp:RequiredFieldValidator>
                <ajaxToolkit:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="ValidatorCalloutExtender21"
                    runat="server" TargetControlID="RequiredFieldValidator1" HighlightCssClass="highlight" Width="300"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cboxCommand" runat="server" />&nbsp;&nbsp;<%= Resources.Resource.ComSpec %>
            </td>
        </tr>
    </table>
</div>