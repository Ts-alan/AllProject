<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_TaskSendFile" Codebehind="TaskSendFile.ascx.cs" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.SendFile%></div>
<div class="divSettings">
<table class="ListContrastTable" style="width:560px">
    <tr>
    
        <td>
           <%=Resources.Resource.SelectFile %>:
        </td>
        <td>
            <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click" Width=80><%=Resources.Resource.Upload%></asp:LinkButton>
            <asp:FileUpload ID="fuClient" runat="server" />
            
         </td>  
        </tr>
        <tr>
            <td>
                <%=Resources.Resource.Information %>
            </td>
            <td>
                <asp:Label ID="lblDetails" runat="server" Text="Select file" />
            </td>
        </tr>
        <tr>
        <td>
           <%=Resources.Resource.SourceFile %>
        </td>
        <td>
            <asp:TextBox ID="tboxSource" runat="server" style="width:300px" ></asp:TextBox>
        </td>
       </tr>
       <tr>
        <td>
            <%=Resources.Resource.DestinationFile %>
        </td>
        <td>
            <asp:TextBox ID="tboxDestination" runat="server" style="width:300px" ></asp:TextBox>
        </td>
    </tr>
</table>
</div>         
        