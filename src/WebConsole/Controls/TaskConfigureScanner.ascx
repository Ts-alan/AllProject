<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureScanner.ascx.cs" Inherits="Controls_TaskConfigureScanner" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.TaskNameRunScanner%></div>
<script type="text/javascript" language="javascript">

function CheckedChanged(el)
{
   var lblCountThread = $get('<%=lblCountThread.ClientID%>');
   lblCountThread.disabled = !el.checked;
      
   var ddlCountThread = $get('<%=ddlCountThread.ClientID%>');
   ddlCountThread.disabled = !el.checked;
}

function RemoteCheckedChanged(el) {
    var tboxRemoteAddress = $get('<%=tboxRemoteAddress.ClientID%>');
    tboxRemoteAddress.disabled = !el.checked;
}

</script>
<ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="800px">
<ajaxToolkit:TabPanel runat="server" ID="tabPanel1">
 <ContentTemplate>
    <table class="ListContrastTable">
     <tr>
       <td>
            <%=Resources.Resource.CongScannerCheckObjects %>&nbsp;&nbsp;<asp:TextBox runat="server" ID="tboxCheckObjects" />    
       </td>
       <td>
           <asp:Label runat="server" ID="lblMode" width="150px"><%=Resources.Resource.CongScannerHeuristic %></asp:Label>
           <asp:DropDownList ID="ddlMode" runat="server" />
       </td>
     </tr>
     <tr>
       <td>
            <asp:CheckBox runat="server" ID="cboxCheckMemory" /><%=Resources.Resource.CongScannerCheckMemory%> 
       </td>
       <td>
            <asp:Label runat="server" ID="lblHeuristicAnalysis" width="150px"> <%=Resources.Resource.CongScannerHeuristicAnalysis %></asp:Label>
            <asp:DropDownList runat="server" ID="ddlHeuristicAnalysis" />
       </td>
       
      </tr>
       <tr>
        <td>
            <asp:CheckBox runat="server" ID="cboxScanBootSectors" /><%=Resources.Resource.CongScannerScanBootSectors %>   
       </td>
       <td>
            <%=Resources.Resource.CongScannerExtension %>
       </td>
            </tr>
       <tr>
       <td>
            <asp:CheckBox runat="server" ID="cboxScanStartup" /><%=Resources.Resource.CongScannerScanStartup %>
       </td>
       <td>
            <asp:CheckBox runat="server" ID="cboxSet" />&nbsp;<asp:Label ID="lblSet" runat="server" width="100px" SkinId="LabelContrast"></asp:Label>
           <asp:TextBox ID="tboxSet" runat="server"></asp:TextBox></td>
            </tr>
       <tr>
       <td>
            <asp:CheckBox runat="server" ID="cboxCheckArchives" /><%=Resources.Resource.CongScannerCheckArchives %>  
       </td>
        <td>
            <asp:CheckBox runat="server" ID="cboxAddArch" />&nbsp;<asp:Label ID="lblAddArch"
                runat="server" width="100px" SkinId="LabelContrast"></asp:Label>
            <asp:TextBox ID="tboxAddArch" runat="server"></asp:TextBox></td>
            </tr>
       <tr>
       <td>
            <asp:CheckBox runat="server" ID="cboxCheckMail" /><%=Resources.Resource.CongScannerCheckMail %> 
       </td>
        <td>
            <asp:CheckBox runat="server" ID="cboxExclude" />&nbsp;<asp:Label ID="lblExclude" runat="server" width="100px" SkinId="LabelContrast"></asp:Label>
            <asp:TextBox ID="tboxExclude" runat="server"></asp:TextBox></td>
       </tr>
       <tr>
        <td>
            <asp:CheckBox runat="server" ID="cboxDetectAdware" /><%=Resources.Resource.CongScannerDetectAdware %>  
       </td>
           <td>
               <asp:CheckBox ID="cboxIsArchiveSize" runat="server" /> <%=Resources.Resource.CongScannerArchivesSize %>
               
           </td>
       </tr>
       <tr>
        <td>
            <asp:CheckBox runat="server" ID="cboxScannerSFX" /><%=Resources.Resource.CongScannerSFX %> 
       </td>
       <td>
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="tboxArchiveSize" runat="server"></asp:TextBox>  
       </td>
     </tr>
    </table>
    <table class="ListContrastTable">
     <tr>
       <td>
            <asp:CheckBox runat="server" ID="cboxUpdate" /><%=Resources.Resource.UpdateForRemoteConsoleScanner %> 
       </td>
     </tr>
    </table>
 </ContentTemplate>
