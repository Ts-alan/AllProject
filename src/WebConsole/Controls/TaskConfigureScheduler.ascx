<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureScheduler.ascx.cs" Inherits="Controls_TaskConfigureScheduler" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:670px"><%=Resources.Resource.ConfigureScheduler%></div>
<script language="javascript" type="text/javascript">

function ChangeExpertAnaliz() {
    var ddl3 = $get('<%=ddlExpertAnaliz.ClientID%>');
    var cbox1 = $get('<%=cboxSuspectedToQtn.ClientID%>');
    var cbox2 = $get('<%=cboxSuspectedDelete.ClientID%>');

    if (ddl3.selectedIndex == 0)
    {
        cbox1.disabled = true;
        cbox2.disabled = true;
    }
    else {
        cbox1.disabled = false;
        cbox2.disabled = false;
    }
}

function NextClick() {
    if (!ValidateTime()) {
        return false;
    }
    var step = $get('<%=hdnStep.ClientID%>');    
    var rbtnProc = $get('<%=rbtnProcess.ClientID%>');
    var rbtnScan = $get('<%=rbtnScan.ClientID%>');
    var rbtnUpdate = $get('<%=rbtnUpdate.ClientID%>');
    
    var btnPrev = $get('<%=btnPrev.ClientID%>');
    var btn = $get('<%=btnAddRule.ClientID%>');
    var tboxName = $get('<%=tboxTaskName.ClientID%>');
    
    var tboxPath = $get('<%=tboxPath.ClientID%>');  
      
    switch(step.value)
    {
        case "0":
            if(tboxName.value == "")
            {
                alert('<%=Resources.Resource.NoCorrectName%>');
                return false;
            }
            if(rbtnProc.checked) step.value = 1;
            else if(rbtnScan.checked) step.value = 2;
                else step.value = 3;
            btnPrev.style["display"] = "";    
        break;
        case "1":
            if(tboxPath.value == "")
            {
                alert('<%=Resources.Resource.NoCorrectPath%>');
                return false;
            }
            step.value = 3;
        break;
        case "2":
            step.value = 3;
        break;
        case "3":
            var rbtn1 = $get('<%=RadioButton1.ClientID%>');
            var rbtn2 = $get('<%=RadioButton2.ClientID%>');
            var rbtn3 = $get('<%=RadioButton3.ClientID%>');
            var rbtn4 = $get('<%=RadioButton4.ClientID%>');
            var rbtn5 = $get('<%=RadioButton5.ClientID%>');
            var rbtn6 = $get('<%=RadioButton6.ClientID%>');
            var arr = [rbtn1, rbtn2, rbtn3, rbtn4, rbtn5, rbtn6 ];
            
            for(var i = 0; i < 6; i++)
            {
                if(arr[i].checked) step.value = 4 + i;
            }
            
        break;
        default:
            step.value = 0;
        break;      
    }
    
      
    var tbl0 = $get('<%=tblMain.ClientID%>');
    var tbl1 = $get('<%=tblProcess.ClientID%>');
    var tbl2 = $get('<%=tblScan.ClientID%>');
    var tbl3 = $get('<%=tblPeriodicity.ClientID%>');
    var tbl4_1 = $get('<%=tblTime1.ClientID%>');
    var tbl4_2 = $get('<%=tblTime2.ClientID%>');
    var tbl4_3 = $get('<%=tblTime3.ClientID%>');
    var tbl4_4 = $get('<%=tblTime4.ClientID%>');
    var tbl4_5 = $get('<%=tblTime5.ClientID%>');
    var tbl4_6 = $get('<%=tblTime6.ClientID%>');
    
    tbl0.style["display"] = "none"; 
    tbl1.style["display"] = "none";
    tbl2.style["display"] = "none";
    tbl3.style["display"] = "none";
    tbl4_1.style["display"] = "none";
    tbl4_2.style["display"] = "none";
    tbl4_3.style["display"] = "none";
    tbl4_4.style["display"] = "none";
    tbl4_5.style["display"] = "none";
    tbl4_6.style["display"] = "none";
    
    switch(step.value)
    {
        case "0":
            tbl0.style["display"] = "";
            btnPrev.style["display"] = "none";
            btn.value = "<%=Resources.Resource.Next%>";
            ClearStep();
            
            return true;
        break;
        case "1":
            tbl1.style["display"] = "";
        break;
        case "2":
            tbl2.style["display"] = "";
        break;
        case "3":
            tbl3.style["display"] = "";
        break;
        case "4":
            tbl4_1.style["display"] = "";
            btn.value = "<%=Resources.Resource.Save%>";            
        break;
        case "5":
            tbl4_2.style["display"] = "";
            btn.value = "<%=Resources.Resource.Save%>";            
        break;
        case "6":
            tbl4_3.style["display"] = "";
            btn.value = "<%=Resources.Resource.Save%>";            
        break;
        case "7":
            tbl4_4.style["display"] = "";
            btn.value = "<%=Resources.Resource.Save%>";            
        break;
        case "8":
            tbl4_5.style["display"] = "";
            btn.value = "<%=Resources.Resource.Save%>";            
        break;
        case "9":
            tbl4_6.style["display"] = "";
            btn.value = "<%=Resources.Resource.Save%>";            
        break;
    }
        
    return false;
}

