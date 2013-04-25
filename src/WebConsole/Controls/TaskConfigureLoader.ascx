<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureLoader.ascx.cs" Inherits="Controls_TaskConfigureLoader" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.CongLdrConfigureLoader%></div>

<script language="javascript" type="text/javascript" >
   
    //Select
    function AddToSelect(path, sel)
    {
    
       if(path.value == "")
        return;
       
       for(var i=0;i<sel.options.length;i++)
            if(sel.options[i].value == path.value)
            {
                var message = "Value '" +
                  path.value + "' is exist in list";
                alert(message);
                return;
            }

        var newOpt = new Option();
        newOpt.text = path.value;
        newOpt.value = path.value;
        
        path.value = "";
        
        sel.options[sel.options.length] = newOpt;
        
        SaveClickHandler(sel);
    }
    
    function DeleteFromSelect(sel)
    {
        if(sel.selectedIndex  == -1) return;
        
        sel.options[sel.selectedIndex] = null;
        
        SaveClickHandler(sel);
    }
    
    function UpOption(sel)
    {        
        var selectedIndex = sel.selectedIndex;
        if((selectedIndex == 0 )|| (selectedIndex == -1)) return;
              
        var selOpt = new Option();
        selOpt.text = sel.options[selectedIndex].value;
        selOpt.value = sel.options[selectedIndex].value;
        
        var revOpt = new Option();
        revOpt.text = sel.options[selectedIndex-1].value;
        revOpt.value = sel.options[selectedIndex-1].value;
        
        sel.options[selectedIndex] = revOpt;
        sel.options[selectedIndex - 1] = selOpt;
        
        sel.options[selectedIndex - 1].selected = true;
        
        SaveClickHandler(sel);
    }
    
    function DownOption(sel)
    {
        var selectedIndex = sel.selectedIndex;
        if((selectedIndex == (sel.length-1) )|| (selectedIndex == -1)) return;
              
        var selOpt = new Option();
        selOpt.text = sel.options[selectedIndex].value;
        selOpt.value = sel.options[selectedIndex].value;
        
        var revOpt = new Option();
        revOpt.text = sel.options[selectedIndex+1].value;
        revOpt.value = sel.options[selectedIndex+1].value;
        
        sel.options[selectedIndex] = revOpt;
        sel.options[selectedIndex + 1] = selOpt;
        
        sel.options[selectedIndex + 1].selected = true;
        
        SaveClickHandler(sel);
    }
    
    function SaveClickHandler(listBox) 
    { 
        var hdn = $get("<%=hdnBlockItems.ClientID%>");  

        var elements = ""; 
        var intCount = listBox.options.length; 

        for (i = 0; i < intCount; i++) 
        {
            elements += listBox.options[i].text + ';'; 
        } 

        hdn.value = elements; 
    } 
    
   
   function CheckScanMemoryClick()
   {   
      element = $get('<%=rbMode.ClientID%>');
      cbox = $get('<%=cboxScanMemory.ClientID%>');
      hidden = $get('<%=hdnScanMemory.ClientID%>');
      
      CheckClick(cbox, element, hidden);
   }
   
   function CheckScanBootClick()
   {   
      element = $get('<%=cboxScanBootFloppy.ClientID%>');
      cbox = $get('<%=cboxScanBoot.ClientID%>');
      hidden = $get('<%=hdnScanBoot.ClientID%>');
            
      CheckClick(cbox, element, hidden);
   }
   
    function CheckMaximumSizeLog()
    {
        element = $get('<%=tboxMaximumSizeLog.ClientID%>');
        cbox = $get('<%=cboxMaximumSizeLog.ClientID%>');
        hidden = $get('<%=hdnMaximumSizeLog.ClientID%>');
        
        CheckClick(cbox, element, hidden);
        
    }
   
   function CheckClick(cbox, element, hidden)
   {
     
     element.disabled =  !cbox.checked;
     hidden.value = !element.disabled;

   }
   
</script>
<ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Width="560px">
<ajaxToolkit:TabPanel runat="server" ID="tabPanel1">
 <ContentTemplate>
    <table class="ListContrastTable">
        <tr>
            <td>
                <asp:CheckBox ID="cboxLaunchLoaderAtStart" runat="server" /><%=Resources.Resource.CongLdrLaunchLoaderAtStart%>
            </td>
        </tr>
         <tr>
            <td>
                <asp:CheckBox ID="cboxEnableMonitorAtStart" runat="server" /><%=Resources.Resource.CongLdrEnableMonitorAtStart%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cboxProtectProcess" runat="server" /><%=Resources.Resource.CongLdrProtectProcess%>
            </td>
        </tr>
    </table>
    <table class="ListContrastTable">
        <tr>
          <td colspan="2">
            <asp:CheckBox ID="cboxDisplayLoadingProgress" runat="server" /><%=Resources.Resource.CongLdrDisplayLoadingProgress%>
          </td>  
        </tr>
        <tr>
            <td>
               <input type="checkbox"  id="cboxScanMemory"  value="" runat="server"
               onclick="CheckScanMemoryClick()" /> <%=Resources.Resource.CongLdrScanMemory%>
               <asp:HiddenField ID="hdnScanMemory" runat="server" />
            </td>
            <td>
                <input type="checkbox" id="cboxScanBoot" runat="server" onclick="CheckScanBootClick()" /><%=Resources.Resource.CongLdrScanBoot%> 
                <asp:HiddenField ID="hdnScanBoot" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButtonList ID="rbMode" runat="server">
                </asp:RadioButtonList>
            </td>
            <td>
                 &nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" id="cboxScanBootFloppy" runat="server" /><%=Resources.Resource.CongLdrScanBootFloppy%> 
            </td>
        </tr>
        <!--<tr>
            <td colspan="2">
                 <asp:CheckBox ID="cboxScanStartupFiles" runat="server" /><%=Resources.Resource.CongLdrScanStartupFiles%>
            </td>
        </tr>-->
    </table>
  </ContentTemplate>
