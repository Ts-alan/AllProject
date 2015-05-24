using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using Newtonsoft.Json;

namespace CCP.WebApi.Helpers
{
    public static class HttpMessageInitializer
    {
        public static HttpResponseMessage GetHttpMessage(HttpStatusCode statusCode, object value )
        {
            var responseMessage = new HttpResponseMessage(statusCode); 
            if (value != null)
            {
                responseMessage.Content =  new ObjectContent(value.GetType(), value, new JsonMediaTypeFormatter()
                {
                    SerializerSettings =
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    }
                });
            }
            return responseMessage;
        }

        public static HttpResponseMessage GetHttpMessage(HttpStatusCode statusCode, string value )
        {
            var responseMessage = new HttpResponseMessage(statusCode); 
            if (value != null)
            {
                responseMessage.Content =  new StringContent(value);
            }
            return responseMessage;
        }

        public static HttpResponseMessage GetHttpMessage(HttpStatusCode statusCode, Stream value)
        {
            var responseMessage = new HttpResponseMessage(statusCode);
            if (value != null)
            {
                responseMessage.Content = new StreamContent(value);
            }
            return responseMessage;
        }
    }
}