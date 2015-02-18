<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Controls_AsyncLoadingStateControl" Codebehind="AsyncLoadingStateControl.ascx.cs" %>
<div runat="server" ID="divLoaderState" style="text-align:center; vertical-align:middle; background-color:White; z-index: 9999; width: 100px; height: 20px;
    top: 3px; left: 50%; position: fixed; display: none; border-color:Black; border-width:thin; border-style:solid;">
    <asp:Label ID="lblLoading" runat="server" Text='<%$ Resources:Resource, Loading %>'></asp:Label>

</div>
<script type="text/javascript" language="javascript">
    //bool value signaling if AsyncLoadingStateControl_ApplicationLoadHandler was registering
    var AsyncLoadingStateControl_ApplicationLoadHandler_Registered = false;
    //register load handler
    Sys.Application.add_load(AsyncLoadingStateControl_ApplicationLoadHandler)

    function AsyncLoadingStateControl_ApplicationLoadHandler(sender, args) {
        //make sure that this is ran only once
        if (!AsyncLoadingStateControl_ApplicationLoadHandler_Registered) {
            //add attribute to PageRequestManager instance that defines whether to allow succeeding async postbacks
            Sys.WebForms.PageRequestManager.getInstance()._disableAsyncPostBack = false;
            //add initialize request handler
            Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(CheckAllowPostBack);
            //add begin request handler
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(ShowLoaderState);
            //add end request handler
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(HideLoaderState);
            //AsyncLoadingStateControl_ApplicationLoadHandler was registered
            AsyncLoadingStateControl_ApplicationLoadHandler_Registered = true;
        }
    }

    function CheckAllowPostBack(sender, args) {
        //if flag is set
        if (Sys.WebForms.PageRequestManager.getInstance()._disableAsyncPostBack) {
            //stop async postback
            args.set_cancel(true);
        }
    }


    function ShowLoaderState(sender, args) {
        //show Loading text
        $('#<%=divLoaderState.ClientID%>').show();
    }

    function HideLoaderState(sender, args) {
        //hide Loading text
        $('#<%=divLoaderState.ClientID%>').hide();
    }
</script>