function PreviousClick()
{
    var step = $get('<%=hdnStep.ClientID%>');    
    var rbtnProc = $get('<%=rbtnProcess.ClientID%>');
    var rbtnScan = $get('<%=rbtnScan.ClientID%>');
    var rbtnUpdate = $get('<%=rbtnUpdate.ClientID%>');
    
    var btnPrev = $get('<%=btnPrev.ClientID%>');
    var btn = $get('<%=btnAddRule.ClientID%>');
      
    switch(step.value)
    {
        case "0":               
        break;
        case "1":
            step.value = 0;
        break;
        case "2":
            step.value = 0;
        break;
        case "3":            
            if(rbtnProc.checked) step.value = 1;
            else if(rbtnScan.checked) step.value = 2;
                else step.value = 0;
        break;
        default:
            step.value = 3;            
        break;        
    }
    
      
    var tbl0 = $get('<%=tblMain.ClientID%>');
    var tbl1 = $get('<%=tblProcess.ClientID%>');
    var tbl2 = $get('<%=tblScan.ClientID%>');
    var tbl3 = $get('<%=tblPeriodicity.ClientID%>');
    var tbl4_1 = $get('<%=tblTime1.ClientID%>');
    var tbl4_2 = $get('<%=tblTime2.ClientID%>');
    var tbl4_3 = $get('<%=tblTime3.ClientID%>');
    var tbl4_4 = $get('<%=tblTime4.ClientID%>');
    var tbl4_5 = $get('<%=tblTime5.ClientID%>');
    var tbl4_6 = $get('<%=tblTime6.ClientID%>');            
    
    tbl0.style["display"] = "none"; 
    tbl1.style["display"] = "none";
    tbl2.style["display"] = "none";
    tbl3.style["display"] = "none";
    tbl4_1.style["display"] = "none";
    tbl4_2.style["display"] = "none";
    tbl4_3.style["display"] = "none";
    tbl4_4.style["display"] = "none";
    tbl4_5.style["display"] = "none";
    tbl4_6.style["display"] = "none";
    
    switch(step.value)
    {
        case "0":
            tbl0.style["display"] = "";
            btnPrev.style["display"] = "none";            
        break;
        case "1":
            tbl1.style["display"] = "";
        break;
        case "2":
            tbl2.style["display"] = "";
        break;
        case "3":
            tbl3.style["display"] = "";
            btn.value = "<%=Resources.Resource.Next%>";
        break;
    }
    
    return false;
}

function ClearStep() {
    var scanPath = $get('<%=hdnPath.ClientID%>');    
    scanPath.value = "";    
    var ddl = $get('<%=ddlScanPath.ClientID%>');
    
    for(var i = 0; i < ddl.options.length; i++)
    {
        scanPath.value += ddl.options[i].value + ';';
    }
    
    ddl.options.length = 0;
}

