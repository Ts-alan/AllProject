<%@ Page Language="C#" validateRequest=false AutoEventWireup="true" CodeFile="Logins.aspx.cs" Inherits="Logins" %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div>
    <table width=100% >
        <tr align="right">
            <td>
                <asp:LinkButton ID="lblRus" runat="server" OnClick="lblRus_Click" Font-Names="Verdana" Font-Size="9pt"></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton ID="lblEng" runat="server" Text="English" OnClick="lblEng_Click" Font-Names="Verdana" Font-Size="9pt"></asp:LinkButton>
            </td>
        </tr>
    </table>
    </div>
    <div align="center" style="margin-top: 10%; width:100%;border: solid 1px #D7D7D7;">
        <asp:Label ID="lblError" runat="server"></asp:Label>
       <asp:Login ID="lgLogin" runat="server" CreateUserUrl="~/Registration.aspx" 
       DestinationPageUrl="~/Default.aspx" OnLoggedIn="lgLogin_LoggedIn"
        DisplayRememberMe="False">
           <TextBoxStyle Width="150px" BorderColor="LightCyan" BorderStyle="Groove" Font-Names="Verdana" />
           <LoginButtonStyle Width="60px" BorderColor="LightCyan" BorderStyle="Groove" Font-Names="Verdana" Font-Size="9pt" />
           <InstructionTextStyle Font-Names="Verdana" Font-Size="9pt" />
           <LabelStyle Font-Names="Verdana" Font-Size="9pt" />
           <TitleTextStyle Font-Names="Verdana" Font-Size="10pt" />
       </asp:Login>
    </div>
    </form>
</body>
</html>
