<%@ Page Language="C#" validateRequest="false" AutoEventWireup="true" Inherits="Logins" Codebehind="Logins.aspx.cs" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" >
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title></title>
        <script src="js/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
        <link rel="Stylesheet" href="App_Themes/style.css" />
        <script type="text/javascript" language="javascript">
            function DropDown(el) {
                this.dd = el;
                this.initEvents();
            }
            DropDown.prototype = {
                initEvents: function () {
                    var obj = this;
                    obj.dd.on('click', function (event) {
                        $(this).toggleClass('active');
                        event.stopPropagation();
                    });
                }
            }
            $(function () {
                var dd = new DropDown($('#dd'));
                $(document).click(function () {
                    $('.wrapper-dropdown').removeClass('active');
                });
            });
        </script>
    </head>
    <body>
        <form id="form1" runat="server">
            <div class="wrapper-dropdown" id="dd"><%=Resources.Resource.Language%>
		        <ul class="dropdown">
		            <li><a href="Logins.aspx?lang=ru"><i class="icon-ru"></i>Русский</a></li>
		            <li><a href="Logins.aspx?lang=en"><i class="icon-eng"></i>English</a></li>
		        </ul>
	        </div>
            <div class="curved-vt-1" style="width:30%;margin-left:33%;margin-top:10%">
                <div class="header"><font style="color:#FF0000">C</font>ontrol <font style="color:#FF0000">C</font>enter</div>
                <div align="center" style="width:100%;">
                   <asp:Label ID="lblError" runat="server"></asp:Label>
                   <asp:Login ID="lgLogin" runat="server" CreateUserUrl="~/Registration.aspx" 
                   DestinationPageUrl="~/Default.aspx" DisplayRememberMe="False">
                       <TextBoxStyle CssClass="input" Width="150px" BorderColor="LightCyan" BorderStyle="Groove" Font-Names="Verdana" />
                       <LoginButtonStyle  CssClass="LoginButton" Width="70px"  />
                       <InstructionTextStyle Font-Names="Verdana" Font-Size="9pt" />
                       <LabelStyle Font-Names="Verdana" Font-Size="9pt" />
                       <TitleTextStyle Font-Names="Verdana" Font-Size="10pt" />
                   </asp:Login>
                </div>
            </div>
        </form>
    </body>
</html>