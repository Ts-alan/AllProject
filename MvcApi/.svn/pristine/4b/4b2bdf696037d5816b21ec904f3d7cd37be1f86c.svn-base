using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.IO;
using System.Web.Script.Serialization;
using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;

namespace MvcApi
{
    public class ApiAttribute : ActionFilterAttribute
    {
        private readonly static string[] _jsonTypes = new string[] { "application/json", "text/json" };
        private readonly static string[] _xmlTypes = new string[] { "application/xml", "text/xml" };
        private readonly static string[] _htmlTypes = { "application/xhtml", "text/html" };
        private readonly static string _formEncodedType = "application/x-www-form-urlencoded";
        private readonly static Regex _typeRegEx = new Regex(@"[\w\*]+/[\w\*]+");
        private readonly static MethodInfo _javascriptDeserialize = typeof(JavaScriptSerializer).GetMethods().Single(m => m.Name == "Deserialize" && m.IsGenericMethod);

        private static Dictionary<string, string> CreateInputDictionary(ActionExecutingContext filterContext)
        {
            var inputStream = filterContext.HttpContext.Request.InputStream;
            var parameters = filterContext.ActionDescriptor.GetParameters();

            if (inputStream.Length == 0 || parameters.Length == 0)
                return null;

            using (var reader = new StreamReader(inputStream))
            {
                // get the params that are using the [ApiBind] attribute
                var apiBindParameters = parameters
                    .Where(p => p.GetCustomAttributes(typeof (ApiBind), false).Length > 0)
                    .Select(p => p.ParameterName)
                    .ToArray();

                if (filterContext.HttpContext.Request.ContentType.Contains(_formEncodedType))
                    // the request is form post in key=value format, build the input from that
                    return reader.ReadToEnd()
                        .Split(new char[] { '&' })
                        .Select(kvp => kvp.Split(new char[] { '=' }))
                        .Where(kvp => !apiBindParameters.Any() || apiBindParameters.Contains(kvp[0], StringComparer.InvariantCultureIgnoreCase))
                        .ToDictionary(kvp => kvp[0], kvp => HttpUtility.UrlDecode(kvp[1]));
                else
                {
                    if (parameters.Length > 1 && apiBindParameters.Length != 1)
                        throw new ArgumentOutOfRangeException("Unable to infer which parameter should be bound from the request input. Use the [ApiBind] attribute on the parameter that should be bound to the input.");
                    
                    return new Dictionary<string, string>() { { parameters.Length == 1 ? parameters.Single().ParameterName : apiBindParameters.Single(), reader.ReadToEnd() } };
                }
            }
        }

        private static void MapActionParameters(ActionExecutingContext filterContext, Func<string, Type, object> deserialize)
        {
            var input = CreateInputDictionary(filterContext);

            if (input == null)
                return;

            foreach (var param in filterContext.ActionDescriptor.GetParameters())
            {
                if (!input.ContainsKey(param.ParameterName))
                    continue;

                try { filterContext.ActionParameters[param.ParameterName] = deserialize(input[param.ParameterName], param.ParameterType); }
                catch { }   // I don't like this anymore than you do, but I havn't found an effective
                            // way of detecting if a parameter can/should be deserialized. 
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (_jsonTypes.Any(type => request.ContentType.Contains(type)))
                MapActionParameters(filterContext, (input, type) => { return _javascriptDeserialize.MakeGenericMethod(new[] { type }).Invoke(new JavaScriptSerializer(), new object[] { input }); });

            else if (_xmlTypes.Any(type => request.ContentType.Contains(type)))
                MapActionParameters(filterContext, (input, type) => { return new XmlSerializer(type).Deserialize(new StringReader(input)); });
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (typeof(RedirectToRouteResult).IsInstanceOfType(filterContext.Result))
                return;

            var acceptTypes = (filterContext.HttpContext.Request.AcceptTypes ?? new[] { "text/html" })
                .Select(a => _typeRegEx.Match(a).Value).ToArray();

            if (_htmlTypes.Any(type => acceptTypes.Contains(type)))
                return;

            var model = filterContext.Controller.ViewData.Model;

            if (_jsonTypes.Any(type => acceptTypes.Contains(type)))
                filterContext.Result = new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            else if (_xmlTypes.Any(type => acceptTypes.Contains(type)))
                filterContext.Result = new XmlResult() { Data = model };
        }
    }
}