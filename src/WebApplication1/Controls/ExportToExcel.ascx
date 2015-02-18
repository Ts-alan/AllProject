<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_ExportToExcel" Codebehind="ExportToExcel.ascx.cs" %>
<asp:LinkButton ID="lbtnExcel" runat="server"  
    Text='<%$ Resources:Resource, ExportToExcel %>' onclick="lbtnExcel_Click"></asp:LinkButton>