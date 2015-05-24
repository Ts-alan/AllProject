using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using CCP.DAL.DataModels;
using CCP.WebApi.Helpers;
using CCP.WebApi.Models;
using CCP.WebApi.Services;
using Newtonsoft.Json;

namespace CCP.WebApi.Controllers
{
    public class SalesPersonsController : ApiController
    {
        private readonly SalesPersonService _salesPersonService = new SalesPersonService();

        // POST: api/SalesPersons
        public HttpResponseMessage Post(DataSourceRequest model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dataSourceResult = _salesPersonService.GetSalesPersons(model);
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, dataSourceResult);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [Route("api/SalesPerson")]
        public HttpResponseMessage Post(SalesPerson model)
        {
            if (model != null && ModelState.IsValid)
            {
                if (model.SalesPersonId == 0)
                {
                   _salesPersonService.AddSalesPerson(model);
                }
                else
                {
                    _salesPersonService.UpdateSalesPerson(model);
                   
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public HttpResponseMessage Get(long data)
        {
            if (data > 0)
            {
                var user = _salesPersonService.GetSalesPerson(data);
                if (user != null)
                {
                    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, user);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