</ajaxToolkit:TabPanel>  
<ajaxToolkit:TabPanel runat="server" ID="tabPanel2">
 <ContentTemplate>
<div class="divSettings" >
    <table class="ListContrastTable">
        <tr>
            <td>
                <asp:Label runat="server" ID="lblLogFile"><%=Resources.Resource.Name %></asp:Label>
            </td>
             <td>
                 <asp:TextBox ID="tboxLogFile" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="cboxMaximumSizeLog" onclick="CheckMaximumSizeLog()" runat="server" /><%=Resources.Resource.CongLdrMaximumSizeLog%>
            </td>
             <td> 
                 <asp:TextBox ID="tboxMaximumSizeLog"   runat="server"/>
                 <asp:HiddenField ID="hdnMaximumSizeLog" runat="server" />
            </td>
        </tr>
    </table>
    <table class="ListContrastTable">
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cboxSoundWarning" runat="server" /><%=Resources.Resource.CongLdrSoundWarning%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cboxTrayIcon" runat="server" /><%=Resources.Resource.CongLdrTrayIcon%>
            </td>
        </tr>
    </table>
</div>
</ContentTemplate>
</ajaxToolkit:TabPanel>
<ajaxToolkit:TabPanel runat="server" ID="tabPanel3">
 <ContentTemplate>
<div class="divSettings">
    <table class="ListContrastTable">
        <tr>
            <td>
                <asp:CheckBox ID="cboxTimeIntervals" runat="server" /><%=Resources.Resource.CongLdrTimeIntervals%>
                <asp:TextBox ID="tboxTimeIntervals" runat="server"></asp:TextBox>
            </td>
             <td>
             <table>
                <tr>
                    <td>
                        <%=Resources.Resource.CongLdrPath%>&nbsp;<asp:TextBox runat="server" id="tboxPath" style="width:160px" />
                    </td>
                    <td>
                       &nbsp;&nbsp;<a id="lbtnUpdateAdd" onclick="AddToSelect($get('<%=tboxPath.ClientID%>'),$get('<%=lboxUpdateList.ClientID%>'));"><%=Resources.Resource.Add%></a><br/>
                       &nbsp;&nbsp;<a id="lbtnDelete" onclick="DeleteFromSelect($get('<%=lboxUpdateList.ClientID%>'));"  ><%=Resources.Resource.Delete%></a>
                    </td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" >
               &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cboxInteractive" runat="server" /><%=Resources.Resource.CongLdrInteractive%>
            </td>
            <td valign="top">
            <table>
                <tr>
                    <td align="left">
                        <asp:ListBox runat="server" ID="lboxUpdateList" Width="200px" />
                        <asp:HiddenField ID="hdnBlockItems" runat="server" />
                    </td>
                    <td valign="top">
                        &nbsp;&nbsp;<a id="lbtnUpSelect" onclick="UpOption($get('<%=lboxUpdateList.ClientID%>'));"><%=Resources.Resource.Up%></a><br/>
                        &nbsp;&nbsp;<a id="lbtnDownSelect" onclick="DownOption($get('<%=lboxUpdateList.ClientID%>'));"><%=Resources.Resource.Down%></a>
                    </td>
                </tr>
            </table>
            </td>
        </tr>
    </table>
    <table class="ListContrastTable">
        <tr>
            <td colspan="2">    
               <asp:CheckBox ID="cboxUseProxyServer" runat="server" /><%=Resources.Resource.CongLdrUseProxyServer%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblCongLdrAddress" SkinId="LabelContrast" width="100px" ></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="tboxAddress" runat="server"></asp:TextBox>
            </td>
             <td>
                 <asp:Label runat="server" ID="lblCongLdrPort" SkinId="LabelContrast" width="100px"></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="tboxPort" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cboxUseAccount" runat="server" /><%=Resources.Resource.CongLdrUseAccount%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblCongLdrUserName" SkinId="LabelContrast" width="100px"></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="tboxUserName" runat="server"></asp:TextBox>
            </td>
             <td>
                <asp:Label runat="server" ID="lblCongLdrPassword" SkinId="LabelContrast" width="100px"></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="tboxPassword" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
</ContentTemplate>
</ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>