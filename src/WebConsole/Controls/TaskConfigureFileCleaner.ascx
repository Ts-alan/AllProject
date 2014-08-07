<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureFileCleaner.ascx.cs"
    Inherits="Controls_TaskConfigureFileCleaner" %>
<div class="tasksection" runat="server" id="HeaderName" style="width: 560px">
    <%=Resources.Resource.TaskNameConfigureFileCleaner%></div>
<div class="divSettings">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#fileCleanerMainPanel").tabs({ cookie: { expires: 30} });
        });
        function pageLoad() 
        {
            
            $("input[class='control']").button();

              $("[trProgramItemSelected]").hover(function(){
                    if($(this).attr('trProgramItemSelected')=="true") return;
                    $(this).css('background-color', 'yellow');
                },
                function () { 
                    if($(this).attr('trProgramItemSelected')=="true") return;           
                    $(this).css('background-color', '');
                }
            );
               
            $("[trProgramItemSelected]").on("click",function(){
                if($(this).attr("trProgramItemSelected")=="true") return;
                $(this).css('background-color', '#3399ff');
                $("[trProgramItemSelected=true]").css('background-color', '');
                $("[trProgramItemSelected=true]").attr('trProgramItemSelected',false);
                $(this).attr('trProgramItemSelected',true);
                
                var rowNo=$(this).children().find('[value]').attr('value');                
                 $('#<%=FileCleanerhdnActiveRowNo.ClientID %>').attr('value',rowNo);
             });  
            var activeRowNo=$('#<%=FileCleanerhdnActiveRowNo.ClientID %>').attr('value');            
            var tableRow=$("[id*='hdnProgramRowNo'][value='"+activeRowNo+"']").parent().parent(); 
            tableRow.attr('trProgramItemSelected',true);    
            $("[trProgramItemSelected='true']").css('background-color', '#3399ff'); 
        };     

        function DialogSetDefaultValues()
        {
            $('#<%=AddProgramDialogName.ClientID %>').val('');    
        };
        function TemplateDialogSetDefaultValues()
        {
            $('#<%=AddTemplateDialogPath.ClientID %>').val('');
            $('#<%=AddTemplateDialogName.ClientID %>').val('');
        };
        function AddProgramButtonClientClick()
        {
            $('#addProgramTable tbody').children("tr").remove();
            var dOpt = {
                width: 550,                                       
                resizable: false,
                close: function(event, ui)
                    {
                        DialogSetDefaultValues();  
                        $('#divOverlay').css('display','none');
                    },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {

                        if($('#<%=AddProgramDialogName.ClientID %>').val()!="")
                        {
                            var btn = '<%=AddProgramHiddenButton.UniqueID %>'; 
                            
                            var tbody=$('#addProgramTable tbody').children("tr");
                            var array=[];
                            $('#addProgramTable tbody').children("tr").each(function(index){
                                
                                template=new Object();
                                template.path=$(this).children()[0].innerHTML;
                                template.filename=$(this).children()[1].innerHTML
                                array.push(template);                                
                            });
                            var json=JSON.stringify(array);
                                __doPostBack(btn, json);                          
                                                                                            
                            $('#AddProgramDialog').dialog('close');    
                        }
                        else{
                            alert($('#<%=HiddenAlertLabel.ClientID%>').html());
                        }                     
                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {                           
                        $('#AddProgramDialog').dialog('close');                            
                    }
                }
            };
            $('#AddProgramDialog').dialog(dOpt);
            
            $('#divOverlay').css('display','inline');
            $('#AddProgramDialog').parent().zIndex(101);
            $('#AddProgramDialog').parent().appendTo(jQuery("form:first"));
        }


        /* Change Phrogram */
        function ChangeProgramButtonClientClick()
        {
            var index=$('#<%=FileCleanerhdnActiveRowNo.ClientID %>').attr('value');
            if(index==0)
                return;
            var tableRow=$("[id*='hdnProgramRowNo'][value='"+index+"']").parent().parent();
            ChangeProgram(tableRow);
        };        
         function ChangeProgram( tableRow)
         {
            var name=tableRow.children().find("[id*='lblProgramName']").text();
            var HdnJson=tableRow.children().find("[id*='hdnProgramJson']");
            var jsonTable=jQuery.parseJSON(HdnJson.val());
            $('#addProgramTable tbody').children("tr").remove();
            $('#<%=AddProgramDialogName.ClientID %>').val(name);
             var dOpt = {
                    width: 550,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            DialogSetDefaultValues();  
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                        if($('#<%=AddProgramDialogName.ClientID %>').val()!="")
                        {
                            var btn = '<%=ChangeProgramRulesHiddenButton.UniqueID %>';
                            var tbody=$('#addProgramTable tbody').children("tr");
                            var array=[];
                            $('#addProgramTable tbody').children("tr").each(function(index){

                                    template=new Object();
                                    template.path=$(this).children()[0].innerHTML;
                                    template.filename=$(this).children()[1].innerHTML;
                                    array.push(template);                                                                 
                            });
                            var json=JSON.stringify(array);
                            HdnJson.val(json);
                             __doPostBack(btn, json);                                                        
                                $('#AddProgramDialog').dialog('close');
                        }
                        else{
                                alert($('#<%=HiddenAlertLabel.ClientID%>').html());
                            } 
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#AddProgramDialog').dialog('close');
                            }
                        }
                    }
                var len=jsonTable.length;

                for(var i=0;i<len;i++)
                {
                    $('#addProgramTable tbody').append('<tr trAddProgramItemSelected="false" ><td>' + jsonTable[i].Path + '</td><td>'+jsonTable[i].FileName+'</td></tr>');
                }
                addProgramTableChangeStyle();
                $('#AddProgramDialog').dialog(dOpt);

                $('#divOverlay').css('display','inline');
                $('#AddProgramDialog').parent().zIndex(101);
                $('#AddProgramDialog').parent().appendTo(jQuery("form:first"));
            };


        /*  Hover/Click Templates   */
        $(document).on("mouseenter","[trAddProgramItemSelected]",function () {
            if ($(this).attr('trAddProgramItemSelected') == "true") return;
            $(this).css('background-color', 'yellow');
        });
        $(document).on("mouseleave", "[trAddProgramItemSelected]",function () {
            if ($(this).attr('trAddProgramItemSelected') == "true") return;
            $(this).css('background-color', '');
        });
        $(document).on("click","[trAddProgramItemSelected]",function(){
            if($(this).attr("trAddProgramItemSelected")=="true") return;
            $(this).css('background-color', '#3399ff');
            $("[trAddProgramItemSelected=true]").css('background-color', '');
            $("[trAddProgramItemSelected=true]").attr('trAddProgramItemSelected',false);
            $(this).attr('trAddProgramItemSelected',true);              
        });

        /*  Add/Delete/Change   Templates */

        function AddProgramDialogButtonClientClick()
        {
            var dOpt = {
                width: 420,                                      
                resizable: false,
                close: function(event, ui)
                    {
                        $('#divOverlay2').css('display','none');
                        TemplateDialogSetDefaultValues(); 
                    },
                buttons: {
                    <%=Resources.Resource.Apply%>: function () {
                        if(Page_ClientValidate('AddTemplateDialogPathValidationGroup')){
                            var path=$('#<%=AddTemplateDialogPath.ClientID %>').val();
                            var name=$('#<%=AddTemplateDialogName.ClientID %>').val();  
                            $('#addProgramTable tbody').append('<tr trAddProgramItemSelected="false" ><td>' + path + '</td><td>'+name+'</td></tr>'); 
                            addProgramTableChangeStyle();                              
                            $('#AddTemplateDialog').dialog('close');
                        }                        
                    },
                    <%=Resources.Resource.CancelButtonText%>: function () {                           
                        $('#AddTemplateDialog').dialog('close');                           
                    }
                }
            };
            $('#AddTemplateDialog').dialog(dOpt);
            $('#divOverlay2').css('display','inline');
            $('#AddTemplateDialog').parent().zIndex(120);
            $('#AddTemplateDialog').parent().appendTo(jQuery("form:first"));
            
       };
       function ChangeProgramDialogButtonClientClick()
        {        
            var row=$("[trAddProgramItemSelected=true]"); 
            
           if(row.children().length==0) return;
            
            var Oldpath=row.children()[0].innerHTML;
            var Oldname=row.children()[1].innerHTML;

            $('#<%=AddTemplateDialogPath.ClientID %>').val(Oldpath);
            $('#<%=AddTemplateDialogName.ClientID %>').val(Oldname);   
                        
            var dOpt = {
                width: 420,                                       
                resizable: false,
                close: function(event, ui)
                    {
                        $('#divOverlay2').css('display','none');
                        TemplateDialogSetDefaultValues(); 
                    },
                buttons: {
                    <%=Resources.Resource.Apply%>: function () {
                        var path=$('#<%=AddTemplateDialogPath.ClientID %>').val();
                        var name=$('#<%=AddTemplateDialogName.ClientID %>').val();                            
                        row.children()[0].innerHTML=path;
                        row.children()[1].innerHTML=name;    
                                                            
                        $('#AddTemplateDialog').dialog('close');
                            
                    },
                    <%=Resources.Resource.CancelButtonText%>: function () {                           
                        $('#AddTemplateDialog').dialog('close');
                        
                        }
                    }
                };
        $('#AddTemplateDialog').dialog(dOpt);
        $('#divOverlay2').css('display','inline');
        $('#AddTemplateDialog').parent().zIndex(120);
        $('#AddTemplateDialog').parent().appendTo(jQuery("form:first"));
    };
    function DeleteProgramDialogButtonClientClick()
    {
        $("[trAddProgramItemSelected=true]").remove();
        addProgramTableChangeStyle();
    }
    function addProgramTableChangeStyle() {
        var i = 0;
        $('#addProgramTable tbody').children("tr").each(function (index) {
            if (i % 2 == 0) {
                gridStyle = "gridViewRow";
            }
            else
                gridStyle = "gridViewRowAlternating";
            var row = $(this);
            row.removeClass();
            row.addClass(gridStyle);
            i++;
        });
    }
    </script>
    <div id='fileCleanerMainPanel'  style="width:550px">
        <ul>
            <li><a href="#tabVCF1"><%=Resources.Resource.Files %></a></li>
            <li><a href="#tabVCF2"><%=Resources.Resource.JournalEvents %></a> </li>
        </ul>
        <div id='tabVCF1'>
        <asp:Label ID="HiddenAlertLabel" runat="server" Style="display: none"><%=Resources.Resource.FirstNameRequiredErrorMessage %> </asp:Label>
        <asp:UpdatePanel ID="FileCleanerUpdatePanel" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="FileCleanerhdnActiveRowNo" Value='0' runat="server" />
                <asp:Table class="ListContrastTable" ID="tblFileCleanerMainPanel" Style="width: 525px" runat="server">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="3">
                            <asp:Panel runat="server" Width="520px" Height="400px" ID="ProgramListPanel" Style="overflow: scroll">
                                <asp:DataList runat="server" ID="ProgramListDataList" OnItemDataBound="ProgramListDataList_ItemDataBound"
                                    OnItemCommand="ProgramListDataList_SelectedIndexChanged" Style="table-layout: fixed;
                                    word-break: break-all;" rules="all">
                                    <HeaderTemplate>
                                        <tr class="gridViewHeader">
                                            <th runat="server" id="tdProgramName" style="width: 420px; text-align: center;">
                                                <asp:Label runat="server"><%=Resources.Resource.Name%></asp:Label>
                                            </th>
                                            <th runat="server" id="tdProgramChecked" style="width: 80px; text-align: center;">
                                                <asp:Label runat="server"><%=Resources.Resource.CheckState%></asp:Label>
                                            </th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr runat="server" id="trProgramItem" trprogramitemselected='false' >
                                            <td runat="server" id="tdProgramName" style="width: 435px;">
                                                <asp:Label runat="server" ID="lblProgramName" Text="" />
                                            </td>
                                            <td runat="server" id="tdProgramChecked" style="width: 65px;">
                                                <asp:CheckBox runat="server" ID="chkProgramChecked" OnCheckedChanged="chkProgramChecked_OnCheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:HiddenField ID="hdnProgramRowNo" Value="0" runat="server" />
                                                <asp:HiddenField ID="hdnProgramJson" Value="" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:DataList>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign='Center'>
                            <asp:Button ID="AddProgramButton" runat='server' Text='<%$ Resources:Resource, Add %>'
                                OnClientClick="AddProgramButtonClientClick()" />
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign='Center'>
                            <asp:Button ID="ChangeProgramButton" runat='server' Text='<%$ Resources:Resource, Change%>'
                                OnClientClick="ChangeProgramButtonClientClick()" />
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign='Right'>
                            <asp:Button ID="DeleteProgramButton" runat="server" Text='<%$ Resources:Resource, Delete %>'
                                OnClick="DeleteProgramButtonClick" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Button ID="AddProgramHiddenButton" runat='server' Style="display: none" OnClick="AddProgramHiddenButtonClick" />
                <asp:Button ID="ChangeProgramRulesHiddenButton" runat='server' Style="display: none"
                    OnClick="ChangeProgramRulesHiddenButtonClick" />
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <div id='tabVCF2'>
            <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                        <asp:Table ID="JournalEventTable"  runat="server" CssClass="ListContrastTable" rules="cols">
                            <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" CssClass="gridViewHeader">
                                <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" >
                                    <asp:Label ID="Label1" runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" >
                                    <asp:Label ID="Label2" runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;" >
                                    <asp:Label ID="Label3" runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;" >
                                    <asp:Label ID="Label4" runat="server"  ><%=Resources.Resource.CCJournal %></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--Диалог для добавления новых правил--%>
    <div id="AddProgramDialog" style="display: none;" class="ui-front">
        <asp:HiddenField ID="AddProgramDialogActiveRowNo" Value='0' runat="server" />
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="3">
                    <asp:Label runat="server"><%=Resources.Resource.Name %></asp:Label>
                    <asp:TextBox ID="AddProgramDialogName" Style="width: 200px" runat='server'></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>&nbsp</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="3">
                    <div style="height: 200px; width: 500px; overflow: scroll">
                        <table id="addProgramTable" rules="cols">
                            <thead class="gridViewHeader">
                                <th runat="server" id="tdAddProgramPath" style="width: 350px; text-align: center;">
                                    <%=Resources.Resource.Path%>
                                </th>
                                <th runat="server" id="tdAddProgramDialogTemplate" style="width: 150px; text-align: center;">
                                    <%=Resources.Resource.Template%>
                                </th>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign='Center'>
                    <asp:Button ID="AddProgramDialogAddTemplate" runat='server' Text='<%$ Resources:Resource, Add %>'
                        OnClientClick="AddProgramDialogButtonClientClick(); return false;" />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign='Center'>
                    <asp:Button ID="AddProgramDialogChangeTemplate" runat='server' Text='<%$ Resources:Resource, Change %>'
                        OnClientClick="ChangeProgramDialogButtonClientClick(); return false;" />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign='Right'>
                    <asp:Button ID="AddProgramDialogDeleteTemplate" runat="server" Text='<%$ Resources:Resource, Delete %>'
                        OnClientClick="DeleteProgramDialogButtonClientClick(); return false" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <div id="AddTemplateDialog" style="display: none;height:62px;min-height:60px" class="ui-front">
        <table>
            <tr>
                <td>
                    <%=Resources.Resource.Path%>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="AddTemplateDialogPath" />
                    <asp:RequiredFieldValidator ID="RequiredAddTemplateDialogPath" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                        ControlToValidate="AddTemplateDialogPath" Display="None" ValidationGroup="AddTemplateDialogPathValidationGroup">
                    </asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRequiredAddTemplateDialogPath" runat="server" TargetControlID="RequiredAddTemplateDialogPath" HighlightCssClass="highlight" PopupPosition="Right" Width="150px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <%=Resources.Resource.Template%>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="AddTemplateDialogName" />
                    <asp:RequiredFieldValidator ID="RequiredAddTemplateDialogName" runat="server" ErrorMessage='<%$ Resources:Resource, ValueRequired %>'
                        ControlToValidate="AddTemplateDialogName" Display="None" ValidationGroup="AddTemplateDialogPathValidationGroup">
                    </asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRequiredAddTemplateDialogName" runat="server" TargetControlID="RequiredAddTemplateDialogName" HighlightCssClass="highlight" PopupPosition="Right" Width="150px" />
                </td>
            </tr>
        </table>
        &nbsp;
    </div>
    <div id="divOverlay" class="ui-widget-overlay ui-front" style="display: none;z-index:100"></div>
    <div id="divOverlay2" class="ui-widget-overlay ui-front" style="display: none;z-index:110"></div>
</div>