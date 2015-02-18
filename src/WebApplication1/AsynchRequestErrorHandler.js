
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
           
            function EndRequestHandler(sender, args)
            {
               if (args.get_error() != undefined)
               {
                   var errorMessage;
                   if (args.get_response().get_statusCode() == '200')
                   {
                       errorMessage = args.get_error().message;
                   }
                   else
                   {
                       // Error occurred somewhere other than the server page.
                       errorMessage = 'An unspecified error occurred. '; 
                   }
                    args.set_errorHandled(true);
               }
            }