<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskRunScanner.ascx.cs" Inherits="Controls_TaskRunScanner" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.TaskNameRunScanner%></div>
<script type="text/javascript" language="javascript">
    function pageLoad() {
        $("#Tabs").tabs({ cookie: { expires: 30} });
       
    }
    function CheckedChanged(el)
    {
       var lblCountThread = $get('<%=lblCountThread.ClientID%>');
       lblCountThread.disabled = !el.checked;
      
       var ddlCountThread = $get('<%=ddlCountThread.ClientID%>');
       ddlCountThread.disabled = !el.checked;
    }

</script>
    <div id="Tabs" style="width:800px">
        <ul>
            <li><a href="#tab1"><%=Resources.Resource.Actions%></a> </li>
            <li><a href="#tab2"><%=Resources.Resource.CongMonitorObjects%></a> </li>
            <li><a href="#tab3"><%=Resources.Resource.CongMonitorReport%></a> </li>
            <li><a href="#tab4"><%=Resources.Resource.CongScannerAdditional%></a> </li>
        </ul>
        <div id="tab1" class="divSettings">
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
                        <asp:TextBox ID="tboxAddArch" runat="server"></asp:TextBox>
                    </td>
                </tr>
               <tr>
                   <td>
                        <asp:CheckBox runat="server" ID="cboxCheckMail" /><%=Resources.Resource.CongScannerCheckMail %> 
                   </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxExclude" />&nbsp;<asp:Label ID="lblExclude" runat="server" width="100px" SkinId="LabelContrast"></asp:Label>
                        <asp:TextBox ID="tboxExclude" runat="server"></asp:TextBox>
                    </td>
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
               <tr></tr>
            </table>
            <table class="ListContrastTable">
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxUpdate" /><%=Resources.Resource.UpdateForRemoteConsoleScanner %> 
                    </td>
                </tr>
            </table>
        </div>
        <div id="tab2" class="divSettings">
            <table class="ListContrastTable">
                <tr>
                    <td> <%=Resources.Resource.CongScannerInfected %> </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxCure" />&nbsp;<%=Resources.Resource.CongScannerCure %>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxCureBoot" />&nbsp;<%=Resources.Resource.CongScannerCureBoot %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton runat="server" ID="rbSkip" GroupName="rbInfFiles" /><%=Resources.Resource.Skip %>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxDeleteArchives" />&nbsp;<%=Resources.Resource.CongScannerDeleteArchives %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton runat="server" ID="rbDelete" GroupName="rbInfFiles" /><%=Resources.Resource.Delete %>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxDeleteMail" />&nbsp;<%=Resources.Resource.CongScannerDeleteMail%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton runat="server" ID="rbRename" GroupName="rbInfFiles" /><%=Resources.Resource.Rename %>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxSaveInfectedToQuarantine" />&nbsp;<%=Resources.Resource.CongScannerSaveInfectedToQuarantine %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton runat="server" ID="rbRemove" GroupName="rbInfFiles" /><%=Resources.Resource.Remove %>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxSaveSusToQuarantine" />&nbsp;<%=Resources.Resource.CongScannerSaveSusToQuarantine %>
                    </td>
                </tr>
                <tr>
                    <td>
        
                    </td>
                    <td> <%=Resources.Resource.CongScannerRemovePathToInfected %></td>
                </tr>
                <tr>
                    <td>
        
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tboxRemove" style="width:300px"/>
                    </td>
                </tr>
        </table>
      </div>
        <div id="tab3" class="divSettings">
            <table class="ListContrastTable" >
                <tr>
                    <td>
                        <asp:CheckBox ID="cboxKeep" runat="server" />&nbsp;<%=Resources.Resource.CongScannerKeep %>
                        <asp:TextBox ID="tboxKeep" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="cboxAdd" runat="server" />&nbsp;<%=Resources.Resource.CongScannerAdd %>
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
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="cboxAddInf" runat="server" />&nbsp;<%=Resources.Resource.CongScannerAdd %>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tab4" class="divSettings">
            <table class="ListContrastTable"">
                <tr>
                    <td>
                        <asp:CheckBox ID="cboxEnableCach" runat="server" />&nbsp;<%=Resources.Resource.CongScannerEnableCach %>
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
                <tr><td>&nbsp;</td></tr>
            </table>            
            <table class="ListContrastTable" style="padding-bottom: 10px;" >
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
                <tr><td>&nbsp;</td></tr>
            </table>
                     
            <table class="ListContrastTable" style="padding-bottom: 5px;padding-top: 5px;" >        
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="cboxShowProgressScan" />
                    </td>            
               </tr>
            </table>
        </div>
    </div>