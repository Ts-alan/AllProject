using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using CCP.DAL.DataModels;
using CCP.WebApi.Helpers;
using CCP.WebApi.Models;
using CCP.WebApi.Resources;
using CCP.WebApi.Services;

namespace CCP.WebApi.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private readonly UserService _userService = new UserService();

        // POST: api/Users
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public HttpResponseMessage Post(DataSourceRequest model)
        {
            if (model != null && ModelState.IsValid)
            {
                var dataSourceResult = _userService.GetUsers(model);
                return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, dataSourceResult);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
        [Authorize(Roles = "Admin")]
        [Route("api/User")]
        public HttpResponseMessage Post(UserModel model)
        {
            string logicError = null;
            ModelStateDictionary modelErrors = null;
            if (model != null )
            {
                if (!ModelState.IsValid)
                {
                    modelErrors = ModelState;
                }
                int result;
                if (model.User.UserId == 0)
                {
                    result = model.ApproverTiers == null ? _userService.Insert(model.User) : _userService.Insert(model);
                }
                else
                {
                    result = model.ApproverTiers == null ? _userService.Update(model.User) : _userService.Update(model);
                }

                if (result == 0)
                {
                    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK,ErrorsModelBuilder.GetErrorsModel(Res.UserSaveOk) );
                    
                }
                if (result == 1)
                {
                    logicError = Res.UserSaveErrorEmail;
                    //return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest,ErrorsModelBuilder.GetErrorsModel(Res.UserSaveErrorEmail) );
                }
                if (result == 2)
                {
                    logicError = Res.UserSaveErrorApprovers;
                    //return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.Conflict,ErrorsModelBuilder.GetErrorsModel(Res.UserSaveErrorApprovers) );
                }
                if (result == 3)
                {
                    logicError = Res.DeletingSalesPersonWithCPR;
                    //return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.Conflict,ErrorsModelBuilder.GetErrorsModel(Res.DeletingSalesPersonWithCPR) );
                }
            }
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, ErrorsModelBuilder.GetErrorsModel(logicError,modelErrors));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public HttpResponseMessage Get(long data)
        {
            if (data > 0)
            {
                var user = _userService.GetUser(data);
                if (user != null)
                {
                    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, user);
                }
            }
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, new User());
        }

        [HttpGet]
        [Route("api/User")]
        public HttpResponseMessage Get(string data)
        {
            if (data != null)
            {
                var user = _userService.GetUser(data);
                if (user != null)
                {
                    return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, user);
                }
            }
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.BadRequest, new User());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/TiersCount")]
        public HttpResponseMessage Get()
        {
            var tiersCount = _userService.GetTiersCount();
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, tiersCount);
        }
        [Authorize(Roles = "Admin, Initiator")]
        [HttpGet]
        [Route("api/User/SalesPerson")]
        public HttpResponseMessage GetSalesPerson()
        {
            var salesPerson = _userService.GetSalesPerson(User.Identity.Name);
            return HttpMessageInitializer.GetHttpMessage(HttpStatusCode.OK, salesPerson);
        }
    }
}