function ValidateTime() {
    var tbl1 = $get('<%=tblTime1.ClientID%>');
    var tbl2 = $get('<%=tblTime2.ClientID%>');
    var tbl3 = $get('<%=tblTime3.ClientID%>');
    var tbl4 = $get('<%=tblTime4.ClientID%>');
    var tbl5 = $get('<%=tblTime5.ClientID%>');
    var tbl6 = $get('<%=tblTime6.ClientID%>');

    var result = true;
    if ($(tbl1).is(":visible")) {
        var tboxMinutes = $get('<%=tboxMinutes.ClientID%>');
        if (tboxMinutes.value == "") {
            alert('<%=Resources.Resource.MinutesRequired%>');
            result = false;
        }
        else if (!IsPositiveInt(tboxMinutes.value))
        {
            alert('<%=Resources.Resource.MinutesIncorrect%>');
            result = false;
        }
    }
    else if ($(tbl2).is(":visible")) {
        var tboxHours = $get('<%=tboxHours.ClientID%>');
        if (tboxHours.value == "") {
            alert('<%=Resources.Resource.HoursRequired%>');
            result = false;
        }
        else if (!IsPositiveInt(tboxHours.value)) {
            alert('<%=Resources.Resource.HoursIncorrect%>');
            result = false;
        }
    }
    else if ($(tbl3).is(":visible")) {
        var tboxDays = $get('<%=tboxDays.ClientID%>');
        var tboxDaysTime = $get('<%=tboxDaysTime.ClientID%>');
        if (tboxDays.value == "") {
            alert('<%=Resources.Resource.DaysRequired%>');
            result = false;
        }
        else if (!IsPositiveInt(tboxDays.value)) {
            alert('<%=Resources.Resource.DaysIncorrect%>');
            result = false;
        }
        else if (tboxDaysTime.value == "") {
            alert('<%=Resources.Resource.TimeRequired%>');
            result = false;
        }
        else if (!IsTime(tboxDaysTime.value)) {
            alert('<%=Resources.Resource.TimeIncorrect%>');
            result = false;
        }
    }
    else if ($(tbl4).is(":visible")) {
        var tboxWeekTime = $get('<%=tboxWeekTime.ClientID%>');
        if (tboxWeekTime.value == "") {
            alert('<%=Resources.Resource.TimeRequired%>');
            result = false;
        }
        else if (!IsTime(tboxWeekTime.value)) {
            alert('<%=Resources.Resource.TimeIncorrect%>');
            result = false;
        }
    }
    else if ($(tbl5).is(":visible")) {
        var tboxMonths = $get('<%=tboxMonths.ClientID%>');
        var tboxMonthTime = $get('<%=tboxMonthTime.ClientID%>');
        if (tboxMonths.value == "") {
            alert('<%=Resources.Resource.DaysSequenceRequired%>');
            result = false;
        }
        else if (!IsDaysSequence(tboxMonths.value)) {
            alert('<%=Resources.Resource.DaysSequenceIncorrect%>');
            result = false;
        }
        else if (tboxMonthTime.value == "") {
            alert('<%=Resources.Resource.TimeRequired%>');
            result = false;
        }
        else if (!IsTime(tboxMonthTime.value)) {
            alert('<%=Resources.Resource.TimeIncorrect%>');
            result = false;
        }
    }
    else if ($(tbl6).is(":visible")) {
        var tboxFixedDateTime = $get('<%=tboxFixedDateTime.ClientID%>');
        if (tboxFixedDateTime.value == "") {
            alert('<%=Resources.Resource.TimeRequired%>');
            result = false;
        }
        else if (!IsTime(tboxFixedDateTime.value)) {
            alert('<%=Resources.Resource.TimeIncorrect%>');
            result = false;
        }
    }
    return result;
}

function IsDaysSequence(value) {
    var time = new RegExp(/^(0?[1-9]|[1-2][0-9]|3[0-1])(,(0?[1-9]|[1-2][0-9]|3[0-1]))*$/);
    return time.test(value);
}

function IsTime(value) {
    var time = new RegExp(/^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$/);
    return time.test(value);
}

function IsPositiveInt(value) {
    if ((parseFloat(value) == parseInt(value)) && !isNaN(value)) {
        if (value > 0) {
            return true;
        }
    } else {
        return false;
    }
}



function ChangeEnabledDay()
{
      var  cbox = $get('<%=rbtnDays1.ClientID%>');
      var  elem = $get('<%=tboxDaysTime.ClientID%>');
      
      elem.disabled = ! cbox.checked;
}

function ChangeEnabledWeek()
{
      var  cbox = $get('<%=rbtnWeek1.ClientID%>');
      var  elem = $get('<%=tboxWeekTime.ClientID%>');
      
      elem.disabled = ! cbox.checked;
}

