<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DevicesPolicy.aspx.cs" MasterPageFile="~/mstrPageMain.master" Inherits="DevicesPolicy" %>
<%@ Register Assembly="PagingControl" Namespace="PagingControls" TagPrefix="cc1" %>
<%@ Register Assembly="DateControl" Namespace="DateControl" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {
        $('.wrapped').each(function () {
            var origTxt = $(this).html();
            var dest = "";
            var counter = 0;
            var maxLenght = 30;
            for (var i = 0; i < origTxt.length; i++) {
                dest += origTxt.substr(i, 1);
                counter++;
                if (counter >= maxLenght) {
                    dest += '<br/>';
                    counter = 0;
                }
            }
            $(this).html(dest);

            $(this).contextMenu({
                menu: 'myMenu'
            }, function (action, el, pos) {
                var tbox = $(el).attr('tbox');
                var cbox = $(el).attr('cbox');
                $get(tbox).value = $(el).text();
                $get(cbox).checked = true;
            });
        });


    });
        
    </script>
 <%-- Context Meun--%>
 		<ul id="myMenu" class="contextMenu">
			<li class="copy"><a href="#copy"><%= Resources.Resource.CopyToFilter%></a></li>
		</ul>
 <%-- Context Meun--%>

<div id="divMessage" runat="server" style="width:90%; min-width: 800px; text-align: center; margin: 30px; display: none;">
</div>
<div id="divContent" runat="server">
    <div class="title"><%=Resources.Resource.DeviceManagment %></div>  
        <div id='tabs'>
          <ul>
            <li><a href='#0'><span><%= Resources.Resource.Computers %></span></a></li>
            <li><a href='#1'><span><%=Resources.Resource.Devices %></span></a></li>
            <li><a href='#2'><span><%=Resources.Resource.Assignment %></span></a></li>
          </ul>
          <div id='0'>          
              <table width="700px">
              <tr>
                <td>
                    <cc1:PagingControl ID="pcPagingTop" runat="server" OnNextPage="pcPaging_NextPage" OnPrevPage="pcPaging_PrevPage" OnHomePage="pcPaging_HomePage" OnLastPage="pcPaging_LastPage" />          
                </td>
                <td align="right">
                    <a runat="server" ID="lbtnFilterComputer" style="cursor: pointer"><%=Resources.Resource.Filter%></a>
                </td>
              </tr>
              </table>
              <asp:DataList ID=DataList1 runat=server class=ListContrastTable Width="700px">
              <HeaderTemplate>
                <tr>
                    <td class="HeaderCell"><%= Resources.Resource.ComputerName %></td>
                    <td class="HeaderCell"><%= Resources.Resource.UserLogin %></td>
                    <td class="HeaderCell"><%= Resources.Resource.IPAddress %></td>
                    <td class="HeaderCell"><%= Resources.Resource.Apply %></td>
                </tr>
              </HeaderTemplate>
                <ItemTemplate>
                    <tr style="text-align:center">
                        <td><div cp=<%#Eval("ID")%> style="cursor:pointer;"><%#Eval("ComputerName")%></div></td>
                        <td><div cp=<%#Eval("ID")%> style="cursor:pointer;"><%#Eval("UserLogin")%></div></td>
                        <td><div cp=<%#Eval("ID")%> style="cursor:pointer;"><%#Eval("IPAddress")%></div></td>
                        <td>
                            <img cpp='<%#Eval("ID")%>' alt='Apply' style="cursor:pointer;" title='<%=Resources.Resource.ApplyPolicy %>' addcomp="true" src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/enabled.gif")%>' />
                        </td>
                        <div cpc=<%#Eval("ID")%> ncp=<%#Eval("ComputerName")%>></div>                    
                        <div cpcp=<%#Eval("ID")%> ncp=<%#Eval("ComputerName")%>></div>
                    </tr>
                </ItemTemplate>
             </asp:DataList>
             <%=Resources.Resource.PromtForAddDevice %><br />
             <cc1:PagingControl ID="pcPaging" runat="server" OnNextPage="pcPaging_NextPage" OnPrevPage="pcPaging_PrevPage" OnHomePage="pcPaging_HomePage" OnLastPage="pcPaging_LastPage" />
         </div>
         <div id='1'>
            <table width="700px">
              <tr>
                <td>
                    <cc1:PagingControl ID="PagingControl1" runat="server" OnNextPage="pcPaging_NextPage2" OnPrevPage="pcPaging_PrevPage2" OnHomePage="pcPaging_HomePage2" OnLastPage="pcPaging_LastPage2" />
                </td>
                <td align="right">
                    <a runat="server" ID="lbtnFilterDevice" style="cursor: pointer"><%=Resources.Resource.Filter%></a>
                </td>
              </tr>
              </table>        
              <asp:DataList ID=DataList2 runat=server class=ListContrastTable Width="700px" OnItemDataBound="DataList2_ItemDataBound">
              <HeaderTemplate>
                <tr>
                    <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderDeviceSerialNo" OnClick="lbtnHeaderDeviceSerialNo_Click"></asp:LinkButton></td>
                    <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderDeviceComment" OnClick="lbtnHeaderDeviceComment_Click"></asp:LinkButton> </td>
                    <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderDeviceLastComputer" OnClick="lbtnHeaderDeviceLastComputer_Click"></asp:LinkButton></td>
                    <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderDeviceLatestInsert" OnClick="lbtnHeaderDeviceLatestInsert_Click"></asp:LinkButton></td>
                    <td class="HeaderCell" colspan="3" ><%= Resources.Resource.Actions %></td>
                </tr>
              </HeaderTemplate>
                <ItemTemplate>
                <!-- This is very similar perversion.. Need to K.I.S.S.! -->
                    <tr style="text-align:center">
                        <td dcp=<%#Eval("ID")%> class="wrapped" no='true' tbox=<%=tboxSerialNumber.ClientID%> cbox=<%=cboxSerialNumber.ClientID%>><%#Eval("SerialNo")%></td>
                        <td dcp=<%#Eval("ID")%> class="wrapped" comment='false' no=true tbox=<%=tboxCommentFilter.ClientID%> cbox=<%=cboxCommentFilter.ClientID%>><%#Eval("Comment")%></td>  
                        <td dcp=<%#Eval("ID")%>  class="wrapped" comment='false' no=true tbox=<%=tboxComputerFilter.ClientID%> cbox=<%=cboxComputerFilter.ClientID%>><%#Eval("LastComputer")%></td>
                        <td dcp=<%#Eval("ID")%>  comment='false' no=true><%#Eval("LastInserted")%>&nbsp;</td>
                         <td dcp=<%#Eval("ID")%> >
                            <img dcp='<%#Eval("ID")%>' alt='Add' style="cursor:pointer;" title='<%=Resources.Resource.Management %>' addcomp="true" src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/add.png")%>' />
                        </td>
                        <td dcp=<%#Eval("ID")%> >
                            <img dcp='<%#Eval("ID")%>' alt='Edit' style="cursor:pointer;" title='<%=Resources.Resource.EditComment %>' comment='true' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/table_edit.png")%>' />
                        </td>
                        <td dcp=<%#Eval("ID")%> >
                            <img dcp=<%#Eval("ID")%> alt='Delete' style="cursor:pointer;" delete='true' title='<%=Resources.Resource.Delete %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/disabled.gif")%>' >
                                </img>
                         </td>
                        <div dcpc=<%#Eval("ID")%> dncp=<%#Eval("SerialNo")%>></div>
                    </tr>
                </ItemTemplate>
             </asp:DataList>
             <cc1:PagingControl ID="PagingControl2" runat="server" OnNextPage="pcPaging_NextPage2" OnPrevPage="pcPaging_PrevPage2" OnHomePage="pcPaging_HomePage2" OnLastPage="pcPaging_LastPage2" />
         </div>
         <div id='2'>
             <table width="700px">
              <tr>
                <td>
                    <cc1:PagingControl ID="PagingControl3" runat="server" OnNextPage="pcPaging_NextPage3" OnPrevPage="pcPaging_PrevPage3" OnHomePage="pcPaging_HomePage3" OnLastPage="pcPaging_LastPage3" />
                </td>
                <td align="right">
                    <a runat="server" ID="lbtnFilterUnknownDevice" style="cursor: pointer"><%=Resources.Resource.Filter%></a>          
                </td>
              </tr>
              </table>
         
              <asp:DataList ID=DataList3 runat=server class=ListContrastTable Width="700px" OnItemDataBound="DataList3_ItemDataBound">
              <HeaderTemplate>
                <tr>
                    <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderUDeviceSerialNo" OnClick="lbtnHeaderUDeviceSerialNo_Click" /></td>
                    <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderUDeviceComment" OnClick="lbtnHeaderUDeviceComment_Click" /></td>
                    <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderUDeviceComputerName" OnClick="lbtnHeaderUDeviceComputerName_Click" /></td>
                    <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderUDeviceInsertedDate" OnClick="lbtnHeaderUDeviceInsertedDate_Click" /></td>
                    <td class="HeaderCell" width="70"><asp:Label runat="server" ID="lblHeaderUDeviceAction"><%=Resources.Resource.Action %></asp:Label>
                    <br /><img acpAll=allowAll action=allow style="cursor:pointer;" title='<%=Resources.Resource.EnableAll %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/enabled.gif")%>' />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <img acpAll=banAll action=disable style="cursor:pointer;" title='<%=Resources.Resource.DisableAll %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/disabled.gif")%>' />
                    </td>
                </tr>
              </HeaderTemplate>
                <ItemTemplate>
                    <tr  rcp style="text-align:center">
                        <td dcp=<%#Eval("ID")%> class="wrapped" no=true tbox=<%=tboxSerialNumberUDevice.ClientID%> cbox=<%=cboxSerialNumberUDevice.ClientID%>><%#Eval("Device.SerialNo")%></td>
                        <td dcp=<%#Eval("ID")%> class="wrapped" no=true tbox=<%=tboxCommentUDevice.ClientID%> cbox=<%=cboxCommentUDevice.ClientID%>><%#Eval("Device.Comment")%></td>
                        <td dcp=<%#Eval("ID")%> class="wrapped" no=true tbox=<%=tboxComputerNameUDevice.ClientID%> cbox=<%=cboxComputerNameUDevice.ClientID%>><%#Eval("Computer.ComputerName")%></td>
                        <td dcp=<%#Eval("ID")%> no=true><%#Eval("LatestInsert")%>&nbsp;</td>
                        <td dcp=<%#Eval("ID")%> no=true>
                            <img acp=<%#Eval("ID")%> action=allow style="cursor:pointer;" title='<%=Resources.Resource.Enable %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/enabled.gif")%>' />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <img acp=<%#Eval("ID")%> action=disable style="cursor:pointer;" title='<%=Resources.Resource.Disable %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/disabled.gif")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
             </asp:DataList>
             <cc1:PagingControl ID="PagingControl4" runat="server" OnNextPage="pcPaging_NextPage3" OnPrevPage="pcPaging_PrevPage3" OnHomePage="pcPaging_HomePage3" OnLastPage="pcPaging_LastPage3" />
         </div>
      </div>   
      <div class='helpMessage' style="visibility:hidden;color:red;"><%= Resources.Resource.DisplayTabChanges%></div>
  
    <asp:Panel ID="filterShow" runat="server" style="display: none; z-index: 3; background-color:#DDD; border: thin solid navy;" Height="95px">
      <table>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermComputerName" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <asp:textbox id="tboxComputerName" Runat="server"></asp:textbox>
            </td>
            <td>
                <asp:checkbox id="cboxComputerName" Runat="server"></asp:checkbox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermUserLogin" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <asp:textbox id="tboxUserlogin" Runat="server"></asp:textbox>
            </td>
            <td>
                <asp:checkbox id="cboxUserLogin" Runat="server"></asp:checkbox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermIPAddress" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
            <asp:textbox id="tboxIPAddress" Runat="server"></asp:textbox> 
            </td>
            <td>
            <asp:checkbox id="cboxIPAddress" Runat="server"></asp:checkbox>  
            </td>
        </tr>
      </table>  
        &nbsp;<asp:LinkButton runat="server" ID="lbtnApplyFilter" OnClick="lbtnApplyFilter_Click"><%=Resources.Resource.Apply%></asp:LinkButton>
        &nbsp;<asp:LinkButton runat="server" ID="lbtnCancelFilter" OnClick="lbtnCancelFilter_Click"><%=Resources.Resource.Clear%></asp:LinkButton>      
    </asp:Panel>
    <ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="lbtnFilterComputer" PopupControlID="filterShow" Position="Right" OffsetX="-365" />

    <asp:Panel ID="filterShowDevice" runat="server" style="display: none; z-index: 3; background-color:#DDD; border: thin solid navy;" Height="200px">
      <table>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermSerialNumber" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <asp:textbox id="tboxSerialNumber" Runat="server"></asp:textbox>
            </td>
            <td>
                <asp:checkbox id="cboxSerialNumber" Runat="server"></asp:checkbox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermCommentFilter" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <asp:textbox id="tboxCommentFilter" Runat="server"></asp:textbox>
            </td>
            <td>
                <asp:checkbox id="cboxCommentFilter" Runat="server"></asp:checkbox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermComputerNameFilter" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <asp:textbox id="tboxComputerFilter" Runat="server" style="width: 310px"></asp:textbox>
            </td>
            <td>
                <asp:checkbox id="cboxComputerFilter" Runat="server"></asp:checkbox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermLastInsertFilter" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <cc2:DateCustomControl ID="dccDateInsertedFilter" RenderInterval="true"  runat="server" ddlSkinID="ddcControl" />
            </td>
            <td>
                <asp:checkbox id="cboxLastInsertFilter" Runat="server"></asp:checkbox>
            </td>
        </tr>
      </table>  
        &nbsp;<asp:LinkButton runat="server" ID="lbtnApplyFilterDevice" OnClick="lbtnApplyFilterDevice_Click"><%=Resources.Resource.Apply%></asp:LinkButton>
        &nbsp;<asp:LinkButton runat="server" ID="lbtnCancelFilterDevice" OnClick="lbtnCancelFilterDevice_Click"><%=Resources.Resource.Clear%></asp:LinkButton>      
    </asp:Panel>
    <ajaxToolkit:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="lbtnFilterDevice" PopupControlID="filterShowDevice" Position="Right" OffsetX="-500" />

    <asp:Panel ID="filterShowUnknownDevice" runat="server" style="display: none; z-index: 3; background-color:#DDD; border: thin solid navy;" Height="180px" Width="520px">
      <table>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermComputerNameUDevice" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <asp:textbox id="tboxComputerNameUDevice" Runat="server" style="width: 310px"></asp:textbox>
            </td>
            <td>
                <asp:checkbox id="cboxComputerNameUDevice" Runat="server"></asp:checkbox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermSerialNumberUDevice" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <asp:textbox id="tboxSerialNumberUDevice" Runat="server" style="width: 310px"></asp:textbox>
            </td>
            <td>
                <asp:checkbox id="cboxSerialNumberUDevice" Runat="server"></asp:checkbox>
            </td>
        </tr>
        <tr style="visibility:hidden">
            <td>
                <asp:DropDownList ID="ddlTermCommentUDevice" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <asp:textbox id="tboxCommentUDevice" Runat="server" style="width: 310px"></asp:textbox>
            </td>
            <td>
                <asp:checkbox id="cboxCommentUDevice" Runat="server"></asp:checkbox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlTermLastInsertUDevice" runat="server" SkinID="ddlLogic">
                </asp:DropDownList>
	        </td>
            <td>
                <cc2:DateCustomControl ID="dccDateInserted" RenderInterval="true"  runat="server" ddlSkinID="ddcControl" />
            </td>
            <td>
                <asp:checkbox id="cboxLastInsertUDevice" Runat="server"></asp:checkbox>
            </td>
        </tr>    
      </table>  
        &nbsp;<asp:LinkButton runat="server" ID="lbtnApplyFilterUDevice" OnClick="lbtnApplyFilterUDevice_Click" ><%=Resources.Resource.Apply%></asp:LinkButton>
        &nbsp;<asp:LinkButton runat="server" ID="lbtnCancelFilterUDevice" OnClick="lbtnCancelFilterUDevice_Click" ><%=Resources.Resource.Clear%></asp:LinkButton>      
    </asp:Panel>
    <ajaxToolkit:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="lbtnFilterUnknownDevice" PopupControlID="filterShowUnknownDevice" Position="Right" OffsetX="-520"/>
</div>
</asp:Content>