</ajaxToolkit:TabPanel>   
<ajaxToolkit:TabPanel runat="server" ID="tabPanel2">
 <ContentTemplate>
    <table class="ListContrastTable">
    <tr>
        <td>
             <%=Resources.Resource.CongScannerInfected %>
        </td>
        <td>
            <%=Resources.Resource.SuspiciousFiles %>
       </td>
       <td>
            <asp:CheckBox runat="server" ID="cboxCureBoot" />&nbsp;<%=Resources.Resource.CongScannerCureBoot %>
       </td>
    </tr>
    <tr>
        <td>
             <asp:CheckBox runat="server" ID="cboxCure" />&nbsp;<%=Resources.Resource.CongScannerCure %>
        </td>
        <td></td>
        <td>
            <asp:CheckBox runat="server" ID="cboxDeleteArchives" />&nbsp;<%=Resources.Resource.CongScannerDeleteArchives %>
       </td>
    </tr>
    <tr>
        <td>
            <asp:RadioButton runat="server" ID="rbSkip" GroupName="rbInfFiles" /><%=Resources.Resource.Skip %>
        </td>
        <td>
            <asp:RadioButton runat="server" ID="rbSkipSusp" GroupName="rbSuspFiles" /><%=Resources.Resource.Skip %>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="cboxDeleteMail" />&nbsp;<%=Resources.Resource.CongScannerDeleteMail%>
        </td>
    </tr>
    <tr>
        <td>
            <asp:RadioButton runat="server" ID="rbDelete" GroupName="rbInfFiles" /><%=Resources.Resource.Delete %>
        </td>
        <td>
            <asp:RadioButton runat="server" ID="rbDeleteSusp" GroupName="rbSuspFiles" /><%=Resources.Resource.Delete%>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="cboxSaveInfectedToQuarantine" />&nbsp;<%=Resources.Resource.CongScannerSaveInfectedToQuarantine %>
       </td>
    </tr>
    <tr>
        <td>
            <asp:RadioButton runat="server" ID="rbRename" GroupName="rbInfFiles" /><%=Resources.Resource.Rename %>
        </td>
        <td>
            <asp:RadioButton runat="server" ID="rbRenameSusp" GroupName="rbSuspFiles" /><%=Resources.Resource.Rename %>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="cboxSaveSusToQuarantine" />&nbsp;<%=Resources.Resource.CongScannerSaveSusToQuarantine %>
        </td>
    </tr>
    <tr>
        <td>
            <asp:RadioButton runat="server" ID="rbRemove" GroupName="rbInfFiles" /><%=Resources.Resource.Remove %>
        </td>
        <td>
            <asp:RadioButton runat="server" ID="rbRemoveSusp" GroupName="rbSuspFiles" /><%=Resources.Resource.Remove %>
        </td>
        <td>
            <%=Resources.Resource.CongScannerRemovePathToInfected %>
        </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td>
            <asp:TextBox runat="server" ID="tboxRemove" style="width:300px"/>
        </td>
    </tr>
    </table>
  </ContentTemplate>
</ajaxToolkit:TabPanel>   
<ajaxToolkit:TabPanel runat="server" ID="tabPanel3">
 <ContentTemplate>   
     <table class="ListContrastTable">
    <tr>
        <td>
            <asp:CheckBox ID="cboxKeep" runat="server" />&nbsp;<%=Resources.Resource.CongScannerKeep %>
            <asp:TextBox ID="tboxKeep" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cboxAdd" runat="server" />&nbsp;<%=Resources.Resource.CongScannerAdd %>
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cboxCleanFiles" runat="server" />&nbsp;<%=Resources.Resource.CongScannerCleanFiles %>
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cboxSaveInfectedToReport" runat="server" />&nbsp;<%=Resources.Resource.CongScannerSaveInfectedToReport %>
            &nbsp;&nbsp;
            <asp:TextBox ID="tboxSaveInfectedToReport" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cboxAddInf" runat="server" />&nbsp;<%=Resources.Resource.CongScannerAdd %>
        </td>
    </tr>
    </table>
   </ContentTemplate>
</ajaxToolkit:TabPanel>   
<ajaxToolkit:TabPanel runat="server" ID="tabPanel4">
 <ContentTemplate>     
     <table class="ListContrastTable"">
    <tr>
        <td>
            <asp:CheckBox ID="cboxEnableCach" runat="server" />&nbsp;<%=Resources.Resource.CongScannerEnableCach %>
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cboxAuthenticode" runat="server" />&nbsp;<%=Resources.Resource.CongScannerAuthenticode%>
        </td>
    </tr>
     <tr>
        <td>
            <asp:CheckBox ID="cboxCheckMacros" runat="server" />&nbsp;<%=Resources.Resource.CongScannerCheckMacros %>
        </td>
    </tr>
     <tr>
        <td>
           <%=Resources.Resource.CongScannerPathToScanner %>&nbsp;&nbsp;<asp:TextBox ID="tboxPathToScanner" runat="server"></asp:TextBox>
        </td>
    </tr>    
    </table>
    <table class="ListContrastTable" style="padding-bottom: 15px;" >
        <tr>
            <td style="padding-top: 5px;padding-bottom:5px;">
                <asp:Label runat="server" ID="lblMultyThreading"><%=Resources.Resource.Multithreading%>:</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="cboxMultyThreading" onclick="CheckedChanged(this)" />
            </td>            
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblCountThread"><%=Resources.Resource.ThreadsCount%>:</asp:Label>
                <asp:DropDownList runat="server" ID="ddlCountThread" SkinID="ddlEdit" style="width:60px; position:absolute; margin-left: 10px;" onmousedown="if(this.options.length>10){this.size=10;}" onchange="this.size=0;" onblur="this.size=0;" ></asp:DropDownList>                
            </td>
        </tr>
    </table>
    <table class="ListContrastTable" style="padding-bottom: 5px;padding-top: 5px;" >        
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="cboxShowProgressScan" />
            </td>            
        </tr>
    </table>
</ContentTemplate>
</ajaxToolkit:TabPanel>
<ajaxToolkit:TabPanel runat="server" ID="tabPanel5">
 <ContentTemplate>     
     <table class="ListContrastTable"">
    <tr>
        <td>
            <asp:CheckBox runat="server" ID="cboxRemoteServer" />&nbsp;<%=Resources.Resource.Server%>
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox runat="server" ID="cboxRemoteClient" onclick="RemoteCheckedChanged(this)" />&nbsp;<%=Resources.Resource.Client%>
        </td>
    </tr>
    <tr>
        <td>
            <table style="margin: 5px 0px 10px 20px;">
                <tr>
                    <td><%=Resources.Resource.CongLdrAddress%></td>
                    <td style="padding-left:10px;">
                        <asp:TextBox runat="server" ID="tboxRemoteAddress" Enabled="false"></asp:TextBox>
                    </td>
                </tr>                
            </table>
        </td>
    </tr>
    </table>
</ContentTemplate>
</ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>