function ChangeEnabledMonth()
{
      var  cbox = $get('<%=rbtnMonth1.ClientID%>');
      var  elem = $get('<%=tboxMonthTime.ClientID%>');
      
      elem.disabled = ! cbox.checked;
}

function ChangeEnabledMaxSizeArchive()
{
      var  cbox1 = $get('<%=cboxCheckArchive.ClientID%>');
      var  cbox2 = $get('<%=cboxMaxSizeArchive.ClientID%>');
      var  tbox = $get('<%=tboxMaxSizeArchive.ClientID%>');
            
      cbox2.disabled = ! cbox1.checked;
      tbox.disabled = ! cbox1.checked;
      
      if(cbox2.disabled == false)
      {        
        tbox.disabled = ! cbox2.checked;
      }
}

function ChangeEnabledDefaultFiles()
{
      var  rbtn = $get('<%=rbtnFiles2.ClientID%>');
      var  cbox1 = $get('<%=cboxFilesDefault1.ClientID%>');
      var  cbox2 = $get('<%=cboxFilesDefault2.ClientID%>');
      var  tbox1 = $get('<%=tboxFilesDefault1.ClientID%>');
      var  tbox2 = $get('<%=tboxFilesDefault2.ClientID%>');
            
      cbox1.disabled = ! rbtn.checked;
      tbox1.disabled = ! rbtn.checked;
      cbox2.disabled = ! rbtn.checked;
      tbox2.disabled = ! rbtn.checked;
      
      if(cbox1.disabled == false)
      {        
        tbox1.disabled = ! cbox1.checked;
      }
      
      if(cbox2.disabled == false)
      {        
        tbox2.disabled = ! cbox2.checked;
      }
}

function ChangeEnabledSaveReport()
{      
      var  cbox = $get('<%=cboxSaveReport.ClientID%>');
      var  cbox1 = $get('<%=cboxAddReport.ClientID%>');
      var  cbox2 = $get('<%=cboxIncludeNameInReport.ClientID%>');
      var  tbox = $get('<%=tboxSaveReport.ClientID%>');      
            
      cbox1.disabled = ! cbox.checked;
      tbox.disabled = ! cbox.checked;
      cbox2.disabled = ! cbox.checked;
}

function ChangeEnabledSaveList()
{      
      var  cbox = $get('<%=cboxSaveList.ClientID%>');
      var  cbox1 = $get('<%=cboxAddList.ClientID%>');      
      var  tbox = $get('<%=tboxSaveList.ClientID%>');      
            
      cbox1.disabled = ! cbox.checked;
      tbox.disabled = ! cbox.checked;      
}

function ChangeEnabledAdditional()
{      
      var  cbox = $get('<%=cboxAdditional2.ClientID%>');      
      var  tbox = $get('<%=tboxAdditional2.ClientID%>');      

      tbox.disabled = ! cbox.checked;      
}

function AddPath()
{
    var opt = document.createElement("option");
    var tbox = $get('<%=tboxScanPath.ClientID%>');
    
    if(tbox.value == "") return;
    
    var ddl = $get('<%=ddlScanPath.ClientID%>');
    ddl.options.add(opt);
    
    opt.text = tbox.value;
    opt.value = tbox.value;
    
    tbox.value = "";    
}

function DeletePath()
{   
    var ddl = $get('<%=ddlScanPath.ClientID%>');
    
    if(ddl.options.length == 0)
    {
        alert("No items!");
        return;
    }
    
    ddl.remove(ddl.selectedIndex);
}
</script>

<asp:HiddenField runat="server" id="hdnStep" Value="0" />
<asp:HiddenField runat="server" id="hdnPath" Value="" />
<asp:HiddenField runat="server" id="hdnEditIndex" Value="-1" />

<table  class="ListContrastTable" runat="server" id="tblProfileMngmt" visible="true" style="width:670px">         
        <tr>
            <td style="padding-left:5px; width: 150px;" align="right" >
                <%=Resources.Resource.Profiles %>
            </td>
            <td style="width: 200px;">            
                <asp:DropDownList ID="ddlProfiles" runat="server" style="width:185px;" ></asp:DropDownList>
            </td>
            <td>            
                <asp:LinkButton runat="server" ID="lbtnCreateProfile" OnClick="lbtnCreateProfile_Click"><%=Resources.Resource.Create %></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lbtnEdit" OnClick="lbtnEdit_Click"><%=Resources.Resource.Edit %></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lbtnRemove" OnClick="lbtnRemove_Click"><%=Resources.Resource.Delete %></asp:LinkButton>
            </td>
        </tr>        
