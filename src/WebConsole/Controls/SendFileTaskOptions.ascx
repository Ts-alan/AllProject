<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SendFileTaskOptions.ascx.cs" Inherits="Controls_SendFileTaskOptions" %>
<%@ Register Assembly="FUA" Namespace="Subgurim.Controles" TagPrefix="fua" %>

<script type="text/javascript" >
    function aaa() {
        alert(123);
    };
    function SetTxtValues(sourceText, destText, infoText) {
        alert(sourceText);
        $("#<%=tboxSource.ClientID %>").val(sourceText);
    };
</script>
<div  runat="server" id="tskSendFile" style="display:none;" title='<%$Resources:Resource, SendFile%>' >



<table class="ListContrastTable" style="width:560px">
    <tr>    
        <td>
           <%=Resources.Resource.SelectFile %>:
        </td>
        <td>
            <fua:FileUploaderAJAX ID="fileUploaderAJAX1" runat="server" Visible="true" MaxFiles="1" />
            <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click" Width='80'   OnClientClick="return false;" ><%=Resources.Resource.Upload%></asp:LinkButton>
            <asp:FileUpload ID="fuClient"  runat="server" />
            
         </td>  
        </tr>
        <tr>
            <td>
                <%=Resources.Resource.Information %>
            </td>
            <td>
                <asp:Label ID='lblDetails' runat='server' Text="Select file" />
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
        