</table>
    
<table  class="ListContrastTable" runat="server" id="tblProfileOptions" style="width:670px" visible="false" >
                    <tr valign="top">            
                       <td style="width:620px; padding-left: 5px;" align="left">
                          <asp:Panel runat="server" Width="620px" Height="200px" ID="pnlRegistry" ScrollBars="Both" BackColor="white">
                             <asp:DataList runat="server" ID="dlTasks" BackColor="White" ShowFooter="False" Width="100%" GridLines="Both" CellPadding="1" CellSpacing="1" OnItemCommand="dlTasks_ItemCommand" OnItemDataBound="dlTasks_ItemDataBound" >
                                <HeaderTemplate>
                                <tr>
                                 <td runat="server" id="tdActionName" style="width: 75px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label1"><%=Resources.Resource.ActionName%></asp:Label>                            
                                 </td>
                                 <td runat="server" id="tdActionType" style="width: 75px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label2"><%=Resources.Resource.ActionType%></asp:Label>
                                 </td>
                                 <td runat="server" id="tdPeriodicityType" style="width: 75px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label3"><%=Resources.Resource.PeriodicityType%></asp:Label>
                                 </td>
                                 <td runat="server" id="tdEnableDisable" style="width: 75px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label8" ><%=Resources.Resource.Enable%>/<%=Resources.Resource.Disable%></asp:Label>
                                 </td>
                                 <td runat="server" id="tdEdit" style="width: 25px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label9" />
                                 </td>
                                 <td runat="server" id="tdDelete" style="width: 25px;" class="listRulesHeader">
                                    <asp:Label runat="server" ID="Label10" />
                                 </td>
                                </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                <tr>
                                 <td class="listRulesItem">
                                    <asp:Literal runat="server" ID="lbl1" Text='<%# Eval("ActionName") %>' />
                                 </td>
                                 <td class="listRulesItem">
                                    <asp:Literal runat="server" ID="lbl2" Text='<%# Eval("ActionType") %>' />
                                 </td>
                                 <td class="listRulesItem">
                                    <asp:Literal runat="server" ID="lbl3" Text='<%# Eval("PeriodicityType") %>' />
                                 </td>
                                 <td class="listRulesItem">
                                    <asp:CheckBox runat="server" ID="cbox8" AutoPostBack="true" OnCheckedChanged="CheckedChanged" />
                                 </td>
                                 <td class="listRulesItem">
                                    <asp:LinkButton runat="server" ID="lbtnEdit" CommandName="EditCommand"><%=Resources.Resource.Edit %></asp:LinkButton>
                                 </td>                        
                                 <td class="listRulesItem">
                                    <asp:LinkButton runat="server" ID="lbtnDelete" CommandName="DeleteCommand"><%=Resources.Resource.Delete %></asp:LinkButton>
                                 </td>
                                </tr>
                                </ItemTemplate>
                             </asp:DataList>       
                          </asp:Panel>              
                       </td>
                    </tr>
                    <tr>
                                <td>
                                    <asp:Button runat="server" ID="lbtnAdd" />
                                    <br />                                
                                </td>
                    </tr>          
                    
</table>

<asp:Panel ID="pnlModalPopapRegistry" runat="server" Style="display: none" CssClass="modalPopupScheduler" >
                                <asp:Panel ID="pnlRuleContentRegistry" runat="server" Style="background-color:#DDDDDD;border:solid 1px Gray;color:Black;">
                                    <table class="ListContrastTable" runat="server" id="tblMain" style="height:375px;">
                                        <tr>
                                            <td style="width:180px;"><%=Resources.Resource.TaskName%>:</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;"><asp:TextBox runat="server" ID="tboxTaskName" /></td>                                            
                                        </tr>
                                        <tr>                                        
                                            <td style="width: 180px;">
                                                <asp:RadioButton ID="rbtnProcess" runat="server" GroupName="rbtnType" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">
                                                <asp:RadioButton ID="rbtnScan" runat="server" GroupName="rbtnType" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">    
                                                <asp:RadioButton ID="rbtnUpdate" runat="server" GroupName="rbtnType" />
                                            </td>                                            
                                        </tr> 
                                        <tr style="height: 100%"><td></td></tr>                           
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblProcess" style="display:none; height:375px;">
                                        <tr>
                                            <td style="width:180px;"><%=Resources.Resource.Path%>:</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;"><asp:TextBox runat="server" ID="tboxPath" /></td>                                            
                                        </tr>
                                        <tr>
                                            <td style="width:180px;"><%=Resources.Resource.Parameters%>:</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;"><asp:TextBox runat="server" ID="tboxParameters" /></td>
                                        </tr>
                                        <tr style="height: 100%"><td></td></tr>
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblScan" style="display:none; height:375px;">
                                        <tr>
                                            <td>
                                                <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" Height="330px">
                                                    <ajaxToolkit:TabPanel runat="server" ID="tabPanel1" HeaderText="Object" >
                                                    <ContentTemplate>
                                                     <table  class="ListContrastTable" style="height:330px;">
                                                        <tr>                            
                                                            <td style="width: 200px;">
                                                                <%=Resources.Resource.ScanMode%>:
                                                            </td>
                                                            <td>
                                                                <%=Resources.Resource.ScanPath%>:
                                                            </td>                                                            
                                                        </tr>
                                                        <tr valign="top">
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlScanMode" SkinID="ddlEdit" Width="120px"></asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlScanPath" SkinID="ddlEdit" Width="250px" />
                                                                <br />
                                                                <asp:TextBox runat="server" ID="tboxScanPath" SkinID="EditBox" Width="120px" />
                                                                <a onclick="AddPath()" style="cursor: pointer"><%=Resources.Resource.Add%></a>&nbsp;
                                                                <a onclick="DeletePath()" style="cursor: pointer"><%=Resources.Resource.Delete%></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table class="ListContrastTable">
                                                                    <tr>
                                                                        <td><%=Resources.Resource.ScaningFiles%>:</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><asp:RadioButton runat="server" ID="rbtnFiles1" GroupName="Files" Checked="true" onclick="ChangeEnabledDefaultFiles()" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButton runat="server" ID="rbtnFiles2" GroupName="Files" onclick="ChangeEnabledDefaultFiles()" /> <br />
                                                                            <asp:CheckBox runat="server" ID="cboxFilesDefault1" onclick="ChangeEnabledDefaultFiles()" /><br />
                                                                            <asp:TextBox runat="server" ID="tboxFilesDefault1" SkinID="EditBox" Width="100px" Enabled="false" /><br />
                                                                            <asp:CheckBox runat="server" ID="cboxFilesDefault2" onclick="ChangeEnabledDefaultFiles()" /><br />
                                                                            <asp:TextBox runat="server" ID="tboxFilesDefault2" SkinID="EditBox" Width="100px" Enabled="false" />
                                                                        </td>                                                            
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButton runat="server" ID="rbtnFiles3" GroupName="Files" onclick="ChangeEnabledDefaultFiles()" /><br />
                                                                            <asp:TextBox runat="server" ID="tboxFilesCustom" SkinID="EditBox" Width="100px" />
                                                                        </td>                                                            
                                                                    </tr>
                                                                </table>                                                                
                                                            </td>
                                                            <td>
                                                                <asp:CheckBoxList runat="server" id="cboxObject"></asp:CheckBoxList>                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="cboxCheckArchive" onclick="ChangeEnabledMaxSizeArchive()" />
                                                                <br />
                                                                <asp:CheckBox runat="server" ID="cboxMaxSizeArchive" onclick="ChangeEnabledMaxSizeArchive()" />
                                                                <br />
                                                                <asp:TextBox runat="server" ID="tboxMaxSizeArchive"></asp:TextBox>
                                                            </td>
                                                        </tr> 
                                                        <tr style="height: 100%"><td></td></tr>                                 
                                                     </table>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                
                                                <ajaxToolkit:TabPanel runat="server" ID="tabPanel2" HeaderText="Actions" >
                                                    <ContentTemplate>
                                                     <table  class="ListContrastTable" style="height:330px;">
                                                        <tr valign="top">                            
                                                            <td>
                                                                <table class="ListContrastTable">
                                                                    <tr>
                                                                        <td><%=Resources.Resource.InfectedFiles%>:</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList runat="server" ID="cboxInfected"></asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>                                                                    
                                                                </table>
                                                            </td>
                                                            <td>
                                                                <table class="ListContrastTable">
                                                                    <tr>
                                                                        <td><%=Resources.Resource.SuspiciousFiles%>:</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>                                                                            
                                                                            <asp:CheckBox runat="server" ID="cboxSuspectedToQtn"></asp:CheckBox>
                                                                            <br />
                                                                            <asp:CheckBox runat="server" ID="cboxSuspectedDelete"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>                                                                    
                                                                </table>
                                                                <br />
                                                                <table class="ListContrastTable">
                                                                    <tr>
                                                                        <td><%=Resources.Resource.Heuristic%>:</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlExpertAnaliz" onchange="ChangeExpertAnaliz()"></asp:DropDownList>
                                                                        </td>
                                                                    </tr>                                                                    
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 100%"><td></td></tr>
                                                     </table>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                
                                                <ajaxToolkit:TabPanel runat="server" ID="tabPanel3" HeaderText="Report" >
                                                    <ContentTemplate>
                                                     <table class="ListContrastTable" style="height:330px;">
                                                        <tr>
                                                        <td>
                                                            <table class="ListContrastTable">
                                                            <tr>                            
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="cboxSaveReport" Checked="true" onclick="ChangeEnabledSaveReport()" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="tboxSaveReport"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:CheckBox runat="server" ID="cboxAddReport" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:CheckBox runat="server" ID="cboxIncludeNameInReport" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                            <table class="ListContrastTable">
                                                            <tr>                            
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="cboxSaveList" Checked="true" onclick="ChangeEnabledSaveList()" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="tboxSaveList"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:CheckBox runat="server" ID="cboxAddList" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                        </tr>
                                                        <tr style="height: 100%"><td></td></tr>
                                                     </table>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                
                                                <ajaxToolkit:TabPanel runat="server" ID="tabPanel4" HeaderText="Additional" >
                                                    <ContentTemplate>
                                                     <table  class="ListContrastTable" style="height:330px;">
                                                        <tr>                            
                                                            <td>
                                                                <asp:CheckBoxList runat="server" ID="cboxAdditional1"></asp:CheckBoxList><br />
                                                                <asp:CheckBox runat="server" ID="cboxAdditional2" onclick="ChangeEnabledAdditional()" />
                                                                <asp:TextBox runat="server" ID="tboxAdditional2" Enabled="false"></asp:TextBox>
                                                                <br />
                                                                <asp:CheckBoxList runat="server" ID="cboxAdditional3"></asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 100%"><td></td></tr>
                                                     </table>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                </ajaxToolkit:TabContainer>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblPeriodicity" style="display:none; height:375px;">
                                        <tr>                                        
                                            <td style="width: 180px;">
                                                <asp:RadioButton ID="RadioButton1" runat="server" GroupName="rbtnPeriodicity" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">
                                                <asp:RadioButton ID="RadioButton2" runat="server" GroupName="rbtnPeriodicity" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">    
                                                <asp:RadioButton ID="RadioButton3" runat="server" GroupName="rbtnPeriodicity" />
                                            </td>                                            
                                        </tr>
                                        <tr>                                        
                                            <td style="width: 180px;">
                                                <asp:RadioButton ID="RadioButton4" runat="server" GroupName="rbtnPeriodicity" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">
                                                <asp:RadioButton ID="RadioButton5" runat="server" GroupName="rbtnPeriodicity" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">    
                                                <asp:RadioButton ID="RadioButton6" runat="server" GroupName="rbtnPeriodicity" />
                                            </td>                                            
                                        </tr>
                                        <tr style="height: 100%"><td></td></tr>
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblTime1" style="display:none; height:375px;">
                                        <tr valign="top">
                                            <td><%=Resources.Resource.RuningEvery%></td>
                                            <td><asp:TextBox runat="server" ID="tboxMinutes" Text="30" SkinID="EditBox" Width="50px" /></td>
                                            <td><%=Resources.Resource.Minutes%></td>
                                        </tr>
                                        <tr style="height: 100%"><td></td></tr>
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblTime2" style="display:none; height:375px;">
                                        <tr>
                                            <td><%=Resources.Resource.RuningEvery%></td>
                                            <td><asp:TextBox runat="server" ID="tboxHours" Text="1" SkinID="EditBox" Width="50px" />
                                            </td>
                                            <td><%=Resources.Resource.Hours%></td>
                                        </tr>
                                        <tr style="height: 100%"><td></td></tr>
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblTime3" style="display:none; height:375px;">
                                        <tr>
                                            <td><%=Resources.Resource.RuningEvery%></td>
                                            <td><asp:TextBox runat="server" ID="tboxDays" Text="1" SkinID="EditBox" Width="50px" /></td>
                                            <td><%=Resources.Resource.Days%></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton runat="server" ID="rbtnDays1" GroupName="Days" Checked="true" onclick="ChangeEnabledDay()" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="tboxDaysTime" SkinID="EditBox" Width="50px" />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:RadioButton runat="server" ID="rbtnDays2" GroupName="Days" onclick="ChangeEnabledDay()" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox runat="server" ID="cboxDays" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr style="height: 100%"><td></td></tr>
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblTime4" style="display:none; height:375px;">
                                        <tr>
                                            <td colspan="2"><%=Resources.Resource.Weekdays%>:</td>                                            
                                        </tr>
                                        <tr>
                                            <td><asp:CheckBox runat="server" ID="cbokWeekMon" Checked="true" /></td>
                                            <td><asp:CheckBox runat="server" ID="cbokWeekFri" Checked="true" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:CheckBox runat="server" ID="cbokWeekTues" Checked="true" /></td>
                                            <td><asp:CheckBox runat="server" ID="cbokWeekSat" Checked="true" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:CheckBox runat="server" ID="cbokWeekWednes" Checked="true" /></td>
                                            <td><asp:CheckBox runat="server" ID="cbokWeekSun" Checked="true" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"><asp:CheckBox runat="server" ID="cboxWeekTh" Checked="true" /></td>                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton runat="server" ID="rbtnWeek1" GroupName="Weeks" Checked="true" onclick="ChangeEnabledWeek()" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tboxWeekTime" SkinID="EditBox" Width="50px" />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButton runat="server" ID="rbtnWeek2" GroupName="Weeks" onclick="ChangeEnabledWeek()" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox runat="server" ID="cboxWeek" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr style="height: 100%"><td></td></tr>
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblTime5" style="display:none; height:375px;">
                                        <tr>
                                            <td colspan="2"><%=Resources.Resource.DaysOfMonth%>:</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"><asp:TextBox runat="server" ID="tboxMonths" SkinID="EditBox" Width="150px" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton runat="server" ID="rbtnMonth1" GroupName="Months" Checked="true" onclick="ChangeEnabledMonth()" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="tboxMonthTime" SkinID="EditBox" Width="50px" />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButton runat="server" ID="rbtnMonth2" GroupName="Months" onclick="ChangeEnabledMonth()" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox runat="server" ID="cboxMonth" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr style="height: 100%"><td></td></tr>
                                    </table>
                                    
                                    <table class="ListContrastTable" runat="server" id="tblTime6" style="display:none; height:375px;">
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="tboxDate" Enabled="false"/>
                                                <asp:ImageButton runat="Server" ID="ibtnCalendar" ImageUrl="~/App_Themes/Main/Images/Calendar_scheduleHS.png" /><br />
                                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="tboxDate" PopupButtonID="ibtnCalendar" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="tboxFixedDateTime"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cboxFixedDate" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr style="height: 100%"><td></td></tr>
                                    </table>
                                    
                                    
                                </asp:Panel>
                                
                                     <div>                                        
                                        <p style="text-align: center;">
                                            <asp:Button ID="btnPrev" runat="server" OnClientClick="return PreviousClick()" style="display: none" />
                                            <asp:Button ID="btnAddRule" runat="server" OnClick="lbtnAdd_Click" OnClientClick="return NextClick()" />
                                            <asp:Button ID="btnCancelRule" runat="server" OnClientClick="ClearStep()" OnClick="btnCancelRule_Click" />
                                        </p>
                                     </div>
                                </asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderRule" runat="server" TargetControlID="lbtnAdd" PopupControlID="pnlModalPopapRegistry" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModalPopapRegistry" DropShadow="true" X="100" Y="100" Drag="False" />